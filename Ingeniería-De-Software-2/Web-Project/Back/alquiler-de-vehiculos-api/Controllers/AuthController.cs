using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth;
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
    public class AuthController : BaseController
    {

        public AuthController(DependencyProvider dependencies, IUserRepository userRepository) : base(dependencies, userRepository) { }
        [HttpPost("GenerateSignUpCode")]
        public async Task<IActionResult> GenerateSignUpCode(UserMailDTO userDTO)
        {
            try
            {
                await _dependencies.UserValidation.IsExistingEmailAsync(userDTO.Mail);
                await _dependencies.GenerateSignUpCode.runAsync(userDTO.Mail);
                var apiResponse = new ApiSuccessResponse<string>() { Data = "" };
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

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserSignUpDTO userDTO)
        {
            try
            {
                await _dependencies.UserValidation.IsExistingEmailAsync(userDTO.Mail);
                _dependencies.UserValidation.SignUpCodeInvalid(userDTO.Mail, userDTO.Code);
                await _dependencies.SignUpUser.runAsync(userDTO);
                var apiResponse = new ApiSuccessResponse<string>() { Data = "" };
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            try
            {
                await _dependencies.UserValidation.VerifyCredentialsAsync(userDTO);

                User user = await GetUserAsync(userDTO.Mail);
                if (user.Role == UserRole.Admin)
                {
                    await _dependencies.GenerateLoginCode.runAsync(user.Mail);
                    var response = new { require_code = true };
                    return StatusCode(200, response);

                }
                var token = _dependencies.LoginUser.Run(user);

                var apiResponse = new ApiSuccessResponse<dynamic>()
                {
                    Data = new
                    {
                        token = token,
                        role = user.Role.ToString()
                    }
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
        [HttpPost("Login2FA")]
        public async Task<IActionResult> Login2FA(UserLogin2FADTO userDTO)
        {
            try
            {
                _dependencies.UserValidation.LoginCodeInvalid(userDTO.Mail, userDTO.Code);

                User user = await GetUserAsync(userDTO.Mail);
                var token = _dependencies.LoginUser.Run(user);

                var apiResponse = new ApiSuccessResponse<dynamic>()
                {
                    Data = new
                    {
                        token = token,
                        role = user.Role.ToString()
                    }
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
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserMailDTO userDTO)
        {
            try
            {
                await _dependencies.UserValidation.NotExistingEmailAsync(userDTO.Mail);

                await _dependencies.ForgotPasswordUser.runAsync(userDTO.Mail);
                var apiResponse = new ApiSuccessResponse<string>() { Data = "" };
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
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordDTO userDTO)
        {
            try
            {
                await _dependencies.UserValidation.NotExistingEmailAsync(userDTO.Mail);
                _dependencies.UserValidation.ResetPasswordCodeInvalid(userDTO.Mail, userDTO.Code);
                await _dependencies.ResetPasswordUser.runAsync(userDTO);
                var apiResponse = new ApiSuccessResponse<string>() { Data = "" };
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
        [HttpDelete("")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var user = await GetUserAsync();
                await _dependencies.UserValidation.UserWithRentals(user);
                await _dependencies.DeleteUser.runAsync(user.Id);
                var apiResponse = new ApiSuccessResponse<string>() {Data=""};
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
