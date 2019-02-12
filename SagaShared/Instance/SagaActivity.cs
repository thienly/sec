using System.Dynamic;
using MongoDB.Bson;

namespace SagaShared.Instance
{
    public abstract class SagaActivity
    {        
        public ExpandoObject Data { get; set; } = new ExpandoObject();

        public void SetData(ExpandoObject data)
        {
            Data = data;
        }

        public ObjectId SagaId { get; set; }
    }    
}