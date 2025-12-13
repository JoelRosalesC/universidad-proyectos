import csv
import sys
from pathlib import Path
sys.path.append(str(Path('..').resolve().parent.parent))
import random
import pandas as pd

#TODO: generate_conections y clase conectividad
#TODO: generate_c2022 y clase censo2022

class Trivia:
    """Clase 'Lago' para guardar todo en un objeto, se encarga de guardar las respuestas correctas e incorrectas"""
    def __init__(self, name: str, answer: str, possible_fake_answers: list, type: str):
        #fake_answers
        self.name = name
        self.answer = answer
        self.type = type
        if type == "c2022":
            self.fake_answers = self._generate_fake_numbers(possible_fake_answers)
            print(self.fake_answers)
        else:
            self.fake_answers = self._generate_fake_locations(answer, possible_fake_answers)
    
    def _generate_fake_locations(self, location: str, possible_fake_locations: list) -> list:

        # Quitar la ubicacion que es la correcta, quitando una ubicacion de la lista
        #not in y no != porque hay UNO que dice rio negro/ neuquen y como cuenta como 2 no puedo
        valid_fake_locations = [loc for loc in possible_fake_locations if loc not in location] 
        
        # Randomly select 3 unique fake locations
        return random.sample(valid_fake_locations, 3)
    
    def _generate_fake_numbers(self, possible_fake_numbers: list) -> list:
        aux = possible_fake_numbers.copy()

        if self.answer >= 80000 and self.answer <= 292000:
            self.answer = possible_fake_numbers[0]
            aux.pop(0)
        
        elif 292000 <= self.answer and self.answer <= 540000:
            self.answer = possible_fake_numbers[1]
            aux.pop(1)

        elif 540000 <= self.answer and self.answer<= 800000:
            self.answer = possible_fake_numbers[2]
            aux.pop(2)

        elif self.answer > 800000:
            self.answer = possible_fake_numbers[3]
            aux.pop(3)

        return aux
    

    def ask(self):
        if self.type == "Lake":
            return f"De que provincia es el lago {self.name}?"
        elif self.type == "Airport":
            return  f"De que provincia es el aeropuerto {self.name}?"
        elif self.type == "c2022":
            return f"El censo de 2022, en {self.name} el total de poblacion fue de:"
        elif self.type == "connection":
            return "Cual de las siguientes localidades posee conectividad?"
        else:
            return "error"
        

def generate_connections():
    from modules.paths import CONNECTIONS_DATA_MODIFIED
    try:
        df = pd.read_csv(CONNECTIONS_DATA_MODIFIED)

        # Filtro las que tienen conectividad con las que no
        no_conectivity_df = df[df['Posee_Conectividad'] == 'NO']
        conectivity_df = df[df['Posee_Conectividad'] == 'SI']

        # Saco aleatoriamente 5 Nombres 'Localidad'
        random_no_connectivity = no_conectivity_df.sample(5)

        connections_list = []

        for _, row in random_no_connectivity.iterrows():
            correct_location = row['Localidad']
            fake_locations = conectivity_df['Localidad'].sample(3).tolist()

            connections_list.append(Trivia(name=correct_location, answer=correct_location, possible_fake_answers=fake_locations, type="connection"))

        return connections_list
    
    except (FileNotFoundError, PermissionError):
        return None
    

def generate_c2022():
    """Genera los datos necesarios para hacer las preguntas:
    Retorna Lista De lagos/aeropuertos/c2022"""
    from modules.paths import C2022_DATA_MODIFIED
    try:
        df = pd.read_csv(C2022_DATA_MODIFIED, encoding='utf-8')

        #Sacar total del pais del dataset
        df = df[df['Jurisdicción'] != "Total del pais"]

        all_answers = ["Entre 80.000 y 292.000", "Entre 292.000 y 540.000", "Entre 540.000 y 800.000", "Más de 800.000+"]
       
        # Total de jurisdicciones
        total_jurisdictions = df.shape[0]

        # Select 5 random jurisdictions and sort them
        random_jurisdictions_index = random.sample(range(total_jurisdictions), 5)
        random_jurisdictions_index.sort()

        # Create a list to store c2022 objects
        c2022_list = []

        for i, row in df.iterrows():
            if i in random_jurisdictions_index:
                c2022_list.append(Trivia(name=row['Jurisdicción'], answer=row['Total de población'], possible_fake_answers=all_answers, type="c2022"))

        return c2022_list
        
    except FileNotFoundError:
        # Para preguntar if x: esto else: esto otro
        return None
    except PermissionError:
        return None

def generate_lakes():
    """Genera los datos necesarios para hacer las preguntas:
    RETORNA LISTA DE lagos/aeropuertos/c2022 """
    from modules.paths import AR_LAKES_MODIFIED

    try:
        df = pd.read_csv(AR_LAKES_MODIFIED, encoding='utf-8')
        provinces = [
                "Tierra del Fuego, Antártida e Islas del Atlántico Sur",
                "Santa Cruz",
                "Río Negro",
                "Neuquén",
                "Chubut",
                "Buenos Aires"
            ]
        # Lagos totales
        total_lakes = df.shape[0]
        # Selecciono 5 "id's" de lagos y los ordeno
        random_lakes_index = random.sample(range(total_lakes), 5)
        random_lakes_index.sort()
        # Creo una lista
        lakes = []

        for i, row in df.iterrows():
            if i in random_lakes_index:
                lakes.append(Trivia(name=row['Nombre'], answer=row['Ubicación'], possible_fake_answers=provinces, type="Lake"))

        return lakes
            
    except FileNotFoundError:
        # Para preguntar if x: esto else: esto otro
        return None
    except PermissionError:
        return None
    
def generate_airports():

    """Genera los datos necesarios para hacer las preguntas
    Retorna lista de Lagos/Aeropuertos/c2022"""
    from modules.paths import AR_AIRPORTS_DATA_MODIFIED
    try:
        df = pd.read_csv(AR_AIRPORTS_DATA_MODIFIED, encoding='utf-8')
        
        # Cambiar a buenos aires (autonomous city) por caba 
        df['region_name'] = df['region_name'].str.replace(r'Buenos Aires (Autonomous City)', 'C.A.B.A')
        df['region_name'] = df['region_name'].str.replace(r' Province', '')
        
        # Sacarles el "province"
        provinces = df['region_name'].unique().tolist()
        
        # Aeropuertos totales
        total_airports = df.shape[0]
        # Selecciono 5 "id's" random y las ordeno
        random_airports_index = random.sample(range(total_airports), 5)
        random_airports_index.sort()
        # lista de aeropuertos
        airports = []

        for i, row in df.iterrows():
            if i in random_airports_index:
                airports.append(Trivia(name=row['name'], answer=row['region_name'], possible_fake_answers=provinces, type="Airport"))

        return airports

    except FileNotFoundError:
        return None
    except PermissionError:
        return None