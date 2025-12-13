using AlquilerDeVehiculosApi.App.Application.Enums;
using Microsoft.EntityFrameworkCore;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    [PrimaryKey(nameof(AdditionalId),nameof(RentalId))]
    public class AdditionalRental
    {
        #nullable disable
        public int AdditionalId { get; set; }
        public int RentalId { get; set; }
        #nullable restore

    }
}