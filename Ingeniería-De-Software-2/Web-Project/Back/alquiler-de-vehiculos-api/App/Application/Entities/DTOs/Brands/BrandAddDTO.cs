using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Brands
{
    public class BrandAddDTO
    {
        [Required]
        public string Name { get; set; }
    }
}