namespace LikeTours.Data.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
     
        public int Status { get; set; }

        public ApiResponse(T data, int status = 200, bool success = true, string message = "")
        {
            Success = success;
            Message = message;
            Data = data;
            Status = status;
        }

    }

}
