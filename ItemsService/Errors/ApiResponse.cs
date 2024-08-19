
namespace OrdersAndItemsService.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string message="")
        {
            StatusCode = statusCode;
            ErrorMessage= message??GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have mode",
                401 => "Authorized,you are not",
                404 => "Resource found,it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger",
                _ => ""
            };

        }

        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
