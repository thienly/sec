using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaShared.Instance
{
    public class SagaStage
    {
        public SagaStage()
        {
            
        }
        public SagaStage(string name,  SagaActivity trans, SagaActivity compensatingTrans)
        {
            CompensatingTrans = compensatingTrans;
            Trans = trans;
            Name = name;
        }
        [BsonRepresentation(BsonType.String)]
        public SagaStateStatus Status { get; private set; } = SagaStateStatus.Created;
        public void SetStatus(SagaStateStatus status)
        {
            Status = status;
        }
        public string Name { get; set; }
        public SagaActivity Trans { get; set; }
        public SagaActivity CompensatingTrans { get; set; }
        public int Order { get; set; }
    }
}