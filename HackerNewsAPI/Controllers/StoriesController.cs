using HackerNewsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    [ApiController]
    [Route("api/stories")]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly IStoriesService _storiesService;

        public StoriesController(ILogger<StoriesController> logger, IStoriesService storiesService)
        {
            _logger = logger;
            _storiesService = storiesService;
        }

        [HttpGet("{count}", Name = "GetBestStories")]
        [ResponseCache(CacheProfileName = "OneMinuteCacheProfile")]
        public async Task<IActionResult> GetBestStories(int count)
        {
            var stories = await _storiesService.GetBestStoriesAsync(count);
            if (stories == null || stories.Count == 0)
            {
                return NotFound();
            }
            return Ok(stories);
        }
    }
}
