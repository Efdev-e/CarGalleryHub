using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarGalleryHub.MVC.Services
{
    public partial class ApiClient
    {
        public class ApiResult<T> 
        {

            public bool Success { get; set; }
            [JsonPropertyName("msg")]
            public string Message { get; set; } = string.Empty;

            [JsonPropertyName("_data")]
            public T? Data { get; set; }
            public List<string> Errors { get; set; } = new();
            public static ApiResult<T> Fail(string message) => new() { Success = false, Message = message };
        }
    }
}
