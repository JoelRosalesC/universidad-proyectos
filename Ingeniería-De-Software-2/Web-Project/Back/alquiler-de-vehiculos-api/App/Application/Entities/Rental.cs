using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class Rental
    {
#nullable disable
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double TotalPrice { get; set; }
        public RentalStatus Status { get; set; }
        public int SelectedVehicleTypeId { get; set; }
        public int DeliveredVehicleId { get; set; }
        public int CancellationPolicyId { get; set; }
        public int CustomerId { get; set; }
        public int? PickedUpEmployeeId { get; set; }
        public int? ReturnedEmployeeId { get; set; }
        public int PickedUpBranchId { get; set; }
        public int? ReturnedBranchId { get; set; }
        public string Additionals { get; set; }
        public List<AdditionalEnum> AdditionalsList { get; set; }
        #nullable restore

    }
}