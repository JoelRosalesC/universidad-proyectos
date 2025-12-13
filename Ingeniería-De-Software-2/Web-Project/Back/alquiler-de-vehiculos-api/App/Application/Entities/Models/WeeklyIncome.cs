
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Models
{
    public class WeeklyIncome
    {
        #nullable disable
        public decimal TotalIncome { get; set; }
        public List<WeekIncome> Weeks { get; set; }
        #nullable restore

    }

    public class WeekIncome
    {
        #nullable disable
        public DateTime Week { get; set; }
        public decimal NumberOfReservations { get; set; }

        #nullable restore
    }
}