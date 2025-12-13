using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth
{
    public class UserLoginDTO
    {
        #nullable disable
        [Required(ErrorMessage = "mail es requerido")]
        [EmailAddress(ErrorMessage = "mail invalido")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "contraseña es requerida")]
        public string Password { get; set; }
        #nullable restore
    }
}