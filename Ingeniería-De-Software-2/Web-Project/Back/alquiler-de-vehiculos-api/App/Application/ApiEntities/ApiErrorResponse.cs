namespace AlquilerDeVehiculosApi.App.Application.ApiEntities
{
    public class ApiErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public string TraceId { get; set; }

        public ApiErrorResponse(string error = "ha ocurrido un error", int status = 400, string type = "Error", string title = "One or more validation errors occurred.", string traceId = "") {
            Errors["generalError"] = [error];
            Status = status;
            Type = type;
            Title = title;
            TraceId = traceId;
        }
        public ApiErrorResponse(Dictionary<string, string[]> errors, int status = 400, string type = "Error", string title = "One or more validation errors occurred.", string traceId = "") {
            Errors = errors;
            Status = status;
            Type = type;
            Title = title;
            TraceId = traceId;
        }
    }
}
