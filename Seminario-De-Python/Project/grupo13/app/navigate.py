if __name__ == "__main__":
    print("no main method available")


def to(page):
    '''Recibe el str de la pagina a la que hay que ir y devuelve el path
    [data, play, register, ranking, stats, home]
    '''
    #'data'="p01_Conociendo_Nuestros_Datos.py"
    #'play'="Pages/p02_Juego.py"
    #'register'="Pages/p03_Formulario_de_registro.py"
    #'ranking'="Pages/p05_Ranking.py"
    #'stats'="Pages/Estadisticas.py" 

    match page:
        case "data": 
            return "Pages/p01_Conociendo_Nuestros_Datos.py"
        case "play": 
            return "Pages/p02_Juego.py"
        case "register":
            return "Pages/p03_Formulario_de_registro.py"
        case "ranking":
            return "Pages/p05_Ranking.py"
        case "stats":
            return "Pages/p06_Estadisticas.py"
        case "home":
            return "pytrivia.py"
        case _:
            return "error"

        
def del_questions_and_answers():
    import streamlit as st
    
    if "game_state" in st.session_state:
        if st.session_state["game_state"] == 6:
            if "questions_and_answers" in st.session_state:
                st.session_state.pop("questions_and_answers", None)      