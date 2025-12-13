import streamlit as st
from pathlib import Path
import sys
sys.path.append(str(Path('.').resolve()))

from modules.paths import PLAYED_GAMES_JSON
from modules import game
import navigate as nav

import random

# Estados del juego:
# 0: Preparación del juego: La persona en la aplicación elije las condiciones para comenzar a jugar se generan las preguntas y se pasa al siguiente estado
# 1-5: El usuario ya eligió las condiciones,  y comienza a contestar las preguntas, hasta terminar.
# 6: El usuario terminó de contestar las preguntas se le da la opción de ir a ranking y ver su partida o de volver a jugar
# En caso de volver a jugar, se vuelve al estado 0 

if "went_ranking" in st.session_state:
    nav.del_questions_and_answers()
    st.session_state.pop("went_ranking", None)

# Primer estado del juego 
if 'game_state' not in st.session_state:
    st.session_state["game_state"] = 0

if 'questions_and_answers' not in st.session_state: 
    st.session_state.questions_and_answers = []

if st.session_state["game_state"] < 1:
    st.title("¡Bienvenido a PyTrivia!")
    st.subheader("Para comenzar a jugar seleccione el usuario y temática")

# Estado base del juego
if st.session_state["game_state"] == 0:
    users = []
    # Json load, acorto los nombres y paso la data a una funcion
    users = game.users()
    if not users:
        st.write("Todavia no hay usuarios ingresados, vaya al apartado de Registrar para crear un nuevo usuario")
    
    else: 
        user_names = [f"{user['name']} ({user['mail']})" for user in users] if users else ["No hay usuarios"]

        # formulario para generar las preguntas y comenzar el juego
        selected = game.game_conditions_form(user_names)

        if selected:
            new_game = game.Game(name=selected['user'], user_identifier=selected['mail'], difficutly=selected["game_diff"], theme=selected["theme"])
            st.write(f"usuario = {new_game.name} | mail:{new_game.user_identifier} | tema:{new_game.theme} | dificultad: {new_game.difficulty}")

            # Guardo en session_state las preguntas generadas y el nuevo juego
            st.session_state.trivia = game.generate_all_trivia(new_game.theme)
            if not st.session_state.trivia:
                st.error("se produjo un error generando las preguntas")
                st.rerun()
        
            st.session_state.new_game = new_game

            # El estado de juego pasa a 1 y hago rerun
            st.session_state["game_state"] = 1 
            st.rerun()


# Estado 1..5 = preguntas con su respectivo num
elif st.session_state["game_state"] >= 1 and st.session_state["game_state"] <= 5:

    # Traigo del session_state las praguntas generadas y el juego
    trivia = st.session_state.trivia
    new_game = st.session_state.new_game

    # Imprime el numero de pregunta en la que el usuario se encuentra
    st.subheader(f"Pregunta {int(st.session_state['game_state'])}") 

    # Variable para cada pregunta
    current_question = trivia[st.session_state["game_state"] - 1]
    game.set_lifelines(new_game)

    # Mezclo las preguntas utilizando el session_state, para que se mezclen una sola vez
    if 'shuffled_answers' not in st.session_state:
        possible_answers = current_question.fake_answers + [current_question.answer]
        random.shuffle(possible_answers)
        st.session_state.shuffled_answers = possible_answers
    else:
        possible_answers = st.session_state.shuffled_answers

    # Dice cuantos comodines le quedan
    st.write("Total de comodines de 50-50:", st.session_state["50-50"])
    st.write("Total de comodines de 'descartar:", st.session_state["discard_one"])

    # COMODINES
    #session_state para 50-50
    if(st.session_state["50-50"] > 0):
        if st.button("50-50"):
            options = st.session_state.shuffled_answers
            game.lifeline_50_50(options, current_question)

    # Otro session_state para descartar una 
    if(st.session_state["discard_one"]):
        if st.button("Descartar una"):
            options = st.session_state.shuffled_answers
            game.discard_one(options, current_question)

    selected_answer = st.radio(current_question.ask(), possible_answers)
    
    # Si la pregunta seleccionada es igual a la pregunta correcta entonces se aumenta en 1 las preguntas correctas
    if st.button("Responder"):
        if selected_answer == current_question.answer:
            new_game.correct()
        
        st.session_state.questions_and_answers.append({
            "question" : current_question.ask(),
            "correct_answer" : current_question.answer,
            "selected_answer" : selected_answer
        })

        # Se agrega uno al estado de juego 
        st.session_state["game_state"] += 1
        # Por último se quita el estado de "respuestas mezcladas" para que se mezclen las preguntas que vienen 
        st.session_state.pop('shuffled_answers', None)
        st.rerun()

# Finaliza el juego en el estado 6
elif st.session_state["game_state"] == 6:
    from datetime import datetime

    
    
    # Se quita el estado de los comodines para que al jugar de nuevo no haya problema
    st.session_state.pop("50-50", None)
    st.session_state.pop("discard_one", None)

    # Traigo el nuevo juego del session_state
    new_game = st.session_state.new_game 

    # Finalizo el juego, esto es para que no se guarde varias veces el usuario
    if "finished" not in st.session_state:
        st.session_state.finished = ":D"
        new_game.game_finished(datetime.now().strftime("%Y-%m-%d %H:%M:%S"), PLAYED_GAMES_JSON)

    st.subheader("Para ver el resultado de la última partida, vaya a ranking")
    if st.button("ir al ranking"):
        if "went_ranking" not in st.session_state:
            st.session_state.went_ranking = "Y"
        st.switch_page(nav.to('ranking'))
    
    st.subheader("Para jugar de nuevo presione en 'jugar de nuevo'")
    if st.button("jugar de nuevo"):
        game.state_reset()
        st.rerun()


# Botones para cancelar el inicio del juego
if st.session_state['game_state'] < 1:
    st.subheader('Si aún no se registro, puede hacerlo pulsando en el siguiente botón:')
    if st.button("Registrarse"):
        game.state_reset()
        st.switch_page(nav.to('register'))

    if st.button("Volver a inicio"):
        game.state_reset()
        st.switch_page(nav.to('home'))


if 0 < st.session_state['game_state'] < 6:
    if st.button("Cancelar"):
        game.state_reset()
        st.rerun()