if __name__ == '__main__':
    print("No main method available")

def surface_size(surface):
    '''Funcion que recibe la superficie en km² y devuelve si es chico, medio o grande'''
    if isinstance(surface, float) and surface >= 0:
        if surface <= 17: return "chico"
        elif surface <= 59: return "medio"
        else: return "grande"
    else: 
        raise f"Wrong type of parameters, expected float, got {isinstance(surface)}"
    
def latitude_longitude_split(data_str):
    """Ingresa el dato que se lee del csv como string y devuelve la latitud y la longitud por separadas"""
    if isinstance(data_str, str):
        data_str = data_str.replace('"', '-').replace('°', '-').replace("'", "-")
        data_str = data_str.split()
        lat = data_str[0].replace("-", " ").split()
        lon = data_str[1].replace("-", " ").split()
        return lat, lon
    else:
        raise f"Wrong type of value, expected str, got {isinstance(data_str)}"
    
def degrees_to_decimal(lat, lon):
    """Funcion que recibe latitud y longitud en grados minutos y segundos y devuelve en grados decimales"""
    # Multiplica por -1 si es sur u oeste
    minus = ["S", "O"]
    # Lista para guardar los valores
    decimals = []
    # Ecuación de latitud, al final multiplica por -1 o por 1, en argentina son todas sur y oeste pero bueno
    latitude = round(float(lat[0]) + (float(lat[1]) / 60.0) + (float(lat[2]) / 3600.0), 10) * (-1 if lat[-1].upper() in minus else 1)
    longitude = round(float(lon[0]) + (float(lon[1]) / 60.0) + (float(lon[2]) / 3600.0), 10) * (-1 if lon[-1].upper() in minus else 1)
    # Se agrega a la lista y se devuelve
    decimals.append(latitude)
    decimals.append(longitude)
    return decimals

def change_row(row):
    """Recibe un diccionario, Llama a la función split de latitud y longitud para despues pasarla a grados decimales"""
    if isinstance(row, dict):
        new_row = {}
        for key, value in row.items():
            if key == 'Coordenadas':
                coordinates = row['Coordenadas']
                lat, lon = latitude_longitude_split(coordinates)
                latitude, longitude = degrees_to_decimal(lat, lon)
                new_row["Latitud"] = f"{latitude:.6f}"
                new_row["Longitud"] = f"{longitude:.6f}"
            else: 
                new_row[key] = value

        return new_row
    else: 
        raise f"Wront type of parameters, expected dict, got {isinstance(row)}"
