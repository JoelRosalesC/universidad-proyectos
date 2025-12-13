using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth
{
    public class UserLogin2FADTO
    {
        #nullable disable
        [Required(ErrorMessage = "mail es requerido")]
        [EmailAddress(ErrorMessage = "mail invalido")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "codigo es requerido")]
        public string Code { get; set; }
        #nullable restore
    }
}