using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Branches
{
    public class BranchAddDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Province { get; set; }
        
        [Required]
        public string Locality { get; set; }
    }
}