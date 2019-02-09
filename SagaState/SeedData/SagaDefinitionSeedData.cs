using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using SagaState.Definition;
using MongoClient = MongoDB.Driver.MongoClient;

namespace SagaState.SeedData
{
    public static class SagaDefinitionSeedData
    {
        public static void SeedData(string mongoDbconnectionString)
        {
            string sagaCollection = Constants.SagaDefCollection;
            var url = new MongoUrl(mongoDbconnectionString);
            var client = new MongoClient(url);            
            var db = client.GetDatabase(url.DatabaseName);
            if (db.GetCollection<SagaStageDefinition>(sagaCollection) == null)
            {
                db.CreateCollection(sagaCollection);
            }
            
            var collection = db.GetCollection<SagaDefinition>(sagaCollection);
            collection.Indexes.CreateOne(new CreateIndexModel<SagaDefinition>(Builders<SagaDefinition>.IndexKeys.Text(_ => _.Name),new CreateIndexOptions(){Unique = true}));
            var sagaDefinitions = Data();
            collection.InsertMany(sagaDefinitions);             
        }

        private static List<SagaDefinition> Data()
        {
            var lst = new List<SagaDefinition>();
            var orderSaga = new SagaDefinition("ORDER SAGA");
            orderSaga.DefineTransaction(new SagaStageDefinition("Purchase order",
                new HttpActivityDefinition("http://google.com" ),
                new HttpActivityDefinition("http://google.com")));
            orderSaga.DefineTransaction(new SagaStageDefinition("Payment",
                new HttpActivityDefinition("http://google.com"),
                new HttpActivityDefinition("http://google.com")));
            lst.Add(orderSaga);
            return lst;
        }
    }
    
}
