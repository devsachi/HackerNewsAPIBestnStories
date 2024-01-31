using HackerNewsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    /// <summary>
    /// No versioning done
    /// It would be good to have versioning
    /// </summary>
    [ApiController]
    [Route("api/stories")]
    [Produces("application/json")]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly IStoriesService _storiesService;

        public StoriesController(ILogger<StoriesController> logger, IStoriesService storiesService)
        {
            _logger = logger;
            _storiesService = storiesService;
        }

        /// <summary>
        /// This API gives the best Stories n by Score
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("{count}", Name = "GetBestStories")]
        [ResponseCache(CacheProfileName = "TenMinuteCacheProfile")]
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
