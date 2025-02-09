namespace Ascendion.InterviewApi.ApplicationParameters
{
    public class AppParams
    {
        public string StoryIdUrl { get; set; }
        public string StoryUrl { get; set; }
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
        public bool IsStoryIdsSorted { get; set; }
    }
}
