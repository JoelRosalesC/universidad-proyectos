using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Brands
{
    public class BrandUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}