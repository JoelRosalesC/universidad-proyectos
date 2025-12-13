lista = [1, 20, 3, 4, 5, 68, 7, 8, 9, 40]

listaI = []
listaP = []

for i in lista:
    if (i % 2 == 0):
        listaP.append(i)
    else:
        listaI.append(i)

#for i, j in zip(listaI, listaP):
#    print("Numero Impar: ", i, ".Numero par: ", j)
#
#for i in listaI:
#    print("Numero Impar:", i)
#
#for i in listaP:
#    print("Numero par: ", i)