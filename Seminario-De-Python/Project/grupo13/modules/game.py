import folium
import streamlit as st
from modules import questions
from pathlib import Path
import json
import sys
sys.path.append(str(Path('.').resolve()))

class Game:
    """Clase juego para poder guardar los datos una vez terminado"""
    def __init__(self, name: str, user_identifier: str,  difficutly: str, theme: str):

        self.name = name
        self.user_identifier = user_identifier
        self.date_time = None           #F
        self.difficulty = difficutly
        self.theme = theme 
        self.correct_answers = 0        #F
        self.score = 0                  #F

    def correct(self):
        """Aumenta en 1 las respuestas correctas"""
        self.correct_answers += 1

    #PLAYED_GAMES_JSON TODO
    def game_finished(self, date_time: str, games_file_path):
        """Al terminar la partida calcula el score y agrega el date_time"""
        self.date_time = date_time
        self.score = (self.correct_answers * 100) * self._diff()
        self.save_to_json(games_file_path)

    def _diff(self):
        """Calcula el multiplicador de dificultad dependiendo de self.diff"""
        match self.difficulty:
            case "Fácil":
                return 1
            case "Media":
                return 1.5
            case "Alta":
                return 2 
            case _:
                return 0.1
    
    def to_dict(self):
        """Convierte la instancia en un diccionario"""

        return {
            "name": self.name,
            "user_identifier": self.user_identifier,
            "date_time": self.date_time,
            "difficulty": self.difficulty,
            "theme": self.theme,
            "correct_answers": self.correct_answers,
            "score": self.score
        }

    def save_to_json(self, games_file_path):
        """Guarda la instancia del juego en un archivo JSON"""
        #TODO: test
        try:
            with open(games_file_path, 'r') as file:
                games = json.load(file)

        except (FileNotFoundError, json.JSONDecodeError, PermissionError):
            games = []

        games.append(self.to_dict())

        try:
            with open(games_file_path, 'w') as file:
                json.dump(games, file, indent=4)
        except (FileNotFoundError, json.JSONDecodeError, PermissionError):
            st.error("Error guardando el archivo")

def state_reset():
    """Reset del estado del juego, para volver a jugar, cancelar el comienzo, o por x causa"""
    if "game_state" in st.session_state:
        st.session_state["game_state"] = 0
        st.session_state["user"] = None
        st.session_state["game_diff"] = None
        st.session_state.pop("finished", None)
        st.session_state.pop("questions_and_answers", None)
        st.session_state.pop("new_game", None)
        st.session_state.pop("50-50", None)
        st.session_state.pop("discard_one", None)

def users():
    """Retorna los usuarios registrados"""

    # modulos de usuarios
    from modules.paths import USER_DATA_JSON
    try:
        with open(USER_DATA_JSON, 'r') as file:
            users = json.load(file)

    except (FileNotFoundError, json.JSONDecodeError, PermissionError):
        return None

    return users

def games_data():
    #TODO:Descripcion
    from modules.paths import PLAYED_GAMES_JSON
    
    games = []

    try:
        with open(PLAYED_GAMES_JSON, 'r') as file:
            content = file.read()

            if not content:
                st.info("Aun sin registros")
                return games

            games = json.loads(content) 
        
    except FileNotFoundError:
        st.write("Todavía no hay usuarios que hayan jugado para generar el ranking")
    except json.JSONDecodeError:
        st.error("El archivo no está en formato JSON válido")
    except PermissionError:
        st.error("Permiso denegado al archivo")
    
    return games

def game_conditions_form(user_names):
    """Formulario para comenzar el juego"""

    # Temas y dificultades por default
    themes = ["Aeropuertos", "Lagos", "Conectividad", "Censo 2022"]
    difficulties = ["Fácil", "Media", "Alta"]
    
    # Formulario para la generacion de preguntas
    with st.form("Preparacion",clear_on_submit=True):

        selected_user = st.selectbox("Seleccione al usuario que jugara:" , user_names)
        difficulty = st.selectbox("Seleccione la dificultad", difficulties)
        theme = st.selectbox("Seleccione el tematica", themes)
        submit_button = st.form_submit_button("Jugar")
    
    # Lo que pasa una vez se hace submit en caso de estar todos los valores
    if submit_button and selected_user and difficulty and theme:
        

        name, mail = selected_user.split(" (")
        mail = mail.rstrip(")")

        selected = {
            "user": name,
            "mail": mail,
            "game_diff": difficulty,
            "theme": theme
        }
        
        return selected
    
    if submit_button and not any(selected_user, difficulty, theme):
        st.error("Por favor, complete todos los campos antes de presionar 'Jugar'")



def generate_all_trivia(theme):
    """Funcion para generar la trivia en funcion del tema que se eligio"""
    if theme == "Lagos":
        return questions.generate_lakes()

    elif theme == "Aeropuertos":
        return questions.generate_airports()

    elif theme == "Censo 2022":
        return questions.generate_c2022()

    elif theme == "Conectividad":
        return questions.generate_connections()
    
    else: 
        return None


