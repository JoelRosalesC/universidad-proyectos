using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.CancellationPolicies
{
    public class CancellationPolicyDTO
    {
        #nullable disable
        public int Id { get; set; }
        public string Description { get; set; }
        [ValidPercentageAttribute]
        public string ReturnPercentage { get; set; }
        #nullable restore
    }
}