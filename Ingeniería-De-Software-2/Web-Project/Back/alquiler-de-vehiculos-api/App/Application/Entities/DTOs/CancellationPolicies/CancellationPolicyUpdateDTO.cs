using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.CancellationPolicies
{
    public class CancellationPolicyUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [ValidPercentageAttribute]
        public string ReturnPercentage { get; set; }
    }
}