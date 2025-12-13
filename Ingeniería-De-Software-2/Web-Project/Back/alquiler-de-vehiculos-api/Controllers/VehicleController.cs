
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerDeVehiculosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : BaseController
    {
        public VehicleController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) { }
        [HttpGet("AllAvailables")]
        public async Task<IActionResult> GetAllAvailables()
        {
            var apiResponse = new ApiSuccessResponse<List<Vehicle>>();
            apiResponse.Data = await _dependencies.GetAvailableVehicles.runAsync();
            return Ok(apiResponse);
        }
        [Authorize(Roles = "Employee")]
        [HttpGet("AllAvailablesByBranch")]
        public async Task<IActionResult> AllAvailablesByBranch()
        {
            var apiResponse = new ApiSuccessResponse<List<Vehicle>>();
            var user = await GetUserAsync();
            apiResponse.Data = await _dependencies.GetAvailableVehiclesByBranch.runAsync(user.Id);
            return Ok(apiResponse);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                await _dependencies.VehicleValidation.NotExistingAsync(id);

                var apiResponse = new ApiSuccessResponse<Vehicle>();
                apiResponse.Data = await _dependencies.GetVehicleById.runAsync(id);
                return Ok(apiResponse);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.Error.Status, ex.Error);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiErrorResponse(ex.Message);
                return StatusCode(500, apiResponse);
            }
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("")]
        public async Task<IActionResult> AddVehicle(VehicleAddDTO vehicleDTO)
        {
            try
            {
                await _dependencies.VehicleValidation.LicensePlateAlreadyExistsAsync(vehicleDTO.LicensePlate);
                await _dependencies.BranchValidation.BranchExistsAsync(vehicleDTO.CurrentBranchId);
                await _dependencies.VehicleTypeValidation.VehicleTypeExistsAsync(vehicleDTO.VehicleTypeId);

                var apiResponse = new ApiSuccessResponse<Vehicle>();
                apiResponse.Data = await _dependencies.AddVehicle.runAsync(vehicleDTO);
                return Ok(apiResponse);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.Error.Status, ex.Error);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiErrorResponse(ex.Message);
                return StatusCode(500, apiResponse);
            }
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("")]
        public async Task<IActionResult> UpdateVehicle(VehicleUpdateDTO vehicleDTO)
        {
            try
            {
                await _dependencies.VehicleValidation.NotExistingAsync(vehicleDTO.Id);
                await _dependencies.VehicleValidation.LicensePlateAlreadyExistsOrIdEqualsAsync(vehicleDTO.LicensePlate, vehicleDTO.Id);
                await _dependencies.BranchValidation.BranchExistsAsync(vehicleDTO.CurrentBranchId);
                await _dependencies.VehicleTypeValidation.VehicleTypeExistsAsync(vehicleDTO.VehicleTypeId);

                var apiResponse = new ApiSuccessResponse<Vehicle>();
                apiResponse.Data = await _dependencies.UpdateVehicle.runAsync(vehicleDTO);
                return Ok(apiResponse);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.Error.Status, ex.Error);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiErrorResponse(ex.Message);
                return StatusCode(500, apiResponse);
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _dependencies.VehicleValidation.NotExistingAsync(id);
                await _dependencies.VehicleValidation.RentedVehicleAsync(id);

                await _dependencies.DeleteVehicle.runAsync(id);

                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "vehicle deleted successfully";
                return Ok(apiResponse);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.Error.Status, ex.Error);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiErrorResponse(ex.Message);
                return StatusCode(500, apiResponse);
            }
        }
        
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("Maintenance")]
        public async Task<IActionResult> SetMaintenance(VehicleSetMaintenanceDTO vehicleDTO)
        {
            try
            {
                await _dependencies.SetMaintenanceVehicle.runAsync(vehicleDTO);
                var apiResponse = new ApiSuccessResponse<bool>();
                apiResponse.Data = true;
                return Ok(apiResponse);
            }
            catch(ApiException ex)
            {
                return StatusCode(ex.Error.Status, ex.Error);
            }
            catch(Exception ex)
            {
                var apiResponse = new ApiErrorResponse(ex.Message);
                return StatusCode(500, apiResponse);
            }
        }
    }
}