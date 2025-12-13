using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees
{
    public class EmployeeUpdateDTO
    {
        #nullable disable
        [Required(ErrorMessage = "id es requerido")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "mail es requerido")]
        [EmailAddress(ErrorMessage = "mail invalido")]
        public string Mail { get; set; }
        
        [Required(ErrorMessage = "nombre es requerido")]
        [RegularExpression(@"^[a-zA-ZÁÉÍÓÚÑáéíóúñ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "apellido es requerido")]
        [RegularExpression(@"^[a-zA-ZÁÉÍÓÚÑáéíóúñ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "DNI es requerido")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "DNI debe tener entre 7 y 8 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "DNI solo puede contener números")]
        public string Dni { get; set; }
        
        [Required(ErrorMessage = "fecha de nacimiento es requerido")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de nacimiento no tiene un formato válido")]
        [MinimumAge(18)]
        public DateTime Birthdate { get; set; }
        
        [Required(ErrorMessage = "numbero de telefono es requerido")]
        [RegularExpression(@"^\+?\d{8,15}$", ErrorMessage = "El número debe contener solo números y puede comenzar con +")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "sucursal de trabajo es requerida")]
        public int WorkBranch { get; set; }
        #nullable restore
    }
}