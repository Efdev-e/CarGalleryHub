using System.Text.Json.Serialization;

namespace CarGalleryHub.MVC.Models.DTOs.Common
{
    public class PagedResultDto<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }
}
