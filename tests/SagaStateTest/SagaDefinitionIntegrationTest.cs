using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SagaState;
using SagaState.Definition;
using SagaState.Instance;
using SagaState.SeedData;
using Xunit;

namespace SagaStateTest
{
    public class SagaDefinitionIntegrationTest
    {
        [Fact]
        public void return_true_if_could_process_seed_data_successfully()
        {
            var connectionString = "mongodb://localhost:27017/saga";
            SagaDefinitionSeedData.SeedData(connectionString);   

        }

        [Fact]
        public async Task return_true_if_could_create_a_saga()
        {
            BsonClassMap.RegisterClassMap<HttpTransactionDefinition>();
            var connectionString = "mongodb://localhost:27017/saga";
            var client = new MongoClient(new MongoUrl(connectionString));
            var mongoDatabase = client.GetDatabase(new MongoUrl(connectionString).DatabaseName);
            var creation = new SagaCreation(mongoDatabase);
            await creation.Create("ORDER SAGA", new SampleData()
            {
                CustomerName = "TEO",
                Price = 100
            });
        }

        internal class SampleData
        {
            public string CustomerName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
