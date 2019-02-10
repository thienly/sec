using System;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SagaState.Definition;
using SagaState.Instance;

namespace SagaState
{
    public class SagaCreation
    {
        private readonly IMongoDatabase _mongoDatabase;
        public SagaCreation(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }
        public async Task Create(string nameofSaga, dynamic data)
        {
            if (_mongoDatabase.GetCollection<Saga>(Constants.SagaInstanceCollection) == null)
            {
                await _mongoDatabase.CreateCollectionAsync(Constants.SagaInstanceCollection);
            }
            var sagaDefinition = await GetDefinition(nameofSaga);
            var saga = new Saga(sagaDefinition.Name);
            saga.AddData(data);
            SagaStage sagaStage = null;
            for (int i = 0; i < sagaDefinition.TransDefinitions.Count; i++)
            {
                var activity = ConvertFrom(sagaDefinition.TransDefinitions[i].Trans);
                var compensating = ConvertFrom(sagaDefinition.TransDefinitions[i].CompensatingTrans);
                if (sagaStage == null)
                {
                    sagaStage = new SagaStage(sagaDefinition.TransDefinitions[i].Name, activity, compensating);
                    sagaStage.PreviousStageName = string.Empty;
                }
                else
                {
                    var next = new SagaStage(sagaDefinition.TransDefinitions[i].Name, activity, compensating);
                    sagaStage.AddNext(next);
                    next.AddPreviousName(sagaStage.Name);
                    if (i == sagaDefinition.TransDefinitions.Count - 1)
                    {
                        next.NextStage = null;
                    }
                }    
            }            
            saga.AddStage(sagaStage);
            await _mongoDatabase.GetCollection<Saga>(Constants.SagaInstanceCollection).InsertOneAsync(saga);
        }

        private async Task<SagaDefinition> GetDefinition(string name)
        {
            var data = await _mongoDatabase.GetCollection<SagaDefinition>(Constants.SagaDefCollection).FindAsync(
                Builders<SagaDefinition>.Filter.Where(s => s.Name == name),
                new FindOptions<SagaDefinition>()
                { Projection = new JsonProjectionDefinition<SagaDefinition, SagaDefinition>("{_id:0}") });
            try
            {
                var sagaDefinition = await data.FirstAsync();
                return sagaDefinition;
            }
            catch (Exception e)
            {
                throw new SagaException($"Can not find saga definition with name : {name}", e);
            }
        }

        private Activity ConvertFrom(IActivityDefinition trans)
        {
            switch (trans)
            {
                case HttpActivityDefinition http:
                    return new HttpActivity(http.Url);
                default:
                    throw new SagaException("The transaction definition is not support");
            }
        }
    }
}

