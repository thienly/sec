using System;

namespace SagaState
{
    public enum SagaStateStatus
    {
        Created,
        Started,
        Completed,
        Failed,
        CompensatingStarted,
        CompensatingCompleted,
        CompensatingFailed,
    }
}
