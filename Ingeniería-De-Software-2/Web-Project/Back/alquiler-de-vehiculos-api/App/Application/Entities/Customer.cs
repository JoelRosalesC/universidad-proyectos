
namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class Customer
    {
#nullable disable
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }

        #nullable restore

    }
}