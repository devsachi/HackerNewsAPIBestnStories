using HackerNewsAPI.Models;
using System.Text.Json;

namespace HackerNewsAPI.Services.Interfaces
{
    public interface IExternalDataService
    {
        Task<List<int>> GetBestStoryIds();
        Task<Story> GetStoryAsync(int storyId);
    }
}
