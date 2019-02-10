using System;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SagaState.Instance;

namespace SagaState.Handlers
{
    public class SagaHttpHandler : ISagaHandler
    {
        private HttpClient _httpClient;
        public SagaHttpHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanHandle(Activity activity)
        {
            return activity.GetType() == typeof(HttpActivity);
        }

        public async Task<SagaTransResult> Execute(Activity activity, dynamic data)
        {
            try
            {
                var httpActivity = (HttpActivity)activity;
                var httpResponseMessage = await _httpClient.PostAsync(httpActivity.Url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                var ensureSuccessStatusCode = httpResponseMessage.EnsureSuccessStatusCode();
                var content = await ensureSuccessStatusCode.Content.ReadAsStringAsync();
                var deserializeObject = JsonConvert.DeserializeObject<ExpandoObject>(content);
                activity.SetData(deserializeObject);
                return new SagaTransResult()
                {
                    Activity = activity,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SagaTransResult()
                {
                    IsSuccess = true,
                    ErrorMessage = e.Message
                };
            }

        }
    }
}