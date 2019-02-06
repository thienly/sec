using System.Collections.Generic;

namespace SagaState.Definition
{
    public class SagaDefinition
    {
        public SagaDefinition(string name)
        {
            Name = name;
        }
        public string Name { get; }        
        public List<SagaStageDefinition> TransDefinitions { get; } = new List<SagaStageDefinition>();
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