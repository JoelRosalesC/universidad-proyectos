using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Application.Exceptions
{
    public class ApiException : Exception
    {
        public ApiErrorResponse Error { get; }

        public ApiException(ApiErrorResponse error)
            : base(error.Title)
        {
            Error = error;
        }
        public ApiException()
            : base("ha ocurrido un error")
        {
            Error = new ApiErrorResponse();
        }
    }
}
