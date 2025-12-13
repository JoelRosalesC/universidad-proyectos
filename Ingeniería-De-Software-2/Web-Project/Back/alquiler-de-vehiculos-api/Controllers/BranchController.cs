using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Branches;
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
    public class BranchController : BaseController
    {
        public BranchController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) {}
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<Branch>>();
                apiResponse.Data = await _dependencies.GetAllBranches.runAsync();
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
        public async Task<IActionResult> GetBranchById(int id)
        {
            try
            {
                await _dependencies.BranchValidation.BranchExistsAsync(id);
                
                var apiResponse = new ApiSuccessResponse<Branch>();
                apiResponse.Data = await _dependencies.GetBranchById.runAsync(id);
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
        public async Task<IActionResult> AddBranch(BranchAddDTO branchDTO)
        {
            try
            {
                await _dependencies.BranchValidation.BranchNameAlreadyExistsAsync(branchDTO.Name);
                
                var apiResponse = new ApiSuccessResponse<Branch>();
                apiResponse.Data = await _dependencies.AddBranch.runAsync(branchDTO);
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
        public async Task<IActionResult> UpdateBranch(BranchUpdateDTO branchDTO)
        {
            try
            {
                await _dependencies.BranchValidation.BranchExistsAsync(branchDTO.Id);
                await _dependencies.BranchValidation.BranchNameAlreadyExistsOrIdEqualsAsync(branchDTO.Name, branchDTO.Id);
                
                var apiResponse = new ApiSuccessResponse<Branch>();
                apiResponse.Data = await _dependencies.UpdateBranch.runAsync(branchDTO);
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
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                await _dependencies.BranchValidation.BranchExistsAsync(id);
                await _dependencies.BranchValidation.IsReadyToBeDeleted(id);
                
                await _dependencies.DeleteBranch.runAsync(id);
                
                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "Sucursal eliminada correctamente";
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