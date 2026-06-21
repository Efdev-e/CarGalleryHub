namespace CarGalleryHub.MVC.Services
{
    public partial class ApiClient
    {
        public class ApiResult<T> 
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T? Data { get; set; }
            public List<string> Errors { get; set; } = new();
            public static ApiResult<T> Fail(string message) => new() { Success = false, Message = message };
        }
    }
}
