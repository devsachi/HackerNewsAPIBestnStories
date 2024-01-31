using HackerNewsAPI.Models;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    public class StoriesCache : IStoriesCache, IDisposable
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
            List<Story> stories = new List<Story>();
            var cachedStory = await _externalDataService.GetStoryAsync(storyIds.First());
            await Parallel.ForEachAsync(storyIds, async (id, cancellationToken) =>
            {
                string cacheKey = "Stories" + id;
                if (!_memoryCache.TryGetValue(cacheKey, out Story cachedStory))

                {

                    cachedStory = await _externalDataService.GetStoryAsync(id);
                    _memoryCacheLock.EnterReadLock();
                    try
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                       .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                       .SetPriority(CacheItemPriority.Normal);
                        _memoryCache.Set(cacheKey, cachedStory, cacheEntryOptions);
                        stories.Add(cachedStory);
                    }
                    finally
                    {
                        _memoryCacheLock.ExitReadLock();
                    }

                }
            });
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

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _memoryCache.Dispose();
                _memoryCacheLock.Dispose();
            }
            _disposed = true;
        }
    }
}
