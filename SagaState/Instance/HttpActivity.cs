namespace SagaState.Instance
{
    public class HttpActivity : IActivity
    {
        public HttpActivity(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}