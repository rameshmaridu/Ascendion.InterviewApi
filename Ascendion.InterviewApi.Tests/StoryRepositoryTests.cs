using Ascendion.InterviewApi.Model;
using Ascendion.InterviewApi.Repository;
using Ascendion.InterviewApi.Service.Contract;
using Moq;

namespace Ascendion.InterviewApi.Tests
{
    [TestFixture]
    public class StoryRepositoryTests
    {
        private Mock<IStoryService> _storyServiceMock;
        private StoryRepository _storyRepository;

        [SetUp]
        public void Setup()
        {
            _storyServiceMock = new Mock<IStoryService>();
            _storyRepository = new StoryRepository(_storyServiceMock.Object);
        }

        [Test]
        public void GetBestStories_ReturnsOrderedStories()
        {
            // Arrange
            var storyApiModels = new[]
            {
                new StoryApiModel { Id = 1, Score = 100, Title = "Story 1" },
                new StoryApiModel { Id = 2, Score = 200, Title = "Story 2" },
                new StoryApiModel { Id = 3, Score = 150, Title = "Story 3" }
            };

            _storyServiceMock.Setup(s => s.GetBestStories(It.IsAny<int>())).Returns(storyApiModels);

            // Act
            var result = _storyRepository.GetBestStories(3);

            // Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("Story 2", result[0].Title);
            Assert.AreEqual("Story 3", result[1].Title);
            Assert.AreEqual("Story 1", result[2].Title);
        }

        [Test]
        public void GetBestStories_HandlesEmptyResult()
        {
            // Arrange
            _storyServiceMock.Setup(s => s.GetBestStories(It.IsAny<int>())).Returns(Array.Empty<StoryApiModel>());

            // Act
            var result = _storyRepository.GetBestStories(3);

            // Assert
            Assert.AreEqual(0, result.Length);
        }
    }
}
