using System.Net;

namespace RestMvc.FunctionalTests
{
    public class HttpRequest
    {
        private readonly string httpMethod;
        private readonly string uri;

        public static HttpRequest Get(string uri)
        {
            return new HttpRequest("GET", uri);
        }

        public HttpRequest(string httpMethod, string uri)
        {
            this.httpMethod = httpMethod;
            this.uri = uri;
        }

        public HttpResponse GetResponse()
        {
            var request = WebRequest.Create(uri);
            request.Method = httpMethod;

            try
            {
                return new HttpResponse((HttpWebResponse)request.GetResponse());
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
