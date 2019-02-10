using System.Threading.Tasks;
using SagaState.Instance;

namespace SagaState.Handlers
{
    public interface ISagaHandler
    {
        bool CanHandle(Activity activity);
        Task<SagaTransResult> Execute(Activity activity, dynamic data);
    }
}