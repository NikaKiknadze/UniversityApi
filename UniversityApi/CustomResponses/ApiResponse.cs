namespace UniversityApi.CustomResponses
{
    public class ApiResponse<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; } = default(T);

        public static ApiResponse<T> SuccesResult(T Data)
        {
            return new ApiResponse<T>()
            {
                Success = true,
                Data = Data
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
