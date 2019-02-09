namespace SagaState.Definition
{
    public class SagaStageDefinition
    {
        public SagaStageDefinition(string name,IActivityDefinition trans, IActivityDefinition compensatingTrans)
        {
            Trans = trans;
            CompensatingTrans = compensatingTrans;
            Name = name;                        
        }
        public string Name { get; set; }        
        public IActivityDefinition Trans { get; set; }
        public IActivityDefinition CompensatingTrans { get; set;}
    }
}