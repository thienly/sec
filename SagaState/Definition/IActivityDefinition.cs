namespace SagaState.Definition
{
    public interface IActivityDefinition
    {

    }

    public class HttpActivityDefinition : IActivityDefinition
    {
        public string Url { get; set; }
        public HttpActivityDefinition(string url)
        {
            Url = url;
        }
        
    }
}