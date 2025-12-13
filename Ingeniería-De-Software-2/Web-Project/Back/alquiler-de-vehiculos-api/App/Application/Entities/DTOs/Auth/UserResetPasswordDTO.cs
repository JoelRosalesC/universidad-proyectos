using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth
{
    public class UserResetPasswordDTO
    {
        #nullable disable
        [Required(ErrorMessage = "mail es requerido")]
        [EmailAddress(ErrorMessage = "mail invalido")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "contraseña es requerida")]
        [MinLength(6, ErrorMessage = "la contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }
        [Required(ErrorMessage = "codigo es requerido")]
        public string Code { get; set; }
        #nullable restore
    }
}