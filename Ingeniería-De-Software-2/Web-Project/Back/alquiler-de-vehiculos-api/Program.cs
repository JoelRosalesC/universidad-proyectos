using System.Text;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Validations;
using AlquilerDeVehiculosApi.App.Database;
using AlquilerDeVehiculosApi.App.Repositories;
using AlquilerDeVehiculosApi.App.UseCases;
using AlquilerDeVehiculosApi.App.UseCases.Branches;
using AlquilerDeVehiculosApi.App.UseCases.Brands;
using AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies;
using AlquilerDeVehiculosApi.App.UseCases.Employees;
using AlquilerDeVehiculosApi.App.UseCases.Shared;
using AlquilerDeVehiculosApi.App.UseCases.VehicleTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleMaintenanceRepository, VehicleMaintenanceRepository>();
builder.Services.AddScoped<IVehicleStatusRepository, VehicleStatusRepository>();
builder.Services.AddScoped<IAdditionalDriverRepository, AdditionalDriverRepository>();
builder.Services.AddScoped<IAdditionalRentalRepository, AdditionalRentalRepository>();
builder.Services.AddScoped<IAdditionalRepository, AdditionalRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICancellationPolicyRepository, CancellationPolicyRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageStorageRepository, ImageStorageRepository>();

// Auth-related repositories
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IHashRepository, HashRepository>();
builder.Services.AddSingleton<IVerifyCodeRepository, VerifyCodeRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();

// Vehicle UseCases
builder.Services.AddTransient<AddVehicleUseCase>();
builder.Services.AddTransient<DeleteVehicleUseCase>();
builder.Services.AddTransient<GetAvailableVehiclesUseCase>();
builder.Services.AddTransient<GetVehicleByIdUseCase>();
builder.Services.AddTransient<UpdateVehicleUseCase>();
builder.Services.AddTransient<SetMaintenanceVehicleUseCase>();
builder.Services.AddTransient<GetAvailableVehiclesByBranchUseCase>();

// VehicleType UseCases
builder.Services.AddTransient<AddVehicleTypeUseCase>();
builder.Services.AddTransient<UpdateVehicleTypeUseCase>();
builder.Services.AddTransient<DeleteVehicleTypeUseCase>();
builder.Services.AddTransient<GetAllVehicleTypesUseCase>();
builder.Services.AddTransient<GetVehicleTypeByIdUseCase>();
builder.Services.AddTransient<VehicleTypeValidation>();
builder.Services.AddTransient<VehicleValidation>();
builder.Services.AddTransient<GetAllVehicleTypeAvailablesUseCase>();

// Auth UseCases
builder.Services.AddTransient<DeleteUserUseCase>();
builder.Services.AddTransient<SignUpUserUseCase>();
builder.Services.AddTransient<LoginUserUseCase>();
builder.Services.AddTransient<ForgotPasswordUserUseCase>();
builder.Services.AddTransient<ResetPasswordUserUseCase>();
builder.Services.AddTransient<GenerateSignUpCodeUseCase>();
builder.Services.AddTransient<GenerateLoginCodeUseCase>();
builder.Services.AddTransient<ConvertInEmailTemplateUseCase>();
builder.Services.AddTransient<UserValidation>();

// Employee UseCases
builder.Services.AddTransient<AddEmployeeUseCase>();
builder.Services.AddTransient<GetAllEmployeesUseCase>();
builder.Services.AddTransient<GetEmployeeByIdUseCase>();
builder.Services.AddTransient<DeleteEmployeeUseCase>();
builder.Services.AddTransient<EmployeeValidation>();
builder.Services.AddTransient<UpdateEmployeeUseCase>();

// Branch UseCases
builder.Services.AddTransient<AddBranchUseCase>();
builder.Services.AddTransient<DeleteBranchUseCase>();
builder.Services.AddTransient<GetAllBranchesUseCase>();
builder.Services.AddTransient<GetBranchByIdUseCase>();
builder.Services.AddTransient<UpdateBranchUseCase>();
builder.Services.AddTransient<BranchValidation>();

// Brand UseCases
builder.Services.AddTransient<AddBrandUseCase>();
builder.Services.AddTransient<UpdateBrandUseCase>();
builder.Services.AddTransient<DeleteBrandUseCase>();
builder.Services.AddTransient<GetAllBrandsUseCase>();
builder.Services.AddTransient<GetBrandByIdUseCase>();
builder.Services.AddTransient<BrandValidation>();

// CancellationPolicy UseCases
builder.Services.AddTransient<AddCancellationPolicyUseCase>();
builder.Services.AddTransient<UpdateCancellationPolicyUseCase>();
builder.Services.AddTransient<DeleteCancellationPolicyUseCase>();
builder.Services.AddTransient<GetAllCancellationPoliciesUseCase>();
builder.Services.AddTransient<GetCancellationPolicyByIdUseCase>();
builder.Services.AddTransient<CancellationPolicyValidation>();

// Rental UseCases
builder.Services.AddTransient<AddRentalUseCase>();
builder.Services.AddTransient<CancelRentalUseCase>();
builder.Services.AddTransient<GetAllRentalsByCustomerUseCase>();
builder.Services.AddTransient<GetRentalsInBranchUseCase>();
builder.Services.AddTransient<PickupVehicleUseCase>();
builder.Services.AddTransient<ReturnVehicleUseCase>();
builder.Services.AddTransient<AddUserRentalUseCase>();
builder.Services.AddTransient<InvalidateRentalUseCase>();
builder.Services.AddTransient<GetAllRentalsUseCase>();

builder.Services.AddTransient<GetAllCustomersUseCase>();

// Statistics UseCases
builder.Services.AddTransient<GetGeneralStatisticsUseCase>();
builder.Services.AddTransient<GetRentedVehiclesUseCase>();
builder.Services.AddTransient<GetWeeklyIncomeUseCase>();
builder.Services.AddTransient<GetDBUseCase>();

builder.Services.AddTransient<RentalValidation>();

builder.Services.AddTransient<SaveImageUseCase>();

builder.Services.AddSingleton<IEmailQueueRepository, EmailQueueRepository>();
builder.Services.AddHostedService<EmailBackgroundService>();

builder.Services.AddScoped<DependencyProvider>();

builder.Services.AddTransient<DatabaseSeeder>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // o tu URL de frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

    // Configuración para el esquema de seguridad de JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa el token JWT así: Bearer {tu token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var profilePicturesPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "Uploads");
if (!Directory.Exists(profilePicturesPath))
{
    Directory.CreateDirectory(profilePicturesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(profilePicturesPath),
    RequestPath = "/images"
});

app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAdminUserAsync();
    await seeder.SeedEmailTemplatesAsync();
    await seeder.SeedVehiclesAsync();
    await seeder.SeedGeneralAsync();
}

app.Run();

