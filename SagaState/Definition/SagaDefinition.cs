using System.Collections.Generic;

namespace SagaWorker.Definition
{
    public class SagaDefinition
    {
        public SagaDefinition(string name)
        {
            Name = name;
        }
        public string Name { get; set; }        
        public List<SagaStageDefinition> TransDefinitions { get; set; } = new List<SagaStageDefinition>();
        public SagaDefinition DefineTransaction(SagaStageDefinition sagaStageDefinition)
        {
            TransDefinitions.Add(sagaStageDefinition);
            return this;
        }        
    }

    public class SagaDefinitionBuilder
    {
            
    }
}