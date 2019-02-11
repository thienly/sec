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
            var orderSaga = new SagaDefinition("TEST");
            
            orderSaga.DefineTransaction(new SagaStageDefinition("Order",
                new HttpActivityDefinition("http://localhost:5002/api/order/Order" ),
                new HttpActivityDefinition("http://localhost:5002/api/order/ReOrder")));            

            orderSaga.DefineTransaction(new SagaStageDefinition("Payment",
                new HttpActivityDefinition("http://localhost:5001/api/payment/Payment" ),
                new HttpActivityDefinition("http://localhost:5001/api/payment/RePayment")));            

            lst.Add(orderSaga);
            return lst;
        }
    }
    
}
