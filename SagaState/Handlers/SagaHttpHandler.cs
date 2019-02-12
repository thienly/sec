using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SagaShared.Instance;

namespace SagaWorker.Handlers
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
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new SagaTransResult()
                    {
                        IsSuccess = false,
                        NeedToRunCompensating = true,
                        ErrorMessage = httpResponseMessage.ReasonPhrase
                    };
                }
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                var deserializeObject = JsonConvert.DeserializeObject<ExpandoObject>(content);
                activity.SetData(deserializeObject);
                return new SagaTransResult()
                {
                    Activity = activity,
                    IsSuccess = true
                };
            }
            catch (HttpRequestException e)
            {
                return new SagaTransResult()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                    NeedToRunCompensating = false
                };
            }
        }
    }
}