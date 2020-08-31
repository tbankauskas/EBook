using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTests.Infrastructure.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> GetJson<TResult>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            return await response.Deserialize<TResult>();
        }

        public static async Task<TResult> PostJson<TResult>(this HttpClient client, string url, object payload)
        {
            var response = await client.PostJson(url, payload);
            return await response.Deserialize<TResult>();
        }

        public static async Task<HttpResponseMessage> PostJson(this HttpClient client, string url, object payload)
        {
            var content = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await client.PostAsync(url, stringContent);
        }
    }
}
