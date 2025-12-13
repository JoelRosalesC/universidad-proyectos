# Alquiler de Vehiculos Api

## Comandos

- Para ejecutar el proyecto debe tener instalado .NET 9. https://dotnet.microsoft.com/es-es/download
- Para visualizar la base de datos sqlite utilizar DB Browser for SQLite. https://sqlitebrowser.org/dl/


### Ejecutar

Para levantar el entorno en desarrollo utilizar el siguiente comando:

```bash
dotnet run
```

Nota: para que se ejecute correctamente se debe tener actualizado el appsettings.json y tambien la base de datos (para este caso ejecutar el comando para actualizar la db)

Este levanta el proyecto en http://localhost:5296/ 

Swagger en http://localhost:5296/swagger/index.html


### Base de datos

Para ejecutar los siguientes comandos es necesario tener instalado las herramientas de entity framework core. 

```bash
dotnet tool install --global dotnet-ef
```

Para agregar una nueva migracion a la base de datos utilizar:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

Para actualizar o crear la base de datos utilizar:

```bash
dotnet ef database update
```