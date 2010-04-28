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

        [Test]
        public void GetCreateShouldReturnGetAttribute()
        {
            Assert.That(ResourceActionAttribute.Create("GET", "test"), Is.EqualTo(new GetAttribute("test")));
        }

        [Test]
        public void PostCreateShouldReturnPostAttribute()
        {
            Assert.That(ResourceActionAttribute.Create("POST", "test"), Is.EqualTo(new PostAttribute("test")));
        }

        [Test]
        public void PutCreateShouldReturnPutAttribute()
        {
            Assert.That(ResourceActionAttribute.Create("PUT", "test"), Is.EqualTo(new PutAttribute("test")));
        }

        [Test]
        public void DeleteCreateShouldReturnDeleteAttribute()
        {
            Assert.That(ResourceActionAttribute.Create("DELETE", "test"), Is.EqualTo(new DeleteAttribute("test")));
        }

        [Test]
        public void CreateShouldReturnNullIfInvalidMethodGiven()
        {
            Assert.That(ResourceActionAttribute.Create("TRACE", "test"), Is.Null);
        }

        [Test]
        public void CreateIsCaseInsensitive()
        {
            Assert.That(ResourceActionAttribute.Create("get", "test"), Is.EqualTo(new GetAttribute("test")));
        }

        [Test]
        public void AllowsEnteringRootedUriTemplate()
        {
            Assert.That(new GetAttribute("/test").ResourceUri, Is.EqualTo("test"));
        }

        [Test]
        public void AllowsEnteringVirtualRootedUriTemplate()
        {
            Assert.That(new GetAttribute("~/test").ResourceUri, Is.EqualTo("test"));
        }
    }
}
