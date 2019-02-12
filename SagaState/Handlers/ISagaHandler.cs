using System.Threading.Tasks;
using SagaShared.Instance;

namespace SagaWorker.Handlers
{
    public interface ISagaHandler
    {
        bool CanHandle(SagaActivity sagaActivity);
        Task<SagaTransResult> Execute(SagaActivity sagaActivity, dynamic data);
    }
}