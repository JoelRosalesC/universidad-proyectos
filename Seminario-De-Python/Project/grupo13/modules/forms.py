import streamlit as st
import re
from datetime import datetime, timedelta


def check_mail(mail):
    # Expresión regular para validar el formato de correo electrónico
    pattern = r"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"
    return re.match(pattern, mail)

def user_register():
    """Esta función permite crear un usuario, ingresando los datos correspondientes"""
    st.header("Registro de usuario")

    # Definir fechas mínima y máxima permitidas
    max_date = datetime.now().date() - timedelta(days=1)  # Evitar seleccionar fecha futura
    min_date = max_date - timedelta(days=365 * 100)  # Permitir fechas hasta 100 años atrás

    # Valor predeterminado de fecha de nacimiento
    default_birthday = max_date - timedelta(days=365 * 25)  # Por ejemplo, 25 años atrás
    
    # Crear un formulario con la opción de limpiar los campos al enviarlo
    with st.form("Registro de usuario"):
        name = st.text_input("Nombre de usuario")
        full_name = st.text_input("Nombre completo")
        mail = st.text_input("Correo electrónico")
        
        # Configurar fecha de nacimiento con mínimo y máximo permitido
        birthday = st.date_input("Fecha de nacimiento", 
                                 min_value=min_date, 
                                 max_value=max_date, 
                                 value=default_birthday)
        
        gender = st.selectbox("Género", ["Masculino", "Femenino", "Prefiero no decirlo"])
        submit_button = st.form_submit_button("Registrarse")

        if submit_button:
            if not check_mail(mail):
                st.error("El correo electrónico ingresado no es válido. Por favor, introduce un correo válido.")
            elif not all([name, full_name, mail, birthday, gender]):
                st.error("Todos los campos son obligatorios. Por favor, completa el formulario.")
            else:
                st.success("¡Usuario registrado correctamente!")
                user = {
                    'name': name, 
                    'full_name': full_name,
                    'mail': mail,
                    'birthday': birthday.isoformat(),
                    'gender': gender
                }
                return user

# Ejecutar la función en Streamlit
if __name__ == "__main__":
    user_register()