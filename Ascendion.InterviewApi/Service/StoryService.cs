using Ascendion.InterviewApi.ApplicationParameters;
using Ascendion.InterviewApi.Model;
using Ascendion.InterviewApi.Service.Contract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ascendion.InterviewApi.Service
{
    public class StoryService : IStoryService
    {
        private readonly ILogger<IStoryService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<AppParams> _config;
        private readonly IHttpRequestProcessor _httpRequestProcessor;

        public StoryService(IMemoryCache memoryCache, IOptions<AppParams> config, IHttpRequestProcessor httpRequestProcessor, ILogger<IStoryService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _httpRequestProcessor = httpRequestProcessor ?? throw new ArgumentNullException(nameof(httpRequestProcessor));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public StoryApiModel[] GetBestStories(int n)
        {
            var result = new StoryApiModel[n];

            try
            {
                var appSettings = _config.Value;
                var bestStoryIds = GetBestStoryIds();

                if (appSettings.IsStoryIdsSorted)
                {
                    for (int i = 0; i < n; i++)
                    {
                        var storyId = bestStoryIds[i];

                        if (!_memoryCache.TryGetValue(storyId, out StoryApiModel story))
                        {
                            var url = string.Format(appSettings.StoryUrl, bestStoryIds[i]);
                            var response = _httpRequestProcessor.GetHttpResponseMessage(url);

                            if (response.IsSuccessStatusCode)
                            {
                                story = response.Content.ReadFromJsonAsync<StoryApiModel>().Result;
                                var cacheExpiryOptions = new MemoryCacheEntryOptions
                                {
                                    AbsoluteExpiration = DateTime.Now.AddSeconds(appSettings.AbsoluteExpiration),
                                    Priority = CacheItemPriority.High,
                                    SlidingExpiration = TimeSpan.FromSeconds(appSettings.SlidingExpiration)
                                };

                                _memoryCache.Set(storyId, story, cacheExpiryOptions);
                            }
                        }

                        result[i] = story;
                    }
                }
                else
                {
                    var stories = new List<StoryApiModel>();
                    foreach (var storyId in bestStoryIds)
                    {
                        if (!_memoryCache.TryGetValue(storyId, out StoryApiModel story))
                        {
                            var url = string.Format(appSettings.StoryUrl, storyId);
                            var response = _httpRequestProcessor.GetHttpResponseMessage(url);

                            if (response.IsSuccessStatusCode)
                            {
                                story = response.Content.ReadFromJsonAsync<StoryApiModel>().Result;
                                var cacheExpiryOptions = new MemoryCacheEntryOptions
                                {
                                    AbsoluteExpiration = DateTime.Now.AddSeconds(appSettings.AbsoluteExpiration),
                                    Priority = CacheItemPriority.High,
                                    SlidingExpiration = TimeSpan.FromSeconds(appSettings.SlidingExpiration)
                                };
                                _memoryCache.Set(storyId, story, cacheExpiryOptions);
                            }
                        }
                        stories.Add(story);
                    }
                    result = stories.OrderByDescending(x => x.Score).Take(n).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}: Exception Occurred, Message:{1}, Datetime:{2}", typeof(StoryService), ex.Message, DateTime.Now.ToString("G"));
            }
            return result;
        }

        public int[] GetBestStoryIds()
        {
            try
            {
                var appSettings = _config.Value;
                var cacheKey = "beststoryids";

                if (!_memoryCache.TryGetValue(cacheKey, out int[] storyIds))
                {
                    var response = _httpRequestProcessor.GetHttpResponseMessage(appSettings.StoryIdUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadFromJsonAsync<int[]>().Result;
                        storyIds = data ?? Array.Empty<int>();

                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddSeconds(150),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromSeconds(120)
                        };

                        _memoryCache.Set(cacheKey, storyIds, cacheExpiryOptions);
                    }
                }

                return storyIds;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}: Exception Occurred, Message:{1}, Datetime:{2}", typeof(StoryService), ex.Message, DateTime.Now.ToString("G"));
            }

            return Array.Empty<int>();
        }
    }
}
