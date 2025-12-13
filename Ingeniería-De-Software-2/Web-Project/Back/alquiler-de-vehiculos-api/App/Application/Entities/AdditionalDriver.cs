using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class AdditionalDriver
    {
        #nullable disable
        public int Id { get; set; }
        public string Mail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        #nullable restore

    }
}