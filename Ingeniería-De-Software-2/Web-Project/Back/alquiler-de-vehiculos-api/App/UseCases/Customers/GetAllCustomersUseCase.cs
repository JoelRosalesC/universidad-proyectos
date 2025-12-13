using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Customers;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetAllCustomersUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public GetAllCustomersUseCase(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<List<CustomerGetDTO>> RunAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customersDTOs = new List<CustomerGetDTO>();

            foreach (var customer in customers)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(customer.UserId);

                    customersDTOs.Add(new CustomerGetDTO
                    {
                        Id = customer.UserId,
                        Mail = user.Mail,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Dni = customer.Dni,
                        Birthdate = customer.Birthdate,
                        PhoneNumber = customer.PhoneNumber
                    });
                }
                catch (ApiException)
                {

                }
            }

            return customersDTOs;
        }
    }
}