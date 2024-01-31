using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services.Interfaces
{
    public interface IStoriesCache
    {
        public Task<List<Story>> GetCachedStoriesAsync(int count);
    }
}
