
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class EmailTemplate
    {
        #nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string InjectedValues { get; set; }
        public EmailTemplateStatus Status { get; set; }
        #nullable restore

    }
}