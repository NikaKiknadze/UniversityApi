namespace UniversityApi.CustomResponses
{
    public class ApiResponse<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccesResult(T? data)
        {
            return new ApiResponse<T>()
            {
                Success = true,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResult(string? error)
        {
            return new ApiResponse<T>()
            {
                Success = false,
                Message = error
            };
        }
    }
}
