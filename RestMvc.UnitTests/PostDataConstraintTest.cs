using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class PostDataConstraintTest
    {
        [Test]
        public void DoesNotMatchIfFormValueMissing()
        {
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.Form).Returns(new NameValueCollection());
            var constraint = new PostDataConstraint("_method", "PUT");

            var actual = constraint.Match(context.Object, null, null, null, RouteDirection.IncomingRequest);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void MatchesIfFormValueMatches()
        {
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.Form).Returns(new NameValueCollection {{"_method", "PUT"}});
            var constraint = new PostDataConstraint("_method", "PUT");

            var actual = constraint.Match(context.Object, null, null, null, RouteDirection.IncomingRequest);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void DoesNotMatchIfFormValueDifferent()
        {
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.Form).Returns(new NameValueCollection {{"_method", "DELETE"}});
            var constraint = new PostDataConstraint("_method", "PUT");

            var actual = constraint.Match(context.Object, null, null, null, RouteDirection.IncomingRequest);

            Assert.That(actual, Is.False);
        }
    }
}
