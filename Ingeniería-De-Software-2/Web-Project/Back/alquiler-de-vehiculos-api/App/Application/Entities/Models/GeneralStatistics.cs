
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Models
{
    public class GeneralStatistics
    {
#nullable disable
        public int RegisteredCustomers { get; set; }
        public int TotalVehicles { get; set; }
        public List<VehicleType> MostRentedVehicleType { get; set; }
        public List<Employee> EmployeesWhoSoldTheMost { get; set; }
        public List<string> MostChosenBranch { get; set; }
        #nullable restore

    }
}