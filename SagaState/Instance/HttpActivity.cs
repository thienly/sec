namespace SagaWorker.Instance
{
    public class HttpActivity : Activity
    {
        public HttpActivity()
        {
            
        }
        public HttpActivity(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}