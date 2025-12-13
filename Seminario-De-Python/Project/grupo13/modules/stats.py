import streamlit as st
import matplotlib.pyplot as plt
import plotly.express as px
import pandas as pd
import plotly.graph_objects as go
from datetime import datetime, timedelta

#timedelta: clase de datetime que permite representar diferencias de tiempo

def filter_between_two_dates(df_games,start_date_key='start_date', end_date_key='end_date'):
    """
    Esta función devuelve el dataframe filtrado entre dos fechas ingresadas por 
    el usuario
    """
    start_date = st.date_input('Fecha de inicio',key=start_date_key)
    end_date = st.date_input('Fecha de fin',key=end_date_key)

    if start_date and end_date: # si ya ingreso los datos
        # Convertir los objetos date a datetime
        start_datetime = pd.to_datetime(start_date.strftime('%Y-%m-%d 00:00:00'))
        end_datetime = pd.to_datetime(end_date.strftime('%Y-%m-%d 23:59:59'))
    else:
        st.info("Por favor selecciona un rango de fechas.")

    # Filtrar el DataFrame por el rango de fechas
    return df_games[(df_games['date_time'] >= start_datetime) & (df_games['date_time'] <= end_datetime)]

# 1)
def users_grouped_gender(df_games,df_users):
    """
       Esta función recibe el dataframe de partidas y el de usuarios y genera un gráfico de torta
       mostrando el porcentaje de usuarios por género 

    """

    # Filtro usuarios duplicados, ya que en las partidas puede repetetirse un usuario
    df_unique_users = df_games.drop_duplicates(subset='user_identifier')

    # Merge a partir de los mails que es la clave única
    df_merged = pd.merge(df_unique_users, df_users, left_on='user_identifier',right_on='mail', how='left')
    
  
    gender_counts = df_merged['gender'].value_counts()

    # Defino colores para el gráfico
    colors = ['#ff9999','#66b3ff','#99ff99','#ffcc99']
    
    # Creo el objeto figura (donde va el grafico) y el objeto ejes
    fig, ax = plt.subplots(figsize=(4,4))
    
    # Creo el gráfico de tortas
    gender_counts.plot(kind='pie', autopct='%1.1f%%', colors=colors,ax=ax)

    # Eliminar la etiqueta del eje y
    ax.set_ylabel('')

    # Establecer un título adecuado
    ax.set_title('Usuarios agrupados por género')

    # Ajustar la leyenda para que se coloque fuera del gráfico
    ax.legend(labels=gender_counts.index, loc='upper left', bbox_to_anchor=(1, 0.5))

    st.pyplot(fig)

# 2)
def percentage_scores_above_mean(df_games):
    """
    Función para mostrar un gráfico de torta con el porcentaje de partidas que 
    tienen una puntuación superior a la media (promedio de calificaciones).

    """
    
    mean_score = df_games['score'].mean()

    # Contar cuántas partidas tienen una puntuación superior a la media
    above_mean = df_games[df_games['score'] > mean_score].shape[0]
    below_mean = df_games[df_games['score'] <= mean_score].shape[0]

    
    score_counts = pd.Series({
        'Superiores a la media': above_mean,
        'Inferiores a la media': below_mean
    })

    # Creo el gráfico
    fig, ax = plt.subplots(figsize=(4, 4))
    colors = ['#7FFF00', '#DC143C']
    score_counts.plot(kind='pie', autopct='%1.1f%%', colors=colors, ax=ax, labels=['Superior a la media', 'Inferior a la media'])

    # Eliminar la etiqueta del eje y
    ax.set_ylabel('')

    ax.set_title('Porcentaje de partidas con puntuación superior a la media')
    
    ax.legend(labels=score_counts.index, loc='upper left', bbox_to_anchor=(1, 0.5))

    st.pyplot(fig)

# 3)
def games_per_day(df_games):
    """
    Esta función recibe el dataframe de partidas y genera un gráfico de barras mostrando
    las partidas por día

    """

    # Convertir la columna 'date_time' a objetos datetime
    df_games['date_time'] = pd.to_datetime(df_games['date_time'], format="%Y-%m-%d %H:%M:%S")
    
    # Contar las partidas por día de la semana 
    count_games_day = df_games['date_time'].dt.day_name().value_counts().reindex(
        ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"], fill_value=0
    )

    # Cambiar los nombres de los días de la semana a español
    count_games_day.index = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo']
    
    # Crear el gráfico de barras con Plotly
    fig = px.bar(
        x=count_games_day.index,
        y=count_games_day.values,
        labels={'x': 'Día de la Semana', 'y': 'Cantidad de Partidas'},
        title='Cantidad de Partidas por Día de la Semana'
    )
    
    st.plotly_chart(fig)

