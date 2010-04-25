using System.Net;

namespace RestMvc.FunctionalTests
{
    public class HttpRequest
    {
        private readonly HttpWebRequest webRequest;

        public HttpRequest(string httpMethod, string uri)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = httpMethod;
            webRequest.ContentLength = 0;
        }

        public HttpResponse GetResponse()
        {
            try
            {
                return new HttpResponse((HttpWebResponse)webRequest.GetResponse());
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                return new HttpResponse((HttpWebResponse)ex.Response);
            }
        }
    }
}
