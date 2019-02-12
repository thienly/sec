using System.Threading.Tasks;
using SagaWorker.Instance;

namespace SagaWorker.Handlers
{
    public interface ISagaHandler
    {
        bool CanHandle(Activity activity);
        Task<SagaTransResult> Execute(Activity activity, dynamic data);
    }
}