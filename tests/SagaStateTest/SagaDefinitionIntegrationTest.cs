using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SagaWorker;
using SagaWorker.Definition;
using SagaWorker.Handlers;
using SagaWorker.Instance;
using SagaWorker.SeedData;
using Xunit;

namespace SagaStateTest
{
    public class SagaDefinitionIntegrationTest
    {
        [Fact]
        public void return_true_if_could_process_seed_data_successfully()
        {
            var connectionString = "mongodb://10.0.19.102:27017/saga";
            SagaDefinitionSeedData.SeedData(connectionString);
        }

        [Fact]
        public async Task return_true_if_could_create_a_saga()
        {
            BsonClassMap.RegisterClassMap<HttpActivityDefinition>();
            var connectionString = "mongodb://10.0.19.102:27017/saga";
            var client = new MongoClient(new MongoUrl(connectionString));
            var mongoDatabase = client.GetDatabase(new MongoUrl(connectionString).DatabaseName);
            var creation = new SagaCreation(mongoDatabase);
            await creation.Create("TEST", new SampleData()
            {
                CustomerName = "TEO",
                Price = 100
            });
        }

        [Fact]
        public async Task test_engine()
        {
            BsonClassMap.RegisterClassMap<Activity>(map =>
            {
                map.MapProperty(x => x.Data).SetIgnoreIfNull(true);
            });
            BsonClassMap.RegisterClassMap<HttpActivity>();
            BsonClassMap.RegisterClassMap<SampleData>();
            var connectionString = "mongodb://10.0.19.102:27017/saga";
            var client = new MongoClient(new MongoUrl(connectionString));
            var mongoDatabase = client.GetDatabase(new MongoUrl(connectionString).DatabaseName);
            var data = await mongoDatabase.GetCollection<Saga>("SagaInstance")
                .FindAsync(Builders<Saga>.Filter.Eq(x => x.Id, ObjectId.Parse("5c613e87eadd404b08ce2480")));
            var saga = data.First();
            var handler = new List<ISagaHandler>();
            handler.Add(new SagaHttpHandler(new HttpClient()));
            var engine = new  SagaEngine(handler,mongoDatabase);
            await engine.Execute(saga);
        }
        internal class SampleData
        {
            public string CustomerName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
