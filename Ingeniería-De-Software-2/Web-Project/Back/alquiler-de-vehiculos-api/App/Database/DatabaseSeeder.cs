
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using NUglify;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;

namespace AlquilerDeVehiculosApi.App.Database
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _dbContext;
        private readonly IHashRepository _hashRepository;
        private readonly IConfiguration _configuration;

        public DatabaseSeeder(DatabaseContext dbContext, IHashRepository hashRepository, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _hashRepository = hashRepository;
            _configuration = configuration;
        }
        public async Task SeedAdminUserAsync()
        {
            if (!_dbContext.Users.Any(u => u.Role == UserRole.Admin))
            {
                var email = _configuration["AdminUser:Email"];
                var password = _configuration["AdminUser:Password"];
                if (password == null || email == null)
                    throw new ApiException(new ApiErrorResponse("email or password admin not defined in configurations."));
                var (hashedPassword, salt) = _hashRepository.HashPassword(password);

                var adminUser = new User
                {
                    Id = 1,
                    Mail = email,
                    Password = hashedPassword,
                    Salt = salt,
                    Role = UserRole.Admin,
                    Status = UserStatus.Active
                };

                _dbContext.Users.Add(adminUser);
                await _dbContext.SaveChangesAsync();

                var admin = new Admin()
                {
                    UserId = adminUser.Id
                };

                _dbContext.Admins.Add(admin);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SeedEmailTemplatesAsync()
        {

            var templatesToSeed = new List<EmailTemplate>
            {
                new EmailTemplate
                {
                    Id = 1,
                    Name = "Forgot Password",
                    Subject = "Solicitud de cambio de Contraseña",
                    Html = MinifyHtmlTemplate(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ForgotPassword.html")),
                    InjectedValues = "minutesExpiration,code",
                    Status = EmailTemplateStatus.Active
                },
                new EmailTemplate
                {
                    Id = 2,
                    Name = "Signup",
                    Subject = "Welcome to EXD",
                    Html = MinifyHtmlTemplate(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Signup.html")),
                    InjectedValues = "urlLogin,ip,browser,os,countryCode,city,lastAccessDate,sessionCreationDate",
                    Status = EmailTemplateStatus.Active
                },
                new EmailTemplate
                {
                    Id = 3,
                    Name = "New Login",
                    Subject = "New Login",
                    Html = MinifyHtmlTemplate(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "NewLogin.html")),
                    InjectedValues = "ip,browser,os,countryCode,city,sessionCreationDate,supportEmail",
                    Status = EmailTemplateStatus.Active
                },
                new EmailTemplate
                {
                    Id = 4,
                    Name = "2FA",
                    Subject = "Verification code",
                    Html = MinifyHtmlTemplate(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "2FA.html")),
                    InjectedValues = "code",
                    Status = EmailTemplateStatus.Active
                }
            };


            // Get existing template names from the database
            var existingTemplateNames = await _dbContext.EmailTemplates
                                                        .Select(et => et.Name)
                                                        .ToListAsync();

            // Filter out templates that already exist
            var newTemplates = templatesToSeed
                                .Where(t => !existingTemplateNames.Contains(t.Name))
                                .ToList();

            if (newTemplates.Any())
            {
                // Add new templates to the database
                _dbContext.EmailTemplates.AddRange(newTemplates);
                await _dbContext.SaveChangesAsync();

                Console.WriteLine($"{newTemplates.Count} new email templates seeded.");
            }
            else
            {
                Console.WriteLine("No new email templates to seed.");
            }
        }
        public async Task SeedVehiclesAsync()
        {
            var vehicles = ReadVehicleCSV.ReadVehicles("Assets/Seed/vehiculos_listado_150.csv");
            int brandId = 0;
            int branchId = 0;
            int cancellationPolicyId = 0;
            int vehicleTypeId = 0;
            var images = GenerateImages();
            foreach (var vehicle in vehicles)
            {
                if (!_dbContext.Brands.Any(u => u.Name == vehicle.Brand))
                {
                    var e = new Brand()
                    {
                        Name = vehicle.Brand
                    };
                    _dbContext.Brands.Add(e);
                    _dbContext.SaveChanges();
                    brandId = e.Id;
                }
                else brandId = _dbContext.Brands.FirstOrDefault(u => u.Name == vehicle.Brand).Id;
                if (!_dbContext.Branches.Any(u => u.Name == vehicle.Branch))
                {
                    var e = new Branch()
                    {
                        Name = vehicle.Branch,
                        Locality = "Default",
                        Province = "Default"
                    };
                    _dbContext.Branches.Add(e);
                    _dbContext.SaveChanges();
                    branchId = e.Id;
                }
                else branchId = _dbContext.Branches.FirstOrDefault(u => u.Name == vehicle.Branch).Id;
                if (!_dbContext.CancellationPolicies.Any(u => u.Description == vehicle.CancellationPolicy))
                {
                    var e = new CancellationPolicy()
                    {
                        Description = vehicle.CancellationPolicy,
                        ReturnPercentage = vehicle.CancellationPolicy == "20% de devolución" ? 0.20 : vehicle.CancellationPolicy == "100% de devolución" ? 1 : 0
                    };
                    _dbContext.CancellationPolicies.Add(e);
                    _dbContext.SaveChanges();
                    cancellationPolicyId = e.Id;
                }
                else cancellationPolicyId = _dbContext.CancellationPolicies.FirstOrDefault(u => u.Description == vehicle.CancellationPolicy).Id;
                if (!_dbContext.VehicleTypes.Any(u => u.BrandId == brandId && u.Model == vehicle.Model && u.PassengerCapacity == vehicle.PassengerCapacity))
                {
                    var e = new VehicleType()
                    {
                        BrandId = brandId,
                        CancellationPolicyId = cancellationPolicyId,
                        PassengerCapacity = vehicle.PassengerCapacity,
                        Model = vehicle.Model,
                        PricePerDay = vehicle.PricePerDay,
                        Category = Enum.Parse<VehicleCategory>(vehicle.Category, ignoreCase: true),
                        ImageUrl = images[new Random().Next(images.Count)]
                    };
                    _dbContext.VehicleTypes.Add(e);
                    _dbContext.SaveChanges();
                    vehicleTypeId = e.Id;
                }
                else vehicleTypeId = _dbContext.VehicleTypes.FirstOrDefault(u => u.BrandId == brandId && u.Model == vehicle.Model && u.PassengerCapacity == vehicle.PassengerCapacity).Id;
                if (!_dbContext.Vehicles.Any(u => u.LicensePlate == vehicle.LicensePlate))
                {
                    var e = new Vehicle()
                    {
                        LicensePlate = vehicle.LicensePlate,
                        VehicleTypeId = vehicleTypeId,
                        Year = vehicle.Year,
                        CurrentBranchId = branchId,
                        Color = "White"
                    };
                    _dbContext.Vehicles.Add(e);
                    _dbContext.SaveChanges();
                }
            }
            Console.WriteLine("Complete Seed Database");
        }
        public async Task SeedGeneralAsync()
        {
            if (!_dbContext.Users.Any(e => e.Mail == "admin0@example.com"))
            {
                var (hashedPassword, salt) = _hashRepository.HashPassword("123456");
                var u = new User()
                {
                    Mail = "admin0@example.com",
                    Password = hashedPassword,
                    Salt = salt,
                    Role = UserRole.Admin,
                    Status = UserStatus.Active
                };
                _dbContext.Users.Add(u);
                _dbContext.SaveChanges();
                var e = new Admin()
                {
                    UserId = u.Id
                };
                _dbContext.Admins.Add(e);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.Employees.Any(e => e.FirstName == "Junggeun"))
            {
                var (hashedPassword, salt) = _hashRepository.HashPassword("123456");
                var u = new User()
                {
                    Mail = "employee@example.com",
                    Password = hashedPassword,
                    Salt = salt,
                    Role = UserRole.Employee,
                    Status = UserStatus.Active
                };
                _dbContext.Users.Add(u);
                _dbContext.SaveChanges();
                var e = new Employee()
                {
                    UserId = u.Id,
                    PhoneNumber = "3245436543",
                    WorkBranch = _dbContext.Branches.FirstOrDefault().Id,
                    FirstName = "Junggeun",
                    LastName = "Ahn",
                    Dni = "00111000",
                    CreationDate = DateTime.Now,
                    Birthdate = new DateTime(2000, 1, 1)
                };
                _dbContext.Employees.Add(e);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.Customers.Any(e => e.FirstName == "Sunsin"))
            {
                var (hashedPassword, salt) = _hashRepository.HashPassword("123456");
                var u = new User()
                {
                    Mail = "user@example.com",
                    Password = hashedPassword,
                    Salt = salt,
                    Role = UserRole.Customer,
                    Status = UserStatus.Active
                };
                _dbContext.Users.Add(u);
                _dbContext.SaveChanges();
                var e = new Customer()
                {
                    UserId = u.Id,
                    PhoneNumber = "3245436541",
                    FirstName = "Sunsin",
                    LastName = "Yi",
                    Dni = "00111001",
                    Birthdate = new DateTime(2000, 1, 1)
                };
                _dbContext.Customers.Add(e);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.Customers.Any(e => e.FirstName == "Bonggil"))
            {
                var (hashedPassword, salt) = _hashRepository.HashPassword("Password123");
                var u = new User()
                {
                    Mail = "olvidemicontrasenia@example.com",
                    Password = hashedPassword,
                    Salt = salt,
                    Role = UserRole.Customer,
                    Status = UserStatus.Active
                };
                _dbContext.Users.Add(u);
                _dbContext.SaveChanges();
                var e = new Customer()
                {
                    UserId = u.Id,
                    PhoneNumber = "3245436541",
                    FirstName = "Bonggil",
                    LastName = "Yun",
                    Dni = "00111010",
                    Birthdate = new DateTime(2000, 1, 1)
                };
                _dbContext.Customers.Add(e);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.Branches.Any(e => e.Name == "El Bolson"))
            {
                var newBranch = new Branch()
                {
                    Name = "El Bolson",
                    Locality = "Default",
                    Province = "Default"
                };
                _dbContext.Branches.Add(newBranch);
                _dbContext.SaveChanges();
            }
        }

        private string MinifyHtmlTemplate(string FilePath)
        {
            string htmlContent = File.ReadAllText(FilePath);

            var result = Uglify.Html(htmlContent);

            if (result.Errors.Count == 0)
            {
                return result.Code;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Message);
                }
                return "";
            }
        }

        private List<string> GenerateImages()
        {
            string origen = "Assets/Images/Seed";
            string destino = "Assets/Images/Uploads";

            var images = new List<string>();

            // Extensiones comunes de imágenes
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            // Obtener todas las imágenes
            var imagenes = Directory.GetFiles(origen)
                .Where(f => extensionesPermitidas.Contains(Path.GetExtension(f).ToLower()));

            foreach (var rutaImagen in imagenes)
            {
                var nombreArchivo = Path.GetFileName(rutaImagen);
                var destinoImagen = Path.Combine(destino, nombreArchivo);


                images.Add(nombreArchivo);
                // Solo copiar si no existe en la carpeta destino
                if (!File.Exists(destinoImagen))
                {
                    File.Copy(rutaImagen, destinoImagen);
                    Console.WriteLine($"Copiado: {nombreArchivo}");
                }
                else
                {
                    Console.WriteLine($"Ya existe: {nombreArchivo}");
                }
            }

            return images;
        }

    }
}