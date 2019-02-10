using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaState.Instance
{
    public class SagaStage
    {
        public SagaStage(string name, Activity compensatingTrans, Activity trans)
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

        public void AddNext(SagaStage next)
        {
            NextStage = next;            
        }
        public void AddPreviousName(string previousName)
        {
            PreviousStageName = previousName;            
        }
        public SagaStage NextStage { get; set; }
        public string PreviousStageName { get; set; }
    }
}