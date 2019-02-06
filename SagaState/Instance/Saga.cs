using System;
using System.Collections.Generic;

namespace SagaState.Instance
{
    public class Saga
    {
        public Saga(string name, List<SagaStage> trans)
        {
            Id = Guid.NewGuid();
            Name = name;
            Trans = trans;
        }

        public SagaStatus Status { get; private set; } = SagaStatus.Created;

        public void SetStatus(SagaStatus status)
        {
            Status = status;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }        
        public List<SagaStage> Trans { get; set; }
        public dynamic Data { get; set; }
    }
}