using AlquilerDeVehiculosApi.App.Application.Enums;
using Microsoft.AspNetCore.Mvc;


namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes
{
    public class GetVehicleTypeAvailablesDTO
    {
        public int BranchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}