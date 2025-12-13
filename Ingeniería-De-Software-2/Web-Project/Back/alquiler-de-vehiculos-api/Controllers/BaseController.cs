using System.Security.Claims;
using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using Microsoft.AspNetCore.Mvc;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly DependencyProvider _dependencies;
        private readonly IUserRepository _userRepository;

        public BaseController(DependencyProvider dependencies, IUserRepository userRepository)
        {
            _dependencies = dependencies;
            _userRepository = userRepository;
        }
        protected int GetUserId()
        {
            string? stringId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(stringId);
            if (stringId == null)
                throw new NullReferenceException("The user not exists.");
            return int.Parse(stringId);
        }
        protected async Task<User> GetUserAsync()
        {
            string? mail = User?.FindFirst(ClaimTypes.Email)?.Value;
            if(mail == null)
                throw new ApiException();
            User user = await _userRepository.GetByMailAsync(mail);
            return user;
        }
        protected async Task<User> GetUserAsync(string userMail)
        {
            var user = await _userRepository.GetByMailAsync(userMail);
            return user;
        }
    }
}