import streamlit as st
import navigate as nav

from pathlib import Path
import sys
sys.path.append(str(Path('.').resolve()))
from modules.game import games_data as game
import pandas as pd

def show_ranking():

    # Si finalizó la partida y no se puso "jugar de nuevo"
    if "questions_and_answers" in st.session_state and st.session_state["game_state"] == 6 and st.session_state["questions_and_answers"]:
        
        st.header("FIN DE LA PARTIDA")
        st.write(f". {st.session_state['new_game'].date_time}")     # imprime fecha
        st.write(f". {st.session_state['new_game'].correct_answers} respuestas correctas de {len(st.session_state.questions_and_answers)}")  # imprime cantidad de respues correctas
        st.write(f". Resultado final de {st.session_state['new_game'].score} puntos")  # imprime el puntaje final
        
        i = 1  # iterador que muestra el numero de pregunta
        for value in st.session_state.questions_and_answers:

            st.subheader(f"Pregunta #{i}")
            st.write(f".- {value['question']}:") # imprime la pregunta

            if value['selected_answer'] == value['correct_answer']: # verifica si se respondio correctamente
                st.success(f"✅ Respuesta correcta: {value['correct_answer']}") # muestra la respuesta correcta
            else:
                st.warning(f"❌ Respuesta del usuario: {value['selected_answer']}") # muestra la respuesta correcta y la seleccionada por el usuario
                st.success(f"✅ Respuesta correcta: {value['correct_answer']}")
            
            i = i + 1
            st.write("---")
 

    st.header("Ranking historico de Puntajes")

    games = game() # obtiene los datos de los juegos almacenados en Json

    if games:

        df_games = pd.DataFrame(games)

        df_games['score'] = df_games['score'].astype(int)

        df_games.sort_values('score', ascending=False, inplace=True, ignore_index=True) # ordena segun score

        df_games['Puesto'] = df_games.index + 1

        df_games = df_games[['Puesto', 'name', 'score', 'user_identifier']]

        df_games.rename(columns={'name': 'Usuario/a', 'score' : 'Puntaje', 'user_identifier' : 'Email'}, inplace=True)

        st.dataframe(df_games.head(15).set_index('Puesto'), width=800)

    else:
        if st.button("Registrarse"):
            st.switch_page(nav.to('register'))

    if st.button("Volver a inicio"):
        st.switch_page(nav.to('home'))
    if st.button("Ir a Jugar"):
        st.switch_page(nav.to('play'))
        

    
if __name__ == "__main__":
  show_ranking()