namespace SagaWorker.Instance
{
    public enum SagaStateStatus
    {
        Created,
        Started,
        Completed,
        Failed,
        FailedAndNotNeedToRunCompensating,
        CompensatingStarted,
        CompensatingCompleted,
        CompensatingFailed,
    }
}
