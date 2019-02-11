using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaState.Instance
{
    public class Saga
    {
        public Saga()
        {

        }
        public Saga(string name)
        {
            Name = name;
        }
        [BsonRepresentation(BsonType.String)]
        public SagaStatus Status
        {
            get
            {
                if (Stages.All(x => x.Status == SagaStateStatus.Created))
                    return SagaStatus.Created;
                if (Stages.Any(x => x.Status == SagaStateStatus.Started))
                    return SagaStatus.Started;
                if (Stages.Any(x => x.Status == SagaStateStatus.Failed || x.Status == SagaStateStatus.CompensatingFailed))
                    return SagaStatus.Failed;
                return SagaStatus.Ended;
            }
        }
        public void AddStage(SagaStage stage)
        {
            Stages.Add(stage);
        }

        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; set; }
        public List<SagaStage> Stages { get; set; } = new List<SagaStage>();

        public dynamic Data { get; private set; }

        public void AddData(dynamic data)
        {
            Data = data;
        }
    }
}