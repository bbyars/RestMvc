using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.UnitTests.Assertions
{
    public class Routes : Constraint
    {
        private readonly IDictionary<string, string> expectations;
        private readonly RouteCollection routes;

        public Routes(RouteCollection routes, IDictionary<string, string> expectations)
        {
            this.routes = routes;
            this.expectations = expectations;
        }

        public static Routes To(object expectations, RouteCollection routes)
        {
            var dictionary = TypeDescriptor.GetProperties(expectations).Cast<PropertyDescriptor>()
                .ToDictionary(property => property.Name, property => property.GetValue(expectations).ToString());

            return new Routes(routes, dictionary);
        }

        public override bool Matches(object requestText)
        {
            var route = FindRoute(new TestRequest(requestText.ToString()));
            Assert.That(route, Is.Not.Null, "Did not find route");

            foreach (var key in expectations.Keys)
            {
                Assert.That(route.Values.ContainsKey(key), Is.True, "Missing route value {0}", key);
                Assert.That(String.Equals(expectations[key], route.Values[key].ToString(), StringComparison.InvariantCultureIgnoreCase),
                    "Expected '{0}', not '{1}' for '{2}'", expectations[key], route.Values[key], key);
            }
            return true;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
        }

        private RouteData FindRoute(TestRequest request)
        {
            var stubRequest = new Mock<HttpRequestBase>();
            stubRequest.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns(request.Url);
            stubRequest.Setup(r => r.HttpMethod).Returns(request.HttpMethod);
            stubRequest.Setup(r => r.Form).Returns(request.Form);
            var stubContext = new Mock<HttpContextBase>();
            stubContext.Setup(ctx => ctx.Request).Returns(stubRequest.Object);

            return routes.GetRouteData(stubContext.Object);
        }

        private class TestRequest
        {
            private readonly string[] parts;

            public TestRequest(string request)
            {
                parts = request.Split(' ');
            }

            public string HttpMethod
            {
                get { return parts[0]; }
            }

            public string Url
            {
                get { return "~" + parts[1];}
            }

            public NameValueCollection Form
            {
                get
                {
                    var result = new NameValueCollection();
                    for (var i = 2; i < parts.Length; i++)
                        result.Add(parts[i].Split('=')[0], parts[i].Split('=')[1]);
                    return result;
                }
            }
        }
    }
}
