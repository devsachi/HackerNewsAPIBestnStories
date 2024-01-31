using HackerNewsAPI.Models;
using HackerNewsAPI.Services.Interfaces;

namespace HackerNewsAPI.Services
{
    /// <summary>
    /// Service which provides the best n stories by Score from cached data
    /// </summary>
    public class StoriesService : IStoriesService
    {
        private readonly IStoriesCache _storiesCache;
        private readonly ILogger<StoriesService> _logger;
        public StoriesService(ILogger<StoriesService> logger, IStoriesCache storiesCache)
        {
            _logger = logger;
            _storiesCache = storiesCache;
        }

        public async Task<List<Story>> GetBestStoriesAsync(int count)
        {
            _logger.LogInformation("Getting best n Stories from Cache");
            var cachedStories = await _storiesCache.GetCachedStoriesAsync(count);
            // This is whee the best n by score is achieved
            return cachedStories.OrderByDescending(s => s.Score).ToList(); ;
        }
    }
}
