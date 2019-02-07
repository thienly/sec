using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Driver;
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
                    var handler = _sagaHandlers.FirstOrDefault(x=>x.CanHandle(sagaStage.Trans));
                    var excute = handler.Excute(data);

                }
            }
            catch (Exception e)
            {
                throw new SagaException($"Can not excute saga {saga.Name} - {saga.Id}",e);
            }
            
            return Task.CompletedTask;
        }
    }

    public class SagaTransResult
    {
        public bool IsSuccess { get; set; }    
    }
    public interface ISagaHandler
    {
        bool CanHandle(IActivity activity);
        SagaTransResult Excute(dynamic data);
    }

    public class SagaHttpHandler : ISagaHandler
    {
        public bool CanHandle(IActivity activity)
        {
            return activity.GetType() == typeof(HttpActivity);
        }

        public SagaTransResult Excute(dynamic data)
        {
            throw new System.NotImplementedException();
        }
    }
}