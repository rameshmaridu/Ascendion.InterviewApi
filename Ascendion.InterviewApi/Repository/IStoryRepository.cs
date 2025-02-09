using Ascendion.InterviewApi.Model;

namespace Ascendion.InterviewApi.Repository
{
    public interface IStoryRepository
    {
        Story[] GetBestStories(int numberOfStories);
    }
}
