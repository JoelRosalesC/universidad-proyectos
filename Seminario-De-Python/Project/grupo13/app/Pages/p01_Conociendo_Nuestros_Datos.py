import streamlit as st
import plotly.graph_objects as go
import sys
from pathlib import Path
import navigate as nav
from streamlit_folium import st_folium

sys.path.append(str(Path('.').resolve()))
from modules import game

#set titulos
st.set_page_config(page_title='Conociendo Nuestros Datos')
st.title("Conociendo Nuestros Datos")

st.divider()

#session_state
if 'selected_data' not in st.session_state:
    st.session_state.selected_data = 'Aeropuertos' #valor por defecto

#columas para botones
col1, col2 = st.columns(2)

#botones
with col1:
    if (st.button('Aeropuertos')):
        st.session_state.selected_data = 'Aeropuertos'
with col2:
    if (st.button('Lagos')):
        st.session_state.selected_data = 'Lagos'

#mostrar los datos segun el boton que se oprime
if st.session_state.selected_data == 'Aeropuertos': #Datos sobre aeropuertos

    st.subheader("Aeropuertos")

    df_airports = game.load_airport_data() #obtiene el dataframe de aeropuertos

    if df_airports is not None:

        #Visor de opciones
        st.session_state.view = st.radio(
            "",
            options=('Mapa', 'Tabla', 'Torta', 'Gráfico')
        )

        if (st.session_state.view == 'Mapa'):
            
            st.subheader("Aeropuertos en argentina:")
            
            st.markdown("""
                <h4>En el siguiente mapa se muestran algunos aeropuertos de Argentina:</h4>
                <h5 style='margin-top: 10px;'>Según su tipo de elevación:</h5>
                <ul>
                    <li style='color: red;'>Rojo: </li> Elevación alta
                    <li style='color: blue;'>Azul: </li> Elevación media
                    <li style='color: green;'>Verde: </li> Elevación baja
                </ul>
            """, unsafe_allow_html=True)


            airport_map = game.generate_map()

            # Aplica la función add_marker_airport_map de game a cada fila del DataFrame df_airports para agregar marcadores al mapa airport_map
            df_airports.apply(lambda row: game.add_marker_airport_map(row, airport_map), axis=1)

            airport_map = game.add_fullscreen(airport_map)

            show_airport_map = airport_map.get_root().render()  # Genera el mapa utilizando Folium y lo renderiza

            st.components.v1.html(show_airport_map, width=800, height=600)  # Muestra el mapa en Streamlit
        
        elif (st.session_state.view == 'Torta'):

            elevation = df_airports.elevation_name.value_counts()   #obtengo los valores de cada tipo de elevacion (alto, medio, bajo)

            fig = go.Figure(data=[go.Pie(labels=elevation.index,    #se crea el grafico. Labels: etiquetas a mostrar (alto,medio,bajo)
                                        values=elevation,          #values: cantidad de cada etiqueta
                                        hole=0.2,                  
                                        marker=dict(colors=['orange', 'blue', 'green', 'red']))]) 

            fig.update_layout(title="Elevacion de los Aeropuertos...")

            st.plotly_chart(fig)

        elif (st.session_state.view == 'Tabla'):

            st.subheader("Aeropuertos ordenados segun su score")

            df_airports_sorted = df_airports.sort_values(by='score', ascending=False) #ordeno df_airports segun la columna score en orden descendente

            df_airports_sorted = df_airports_sorted[['Nombre', 'Region', 'Elevación', 'IATA Code', 'LOCAL Code', 'score']] # obtengo en df_airports_sorted informacion de algunas columnas

            st.dataframe(df_airports_sorted.reset_index(drop=True), height=600) 

        else:

            #se cambian algunos nombres
            df_airports.prov_name.replace({
                'Buenos Aires, Ciudad Autónoma de': 'CABA',
                'Tierra del Fuego, Antártida e Islas del Atlántico Sur': 'Tierra del Fuego'
            }, inplace=True)

            provinces = df_airports['Region'].value_counts(ascending=True) # Calcula el conteo segun la columna "Region" y ordena ascendentemente

            fig = go.Figure(data=go.Bar(x=provinces.index, y=provinces,     #se crea grafico de barra
                                        text=provinces.index,
                                        hoverinfo='x+y'))

            fig.update_layout(title="Cant. Aeropuertos por Provincias",
                            xaxis_title="Provincias",
                            yaxis_title="Cant. Aeropuertos",
                            height=700,
                            width=900)
            
            st.plotly_chart(fig)

elif (st.session_state.selected_data == 'Lagos'): #Datos sobre Lagos
    
    st.subheader("Lagos")

    df_lake = game.load_lake_data() #obtiene el dataframe de Lagos

    if df_lake is not None:
        # visor de opciones
        st.session_state.view = st.radio(
            "",
            options=('Mapa', 'Tabla', 'Gráfico')
        )

        if (st.session_state.view == 'Mapa'):

            st.subheader("Algunos Lagos en argentina:")

            st.markdown("""
                <h4>En el siguiente mapa se muestran algunos lagos en Argentina:</h4>
                <h5 style='margin-top: 10px;'>Según su tamaño:</h5>
                <ul>
                    <li style='color: red;'>Rojo: </li> Grande
                    <li style='color: blue;'>Azul: </li> Medio
                    <li style='color: green;'>Verde: </li> Chico
                </ul>
            """, unsafe_allow_html=True)

            lake_map = game.generate_map()  

            # Aplica la función add_marker_lake_map de game a cada fila del DataFrame df_lake para agregar marcadores al mapa lake_map
            df_lake.apply(lambda row: game.add_marker_lake_map(row, lake_map), axis=1) 

            lake_map = game.add_fullscreen(lake_map)

            lake_map = lake_map.get_root().render()  # Genera el mapa utilizando Folium y lo renderiza

            st.components.v1.html(lake_map, width=800, height=600) # Muestra el mapa en Streamlit
        
        elif (st.session_state.view == 'Tabla'):

            st.subheader("Tabla de datos sobre Lagos")
            st.write("Superficie, Profundides medias y Profundidades máximas de algunos Lagos:")

            #se limpian valores nulos si ambas columnas (maxima y media) lo contienen.
            df_lakes_cleaned = df_lake.dropna(subset=['Profundidad máxima (m)', 'Profundidad media (m)'], how='all')    

            # se guarda en df_lakes_cleaned informacion de algunas columnas en especifico
            df_lakes_cleaned  = df_lakes_cleaned[['Nombre', 'Ubicación', 'Superficie (km²)', 'Profundidad máxima (m)', 'Profundidad media (m)', 'Sup Tamaño']]

            st.dataframe(df_lakes_cleaned.reset_index(drop=True), height=500)

        else:

            size_values = df_lake['Sup Tamaño'].value_counts(ascending=True) #obtengo los valores segun la columna 'Sup Tamaño' (grande, medio, chico) y sus cantidades
            
            fig = go.Figure(data=go.Bar(x=size_values.index, y=size_values,    #se crea grafico de barra
                            marker=dict(color = 'DarkSlateGrey'),
                            text=size_values.index,
                            hoverinfo='x+y'))

            fig.update_layout(title="Cantidad de Lagos según su tamaño",
                            xaxis_title="Tamaños",
                            yaxis_title="Cant. Lagos",
                            height=600,
                            width=700)
            
            st.plotly_chart(fig)

nav.del_questions_and_answers()

if st.button("Volver a inicio"):
    st.switch_page(nav.to('home'))
