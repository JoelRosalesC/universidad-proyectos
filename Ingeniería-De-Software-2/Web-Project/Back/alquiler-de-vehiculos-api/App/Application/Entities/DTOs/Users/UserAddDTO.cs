using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Users
{
    public class UserAddDTO
    {
        #nullable disable
        [Required]
        [StringLength(100, ErrorMessage = "max 100 characters")]
        public string Name { get; set; }
        #nullable restore
    }
}