

using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using System.Threading.Tasks;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class UserValidation
    {
        private IUserRepository _userRepository;
        private IHashRepository _hashRepository;
        private IVerifyCodeRepository _verifyCodeRepository;
        private IRentalRepository _rentalRepository;
        public UserValidation(IUserRepository userRepository, IHashRepository hashRepository, IVerifyCodeRepository verifyCodeRepository, IRentalRepository rentalRepository)
        {
            _userRepository = userRepository;
            _hashRepository = hashRepository;
            _verifyCodeRepository = verifyCodeRepository;
            _rentalRepository = rentalRepository;
        }
        public async Task IsExistingEmailAsync(string mail)
        {
            try
            {
                var user = await _userRepository.GetByMailAsync(mail);
            }
            catch
            {
                return;
            }
            throw new ApiException(new ApiErrorResponse("el correo electrónico ya está registrado. Por favor, usa otro o inicia sesión."));
        }
        public async Task NotExistingEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByMailAsync(email);
            }
            catch (ApiException)
            {
                throw new ApiException(new ApiErrorResponse("correo electrónico invalido"));
            }
        }
        public async Task NotExistingUserIdAsync(int id)
        {
            await _userRepository.GetByIdAsync(id);
        }
        public async Task VerifyCredentialsAsync(UserLoginDTO userDTO)
        {
            try
            {
                User user = await _userRepository.GetByMailAsync(userDTO.Mail);
                if (!_hashRepository.VerifyPassword(userDTO.Password, user.Password, user.Salt))
                    throw new ApiException();
            }
            catch (ApiException)
            {
                throw new ApiException(new ApiErrorResponse("credenciales invalidas"));
            }
        }
        public async Task NotAdmin(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user.Role != UserRole.Admin)
                throw new ApiException(new ApiErrorResponse("You must be an administrator to perform this operation."));
        }
        public async Task NotAdminAndOwner(int userId, int idOwner)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user.Role != UserRole.Admin && userId != idOwner)
                throw new ApiException(new ApiErrorResponse("You must be an administrator or owner to perform this operation."));
        }
        public void SignUpCodeInvalid(string email, string code)
        {
            if (!_verifyCodeRepository.Verify(email, code, VerifyCodeType.SignUp))
                throw new ApiException(new ApiErrorResponse("el codigo ingresado es invalido"));
        }
        public void ResetPasswordCodeInvalid(string email, string code)
        {
            if (!_verifyCodeRepository.Verify(email, code, VerifyCodeType.ResetPassword))
                throw new ApiException(new ApiErrorResponse("el codigo ingresado es invalido"));
        }
        public void LoginCodeInvalid(string email, string code)
        {
            if (!_verifyCodeRepository.Verify(email, code, VerifyCodeType.Login))
                throw new ApiException(new ApiErrorResponse("el codigo ingresado es invalido"));
        }
        public async Task UserWithRentals(User user)
        {
            var rentals = await _rentalRepository.GetAllAsync();
            if (rentals.Where(r => r.CustomerId == user.Id && (r.Status == RentalStatus.Pending || r.Status == RentalStatus.Rented )).Count() > 0)
                throw new ApiException(new ApiErrorResponse("el usuario no puede ser eliminado porque tiene alquileres pendientes"));
        }
    }
}