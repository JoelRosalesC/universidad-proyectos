using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees
{
    public class EmployeeGetDTO
    {
        #nullable disable
        public int Id { get; set; }
        public string Mail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int WorkBranch { get; set; }
        #nullable restore
    }
}