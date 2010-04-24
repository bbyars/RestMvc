using System.Net;

namespace RestMvc.FunctionalTests
{
    public class HttpRequest
    {
        private readonly HttpWebRequest webRequest;

        public static HttpRequest Get(string uri)
        {
            return new HttpRequest("GET", uri);
        }

        public static HttpRequest Post(string uri)
        {
            return new HttpRequest("POST", uri);
        }

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
