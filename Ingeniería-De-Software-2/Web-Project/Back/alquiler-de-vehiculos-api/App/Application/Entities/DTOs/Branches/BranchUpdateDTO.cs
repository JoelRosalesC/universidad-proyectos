using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Branches
{
    public class BranchUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Province { get; set; }
        
        [Required]
        public string Locality { get; set; }
    }
}