# 4)
def average_between_dates(df_games):
    """
    Función que calcula el promedio de respuestas correctas mensuales dentro de un rango de fechas
    ingresadas por el usuario y muestra un gráfico de barras por mes.
    """
    st.markdown("<h3>Promedio de Preguntas Acertadas Mensuales</h3>", unsafe_allow_html=True)
    
    df_filtrado = filter_between_two_dates (df_games,'filter1_start_date','filter1_end_date')

    if not df_filtrado.empty:
        # Calcular el promedio de respuestas correctas por mes
        # Recordatorio : %B es un formato en pandas que devuelve el nombre completo del mes
        df_filtrado['month'] = df_filtrado['date_time'].dt.strftime('%B')  
        df_filtrado['correct_answers_percentage'] = df_filtrado['correct_answers'] / 5 * 100  # Calcula el porcentaje

        average_month = df_filtrado.groupby('month')['correct_answers_percentage'].mean().reset_index()

        # Crear el gráfico 
        fig = px.bar(average_month, x='month', y='correct_answers_percentage',
                        labels={'month': 'Mes', 'correct_answers_percentage': 'Promedio de Respuestas Correctas (%)'},
                        title='Promedio de Preguntas Acertadas Mensuales',
                        category_orders={'month': list(average_month['month'])})  # Ordenar por el orden de los meses

        # Configuro el eje y
        fig.update_yaxes(range=[0, 100], tickvals=list(range(0, 101, 10)))

        
        st.plotly_chart(fig)
    else:
        st.warning("No hay datos disponibles para el rango de fechas seleccionado.")


# 5)
def top_ten(df_games):
    """
    Esta función genera un gráfico con los 10 usuarios con mayor puntaje
    entre dos fechas seleccionadas por el usuario

    """
    st.markdown("<h3>Top 10 de usuarios</h3>", unsafe_allow_html=True)
    st.markdown("<h5>(entre las dos fechas ingresadas)</h5>", unsafe_allow_html=True)

    df_filter = filter_between_two_dates(df_games, 'filter2_start_date', 'filter2_end_date')

    if not df_filter.empty:

        # Agrupar por 'user_identifier' y sumar las 'correct_answers'
        total_score = df_filter.groupby('user_identifier')['score'].sum().reset_index()
        total_score.columns = ['user_identifier', 'total_score']

        # Ordenar descendentemente por 'total_score'
        top_scores = total_score.sort_values('total_score', ascending=False).head(10)

        fig = go.Figure(data=[go.Table(
            header=dict(values=list(top_scores.columns),
                    fill_color='white',  # Fondo blanco para el encabezado
                    line_color='black',  # Color de línea negro para los bordes
                    align='left',
                    font=dict(color='black', size=12)),  # Texto negro en tamaño 12
            cells=dict(values=[top_scores['user_identifier'], top_scores['total_score']],
                fill_color='white',  # Fondo blanco para las celdas
                line_color='black',  # Color de línea negro para los bordes de las celdas
                align='left',
                font=dict(color='black', size=12)))  # Texto negro en tamaño 12
        ])

        
        # Mostrar la tabla
        st.plotly_chart(fig)
    else:
        st.warning("No hay datos disponibles para el rango de fechas seleccionado.")

