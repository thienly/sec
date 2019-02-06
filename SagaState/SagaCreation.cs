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

            return Task.CompletedTask;
        }

        private async Task<SagaDefinition> GetDefinition(string name)
        {
            var data = await _mongoDatabase.GetCollection<SagaDefinition>(Constants.SagaDefCollection).FindAsync(Builders<SagaDefinition>.Filter.Where(s => s.Name == name));
            if (await data.AnyAsync())
            {
                var sagaDefinition = await data.FirstAsync();
                return sagaDefinition;
            }
            throw new SagaException($"Can not find saga definition with name : {name}");
        }
    }
}

