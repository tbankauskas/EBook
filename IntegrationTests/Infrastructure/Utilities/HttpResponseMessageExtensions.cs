using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTests.Infrastructure.Utilities
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> Deserialize<T>(this HttpResponseMessage message)
        {
            var response = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
