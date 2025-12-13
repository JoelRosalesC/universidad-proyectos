# PyTrivia

# Índice
<br>

-[Requisitos](#requisitos)
-[Descarga](#descargar-el-repositorio)
-[Guía](#guía-de-uso)
    -[Entorno-Virtual](#crear-el-entorno-virtual)
    -[Procesamiento](#para-procesar-los-datasets)
    -[Consulta](#para-consultar-los-datasets)
    -[Página](#ejecutar-la-página)
-[Créditos](#autores)

</br>

## Requisitos
Para poder ejecutar el programa deberá tener instalado alguna version de Python 3.11.
En caso de no tener la versión de Python o Python instalado puede descargarla en este [link](https://www.python.org/downloads/)

## Descargar el repositorio
- Abra la terminal o la consola, luego diríjase a la carpeta donde desea descargar la aplicación y ejecute:
- ```git init ```

- ```git clone https://gitlab.catedras.linti.unlp.edu.ar/python2024/code/grupo13 ```

- En caso de no tener la herramienta git instalada puede descargarla de el siguiente [link](https://gitforwindows.org/)

# Guía de uso
Una vez descargado el repositorio, debera ejecutar el siguiente comando:
```cd grupo13 ```
una vez dentro de la carpeta grupo13, debera crear un entorno virtual como se indica a continuación.


### Crear el entorno virtual
- Para crear el entorno virtual esciba en la terminal o consola:
- ```python -m venv pytrivia```
- Una vez creado el entorno virtual, activarlo 
- Para Windows: ```source pytrivia/Scripts/activate ```
- Para Linux:   ```source pytrivia/bin/activate ```

Una vez este dentro del entorno virtual instalar los requerimentos 
- ```pip install -r requirements.txt ```

### Para procesar los datasets
- Ejecute los siguientes programas en jupyter notebook
- ```jupyter_notebooks\Procesamiento\datasets_aeropuertos\inciso_1.ipynb```
- ```jupyter_notebooks\Procesamiento\datasets_conectividad\inciso_2.ipynb```
- ```jupyter_notebooks\Procesamiento\datasets_censo22\inciso_3.ipynb```
- ```jupyter_notebooks\Procesamiento\datasets_lagos\inciso_4.ipynb```

### Para consultar los datasets
- Ejecute los siguientes programas en jupyter notebook ("Las consultas deben ejecutarse despues de procesar los datasets")
- ```jupyter_notebooks\Consultas\consultas_1_6_7_8.ipynb```
- ```jupyter_notebooks\Consultas\consultas_2_3_4_5.ipynb```
- ```jupyter_notebooks\Consultas\consultas_9_10_11_12.ipynb```

### Ejecutar la página 
Primero debera ejecutar el siguiente comando en la terminal/consola: 
(Asumimos que se encuentra ubicado en la carpeta raiz e instalo anteriormente) ``` requirements.txt ```

```streamlit run app/pytrivia.py```

Si ejecuto los pasos correctamente debería abrirse la aplicación en el navegador. A jugar!

# Autores:
<br>

- Murray Roppel, Andrés.
- Rosales, Joel.
- Agüero Díaz, Alejandro Víctor Manuel.

</br>