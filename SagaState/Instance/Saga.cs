﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaState.Instance
{
    public class Saga
    {
        public Saga(string name)
        {            
            Name = name;
        }
        [BsonRepresentation(BsonType.String)]
        public SagaStatus Status { get; private set; } = SagaStatus.Created;

        public void SetStatus(SagaStatus status)
        {
            Status = status;
        }

        public void AddStage(SagaStage stage)
        {
            Stage = stage;
        }                

        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; set; }
        public SagaStage Stage { get; set; }
        public dynamic Data { get; private set;}

        public void AddData(dynamic data)
        {
            Data = data;
        }
    }
}