using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var dictionary = new Dictionary<string, string>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(expectations))
                dictionary.Add(property.Name, property.GetValue(expectations).ToString());

            return new Routes(routes, dictionary);
        }

        public override bool Matches(object request)
        {
            var httpMethod = request.ToString().Split(' ')[0];
            var url = "~" + request.ToString().Split(' ')[1];
            var route = FindRoute(url, httpMethod);
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

        private RouteData FindRoute(string url, string httpMethod)
        {
            var stubRequest = new Mock<HttpRequestBase>();
            stubRequest.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns(url);
            stubRequest.Setup(r => r.HttpMethod).Returns(httpMethod);
            var stubContext = new Mock<HttpContextBase>();
            stubContext.Setup(ctx => ctx.Request).Returns(stubRequest.Object);

            return routes.GetRouteData(stubContext.Object);
        }
    }
}