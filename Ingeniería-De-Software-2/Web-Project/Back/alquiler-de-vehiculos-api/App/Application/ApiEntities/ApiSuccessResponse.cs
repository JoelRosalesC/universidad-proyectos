namespace AlquilerDeVehiculosApi.App.Application.ApiEntities
{
    public class ApiSuccessResponse<T>
    {
        #nullable disable
        public T Data { get; set; }
        public object Meta { get; set; }
        #nullable restore
    }
}
