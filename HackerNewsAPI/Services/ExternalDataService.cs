using HackerNewsAPI.Models;
using HackerNewsAPI.Services.Interfaces;
using Newtonsoft.Json;

namespace HackerNewsAPI.Services
{
 /// <summary>
 /// used to fetch data from external source , in this case hacker-news
 /// </summary>
    public class ExternalDataService : IExternalDataService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";
        private readonly ILogger<ExternalDataService> _logger;

        public ExternalDataService(IHttpClientFactory clientFactory, ILogger<ExternalDataService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<List<int>> GetBestStoryIds()
        {
            _logger.LogInformation("Getting best Story Ids");
            using var httpClient = _clientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/beststories.json").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var storyIdsJson = await response.Content.ReadAsStringAsync();
                var bestStoriesIds = JsonConvert.DeserializeObject<List<int>>(storyIdsJson);
                _logger.LogInformation("Best Story Ids retrieved ");
                return bestStoriesIds;

            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                _logger.LogError(msg);
                throw new Exception(msg);
            }
        }

        public async Task<Story> GetStoryAsync(int storyId)
        {
            _logger.LogInformation($"Getting Story for id {storyId}");
            using var httpClient = _clientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/item/{storyId}.json").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var storyJson = await response.Content.ReadAsStringAsync();
                var story = JsonConvert.DeserializeObject<Story>(storyJson);
                _logger.LogInformation($"Retrived Story for id {storyId}");
                return story ?? new Story();

            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                _logger.LogError(msg);
                return new Story();
            }
        }
    }
}
