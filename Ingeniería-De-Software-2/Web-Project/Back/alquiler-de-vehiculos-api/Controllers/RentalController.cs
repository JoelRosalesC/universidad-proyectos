using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerDeVehiculosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : BaseController
    {
        public RentalController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) { }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> CreateRental(RentalAddDTO rentalDTO)
        {
            try
            {
                var user = await GetUserAsync();
                _dependencies.RentalValidation.ValidateDate(rentalDTO.StartDate, rentalDTO.EndDate);
                _dependencies.RentalValidation.ValidateCard(rentalDTO.CardNumber);
                _dependencies.RentalValidation.ValidateExpirationDate(rentalDTO.ExpirationDate);
                _dependencies.RentalValidation.ValidateCvv(rentalDTO.CvvCode);
                await _dependencies.AddRental.RunAsync(rentalDTO, user.Id);
                var apiResponse = new ApiSuccessResponse<string>();
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
        [Authorize(Roles = "Employee")]
        [HttpPost("RentalByUser")]
        public async Task<IActionResult> CreateRental(UserRentalAddDTO rentalDTO)
        {
            try
            {
                var user = await GetUserAsync();
                await _dependencies.AddUserRental.RunAsync(rentalDTO, user.Id);
                var apiResponse = new ApiSuccessResponse<string>();
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
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetMyRentals()
        {
            try
            {
                var user = await GetUserAsync();
                var rentals = await _dependencies.GetAllRentalsByCustomer.RunAsync(user.Id);
                var apiResponse = new ApiSuccessResponse<List<Rental>>();
                apiResponse.Data = rentals;
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelRental(int id)
        {
            try
            {
                await _dependencies.RentalValidation.CancelRentalValidateAsync(id);
                await _dependencies.CancelRental.RunAsync(id);
                var apiResponse = new ApiSuccessResponse<string>();
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
        [Authorize(Roles = "Employee")]
        [HttpDelete("Invalidate/{id}")]
        public async Task<IActionResult> InvalidateRental(int id)
        {
            try
            {
                await _dependencies.RentalValidation.InvalidateRentalValidateAsync(id);
                await _dependencies.InvalidateRental.RunAsync(id);
                var apiResponse = new ApiSuccessResponse<string>();
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
        [Authorize(Roles = "Employee")]
        [HttpGet("GetRentalsInBranch")]
        public async Task<IActionResult> GetRentalsInBranch()
        {
            try
            {
                var user = await GetUserAsync();
                var rentals = await _dependencies.GetRentalsInBranch.RunAsync(user.Id);
                var apiResponse = new ApiSuccessResponse<List<Rental>>()
                {
                    Data = rentals
                };
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
        [Authorize(Roles = "Employee")]
        [HttpPost("PickupVehicle")]
        public async Task<IActionResult> PickupVehicle(PickupVehicleDTO rentalDTO)
        {
            try
            {
                var user = await GetUserAsync();
                await _dependencies.RentalValidation.ValidateRentalStatus(rentalDTO.RentalId, RentalStatus.Pending);
                await _dependencies.PickupVehicle.RunAsync(rentalDTO, user.Id);
                var apiResponse = new ApiSuccessResponse<string>();
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
        [Authorize]
        [HttpGet("AllRentalsByStatus/{status}")]
        public async Task<IActionResult> GetAllRentalsByStatus(RentalStatus status)
        {
            try
            {
                var user = await GetUserAsync();
                var rentals = await _dependencies.GetAllRentals.RunAsync();
                var apiResponse = new ApiSuccessResponse<List<Rental>>();
                apiResponse.Data = rentals.Where(r => r.Status == status).ToList();
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
        [Authorize(Roles = "Employee")]
        [HttpPost("ReturnVehicle")]
        public async Task<IActionResult> ReturnVehicle(ReturnVehicleDTO rentalDTO)
        {
            try
            {
                var user = await GetUserAsync();
                await _dependencies.RentalValidation.ValidateRentalStatus(rentalDTO.RentalId, RentalStatus.Rented);
                await _dependencies.ReturnVehicle.RunAsync(rentalDTO, user.Id);
                var apiResponse = new ApiSuccessResponse<string>();
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
    }
}