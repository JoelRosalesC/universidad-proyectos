namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class Branch
    {
        #nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string Locality { get; set; }

        #nullable restore

    }
}