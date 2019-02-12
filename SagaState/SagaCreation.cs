using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SagaWorker.Definition;
using SagaWorker.Instance;

namespace SagaWorker
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
            for (int i = 0; i < sagaDefinition.TransDefinitions.Count; i++)
            {
                var activity = ConvertFrom(sagaDefinition.TransDefinitions[i].Trans);
                var compensating = ConvertFrom(sagaDefinition.TransDefinitions[i].CompensatingTrans);
                var sagaStage = new SagaStage(sagaDefinition.TransDefinitions[i].Name, activity, compensating);
                sagaStage.Order = i + 1;
                saga.AddStage(sagaStage);
            }            
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

