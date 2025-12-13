# PyTrivia

Proyecto académico desarrollado para la materia **Seminario de Python**.
La aplicación procesa distintos datasets y ofrece una interfaz web interactiva construida con **Streamlit**.

---

## 📌 Requisitos

* **Python 3.11.x** (recomendado)
* **Git**
* Sistema operativo: Windows o Linux

> ⚠️ Importante: versiones distintas de Python pueden generar errores al instalar dependencias.

---

## 📥 Descargar el repositorio

Abra una terminal y ejecute:

```bash
git clone https://gitlab.catedras.linti.unlp.edu.ar/python2024/code/grupo13
cd grupo13
```

---

## 🧪 Entorno virtual

### Crear el entorno virtual

```bash
python -m venv venv
```

### Activar el entorno virtual

**Windows**

```bash
venv\Scripts\activate ó source venv/Scripts/activate
```

**Linux / macOS**

```bash
source venv/bin/activate
```

### Actualizar herramientas base

```bash
python -m pip install --upgrade pip setuptools wheel
```

### Instalar dependencias

```bash
pip install -r requirements.txt
```

Si se producen errores al instalar dependencias (especialmente en Windows), puede utilizar el archivo alternativo sin versiones fijas:

```bash
pip install -r requirements_alt.txt
```

bash
pip install -r requirements.txt

````

---

## ⚙️ Procesamiento de datasets

> Estos notebooks deben ejecutarse **antes** de realizar consultas.

Ejecutar en el siguiente orden:

```text
jupyter_notebooks/Procesamiento/datasets_aeropuertos/inciso_1.ipynb
jupyter_notebooks/Procesamiento/datasets_conectividad/inciso_2.ipynb
jupyter_notebooks/Procesamiento/datasets_censo22/inciso_3.ipynb
jupyter_notebooks/Procesamiento/datasets_lagos/inciso_4.ipynb
````

Se recomienda abrirlos y ejecutarlos usando **VS Code** o **Jupyter Lab**.

---

## 🔍 Consultas

Una vez procesados los datasets, ejecutar:

```text
jupyter_notebooks/Consultas/consultas_1_6_7_8.ipynb
jupyter_notebooks/Consultas/consultas_2_3_4_5.ipynb
jupyter_notebooks/Consultas/consultas_9_10_11_12.ipynb
```

---

## 🌐 Ejecutar la aplicación web

Desde la raíz del proyecto:

```bash
streamlit run app/pytrivia.py
```

La aplicación se abrirá automáticamente en el navegador.

---

## 👥 Autores

* Murray Roppel, Andrés
* Rosales, Joel
* Agüero Díaz, Alejandro Víctor Manuel
