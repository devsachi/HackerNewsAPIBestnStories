using HackerNewsAPI.Models;
using HackerNewsAPI.Services.Interfaces;

namespace HackerNewsAPI.Services
{
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
            var cachedStories = await _storiesCache.GetCachedStoriesAsync(count);
            return cachedStories.OrderByDescending(s => s.Score).ToList(); ;
        }
    }
}
