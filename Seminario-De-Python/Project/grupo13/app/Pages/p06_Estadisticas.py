import streamlit as st
import pandas as pd
import sys
from pathlib import Path
sys.path.append(str(Path('.').resolve()))
from modules.paths import PLAYED_GAMES_JSON as games
from modules.paths import USER_DATA_JSON as users
from modules import stats
import navigate as nav

nav.del_questions_and_answers()


st.header("Estadísticas del juego")

# Abro los archivos de usuarios y de partidas
try:
    df_games=pd.read_json(games)
    df_users=pd.read_json(users)

    # 1) Gráfico torta de usuarios agrupando por género
    stats.users_grouped_gender(df_games,df_users)

    # 2) Gráfico torta con puntajes mayores a la media
    stats.percentage_scores_above_mean(df_games)

    # 3) Gráfico de barras partidas cada día de la semana
    stats.games_per_day(df_games)

    # 4) Promedio de preguntas acertadas mensuales entre un rango de dos fechas
    stats.average_between_dates(df_games) 

    # 5) Top 10 usuarios entre dos fechas
    stats.top_ten(df_games)

    # 6) Ordenar por dificultad, ubicando primero los que tienen mayores errores en las respuestas 
    stats.sorted_by_difficulty(df_games)

    # 7) Gráfico de lineas que permite seleccionar dos usuarios y compararlos
    # Evolución de su puntaje a lo largo del tiempo
    stats.compare_users(df_games)

    # 8) Listar para cada género cuál es la temática en la cual demuestra mayor conocimiento
    stats.filter_max_by_gender(df_games,df_users)

    # 9) Listar cada dificultad de juego junto con el puntaje promedio obtenido en cada una y
    # con la cantidad de veces que fue elegida
    stats.average_by_difficulty(df_games)

    # 10) Listado de usuarios en racha. Lista los usuarios que registran una partida con un
    #   puntaje mayor a cero en todos los días durante los últimos 7 días.
    stats.on_strake(df_games)

    if st.button("Volver a inicio"):
        st.switch_page(nav.to('home'))
    if st.button("Jugar"):
        st.switch_page(nav.to('play'))
        
except FileNotFoundError:
    print("Alguno de los archivos no se encontró.")
    st.write('Todavía no hay partidas jugadas con las que mostrar estadísticas')
    if st.button("¡Jugar ahora!"):
        st.switch_page(nav.to('play'))
    if st.button("Volver a inicio"):
        st.switch_page(nav.to('home'))
except ValueError:
    print(f"Error en el formato de algún archivo.")
except Exception as e:
    print(f"Ocurrió un error inesperado: {e}")


