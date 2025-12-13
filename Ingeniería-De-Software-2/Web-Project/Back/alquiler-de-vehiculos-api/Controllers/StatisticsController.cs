using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Statistics;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
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
    public class StatisticsController : BaseController
    {
        public StatisticsController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) { }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetDB")]
        public async Task<IActionResult> GetGeneralStatistics()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<dynamic>();
                apiResponse.Data = await _dependencies.GetDB.runAsync();
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

        [Authorize(Roles = "Admin")]
        [HttpPost("GeneralStatistics")]
        public async Task<IActionResult> GetGeneralStatistics(StatisticsDateRangeDTO statisticsDTO)
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<GeneralStatistics>();
                apiResponse.Data = await _dependencies.GetGeneralStatistics.runAsync(statisticsDTO);
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
        [Authorize(Roles = "Admin")]
        [HttpPost("RentedVehicles")]
        public async Task<IActionResult> GetRentedVehicles(StatisticsDateRangeDTO statisticsDTO)
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<Vehicle>>();
                apiResponse.Data = await _dependencies.GetRentedVehicles.runAsync(statisticsDTO);
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
        [Authorize(Roles = "Admin")]
        [HttpPost("WeeklyIncome")]
        public async Task<IActionResult> GetWeeklyIncome()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<WeeklyIncome>();
                apiResponse.Data = await _dependencies.GetWeeklyIncome.runAsync();
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