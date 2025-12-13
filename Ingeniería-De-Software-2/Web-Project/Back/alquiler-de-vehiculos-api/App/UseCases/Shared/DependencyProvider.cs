
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Validations;
using AlquilerDeVehiculosApi.App.UseCases.Branches;
using AlquilerDeVehiculosApi.App.UseCases.Brands;
using AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies;
using AlquilerDeVehiculosApi.App.UseCases.Employees;
using AlquilerDeVehiculosApi.App.UseCases.VehicleTypes;

namespace AlquilerDeVehiculosApi.App.UseCases.Shared
{
    public class DependencyProvider
    {
        // Vehicle UseCases
        public AddVehicleUseCase AddVehicle { get; private set; }
        public DeleteVehicleUseCase DeleteVehicle { get; private set; }
        public GetAvailableVehiclesUseCase GetAvailableVehicles { get; private set; }
        public GetVehicleByIdUseCase GetVehicleById { get; private set; }
        public UpdateVehicleUseCase UpdateVehicle { get; private set; }
        public VehicleValidation VehicleValidation { get; private set; }
        public SetMaintenanceVehicleUseCase SetMaintenanceVehicle { get; private set; }
        public GetAvailableVehiclesByBranchUseCase GetAvailableVehiclesByBranch { get; private set; }
        
        // Branch UseCases
        public AddBranchUseCase AddBranch { get; private set; }
        public DeleteBranchUseCase DeleteBranch { get; private set; }
        public GetAllBranchesUseCase GetAllBranches { get; private set; }
        public GetBranchByIdUseCase GetBranchById { get; private set; }
        public UpdateBranchUseCase UpdateBranch { get; private set; }
        public BranchValidation BranchValidation { get; private set; }

        // Brand UseCases
        public AddBrandUseCase AddBrand { get; private set; }
        public UpdateBrandUseCase UpdateBrand { get; private set; }
        public DeleteBrandUseCase DeleteBrand { get; private set; }
        public GetAllBrandsUseCase GetAllBrands { get; private set; }
        public GetBrandByIdUseCase GetBrandById { get; private set; }
        public BrandValidation BrandValidation { get; private set; }
        
        // VehicleType UseCases
        public AddVehicleTypeUseCase AddVehicleType { get; private set; }
        public UpdateVehicleTypeUseCase UpdateVehicleType { get; private set; }
        public DeleteVehicleTypeUseCase DeleteVehicleType { get; private set; }
        public GetAllVehicleTypesUseCase GetAllVehicleTypes { get; private set; }
        public GetVehicleTypeByIdUseCase GetVehicleTypeById { get; private set; }
        public VehicleTypeValidation VehicleTypeValidation { get; private set; }
        public GetAllVehicleTypeAvailablesUseCase GetAllVehicleTypeAvailables { get; private set; }

        // Auth UseCases
        public DeleteUserUseCase DeleteUser { get; private set; }
        public SignUpUserUseCase SignUpUser { get; private set; }
        public LoginUserUseCase LoginUser { get; private set; }
        public ForgotPasswordUserUseCase ForgotPasswordUser { get; private set; }
        public ResetPasswordUserUseCase ResetPasswordUser { get; private set; }
        public GenerateSignUpCodeUseCase GenerateSignUpCode { get; private set; }
        public GenerateLoginCodeUseCase GenerateLoginCode { get; private set; }
        public ConvertInEmailTemplateUseCase ConvertInEmailTemplate { get; private set; }
        public UserValidation UserValidation { get; private set; }
        
        // Employee UseCases
        public AddEmployeeUseCase AddEmployee { get; private set; }
        public GetAllEmployeesUseCase GetAllEmployees { get; private set; }
        public GetEmployeeByIdUseCase GetEmployeeById { get; private set; }
        public DeleteEmployeeUseCase DeleteEmployee { get; private set; }
        public EmployeeValidation EmployeeValidation { get; private set; }
        public UpdateEmployeeUseCase UpdateEmployee { get; private set; }

        // CancellationPolicy UseCases
        public AddCancellationPolicyUseCase AddCancellationPolicy { get; private set; }
        public UpdateCancellationPolicyUseCase UpdateCancellationPolicy { get; private set; }
        public DeleteCancellationPolicyUseCase DeleteCancellationPolicy { get; private set; }
        public GetAllCancellationPoliciesUseCase GetAllCancellationPolicies { get; private set; }
        public GetCancellationPolicyByIdUseCase GetCancellationPolicyById { get; private set; }
        public CancellationPolicyValidation CancellationPolicyValidation { get; private set; }

