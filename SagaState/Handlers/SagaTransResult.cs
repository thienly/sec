using SagaState.Instance;

namespace SagaState.Handlers
{
    public class SagaTransResult
    {
        public Activity Activity { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}