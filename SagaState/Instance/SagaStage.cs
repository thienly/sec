using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaWorker.Instance
{
    public class SagaStage
    {
        public SagaStage()
        {
            
        }
        public SagaStage(string name,  Activity trans, Activity compensatingTrans)
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
        public Activity Trans { get; set; }
        public Activity CompensatingTrans { get; set; }
        public int Order { get; set; }
    }
}