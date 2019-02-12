namespace SagaShared.Instance
{
    public class HttpSagaActivity : SagaActivity
    {
        public HttpSagaActivity()
        {
            
        }
        public HttpSagaActivity(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}