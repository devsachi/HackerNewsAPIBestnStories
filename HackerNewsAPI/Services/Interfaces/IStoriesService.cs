using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services.Interfaces
{
    public interface IStoriesService
    {
        public Task<List<Story>> GetBestStoriesAsync(int count);
    }
}
