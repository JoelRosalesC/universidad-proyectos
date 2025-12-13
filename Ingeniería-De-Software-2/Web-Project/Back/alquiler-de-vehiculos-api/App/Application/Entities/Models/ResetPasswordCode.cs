
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Models
{
    public class VerificationCode
    {
        #nullable disable
        public string Code { get; set; }
        public int FailedAttempts  { get; set; }
        public VerifyCodeType Type { get; set; }
        #nullable restore

    }
}