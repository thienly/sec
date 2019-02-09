namespace SagaState.Instance
{
    public class HttpActivity : Activity
    {
        public HttpActivity(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}