namespace OrdersAndItemsService.Errors
{
    public class ApiException(int statusCode, string message = "", string details = "") : ApiResponse(statusCode,message)
    {
        public string Details { get; set; } = details;
    }
}
