import streamlit as st
import sys
from pathlib import Path
import importlib.util
import navigate as nav
# Para moverse entre paginas = st.switch_page(nav.to(x)) x = [data, play, register, ranking, stats, home]

# Título y subtítulos
st.title("¡Bienvenido a PyTrivia!")
st.subheader("PyTrivia es un juego de trivia multiple-choice acerca de temas como: aeropuertos, censos, lagos y conectividad en la Argentina.")

# Descripción del juego
with st.expander("Descripción del juego"):
    st.write(
        """
        Se le presentarán 5 preguntas, cada una con 3 opciones incorrectas y una correcta.
        En caso de acertar, se le sumarán puntos que lo podrían posicionar en el top del ranking.
        """
    )

# Tutorial
with st.expander("¿Cómo jugar?"):
    st.markdown(
        """
        Para **comenzar a jugar**, primero deberá **registrarse**. Puede hacerlo haciendo clic en el botón 'Registrarse' o a través de la barra lateral dirigiéndose
        a la sección 'Formulario de Registro'.
        Una vez registrado, deberá ir a la sección de **'Juego'**, elegir su usuario, la dificultad y el tema.
        """
    )

# Dificultades
with st.expander("Dificultades del juego"):
    st.success("**Fácil**: El jugador obtendrá dos comodines de cada tipo. Cada respuesta correcta suma 100 puntos")
    st.warning("**Media**: El jugador obtendrá un comodín de cada tipo. Cada respuesta correcta suma 150 puntos")
    st.error("**Alta**: No se le otorgan comodines al jugador. Cada respuesta correcta suma 200 puntos")

# Explicación de comodines
with st.expander("¿Cómo funcionan los comodines?"):
    st.markdown(
        """
        - **Comodín 50/50**: Permite descartar 2 de las 4 opciones
        - **Comodín descarte de opción**: permite descartar una de las opciones

        ##### ¡Atención!
        Tenga en cuenta que, la cantidad de comodines es para toda la partida, es decir, si por ejemplo
        jugando en dificultad media utiliza todos los comodines en la primera pregunta, luego no tendrá comodines
        para el resto de la partida.
        """
    )

nav.del_questions_and_answers()
    

pages = ["data", "play", "register", "ranking", "stats", "home"]
page_name = ["Conociendo nuestros datos", "Juego", "Formulario de registro", "Ranking", "Estadísticas"]

with st.form("Páginas",clear_on_submit=True):

        selected_page = st.selectbox("A que página desea ir?", page_name)
        submit_button = st.form_submit_button("ir")

if submit_button:

    # Indice de la pagina seleccionada 
    index = page_name.index(selected_page)
    # Ir a la pagina a traves del indice seleccionado recién
    st.switch_page(nav.to(pages[index]))

# Para moverse entre paginas = st.switch_page(nav.to(x)) x = [data, play, register, ranking, stats, home]
    


