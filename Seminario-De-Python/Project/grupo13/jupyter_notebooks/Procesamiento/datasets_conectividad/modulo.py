

def get_keys():
    """Devuelve una lista de claves que representan los tipos de conectividad."""
    return ['ADSL', 'CABLEMODEM', 'DIALUP', 'FIBRAOPTICA', 'SATELITAL', 'WIRELESS', 'TELEFONIAFIJA', '3G', '4G']


def review(line):
    """Evalua una linea del dataset para verificar si posee conectividad, si es asi retornara True.
       Evalua en 'line' y modifica '-' por la palabra 'NO'.

    Args:
        line (dict): Recibe una linea del dataset.

    Returns:
        Boolean: Si posee alguna conectividad retornara True.
    """
    keys = get_keys()
    has_connectivity = False
    for key in keys:

        if (not has_connectivity and line[key] == 'SI'):
            has_connectivity = True

        if ('-' in line[key]):
            line[key] = 'NO'
        
    return has_connectivity


def writing_has_conectivity(has_connectivity, line):
    """Escribe en el encabezado Posee_Conectividad 'SI', si alguna de 
       las claves de tipos de conectividad contiene un 'SI', caso contrario escribe un 'NO'.

    Args:
        has_connectivity (bool): Indica si tiene o no alguno de los distintos tipos de conectividad.
        line (dict): Recibe una linea del dataset.
    """
    if (has_connectivity):
        line['Posee_Conectividad'] = 'SI'
    else:
        line['Posee_Conectividad'] = 'NO'