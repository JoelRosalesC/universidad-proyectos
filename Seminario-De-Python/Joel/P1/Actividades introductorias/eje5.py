lista = []

while True:
    num = int(input("Ingrese un numero: (0 para detener)"))
    if (num == 0):
        break
    lista.append(num)

for i in lista:
    if (i < 0):
        break
    print("Numero: ", i)