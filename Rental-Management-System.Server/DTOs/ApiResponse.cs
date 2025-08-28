namespace Rental_Management_System.Server.DTOs
{
    public class ApiResponse<T>
    {
        public bool success { get; set; } = true;
        public T Data { get; set; }
        public string Error { get; set; }

        public static ApiResponse<T> Fail (string error)
        {
            return new ApiResponse<T> { success = false, Error = error };
        }

        public ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T> {success = true, Data = data};
        }
    }

}
