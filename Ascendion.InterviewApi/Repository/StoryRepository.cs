using Ascendion.InterviewApi.Model;
using Ascendion.InterviewApi.Service.Contract;

namespace Ascendion.InterviewApi.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IStoryService _storyService;
        public StoryRepository(IStoryService storyService)
        {
            _storyService = storyService;
        }

        public Story[] GetBestStories(int numberOfStories)
        {
            return _storyService.GetBestStories(numberOfStories).Select(GetStory).OrderByDescending(x => x.Score).ToArray(); ;
        }

        private Story GetStory(StoryApiModel storyApiModel)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return new Story()
            {
                PostedBy = storyApiModel.By,
                CommentCount = storyApiModel.Descendants,
                Score = storyApiModel.Score,
                Title = storyApiModel.Title,
                Uri = storyApiModel.Url,
                Time = dateTime.AddSeconds(storyApiModel.Time).ToLocalTime()
            };
        }
    }
}
