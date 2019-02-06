using System;
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
    }
}
