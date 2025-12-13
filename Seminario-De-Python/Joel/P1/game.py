import random
# Lista de palabras posibles
words = ["python", "programación", "computadora", "código", "desarrollo",
"inteligencia"]
# Elegir una palabra al azar
secret_word = random.choice(words)
# Número máximo de fallos permitidos
max_f = 10
# Lista para almacenar las letras adivinadas
guessed_letters = []

def esvocal(letra):
    vocales = "aeiou"
    return letra in vocales

def facil():
    word = ""
    for letra in secret_word:
        if (esvocal(letra)):
            word += letra
        else:
            word += "_"
    return word

def medio():
    return secret_word[0]+ "_" * (len(secret_word) - 2) + secret_word[-1]

def dificil():
    return "_" * len(secret_word)

print("¡Bienvenido al juego de adivinanzas!")

print(""" 
--Seleccione una Dificultad--
Facil: Se muestran una vez todas las vocales.
Medio: Se muestra una vez la primera y la ultima letra de la palabra.
Dificil: No se muestra ninguna letra.
    """)

#seleccion de dificultad
while True:
    dificultad = input("..: ").lower()
    if (dificultad == "facil"):
        word_displayed = facil()
        break
    elif (dificultad == "medio"):
        word_displayed = medio()
        break
    elif (dificultad == "dificil"):
        word_displayed = dificil()
        break
    else:
        print("Opcion invalida. Intenta de nuevo")
    

print("Estoy pensando en una palabra. ¿Puedes adivinar cuál es?")

# Mostrarla palabra parcialmente adivinada
print(f"Palabra: {word_displayed}")
#contador
i = 0   
while (i < max_f):
 # Pedir al jugador que ingrese una letra
    letter = input("Ingresa una letra: ").lower()

 #verificar que se ingreso una letra 
    if (len(letter)!= 1 or not letter.isalpha()):
        print("Letra no valida. Intenta de nuevo.")
        i+=1
        continue

 # Verificar si la letra ya ha sido adivinada
    if letter in guessed_letters:
        print("Ya has intentado con esa letra. Intenta con otra.")
        i+= 1
        continue
 # Agregar la letra a la lista de letras adivinadas
    guessed_letters.append(letter)
 # Verificar si la letra está en la palabra secreta
    if letter in secret_word:
        print("¡Bien hecho! La letra está en la palabra.")
    else:
        print("Lo siento, la letra no está en la palabra.")
        i += 1
 # Mostrar la palabra parcialmente adivinada
    letters = []
    for letter in secret_word:
        if letter in guessed_letters:
            letters.append(letter)
        else:
            letters.append("_")
    word_displayed = "".join(letters)
    print(f"Palabra: {word_displayed}")
 # Verificar si se ha adivinado la palabra completa
    if word_displayed == secret_word:
        print(f"¡Felicidades! Has adivinado la palabra secreta: {secret_word}")
        break
else:
    print(f"¡Oh no! Has alcanzado los {max_f} fallos permitidos.")
    print(f"La palabra secreta era: {secret_word}")