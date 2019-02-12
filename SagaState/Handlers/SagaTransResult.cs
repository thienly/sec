using SagaShared.Instance;

namespace SagaWorker.Handlers
{
    public class SagaTransResult
    {
        public SagaActivity SagaActivity { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool NeedToRunCompensating { get; set; }
    }
}