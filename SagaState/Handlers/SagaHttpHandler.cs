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

        public bool CanHandle(SagaActivity sagaActivity)
        {
            return sagaActivity.GetType() == typeof(HttpSagaActivity);
        }

        public async Task<SagaTransResult> Execute(SagaActivity sagaActivity, dynamic data)
        {
            try
            {
                
                var httpActivity = (HttpSagaActivity)sagaActivity;
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
                sagaActivity.SetData(deserializeObject);
                return new SagaTransResult()
                {
                    SagaActivity = sagaActivity,
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