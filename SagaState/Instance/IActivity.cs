using System.Dynamic;

namespace SagaState.Instance
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