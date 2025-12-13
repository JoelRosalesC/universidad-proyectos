

using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class User
    {
        #nullable disable
        public int Id { get; set; }
        public string Mail  { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
        #nullable restore

    }
}