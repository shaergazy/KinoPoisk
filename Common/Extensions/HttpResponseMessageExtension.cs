using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<T> FromJson<T>(this HttpResponseMessage response) where T : class
        {
            return (await response.Content.ReadAsStringAsync()).FromJson<T>();
        }
    }
}
