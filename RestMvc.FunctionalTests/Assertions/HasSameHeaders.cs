using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace RestMvc.FunctionalTests.Assertions
{
    public class HasSameHeaders : Constraint
    {
        private readonly NameValueCollection expectedHeaders;
        private NameValueCollection actualHeaders;

        public HasSameHeaders(NameValueCollection expectedHeaders)
        {
            this.expectedHeaders = expectedHeaders;
        }

        public static HasSameHeaders As(HttpResponse expectedResponse)
        {
            return new HasSameHeaders(expectedResponse.Headers);
        }

        public override bool Matches(object actualResponse)
        {
            actualHeaders = ((HttpResponse)actualResponse).Headers;
            var result = expectedHeaders.Count == actualHeaders.Count;
            foreach (string header in expectedHeaders.Keys)
            {
                // Avoid flaky date comparisons
                if (header == "Date")
                    result &= actualHeaders["Date"] != null;
                else
                    result &= actualHeaders[header] == expectedHeaders[header];
            }
            return result;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteExpectedValue(Description(expectedHeaders));
            writer.WritePredicate(string.Format("{0}but was{0}", Environment.NewLine));
            writer.WriteActualValue(Description(actualHeaders));
        }

        private static string Description(NameValueCollection headers)
        {
            var lines = new List<string>();
            foreach (string header in headers.Keys)
                lines.Add(string.Format("{0}: {1}", header, headers[header]));
            return string.Join(Environment.NewLine, lines.ToArray());

        }
    }
}