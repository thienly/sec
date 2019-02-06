namespace SagaState.Definition
{
    public class SagaStageDefinition
    {
        public SagaStageDefinition(string name,ITransactionDefinition trans, ITransactionDefinition compensatingTrans)
        {
            Trans = trans;
            CompensatingTrans = compensatingTrans;
            Name = name;                        
        }
        public string Name { get; set; }        
        public ITransactionDefinition Trans { get; set; }
        public ITransactionDefinition CompensatingTrans { get; set;}
    }
}