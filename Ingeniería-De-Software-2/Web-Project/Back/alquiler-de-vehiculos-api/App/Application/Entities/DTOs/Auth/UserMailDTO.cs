using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth
{
    public class UserMailDTO
    {
        #nullable disable
        [Required(ErrorMessage = "mail es requerido")]
        [EmailAddress(ErrorMessage = "mail invalido")]
        public string Mail { get; set; }
        #nullable restore
    }
}