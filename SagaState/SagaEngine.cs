using System.Threading.Tasks;
using SagaState.Instance;

namespace SagaState
{
    public class SagaEngine
    {
        public Task Excute(Saga saga)
        {
            return Task.CompletedTask;
        }
    }
}