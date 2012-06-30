using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace RestMvc.FunctionalTests
{
    public class HttpResponse
    {
        private readonly HttpWebResponse response;
        private string body;

        public HttpResponse(HttpWebResponse response)
        {
            this.response = response;
        }

        public int StatusCode
        {
            get { return (int)response.StatusCode; }
        }

        public string ContentType
        {
            get { return response.ContentType; }
        }

        public string Body
        {
            get
            {
                if (body == null)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        body = reader.ReadToEnd();
                    }
                }
                return body;
            }
        }

        public NameValueCollection Headers
        {
            get { return response.Headers; }
        }
    }
}