def set_lifelines(current_game):
    if "50-50" and "discard_one" not in st.session_state:
        if current_game.difficulty == "Fácil":
            st.session_state["50-50"] = 2
            st.session_state["discard_one"] = 2

        elif current_game.difficulty == "Media":
            st.session_state["50-50"] = 1
            st.session_state["discard_one"] = 1

        else:
            st.session_state["50-50"] = 0
            st.session_state["discard_one"] = 0


def lifeline_50_50(options, current_question):
    """Recibe las opciones: List. Y el juego actual de tipo Game"""
    if len(options) > 2:
        count_removed = 0
        for answer in options:
            # Si la respuesta no es la correcta 
            if answer != current_question.answer:
                options.remove(answer)
                count_removed += 1
            if count_removed == 2:
                st.session_state["50-50"] -= 1
                st.session_state.shuffled_answers = options
                st.rerun()
    else:
        st.error("No se pueden descartar dos respuestas, ya hay muchas descartadas")


def discard_one(options, current_question):
    if len(options) > 1:
        for answer in options:
            #Si la respuesta no es la correcta
            if answer != current_question.answer:
                options.remove(answer)
                st.session_state["discard_one"] -= 1
                st.session_state.shuffled_answers = options
                st.rerun()
    else:
        st.error("La correcta es la que queda 💀")

def load_airport_data():
    """Filtra datos de aeropuertos, rellena los valores NaN de algunas columnas con "Desconocido" y devuelve el DataFrame resultante."""
    from modules.paths import AR_AIRPORTS_DATA_MODIFIED
    import pandas as pd
    try:
        df_airport = pd.read_csv(AR_AIRPORTS_DATA_MODIFIED)
    except FileNotFoundError:
        st.info("El dataset ar_airports.csv aun no se ha procesado")
        return None
    else:
        df_airports = df_airport[df_airport.type.str.contains('airport')]
        df_airports.fillna({
            'elevation_ft': 'Desconocido',
            'iata_code': 'Desconocido',
            'local_code': 'Desconocido',
            'elevation_name' : 'Desconocido'
        }, inplace=True)

        df_airports.rename(columns={
            'name': 'Nombre',
            'region_name': 'Region',
            'elevation_ft': 'Elevación',
            'iata_code': 'IATA Code',
            'local_code': 'LOCAL Code'
        }, inplace=True)

        return df_airports

def load_lake_data():
    """Filtra datos de Lagos, rellena los valores NaN de algunas columnas con "Desconocido" y devuelve el dataframe resultante"""
    from modules.paths import AR_LAKES_MODIFIED
    import pandas as pd

    try:
        df_lake = pd.read_csv(AR_LAKES_MODIFIED)
    except FileNotFoundError:
        st.info("El dataset lagos_arg.csv aun no se ha procesado")
        return None
    else:
        df_lake.fillna({
            'Profundidad máxima (m)': 'Desconocido',
            'Profundidad media (m)': 'Desconocido',
        }, inplace=True)

        return df_lake

def generate_map():
    """Genera y retorna un mapa hecho con folium"""
    attr = (
    '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> '
    'contributors, &copy; <a href="https://cartodb.com/attributions">CartoDB</a>'
    )
    
    tiles = 'https://wms.ign.gob.ar/geoserver/gwc/service/tms/1.0.0/capabaseargenmap@EPSG%3A3857@png/{z}/{x}/{-y}.png'
    m = folium.Map(
        location=(-33.457606, -65.346857),
        control_scale=True,
        zoom_start=5,
        name='es',
        tiles=tiles,
        attr=attr
    )
    return m

def add_fullscreen(map):
    """Agrega a 'map' la opcion de visualizar el mapa en pantalla completa"""
    folium.plugins.Fullscreen(
        position="topright",
        title="Pantalla Completa",
        title_cancel="Cancelar",
        force_separate_button= True
    ).add_to(map)
    return map

def add_marker_airport_map(row, map):
    """Agrega marcadores al mapa de aeropuertos"""

    if row['elevation_name'] != 'Desconocido':
        color = 'red' if row['elevation_name'] == 'alto' else ('blue' if row['elevation_name'] == 'medio' else 'green')
        folium.Marker(
            [row['latitude_deg'], row['longitude_deg']],
            popup=row['Nombre'],
            icon=folium.Icon(color=color)
        ).add_to(map)

def add_marker_lake_map(row, map):
    """Agrega marcadores al mapa de lagos"""

    color = 'red' if row['Sup Tamaño'] == 'grande' else ('blue' if row['Sup Tamaño'] == 'medio' else 'green')
    folium.Marker(
        [row['Latitud'], row['Longitud']],
        popup=row['Nombre'],
        icon=folium.Icon(color=color)
    ).add_to(map)