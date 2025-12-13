import csv

def value_category(elevation):
    """Esta función recibe un parametro con la elevacion de un aeropuerto
    y devuelve una clasificación cualitativa del mismo: alto,medio o bajo.
    En caso de que el aeropuerto no tenga ese dato devuelve N/A"""
    
    if elevation =='':
        return 'N/A'
    elif int(elevation)<=131:
        return 'bajo'
    elif int(elevation)>131 and int(elevation)<=903:
        return'medio'
    else:
        return 'alto'
    

def build_dic_prov(ARDATA):
    """Esta función recibe el csv ARDATA y devuelve una lista de diccionarios
       donde para cada ciudad se encuentra asociada su provincia"""
    dic_prov_name=[]
    with ARDATA.open(mode='r',encoding='utf-8') as ar_file:
        reader=csv.reader(ar_file)
        next(reader)# salteo el encabezado
        for row in reader:
            dic_prov_name.append({'City':row[0],'Prov':row[5]})
    return dic_prov_name
     
    
def remove_accents(word):
    accents = {'á': 'a', 'é': 'e', 'í': 'i', 'ó': 'o', 'ú': 'u', 'Á': 'A', 
               'É': 'E', 'Í': 'I', 'Ó': 'O', 'Ú': 'U'}

    for accented_char, non_accented_char in accents.items():
        word = word.replace(accented_char, non_accented_char)
    
    return word