lista = []

for i in range(10):
    lista.append(int(input("Agregar numero: ")))


cadena = ""
for num in lista:
    if (num % 3 != 0):
        cadena += str(num) + "-"

print(cadena)
