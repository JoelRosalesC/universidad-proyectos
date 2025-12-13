

def goleador_del_equipo (dic):
    """Esta funcion se encarga buscar en el diccionario al jugador o jugadora
        con más goles del equipo.

    Args:
        dic (dict): Recibe el diccionario que contiene todos los datos.

    Returns:
        dict: Retorna un diccionario del goleador del equipo.
    """
    max_goles = -1
    goleador = {}
    for name, datos in dic.items():
        if datos['goles'] > max_goles:
            max_goles = datos['goles']
            goleador = {
                'Nombre': name,
                'Goles': datos['goles'],
                'Goles evitados': datos['goles evitados'],
                'Asistencias': datos['asistencias']
            }
    
    return goleador


def promedio_partido(goles, partidos):
    """Esta funcion suma todos los goles realizados a lo largo de la temporada
        y calcula el promedio de goles por partido.

    Args:
        goles (list): Recibe la lista de goles totales.
        partidos (int): Recibe la cantidad total de partidos jugados en la temporada.

    Returns:
        float: Retorna el promedio de goles por partido a lo largo de la temporada.
    """
    return sum(goles) / partidos


def promedio_player(goleador, partidos):
    """Esta funcion toma los goles anotados por el goleador y calcula el promedio
        de goles por partidos jugados a lo largo de la temporada.

    Args:
        goleador (dict): Recibe los datos del goleador del equipo.
        partidos (int): Recibe la cantidad total de partidos jugados.

    Returns:
        float: Retorna el promedio de goles por partido del goleador.
    """
    return goleador['Goles'] / partidos