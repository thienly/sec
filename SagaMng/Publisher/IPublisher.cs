using System.Threading.Tasks;
using SagaShared.Instance;

namespace SagaMng.Publisher
{
    public interface IPublisher
    {
        Task Publish(SagaActivity data);
    }
}
