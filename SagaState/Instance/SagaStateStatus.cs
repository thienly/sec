namespace SagaState.Instance
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
