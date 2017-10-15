using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;

namespace IOTA.Slackbot.Slack
{
    public static class JsonHttpUtils
    {
        public static async Task<TResponse> PostJsonAsync<TResponse>(
            this HttpClient client,
            string requestUri,
            object requestBody)
        {
            var response = await client.PostAsync(
                requestUri,
                requestBody.ToJsonHttpContent());
            return await response.EnsureSuccess().DeserializeAsync<TResponse>();
        }

        public static HttpResponseMessage EnsureSuccess(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            throw new Exception("Sucess status code was expected instead of " + response.StatusCode);
        }

        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
        {
            //Todo: Use stream instead ?
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception e)
            {
                throw new Exception("Fail to deserialize " + responseString);
            }
        }

        public static HttpContent ToJsonHttpContent(this Object obj)
        {
            return obj != null ?
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                : null;
        }
    }
}