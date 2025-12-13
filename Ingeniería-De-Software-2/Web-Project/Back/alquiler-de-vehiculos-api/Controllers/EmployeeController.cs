using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerDeVehiculosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        public EmployeeController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) {}
        
        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<EmployeeGetDTO>>();
                apiResponse.Data = await _dependencies.GetAllEmployees.RunAsync();
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
        
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                await _dependencies.EmployeeValidation.EmployeeExistsAsync(id);
                
                var apiResponse = new ApiSuccessResponse<EmployeeGetDTO>();
                apiResponse.Data = await _dependencies.GetEmployeeById.RunAsync(id);
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
        
        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddEmployee(EmployeeAddDTO employeeDTO)
        {
            try
            {
                await _dependencies.EmployeeValidation.EmailAlreadyExistsAsync(employeeDTO.Mail);
                await _dependencies.EmployeeValidation.DniAlreadyExistsAsync(employeeDTO.Dni);
                
                var apiResponse = new ApiSuccessResponse<EmployeeGetDTO>();
                apiResponse.Data = await _dependencies.AddEmployee.RunAsync(employeeDTO);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> UpdateEmployee(EmployeeUpdateDTO employeeDTO)
        {
            try
            {
                await _dependencies.EmployeeValidation.EmployeeExistsAsync(employeeDTO.Id);
                await _dependencies.EmployeeValidation.EmailAlreadyExistsUpdateAsync(employeeDTO.Id, employeeDTO.Mail);
                await _dependencies.EmployeeValidation.DniAlreadyExistsUpdateAsync(employeeDTO.Id, employeeDTO.Dni);
                var apiResponse = new ApiSuccessResponse<EmployeeGetDTO>();
                apiResponse.Data = await _dependencies.UpdateEmployee.RunAsync(employeeDTO);
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _dependencies.EmployeeValidation.EmployeeExistsAsync(id);
                
                await _dependencies.DeleteEmployee.RunAsync(id);
                
                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "Employee deleted successfully";
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