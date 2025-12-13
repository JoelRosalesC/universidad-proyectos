
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Models
{
    public class EmailMessage
    {
        #nullable disable
        public string ToEmail { get; set; }
        public string Subject  { get; set; }
        public string Body { get; set; }
        #nullable restore

    }
}