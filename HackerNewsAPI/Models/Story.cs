using System.Text.Json.Serialization;

namespace HackerNewsAPI.Models
{
    public class Story
    {
        /// <summary>
        /// The id of the story
        /// </summary> 
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The title of the story
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The type of the story
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }


        /// <summary>
        /// The URL of the story
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// The username of the story's author
        /// </summary>
        [JsonPropertyName("by")]
        public string By { get; set; }

        /// <summary>
        /// Creation date of the story
        /// </summary>
        [JsonPropertyName("time")]
        public long Time { get; set; }

        /// <summary>
        /// The story's score
        /// </summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>
        /// Collection of HackerNews IDs the belongs to this Story a.k.a. comments
        /// </summary>
        [JsonPropertyName("kids")]
        public List<int> Kids { get; set; }
        /// <summary>
        /// Collection of HackerNews IDs the belongs to this Story a.k.a. comments
        /// </summary>
        [JsonPropertyName("descendants")]
        public List<int> Descendents { get; set; }
    }
}
