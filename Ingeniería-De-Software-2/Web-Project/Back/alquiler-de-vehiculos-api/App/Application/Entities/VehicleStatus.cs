namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class VehicleStatus
    {
        #nullable disable
        public int Id { get; set; }
        public string Description { get; set; }
        public int RentalId { get; set; }

        #nullable restore

    }
}