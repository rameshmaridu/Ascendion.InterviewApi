using Ascendion.InterviewApi.Model;

namespace Ascendion.InterviewApi.Service.Contract
{
    public interface IHttpRequestProcessor
    {
        // Note: only adding Get method for now, can add more methods as needed
        // Get the response message from the given URL
        HttpResponseMessage GetHttpResponseMessage(string url);
    }
}
