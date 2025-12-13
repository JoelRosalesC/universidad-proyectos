
using System.Globalization;
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Models
{
    public class VehicleCSV
    {
        #nullable disable
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Branch { get; set; }
        public int PassengerCapacity { get; set; }
        public string CancellationPolicy { get; set; }
        public string Category { get; set; }
        public double PricePerDay { get; set; }
        #nullable restore
    }
    public class ReadVehicleCSV
    {
        public static List<VehicleCSV> ReadVehicles(string rutaArchivo)
        {
            var vehiculos = new List<VehicleCSV>();

            using (var reader = new StreamReader(rutaArchivo))
            {
                bool primeraLinea = true;
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();

                    // Ignorar encabezado
                    if (primeraLinea)
                    {
                        primeraLinea = false;
                        continue;
                    }

                    var columnas = linea.Split(',');

                    var vehiculo = new VehicleCSV
                    {
                        LicensePlate = columnas[0],
                        Brand = columnas[1],
                        Model = columnas[2],
                        Year = columnas[3],
                        Branch = columnas[4],
                        PassengerCapacity = int.Parse(columnas[5]),
                        CancellationPolicy = columnas[6],
                        Category = columnas[7].Replace(" ", ""),
                        PricePerDay = double.Parse(columnas[8], CultureInfo.InvariantCulture)
                    };

                    vehiculos.Add(vehiculo);
                }
            }

            return vehiculos;
        }

    }
}