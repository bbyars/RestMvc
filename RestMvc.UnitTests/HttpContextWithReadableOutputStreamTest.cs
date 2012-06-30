using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class HttpContextWithReadableOutputStreamTest
    {
        [Test]
        public void ShouldReplaceHttpContextDuringLifetime()
        {
            var controller = new RestfulController().WithStubbedContext();
            var baseContext = controller.ControllerContext.HttpContext;

            using (new HttpContextWithReadableOutputStream(controller))
            {
                Assert.That(controller.ControllerContext.HttpContext, Is.Not.SameAs(baseContext));
            }
            Assert.That(controller.ControllerContext.HttpContext, Is.SameAs(baseContext));
        }
    }
}
