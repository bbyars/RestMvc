using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Attributes;

namespace RestMvc.UnitTests.Attributes
{
    [TestFixture]
    public class ResourceActionAttributeTest
    {
        [Test]
        public void GetAttributeShouldReturnGetMethod()
        {
            Assert.That(new GetAttribute("").HttpMethod, Is.EqualTo("GET"));
        }

        [Test]
        public void PostAttributeShouldReturnPostMethod()
        {
            Assert.That(new PostAttribute("").HttpMethod, Is.EqualTo("POST"));
        }

        [Test]
        public void PutAttributeShouldReturnPutMethod()
        {
            Assert.That(new PutAttribute("").HttpMethod, Is.EqualTo("PUT"));
        }

        [Test]
        public void DeleteAttributeShouldReturnDeleteMethod()
        {
            Assert.That(new DeleteAttribute("").HttpMethod, Is.EqualTo("DELETE"));
        }

        [Test]
        public void TestToString()
        {
            Assert.That(new GetAttribute("resource").ToString(), Is.EqualTo("GET resource"));
        }

        [Test]
        public void EqualIfMethodAndResourceEqual()
        {
            Assert.That(new GetAttribute("resource"), Is.EqualTo(new GetAttribute("resource")));
        }

        [Test]
        public void NotEqualIfResourceDifferent()
        {
            Assert.That(new GetAttribute("1"), Is.Not.EqualTo(new GetAttribute("2")));
        }

        [Test]
        public void NotEqualIfVerbDifferent()
        {
            Assert.That(new GetAttribute("resource"), Is.Not.EqualTo(new PutAttribute("resource")));
        }
    }
}
