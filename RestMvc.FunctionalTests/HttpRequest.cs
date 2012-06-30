using System.Net;
using System.Text;

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

        public HttpRequest WithAcceptTypes(params string[] acceptTypes)
        {
            webRequest.Accept = string.Join(", ", acceptTypes);
            return this;
        }

        public HttpRequest WithPostData(string key, string value)
        {
            var buffer = Encoding.ASCII.GetBytes(string.Format("{0}={1}", key, value));
            webRequest.ContentLength = buffer.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var postData = webRequest.GetRequestStream();
            postData.Write(buffer, 0, buffer.Length);
            postData.Close();
            return this;
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
