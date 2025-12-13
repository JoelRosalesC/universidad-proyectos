

using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class SignUpUserUseCase
    {
        private IUserRepository _userRepository;
        private ICustomerRepository _customerRepository;
        private IHashRepository _hashRepository;
        private IEmailQueueRepository _emailQueueRepository;
        private ConvertInEmailTemplateUseCase _convertInEmailTemplateUseCase;

        public SignUpUserUseCase(IUserRepository userRepository, ICustomerRepository customerRepository, IHashRepository hashRepository, IEmailQueueRepository emailQueueRepository, ConvertInEmailTemplateUseCase convertInEmailTemplateUseCase)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _hashRepository = hashRepository;
            _emailQueueRepository = emailQueueRepository;
            _convertInEmailTemplateUseCase = convertInEmailTemplateUseCase;
        }

        public async Task runAsync(UserSignUpDTO userDTO)
        {
            User newUser = new User();
            newUser.Status = UserStatus.Active;
            newUser.Mail = userDTO.Mail;
            var (hash, salt) = _hashRepository.HashPassword(userDTO.Password);
            newUser.Password = hash;
            newUser.Salt = salt;
            newUser = await _userRepository.AddAsync(newUser);
            Customer customer = new Customer()
            {
                UserId = newUser.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Dni = userDTO.Dni,
                PhoneNumber = userDTO.PhoneNumber,
                Birthdate = userDTO.Birthdate,
                CreationDate = DateTime.Now
            };
            await _customerRepository.AddAsync(customer);

            EmailTemplate emailTemplate = await _convertInEmailTemplateUseCase.runAsync("Signup", "urlLogin","ip","browser","os","countryCode","city","lastAccessDate","sessionCreationDate");
            await _emailQueueRepository.EnqueueAsync(userDTO.Mail, "¡Bienvenido!", emailTemplate.Html);
        }
    }
}