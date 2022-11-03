namespace Jobsity.EwsChat.Server.ExternalClients
{
    public interface IHttpClientPostWraperBase<T> where T : class
    {
        Task<T> Post(T postObject, string url, Dictionary<string, string>? headers = null);
    }
}
