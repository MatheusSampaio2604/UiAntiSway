using Infra.RequestApi.Interface;
using Newtonsoft.Json;
using System.Text;

namespace Infra.RequestApi
{
    public class GeneralApi : InterGeneralApi
    {
        private readonly HttpClient _httpClient;

        public GeneralApi(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                if (typeof(TResponse) == typeof(string))
                {
                    return (TResponse)(object)responseData;
                }

                if (typeof(TResponse) == typeof(bool))
                {
                    return (TResponse)(object)Convert.ToBoolean(responseData);
                }

                return JsonConvert.DeserializeObject<TResponse>(responseData);
            }
            else
            {
                throw new HttpRequestException($"Failed to GET data from {url}. Status code: {response.StatusCode}");
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                if (typeof(TResponse) == typeof(string))
                {
                    return (TResponse)(object)responseData;
                }

                if (typeof(TResponse) == typeof(bool))
                {
                    return (TResponse)(object)Convert.ToBoolean(responseData);
                }
                return JsonConvert.DeserializeObject<TResponse>(responseData);
            }
            else
            {
                throw new HttpRequestException($"Failed to POST data to {url}. Status code: {response.StatusCode}");
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                if (typeof(TResponse) == typeof(string))
                {
                    return (TResponse)(object)responseData;
                }

                if (typeof(TResponse) == typeof(bool))
                {
                    return (TResponse)(object)Convert.ToBoolean(responseData);
                }

                return JsonConvert.DeserializeObject<TResponse>(responseData);
            }
            else
            {
                throw new HttpRequestException($"Failed to PUT data to {url}. Status code: {response.StatusCode}");
            }
        }

        public async Task<TResponse?> DeleteAsync<TRequest, TResponse>(string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                if (typeof(TResponse) == typeof(string))
                    return (TResponse)(object)responseData;

                if (typeof(TResponse) == typeof(bool))
                    return (TResponse)(object)Convert.ToBoolean(responseData);

                return JsonConvert.DeserializeObject<TResponse>(responseData);
            }
            else
            {
                throw new HttpRequestException($"Failed to DELETE data from {url}. Status code: {response.StatusCode}");
            }
        }
    }
}
