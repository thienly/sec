using System;

namespace SagaState.Definition
{
    public interface ITransactionDefinition
    {

    }

    public class HttpTransactionDefinition : ITransactionDefinition
    {
        public string Url { get; set; }

        public HttpTransactionDefinition(string url)
        {
            Url = url;
        }
        
    }
}