using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;
using SagaState.Instance;

namespace SagaState
{
    public class SagaEngine
    {
        private IEnumerable<ISagaHandler> _sagaHandlers;
        private HttpClient _client;
        private IMongoDatabase _mongoDatabase;
        public SagaEngine(HttpClient client, IEnumerable<ISagaHandler> sagaHandlers, IMongoDatabase mongoDatabase)
        {
            _client = client;
            _sagaHandlers = sagaHandlers;
            _mongoDatabase = mongoDatabase;
        }
        public Task Excute(Saga saga)
        {
            try
            {
                dynamic data = saga.Data;
                foreach (var sagaStage in saga.Trans)
                {
                    var handler = _sagaHandlers.FirstOrDefault(x => x.CanHandle(sagaStage.Trans));                    
                }
            }
            catch (Exception e)
            {
                throw new SagaException($"Can not excute saga {saga.Name} - {saga.Id}", e);
            }

            return Task.CompletedTask;
        }
    }

    public class SagaTransResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
    public interface ISagaHandler
    {
        bool CanHandle(Activity activity);
        Task<SagaTransResult> Excute(Activity activity, dynamic data);
    }

    public class SagaHttpHandler : ISagaHandler
    {
        private HttpClient _httpClient;
        private IMongoDatabase _mongoDatabase;
        public SagaHttpHandler(HttpClient httpClient, IMongoDatabase mongoDatabase)
        {
            _httpClient = httpClient;
            _mongoDatabase = mongoDatabase;
        }

        public bool CanHandle(Activity activity)
        {
            return activity.GetType() == typeof(HttpActivity);
        }

        public async Task<SagaTransResult> Excute(Activity activity, dynamic data)
        {
            try
            {
                var httpActivity = (HttpActivity)activity;
                var httpResponseMessage = await _httpClient.PostAsync(httpActivity.Url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                var ensureSuccessStatusCode = httpResponseMessage.EnsureSuccessStatusCode();
                var content = await ensureSuccessStatusCode.Content.ReadAsStringAsync();
                var deserializeObject = JsonConvert.DeserializeObject<ExpandoObject>(content);
                //_mongoDatabase
                return new SagaTransResult()
                {
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