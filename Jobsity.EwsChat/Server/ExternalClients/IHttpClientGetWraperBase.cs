namespace Jobsity.EwsChat.Server.ExternalClients
{
    public interface IHttpClientGetWraperBase<T> where T : class
    {
        Task<T> Get(string url, Dictionary<string, string>? headers = null);
    }
}
