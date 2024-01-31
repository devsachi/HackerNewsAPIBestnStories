using HackerNewsAPI.Models;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    /// <summary>
    /// Caching Service used for faster response time
    /// </summary>
    public class StoriesCache : IStoriesCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IExternalDataService _externalDataService;
        private readonly ILogger<StoriesCache> _logger;
        private readonly ReaderWriterLockSlim _memoryCacheLock = new ReaderWriterLockSlim();
        private bool _disposed;

        public StoriesCache(ILogger<StoriesCache> logger, IExternalDataService externalDataService, IMemoryCache memoryCache)
        {

            _logger = logger;
            _memoryCache = memoryCache;
            _externalDataService = externalDataService;
        }

        public async Task<List<Story>> GetCachedStoriesAsync(int count)
        {
            var storyIds = await GetCachedStoryIdsAsync();
            List<Story> stories = [];
            string cacheKey = "Stories";
            if (_memoryCache.TryGetValue(cacheKey, out List<Story> cachedStories))
                return cachedStories?? stories;
            await Parallel.ForEachAsync(storyIds, async (storyId, cancellationToken) =>
        {
            var cachedStory = await _externalDataService.GetStoryAsync(storyId);

            stories.Add(cachedStory);
        });
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                       .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                       .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(cacheKey, stories, cacheEntryOptions);
            return stories;
        }

        private async Task<List<int>> GetCachedStoryIdsAsync()
        {
            const string cacheKey = "StoryIds";

            if (!_memoryCache.TryGetValue(cacheKey, out List<int> cachedStoryIds))
            {
                cachedStoryIds = await _externalDataService.GetBestStoryIds(); ;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                  .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                  .SetPriority(CacheItemPriority.Normal);
                _memoryCache.Set(cacheKey, cachedStoryIds, TimeSpan.FromMinutes(10));

            }

            return cachedStoryIds ?? [];

        }

       
    }
}
