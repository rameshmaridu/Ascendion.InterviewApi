using Ascendion.InterviewApi.Model;

namespace Ascendion.InterviewApi.Service.Contract
{
    public interface IStoryService
    {
        StoryApiModel[] GetBestStories(int numberOfStories);
        int[] GetBestStoryIds();
    }
}
