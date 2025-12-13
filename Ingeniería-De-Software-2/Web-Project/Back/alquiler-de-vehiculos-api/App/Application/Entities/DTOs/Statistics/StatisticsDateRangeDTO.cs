using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Statistics
{
    public class StatisticsDateRangeDTO
    {
        #nullable disable
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        #nullable restore
    }
}