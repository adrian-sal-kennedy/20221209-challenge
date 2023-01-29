using Newtonsoft.Json;

namespace Ch1FlyoutPageModel.Models
{
    /// <summary>
    /// Model to contain the API response we're working with. Looking at the data
    /// it appears to represent an Album, so I've named it thus. I've made it a
    /// record instead of a class to simplify equality testing in the hypothetical
    /// scenario of these being stored locally in an SQLite or similar database.
    /// </summary>
    public record Album : BaseModel
    {
        [JsonProperty("albumId")] public int AlbumId { get; set; }
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("title")] public string? Title { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("thumbnailUrl")] public string? ThumbnailUrl { get; set; }
    }
}
