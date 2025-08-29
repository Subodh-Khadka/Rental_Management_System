namespace Rental_Management_System.Server.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = null)
        {
            return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T> { Success = false, Data = default, Message = message };
        }
    }
}
