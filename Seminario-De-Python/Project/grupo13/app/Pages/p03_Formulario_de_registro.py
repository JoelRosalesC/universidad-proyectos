

def show_register():
    """Funcion para la pagina de registro"""
    import streamlit as st
    import sys
    from pathlib import Path
    import json

    sys.path.append(str(Path('.').resolve()))
    from modules.forms import user_register
    from modules.paths import USER_DATA_JSON

    import navigate as nav

    nav.del_questions_and_answers()

    st.set_page_config(page_title="Registro")
    st.title("Formulario de registro")
    st.subheader("Ingrese sus datos:")

    user_form = user_register()
    if st.button("Volver a inicio"):
        st.switch_page(nav.to('home'))
    # Si el archivo no existe, lo creo
    if not USER_DATA_JSON.is_file(): 
        with open(USER_DATA_JSON, mode="w")as user_file_json:
            json.dump([],user_file_json)


    if user_form is None:
        st.write('Por favor, complete todos los campos')
    else:
        # Leo mi archivo de usuarios
        with open(USER_DATA_JSON, mode="r")as user_file_json:
                users = []
                try:
                    users = json.load(user_file_json)  # Guardo la lista de usuarios si el archivo no está vacío        
                except json.decoder.JSONDecodeError:
                    # Si el archivo JSON está vacío o no contiene datos válidos, users seguirá siendo una lista vacía
                    pass

        actualizar=False
        if users:
            for i,user in enumerate(users):
                if user['mail']==user_form['mail']:
                    users[i]=user_form # Actualizo todos los datos del usuario
                    actualizar=True
                    st.write('Usuario actualizado con éxito')
                    break
        if not actualizar: # Si el usuario no fue actualizado lo agrego 
            users.append(user_form)
            st.write('Usuario creado exitosamente')
                        
        # Escribo el archivo Json con el usuario actualizado o el nuevo usuario 
        with open(USER_DATA_JSON,mode='w')as user_file_json:
            json.dump(users,user_file_json,indent=4)
        

if __name__ == "__main__":
    show_register()