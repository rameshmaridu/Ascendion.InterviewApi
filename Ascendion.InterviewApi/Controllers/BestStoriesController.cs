using Ascendion.InterviewApi.Model;
using Ascendion.InterviewApi.Repository;
using Ascendion.InterviewApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ascendion.InterviewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly ILogger<BestStoriesController> _logger;
        private readonly IStoryRepository _storyRepository;

        public BestStoriesController(IStoryRepository storyRepository, ILogger<BestStoriesController> logger)
        {
            _logger = logger;
            _storyRepository = storyRepository;
        }

        [HttpGet("GetBestStories")]
        public async Task<IActionResult> Get(int numberOfStories)
        {
            try
            {
                return Ok(_storyRepository.GetBestStories(numberOfStories));
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}: Exception Occured, Message:{1}, Datetime:{2}", typeof(StoryService), ex.Message, DateTime.Now.ToString("G"));

                return BadRequest(ex.Message);
            }
        }
    }
}
