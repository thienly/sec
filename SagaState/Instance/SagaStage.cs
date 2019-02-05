namespace SagaState.Instance
{
    public class SagaStage
    {
        public SagaStage(string name,IActivity compensatingTrans, IActivity trans)
        {
            CompensatingTrans = compensatingTrans;
            Trans = trans;
            Name = name;
        }
        public SagaStateStatus Status { get; private set; } = SagaStateStatus.Created;
        public void SetStatus(SagaStateStatus status)
        {
            Status = status;
        }
        public string Name { get; set; }
        public IActivity Trans { get; }        
        public IActivity CompensatingTrans { get; }
    }
}