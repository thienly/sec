using System.Dynamic;

namespace SagaWorker.Instance
{
    public abstract class Activity
    {        
        public ExpandoObject Data { get; set; } = new ExpandoObject();

        public void SetData(ExpandoObject data)
        {
            Data = data;
        }
    }    
}