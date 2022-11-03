using System.Net.Mime;
using System.Text.Json;

namespace Jobsity.EwsChat.Server.ExternalClients
{
    public abstract class HttpClientWrapperBase<T> : IHttpClientGetWraperBase<T> where T : class
    {
        private readonly HttpClient _httpClient;

        protected HttpClientWrapperBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get(string url, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));

            CreateRequestHeaders(headers, request);

            return await HandleRequest(request);
        }

        private static void CreateRequestHeaders(Dictionary<string, string>? headers, HttpRequestMessage request)
        {
            if (headers == null) return;
            foreach (var headerKey in headers.Keys)
            {
                request.Headers.Add(headerKey, headers[headerKey]);
            }
        }

        private async Task<T> HandleRequest(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            dynamic? resultContent;
            if (typeof(T) == typeof(Stream))
            {
                resultContent = await response.Content.ReadAsStreamAsync();
                return resultContent;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            resultContent = responseContent;

            var contentType = response.Content.Headers.ContentType;
            if (contentType is {MediaType: MediaTypeNames.Application.Json})
            {
                resultContent = JsonSerializer.Deserialize<T>(responseContent);
            }

            return resultContent ?? Activator.CreateInstance<T>();
        }
    }
}
