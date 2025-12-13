using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Brands;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerDeVehiculosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        public BrandController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) {}
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var apiResponse = new ApiSuccessResponse<List<Brand>>();
                apiResponse.Data = await _dependencies.GetAllBrands.runAsync();
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
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                await _dependencies.BrandValidation.BrandExistsAsync(id);
                
                var apiResponse = new ApiSuccessResponse<Brand>();
                apiResponse.Data = await _dependencies.GetBrandById.runAsync(id);
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
        public async Task<IActionResult> AddBrand(BrandAddDTO brandDTO)
        {
            try
            {
                await _dependencies.BrandValidation.BrandNameAlreadyExistsAsync(brandDTO.Name);
                
                var apiResponse = new ApiSuccessResponse<Brand>();
                apiResponse.Data = await _dependencies.AddBrand.runAsync(brandDTO);
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
        public async Task<IActionResult> UpdateBrand(BrandUpdateDTO brandDTO)
        {
            try
            {
                await _dependencies.BrandValidation.BrandExistsAsync(brandDTO.Id);
                await _dependencies.BrandValidation.BrandNameAlreadyExistsAsync(brandDTO.Name);
                
                var apiResponse = new ApiSuccessResponse<Brand>();
                apiResponse.Data = await _dependencies.UpdateBrand.runAsync(brandDTO);
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
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                await _dependencies.BrandValidation.BrandExistsAsync(id);
                
                await _dependencies.DeleteBrand.runAsync(id);
                
                var apiResponse = new ApiSuccessResponse<string>();
                apiResponse.Data = "Brand deleted successfully";
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