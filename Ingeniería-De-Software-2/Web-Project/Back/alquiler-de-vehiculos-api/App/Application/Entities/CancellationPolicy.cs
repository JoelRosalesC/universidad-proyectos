namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class CancellationPolicy
    {
        #nullable disable
        public int Id { get; set; }
        public string Description { get; set; }
        public double ReturnPercentage { get; set; }

        #nullable restore

    }
}