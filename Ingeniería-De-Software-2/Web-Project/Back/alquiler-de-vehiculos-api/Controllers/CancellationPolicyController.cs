using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.CancellationPolicies;
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
    public class CancellationPolicyController : BaseController
    {
        private readonly CancellationPolicyValidation _cancellationPolicyValidation;

        public CancellationPolicyController(DependencyProvider dependencies, IUserRepository userRepository, CancellationPolicyValidation cancellationPolicyValidation) 
            : base(dependencies, userRepository)
        {
            _cancellationPolicyValidation = cancellationPolicyValidation;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllCancellationPolicies()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<CancellationPolicy>>();
                apiResponse.Data = await _dependencies.GetAllCancellationPolicies.runAsync();
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
        public async Task<IActionResult> GetCancellationPolicyById(int id)
        {
            try
            {
                await _cancellationPolicyValidation.CancellationPolicyExistsAsync(id);
                
                var apiResponse = new ApiSuccessResponse<CancellationPolicy>();
                apiResponse.Data = await _dependencies.GetCancellationPolicyById.runAsync(id);
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
        public async Task<IActionResult> AddCancellationPolicy(CancellationPolicyAddDTO cancellationPolicyDTO)
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<CancellationPolicy>();
                apiResponse.Data = await _dependencies.AddCancellationPolicy.runAsync(cancellationPolicyDTO);
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
        public async Task<IActionResult> UpdateCancellationPolicy(CancellationPolicyUpdateDTO cancellationPolicyDTO)
        {
            try
            {
                await _cancellationPolicyValidation.CancellationPolicyExistsAsync(cancellationPolicyDTO.Id);
                
                var apiResponse = new ApiSuccessResponse<CancellationPolicy>();
                apiResponse.Data = await _dependencies.UpdateCancellationPolicy.runAsync(cancellationPolicyDTO);
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
        public async Task<IActionResult> DeleteCancellationPolicy(int id)
        {
            try
            {
                await _cancellationPolicyValidation.CancellationPolicyExistsAsync(id);
                
                await _dependencies.DeleteCancellationPolicy.runAsync(id);
                
                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "Cancellation policy deleted successfully";
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