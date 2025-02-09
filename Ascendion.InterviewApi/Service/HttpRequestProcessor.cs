using Ascendion.InterviewApi.Service.Contract;

namespace Ascendion.InterviewApi.Service
{
    public class HttpRequestProcessor : IHttpRequestProcessor
    {
        public HttpResponseMessage GetHttpResponseMessage(string url)
        {
            using HttpClient client = new();
            return client.GetAsync(url).Result;
        }
    }
}
