using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SagaState.Handlers;
using SagaState.Instance;

namespace SagaState
{
    public class SagaEngine
    {
        private readonly IEnumerable<ISagaHandler> _sagaHandlers;
        private readonly IMongoDatabase _mongoDatabase;
        public SagaEngine(IEnumerable<ISagaHandler> sagaHandlers, IMongoDatabase mongoDatabase)
        {
            _sagaHandlers = sagaHandlers;
            _mongoDatabase = mongoDatabase;
        }
        public async Task Execute(Saga saga)
        {
            try
            {
                var orderedSagaStages = saga.Stages.OrderBy(x=>x.Order).ToList();
                for (int i = 0; i < orderedSagaStages.Count; i++)
                {
                    orderedSagaStages[i].SetStatus(SagaStateStatus.Started);
                    var handler = _sagaHandlers.FirstOrDefault(x => x.CanHandle(orderedSagaStages[i].Trans));
                    SagaTransResult result = await handler.Execute(orderedSagaStages[i].Trans,saga.Data);
                    if (!result.IsSuccess)
                    {
                        if (!result.NeedToRunCompensating)
                        {
                            orderedSagaStages[i].SetStatus(SagaStateStatus.FailedAndNotNeedToRunCompensating);
                        }
                        else
                        {
                            orderedSagaStages[i].SetStatus(SagaStateStatus.Failed);                        
                        }                                                                        
                        await PerformRollBack(saga);
                        break;
                    }
                    orderedSagaStages[i].SetStatus(SagaStateStatus.Completed);
                }

                await _mongoDatabase.GetCollection<Saga>("SagaInstance").ReplaceOneAsync(
                    Builders<Saga>.Filter.Eq(x => x.Id, saga.Id),saga);
            }
            catch (Exception e)
            {
                throw new SagaException($"Can not excute saga {saga.Name} - {saga.Id}", e);
            }
        }

        private async Task PerformRollBack(Saga saga)
        {
            var sagaStages = saga.Stages.Where(x =>
                x.Status == SagaStateStatus.Completed || x.Status == SagaStateStatus.Failed);
            foreach (var sagaStage in sagaStages)
            {
                
                var handler = _sagaHandlers.FirstOrDefault(x => x.CanHandle(sagaStage.CompensatingTrans));
                SagaTransResult result = await handler.Execute(sagaStage.CompensatingTrans, saga.Data);
                if (result.IsSuccess)
                {                    
                    sagaStage.SetStatus(SagaStateStatus.CompensatingCompleted);    
                }
                else
                {
                    sagaStage.SetStatus(SagaStateStatus.CompensatingFailed);    
                }
            }            
        }
    }
}