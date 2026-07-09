using Azure;
using CarGalleryHub.MVC.Exceptions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace CarGalleryHub.MVC.Services
{
    public partial class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private HttpRequestMessage CreateRequestAndReturn(string url, HttpMethod httpMethod) 
        {
            if (string.IsNullOrEmpty(url) || httpMethod == null)
                throw new AppException("Url or httpMethod is Empty");
            return CreateRequestAndReturn(url,httpMethod, null);
        }

        private HttpRequestMessage CreateRequestAndReturn(string url, HttpMethod httpMethod, object? body = null)
        {
            if (string.IsNullOrEmpty(url) || httpMethod == null)
                throw new AppException("Url or httpMethod is Empty");

            var request = new HttpRequestMessage();
            request.Method = httpMethod;
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var token = session.GetString("JwtToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }

            if (body != null)
                request.Content = Serialize(body);

            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            return request;
        }

        public async Task<ApiResult<T>> GetAsync<T>(string url) 
        {
            if (string.IsNullOrEmpty(url))
                throw new AppException("Url is empty");


            var request = CreateRequestAndReturn(url, HttpMethod.Get);

            var response = await _httpClient.SendAsync(request);

            return await ParseAsync<T>(response);
        }

        public async Task<ApiResult<T>> PostAsync<T>(string url, object body)
        {
            if (string.IsNullOrEmpty(url) || body == null)
                throw new AppException("Url or Body is empty");


            var request = CreateRequestAndReturn(url,HttpMethod.Post, body);

            var response = await _httpClient.SendAsync(request);

            return await ParseAsync<T>(response);
        }

        public async Task<ApiResult<T>> PostNoBodyAsync<T>(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new AppException("Url is empty");


            var request = CreateRequestAndReturn(url, HttpMethod.Post);

            var response = await _httpClient.SendAsync(request);

            return await ParseAsync<T>(response);
        }

        public async Task<ApiResult<T>> PutAsync<T>(string url, object body)
        {
            if (string.IsNullOrEmpty(url) || body == null)
                throw new AppException("Url or Body is empty");


            var request = CreateRequestAndReturn(url, HttpMethod.Put, body);

            var response = await _httpClient.SendAsync(request);

            return await ParseAsync<T>(response);
        }

        public async Task<ApiResult<T>> DeleteAsync<T>(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new AppException("Url is empty");


            var request = CreateRequestAndReturn(url, HttpMethod.Delete);

            var response = await _httpClient.SendAsync(request);

            return await ParseAsync<T>(response);
        }


        private static StringContent Serialize(object body) => new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");



        private static async Task<ApiResult<T>> ParseAsync<T>(HttpResponseMessage httpResponse) 
        {
            var json = await httpResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return httpResponse.IsSuccessStatusCode
                    ? ApiResult<T>.CreateSuccess()
                    : ApiResult<T>.Fail($"Http {(int)httpResponse.StatusCode}:İşlem basarısız ");
            }

            try 
            {
                var result = JsonSerializer.Deserialize<ApiResult<T>>(json, JsonOptions);
                return result ?? ApiResult<T>.Fail("Yanıt Ayrıştırılamadı");
            }
            catch 
            {
                return ApiResult<T>.Fail($"Http {(int)httpResponse.StatusCode}:İşlem basarısız ");
            }
        }
    }
}
