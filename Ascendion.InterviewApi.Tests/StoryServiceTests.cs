using Ascendion.InterviewApi.ApplicationParameters;
using Ascendion.InterviewApi.Model;
using Ascendion.InterviewApi.Service;
using Ascendion.InterviewApi.Service.Contract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Ascendion.InterviewApi.Tests
{
    [TestFixture]
    public class StoryServiceTests
    {
        private Mock<IMemoryCache> _memoryCacheMock;
        private Mock<IOptions<AppParams>> _configMock;
        private Mock<IHttpRequestProcessor> _httpRequestProcessorMock;
        private Mock<ILogger<IStoryService>> _loggerMock;
        private StoryService _storyService;

        [SetUp]
        public void Setup()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _configMock = new Mock<IOptions<AppParams>>();
            _httpRequestProcessorMock = new Mock<IHttpRequestProcessor>();
            _loggerMock = new Mock<ILogger<IStoryService>>();
            _storyService = new StoryService(_memoryCacheMock.Object, _configMock.Object, _httpRequestProcessorMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetBestStories_ReturnsStories_WhenCacheIsEmpty()
        {
            // Arrange
            var appParams = new AppParams
            {
                StoryIdUrl = "http://example.com/storyids",
                StoryUrl = "http://example.com/story/1",
                AbsoluteExpiration = 150,
                SlidingExpiration = 120,
                IsStoryIdsSorted = true
            };
            _configMock.Setup(x => x.Value).Returns(appParams);

            var storyIds = new[] { 1, 2, 3 };
            _httpRequestProcessorMock.Setup(x => x.GetHttpResponseMessage(appParams.StoryIdUrl))
                .Returns(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(storyIds)
                });

            var story = new StoryApiModel { Id = 1, Title = "Test Story", Score = 100 };
            _httpRequestProcessorMock.Setup(x => x.GetHttpResponseMessage(appParams.StoryUrl))
                .Returns(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(story)
                });
            _memoryCacheMock.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);
            _memoryCacheMock.Setup(mc => mc.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = _storyService.GetBestStories(1);

            // Assert
            Assert.AreEqual("Test Story", result[0].Title);


        }

        [Test]
        public void GetBestStoryIds_ReturnsIds_WhenCacheIsEmpty()
        {
            // Arrange
            var appParams = new AppParams
            {
                StoryIdUrl = "http://example.com/storyids",
                StoryUrl = "http://example.com/story/{0}",
                AbsoluteExpiration = 150,
                SlidingExpiration = 120,
                IsStoryIdsSorted = true
            };
            _configMock.Setup(x => x.Value).Returns(appParams);

            var storyIds = new[] { 1, 2, 3 };
            _httpRequestProcessorMock.Setup(x => x.GetHttpResponseMessage(appParams.StoryIdUrl))
                .Returns(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(storyIds)
                });
            _memoryCacheMock.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);
            _memoryCacheMock.Setup(mc => mc.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = _storyService.GetBestStoryIds();

            // Assert
            Assert.AreEqual(storyIds, result);

        }
    }
}
