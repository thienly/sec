using System.Text;
using System.Threading.Tasks;
using SagaState.Definition;

namespace SagaState
{
    public class SagaCreation
    {
        public Task Create(string nameofSaga, dynamic data)
        {
            return Task.CompletedTask;
        }
    }
}
