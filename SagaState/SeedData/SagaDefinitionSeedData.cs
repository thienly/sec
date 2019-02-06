using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace SagaState.SeedData
{
    public static class SagaDefinitionSeedData
    {
        public static void SeedData(string mongoDbconnectionString)
        {
            var url = new MongoUrl(mongoDbconnectionString);
            var client = new MongoClient(url);
            var db = client.GetDatabase(url.DatabaseName);
            db.CreateCollection("SagaDefinition");
        }
        
    }
}
