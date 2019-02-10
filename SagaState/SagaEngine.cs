using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Driver;
using SagaState.Handlers;
using SagaState.Instance;

namespace SagaState
{
    public class SagaEngine
    {
        private IEnumerable<ISagaHandler> _sagaHandlers;
        private IMongoDatabase _mongoDatabase;
        public SagaEngine(IEnumerable<ISagaHandler> sagaHandlers, IMongoDatabase mongoDatabase)
        {
            _sagaHandlers = sagaHandlers;
            _mongoDatabase = mongoDatabase;
        }
        public async Task Execute(Saga saga)
        {
            try
            {                               
            }
            catch (Exception e)
            {
                throw new SagaException($"Can not excute saga {saga.Name} - {saga.Id}", e);
            }
        }        
    }
}