using System;

namespace SagaState
{
    public class SagaDefinition
    {
        public SagaStatus Status { get; private set; }

        public void SetStatus(SagaStatus status)
        {
            Status = status;
        }
    }

    public class SagaStageDefinition
    {
        public SagaStageDefinition(IActivityDefinition trans, IActivityDefinition compensatingTrans)
        {
            Trans = trans;
            CompensatingTrans = compensatingTrans;
        }

        public IActivityDefinition Trans { get; }
        public IActivityDefinition CompensatingTrans { get; }
    }

    public interface IActivityDefinition
    {

    }

    public enum SagaStatus
    {
        Created,
        Started,
        Aborted
    }
}
