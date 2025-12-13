using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Users
{
    public class UserUpdateDTO
    {
        #nullable disable
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "max 100 characters")]
        public string Name { get; set; }
        #nullable restore
    }
}