# 6)
def sorted_by_difficulty(df_games):
    """
    Esta función genera un gráfico de barras mostrando para cada nivel de dificultad
    el porcentaje de errores cometidos por los usuarios en las partidas

    """

    # Filtrar por nivel de dificultad y calcular errores 
    df_filter_high = df_games[df_games['difficulty'] == 'Dificil']
    df_filter_high['mistakes'] = 5 - df_filter_high['correct_answers']

    df_filter_half = df_games[df_games['difficulty'] == 'Medio']
    df_filter_half['mistakes'] = 5 - df_filter_half['correct_answers']

    df_filter_low = df_games[df_games['difficulty'] == 'Fácil']
    df_filter_low['mistakes'] = 5 - df_filter_low['correct_answers']

    # Calcular porcentajes de errores
    total_high = len(df_filter_high)
    total_half = len(df_filter_half)
    total_low = len(df_filter_low)

    percent_high = (df_filter_high['mistakes'].sum() / (total_high * 5)) * 100
    percent_half = (df_filter_half['mistakes'].sum() / (total_half * 5)) * 100
    percent_low = (df_filter_low['mistakes'].sum() / (total_low * 5)) * 100

    # Creo otro dataframe para luego ordenarlo
    df_to_order = pd.DataFrame({
        'Dificultad': ['Dificil', 'Medio', 'Fácil'],
        'Porcentaje de Errores': [percent_high, percent_half, percent_low]
    })

    # Ordenar porcentaje de errores de mayor a menor
    # ascending en false para que se ordene descendentemente
    df_to_order = df_to_order.sort_values(by='Porcentaje de Errores', ascending=False)

    # Crear el gráfico de barras 
    # recordatorio: add trace es para agregar nueva serie de datos
    fig = go.Figure()

    fig.add_trace(go.Bar(
        x=df_to_order['Dificultad'],
        y=df_to_order['Porcentaje de Errores'],
        marker_color=['indianred', 'lightsalmon', 'lightblue'],
        text=df_to_order['Porcentaje de Errores'].apply(lambda x: f'{x:.2f}%'),  # ordeno tambien los texto según el orden de porcentajes
        textposition='auto',
        textfont=dict(size=14),
    ))

    fig.update_layout(
        title='Porcentaje de Errores por Nivel de Dificultad',
        xaxis_title='Dificultad',
        yaxis_title='Porcentaje de Errores (%)',
        yaxis=dict(tickvals=list(range(0, 101, 10))),
        yaxis_range=[0, 100],
    )

    st.plotly_chart(fig)
    

# 7)
def compare_users(df_games):
    """
    Esta función recibe el dataframe games y permite seleccionar dos usuarios para los cuales
    muestra un gráfico de líneas con la evolución de su puntuación en el tiempo.
    """

    st.markdown("<h3>Comparar puntajes de dos usuarios a lo largo del tiempo</h3>", unsafe_allow_html=True)
    
    # Mostrar una lista con los usuarioss
    user_list = df_games['user_identifier'].unique()
    selected_users = st.multiselect('Selecciona dos usuarios para comparar', user_list)
    
    if len(selected_users) != 2:
            st.warning("Por favor, selecciona exactamente dos usuarios.")
            return
    
    # Filtrar datos de juegos por los usuarios seleccionados
    filtered_df = df_games[df_games['user_identifier'].isin(selected_users)]
    
    # Asegurarse de que las fechas están en formato datetime
    filtered_df['date_time'] = pd.to_datetime(filtered_df['date_time'])
    
    
    fig, ax = plt.subplots(figsize=(10, 6))
    
    # Iterar sobre cada usuario seleccionado
    for user in selected_users:
        user_data = filtered_df[filtered_df['user_identifier'] == user]
        ax.plot(user_data['date_time'], user_data['score'], marker='o', label=user)
    
    ax.set_xlabel('Fecha')
    ax.set_ylabel('Puntaje')
    ax.set_title('Evolución del puntaje de usuarios seleccionados')
    ax.legend()

    
    # Guardo fecha minimas y máximas para losm limites del eje x
    min_date = filtered_df['date_time'].min()
    max_date = filtered_df['date_time'].max()
    
    # indico el rango de frecuencia como mensual 'MS'
    date_range = pd.date_range(start=min_date, end=max_date, freq='MS')
    ax.set_xticks(date_range)
    
    # Itero sobre cada fecha para mostrar el nombre abreviado del mes junto con el año
    ax.set_xticklabels([date.strftime('%b\n%Y') for date in date_range])
    
    # Mostrar el gráfico en Streamlit
    st.pyplot(fig)

