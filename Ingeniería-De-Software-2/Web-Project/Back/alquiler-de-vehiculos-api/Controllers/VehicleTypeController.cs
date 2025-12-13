using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Validations;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerDeVehiculosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : BaseController
    {
        private readonly VehicleTypeValidation _vehicleTypeValidation;

        public VehicleTypeController(DependencyProvider dependencies, IUserRepository userRepository, VehicleTypeValidation vehicleTypeValidation) : base(dependencies, userRepository)
        {
            _vehicleTypeValidation = vehicleTypeValidation;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllVehicleTypes()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<VehicleType>>();
                apiResponse.Data = await _dependencies.GetAllVehicleTypes.runAsync();
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
        [HttpPost("Availables")]
        public async Task<IActionResult> GetAllVehicleTypeAvailables(GetVehicleTypeAvailablesDTO VehicleTypeDTO)
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<VehicleType>>();
                apiResponse.Data = await _dependencies.GetAllVehicleTypeAvailables.runAsync(VehicleTypeDTO.BranchId, VehicleTypeDTO.StartDate.Date, VehicleTypeDTO.EndDate.Date);
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
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleTypeById(int id)
        {
            try
            {
                await _vehicleTypeValidation.VehicleTypeExistsAsync(id);
                
                var apiResponse = new ApiSuccessResponse<VehicleType>();
                apiResponse.Data = await _dependencies.GetVehicleTypeById.runAsync(id);
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
        
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("")]
        public async Task<IActionResult> AddVehicleType(VehicleTypeAddDTO vehicleTypeDTO)
        {
            try
            {
                await _vehicleTypeValidation.BrandExistsAsync(vehicleTypeDTO.BrandId);
                await _vehicleTypeValidation.CancellationPolicyExistsAsync(vehicleTypeDTO.CancellationPolicyId);
                _dependencies.VehicleTypeValidation.ImageRequired(vehicleTypeDTO.Image);
                _dependencies.VehicleTypeValidation.FileFormatNotSupported(vehicleTypeDTO.Image);
                await _dependencies.VehicleTypeValidation.VehicleTypeRepeatedAsync(vehicleTypeDTO.BrandId, vehicleTypeDTO.Model, int.Parse(vehicleTypeDTO.PassengerCapacity));

                string fileName = await _dependencies.SaveImage.runAsync(vehicleTypeDTO.Image);
                
                var apiResponse = new ApiSuccessResponse<VehicleType>();
                apiResponse.Data = await _dependencies.AddVehicleType.runAsync(vehicleTypeDTO, fileName);
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
        
        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("")]
        public async Task<IActionResult> UpdateVehicleType(VehicleTypeUpdateDTO vehicleTypeDTO)
        {
            try
            {
                await _vehicleTypeValidation.VehicleTypeExistsAsync(vehicleTypeDTO.Id);
                await _vehicleTypeValidation.BrandExistsAsync(vehicleTypeDTO.BrandId);
                await _vehicleTypeValidation.CancellationPolicyExistsAsync(vehicleTypeDTO.CancellationPolicyId);
                _dependencies.VehicleTypeValidation.FileFormatNotSupported(vehicleTypeDTO.Image);

                string fileName = "";
                if(!(vehicleTypeDTO.Image == null || vehicleTypeDTO.Image.Length == 0))
                    fileName = await _dependencies.SaveImage.runAsync(vehicleTypeDTO.Image);
                
                var apiResponse = new ApiSuccessResponse<VehicleType>();
                apiResponse.Data = await _dependencies.UpdateVehicleType.runAsync(vehicleTypeDTO, fileName);
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
        
        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            try
            {
                await _vehicleTypeValidation.VehicleTypeExistsAsync(id);
                
                await _dependencies.DeleteVehicleType.runAsync(id);
                
                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "Vehicle type deleted successfully";
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