        // Rental UseCases
        public AddRentalUseCase AddRental { get; private set; }
        public CancelRentalUseCase CancelRental { get; private set; }
        public GetAllRentalsByCustomerUseCase GetAllRentalsByCustomer { get; private set; }
        public GetRentalsInBranchUseCase GetRentalsInBranch { get; private set; }
        public PickupVehicleUseCase PickupVehicle { get; private set; }
        public ReturnVehicleUseCase ReturnVehicle { get; private set; }
        public AddUserRentalUseCase AddUserRental { get; private set; }
        public InvalidateRentalUseCase InvalidateRental { get; private set; }
        public GetAllRentalsUseCase GetAllRentals { get; private set; }

        public GetAllCustomersUseCase GetAllCustomers { get; private set; }

        // Statistics
        public GetGeneralStatisticsUseCase GetGeneralStatistics { get; private set; }
        public GetRentedVehiclesUseCase GetRentedVehicles { get; private set; }
        public GetWeeklyIncomeUseCase GetWeeklyIncome { get; private set; }
        public GetDBUseCase GetDB { get; private set; }
        
        public SaveImageUseCase SaveImage
        { get; private set; }
        
        public RentalValidation RentalValidation { get; private set; }

        public DependencyProvider(
            // Vehicle dependencies
            AddVehicleUseCase addVehicle,
            DeleteVehicleUseCase deleteVehicleUseCase,
            GetAvailableVehiclesUseCase getAvailableVehicles,
            GetVehicleByIdUseCase getVehicleByIdUseCase,
            UpdateVehicleUseCase updateVehicle,
            VehicleValidation vehicleValidation,
            SetMaintenanceVehicleUseCase setMaintenanceVehicle,
            GetAvailableVehiclesByBranchUseCase getAvailableVehiclesByBranch,

            // Branch dependencies
            AddBranchUseCase addBranch,
            DeleteBranchUseCase deleteBranch,
            GetAllBranchesUseCase getAllBranches,
            GetBranchByIdUseCase getBranchById,
            UpdateBranchUseCase updateBranch,
            BranchValidation branchValidation,

            // Brand dependencies
            AddBrandUseCase addBrand,
            UpdateBrandUseCase updateBrand,
            DeleteBrandUseCase deleteBrand,
            GetAllBrandsUseCase getAllBrands,
            GetBrandByIdUseCase getBrandById,
            BrandValidation brandValidation,

            // VehicleType dependencies
            AddVehicleTypeUseCase addVehicleType,
            UpdateVehicleTypeUseCase updateVehicleType,
            DeleteVehicleTypeUseCase deleteVehicleType,
            GetAllVehicleTypesUseCase getAllVehicleTypes,
            GetVehicleTypeByIdUseCase getVehicleTypeById,
            VehicleTypeValidation vehicleTypeValidation,
            GetAllVehicleTypeAvailablesUseCase getAllVehicleTypeAvailables,

            // Auth dependencies
            DeleteUserUseCase deleteUser,
            SignUpUserUseCase signUpUser,
            LoginUserUseCase loginUser,
            ForgotPasswordUserUseCase forgotPasswordUser,
            ResetPasswordUserUseCase resetPasswordUser,
            GenerateSignUpCodeUseCase generateSignUpCode,
            GenerateLoginCodeUseCase generateLoginCode,
            ConvertInEmailTemplateUseCase convertInEmailTemplate,
            UserValidation userValidation,

            // Employee dependencies
            AddEmployeeUseCase addEmployee,
            GetAllEmployeesUseCase getAllEmployees,
            GetEmployeeByIdUseCase getEmployeeById,
            DeleteEmployeeUseCase deleteEmployee,
            EmployeeValidation employeeValidation,
            UpdateEmployeeUseCase updateEmployee,

            // CancellationPolicy dependencies
            AddCancellationPolicyUseCase addCancellationPolicy,
            UpdateCancellationPolicyUseCase updateCancellationPolicy,
            DeleteCancellationPolicyUseCase deleteCancellationPolicy,
            GetAllCancellationPoliciesUseCase getAllCancellationPolicies,
            GetCancellationPolicyByIdUseCase getCancellationPolicyById,
            CancellationPolicyValidation cancellationPolicyValidation,

            // Customer dependencies
            GetAllCustomersUseCase getAllCustomers,

            // Rental dependencies
            AddRentalUseCase addRental,
            CancelRentalUseCase cancelRental,
            GetAllRentalsByCustomerUseCase getAllRentalsByCustome,
            GetRentalsInBranchUseCase getRentalsInBranch,
            PickupVehicleUseCase pickupVehicle,
            ReturnVehicleUseCase returnVehicle,
            AddUserRentalUseCase addUserRental,
            InvalidateRentalUseCase invalidateRental,
            GetAllRentalsUseCase getAllRentals,

            // Statistics dependencies
            GetGeneralStatisticsUseCase getGeneralStatistics,
            GetRentedVehiclesUseCase getRentedVehicles,
            GetWeeklyIncomeUseCase getWeeklyIncome,
            GetDBUseCase getDB,

            SaveImageUseCase saveImage,

            RentalValidation rentalValidation)
        {
            // Vehicle assignments
            AddVehicle = addVehicle;
            DeleteVehicle = deleteVehicleUseCase;
            GetAvailableVehicles = getAvailableVehicles;
            GetVehicleById = getVehicleByIdUseCase;
            UpdateVehicle = updateVehicle;
            VehicleValidation = vehicleValidation;
            SetMaintenanceVehicle = setMaintenanceVehicle;
            GetAvailableVehiclesByBranch = getAvailableVehiclesByBranch;

            // Branch assignments
            AddBranch = addBranch;
            DeleteBranch = deleteBranch;
            GetAllBranches = getAllBranches;
            GetBranchById = getBranchById;
            UpdateBranch = updateBranch;
            BranchValidation = branchValidation;

            // Brand assignments
            AddBrand = addBrand;
            UpdateBrand = updateBrand;
            DeleteBrand = deleteBrand;
            GetAllBrands = getAllBrands;
            GetBrandById = getBrandById;
            BrandValidation = brandValidation;

            // VehicleType assignments
            AddVehicleType = addVehicleType;
            UpdateVehicleType = updateVehicleType;
            DeleteVehicleType = deleteVehicleType;
            GetAllVehicleTypes = getAllVehicleTypes;
            GetVehicleTypeById = getVehicleTypeById;
            VehicleTypeValidation = vehicleTypeValidation;
            GetAllVehicleTypeAvailables = getAllVehicleTypeAvailables;

            GetAllCustomers = getAllCustomers;

            // Auth assignments
            DeleteUser = deleteUser;
            SignUpUser = signUpUser;
            LoginUser = loginUser;
            ForgotPasswordUser = forgotPasswordUser;
            ResetPasswordUser = resetPasswordUser;
            GenerateSignUpCode = generateSignUpCode;
            GenerateLoginCode = generateLoginCode;
            ConvertInEmailTemplate = convertInEmailTemplate;
            UserValidation = userValidation;

            // Employee assignments
            AddEmployee = addEmployee;
            GetAllEmployees = getAllEmployees;
            GetEmployeeById = getEmployeeById;
            DeleteEmployee = deleteEmployee;
            EmployeeValidation = employeeValidation;
            UpdateEmployee = updateEmployee;

            // CancellationPolicy assignments
            AddCancellationPolicy = addCancellationPolicy;
            UpdateCancellationPolicy = updateCancellationPolicy;
            DeleteCancellationPolicy = deleteCancellationPolicy;
            GetAllCancellationPolicies = getAllCancellationPolicies;
            GetCancellationPolicyById = getCancellationPolicyById;
            CancellationPolicyValidation = cancellationPolicyValidation;

            // Rental assignments
            AddRental = addRental;
            CancelRental = cancelRental;
            GetAllRentalsByCustomer = getAllRentalsByCustome;
            GetRentalsInBranch = getRentalsInBranch;
            PickupVehicle = pickupVehicle;
            ReturnVehicle = returnVehicle;
            AddUserRental = addUserRental;
            InvalidateRental = invalidateRental;
            GetAllRentals = getAllRentals;

            // Statistics assignments
            GetGeneralStatistics = getGeneralStatistics;
            GetRentedVehicles = getRentedVehicles;
            GetWeeklyIncome = getWeeklyIncome;
            GetDB = getDB;

            SaveImage = saveImage;

            RentalValidation = rentalValidation;
        }
    }
}