# 8)
def filter_max_by_gender(df_games, df_users):
    
    """
    Esta función recibe el dataframe de las partidas y genera un gráfico de tortas mostrando
    para cada género la temática en la cuál tiene mayor conocimiento

    """

    
    st.markdown("<h3>Temática con mayor puntaje para cada género</h3>", unsafe_allow_html=True)

    # Merge para obtener el genero de cada usuario
    df_merged = pd.merge(df_games, df_users, left_on='user_identifier', right_on='mail', how='left')
    
    # Filtro por género
    df_male = df_merged[df_merged['gender'] == 'Masculino']
    df_female = df_merged[df_merged['gender'] == 'Femenino']
    df_x = df_merged[df_merged['gender'] == 'Prefiero no decirlo']
    
    # Sumar puntos por género
    male_scores = df_male.groupby('theme')['score'].sum()
    female_scores = df_female.groupby('theme')['score'].sum()
    x_scores = df_x.groupby('theme')['score'].sum()
    
    # Tema con mayor puntuación para cada género
    max_male_theme = male_scores.idxmax() if not male_scores.empty else None
    max_female_theme = female_scores.idxmax() if not female_scores.empty else None
    max_x_theme = x_scores.idxmax() if not x_scores.empty else None

    # Puntajes máximos para cada género
    max_male_score = male_scores.max() if not male_scores.empty else 0
    max_female_score = female_scores.max() if not female_scores.empty else 0
    max_x_score = x_scores.max() if not x_scores.empty else 0

    # Crear un diccionario con los resultados
    result = {
        'Masculino': {'theme': max_male_theme, 'score': max_male_score},
        'Femenino': {'theme': max_female_theme, 'score': max_female_score},
        'Prefiero no decirlo': {'theme': max_x_theme, 'score': max_x_score}
    }

    # Crear el 'fondo para el gráfico'
    fig, ax = plt.subplots(figsize=(6, 6))
    
    # Datos para el gráfico de torta
    # construyo string tema con el genero y el tema, y verifico si no es vacia
    labels = [f"{gender} ({result[gender]['theme']})" if result[gender]['theme'] else gender for gender in result.keys()]
    sizes = [result[gender]['score'] for gender in result.keys()]
    colors = ['skyblue', 'lightgreen', 'lightcoral']

    # gráfico de torta
    ax.pie(sizes, labels=labels, autopct='%1.1f%%', colors=colors)
    
   
    ax.set_title('Puntuaciones máximas por tema y género')

    st.pyplot(fig)


# 9)
def average_by_difficulty(df_games):
    """
    Esta función recibe el dataframe de las partidas y genera una tabla donde muestra 
    para cada dificultad, la cantidad de veces que fue elegida y el promedio de puntos

    """

    st.markdown("<h3>Elecciones de dificultad y promedio de puntaje</h3>", unsafe_allow_html=True)

    # Filtrar los datos por dificultad
    df_high = df_games[df_games['difficulty'] == 'Alta']
    df_medium = df_games[df_games['difficulty'] == 'Medio']
    df_low = df_games[df_games['difficulty'] == 'Fácil']
    
    # Calcular puntaje promedio y cantidad de veces elegida para cada dificultad
    avg_high_score = df_high['score'].mean() if not df_high.empty else 0
    count_high = df_high.shape[0]
    
    avg_medium_score = df_medium['score'].mean() if not df_medium.empty else 0
    count_medium = df_medium.shape[0]
    
    avg_low_score = df_low['score'].mean() if not df_low.empty else 0
    count_low = df_low.shape[0]
    
    # Crear un DataFrame con los resultados
    result = pd.DataFrame({
        'Dificultad': ['Alta', 'Medio', 'Fácil'],
        'Puntaje Promedio': [avg_high_score, avg_medium_score, avg_low_score],
        'Cantidad de Veces Elegida': [count_high, count_medium, count_low]
    })
    
    
    fig = go.Figure(data=[go.Table(
        header=dict(values=list(result.columns),
                    fill_color='black',
                    align='left'),
        cells=dict(values=[result.Dificultad, result['Puntaje Promedio'], result['Cantidad de Veces Elegida']],
                   fill_color='black',
                   align='left'))
    ])
    
   
    
    st.plotly_chart(fig)

# 10)
def on_strake(df_games):
    """
    Esta función recibe el dataframe con las partidas y genera una lista con los usuarios que
    sumaron mas de 0 puntos todos los días, durante los últimos 7 días.

    """
    st.markdown("<h3>Usuarios en racha</h3>", unsafe_allow_html=True)
    st.markdown("<h4>(Una partida con puntaje mayor a 0 cada día en los últimos 7 días)</h4>", unsafe_allow_html=True)

    # Convierto la columna date_time a tipo datetime para el filtrado
    df_games['date_time'] = pd.to_datetime(df_games['date_time']).dt.date

    current_date = datetime.now().date()

    # Obtengo los últimos 7 días
    seven_days_ago = current_date - timedelta(days=7)
    df_last_7_days = df_games[df_games['date_time'] >= seven_days_ago]

    

    # agg me permite multiples operaciones a datos agrupados
    df_user_days_played = df_last_7_days.groupby('user_identifier').agg({
        'date_time': 'nunique', # Cuento el num de fechas únicas
        'score': lambda x: (x > 0).all()
    }).reset_index() #convierto user_identifier en un índice normal

    # Filtrar usuarios que han jugado exactamente 7 días y todos sus puntajes son mayores a 0
    users_on_streak = df_user_days_played[
        (df_user_days_played['date_time'] == 7) & (df_user_days_played['score'])
    ]['user_identifier'].tolist()

    if users_on_streak:
        st.write("Usuarios en racha:")
        st.write(users_on_streak)

    else:
        st.write("No hay usuarios en racha :(")
