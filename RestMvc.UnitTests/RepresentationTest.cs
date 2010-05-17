using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class RepresentationTest
    {
        [Test]
        public void ViewNameIncludesFormat()
        {
            var route = new RouteData();
            route.Values["action"] = "action";
            route.Values["format"] = "format";
            var context = new ControllerContext {RouteData = route};

            Assert.That(Representation.GetViewName(context), Is.EqualTo("action.format"));
        }
    }
}
