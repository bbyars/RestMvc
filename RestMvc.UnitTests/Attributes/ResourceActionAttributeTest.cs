using System;
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
        public void ShouldNotSupportDifferentUri()
        {
            Assert.That(new GetAttribute("test").SupportsUri(""), Is.False);
        }

        [Test]
        public void ShouldSupportSameUri()
        {
            Assert.That(new GetAttribute("test").SupportsUri("test"));
        }

        [Test]
        public void ShouldSupportSameUriCaseInsensitive()
        {
            Assert.That(new GetAttribute("test").SupportsUri("TEST"));
        }

        [Test]
        public void ShouldSupportSecondUri()
        {
            Assert.That(new GetAttribute("first", "second").SupportsUri("second"));
        }

        [Test]
        public void ShouldNotContainDifferentHttpMethod()
        {
            Assert.That(new GetAttribute("test").Contains(new PutAttribute("test")), Is.False);
        }

        [Test]
        public void ShouldNotContainIfDifferentUri()
        {
            Assert.That(new GetAttribute("first").Contains(new GetAttribute("second")), Is.False);
        }

        [Test]
        public void ShouldContainSameMethodAndUri()
        {
            Assert.That(new GetAttribute("test").Contains(new GetAttribute("test")));
        }

        [Test]
        public void ShouldContainSecondUri()
        {
            var attribute = new GetAttribute("first", "second");
            Assert.That(attribute.Contains(new GetAttribute("second")));
        }

        [Test]
        public void ShouldNotContainEmptyString()
        {
            Assert.That(new GetAttribute("test").Contains(new GetAttribute("")), Is.False);
        }

        [Test]
        public void ContainerMustBeASupersetOfContained()
        {
            var attribute = new GetAttribute("first", "second");
            Assert.That(attribute.Contains(new GetAttribute("second", "third")), Is.False);
        }

        [Test]
        public void TestToString()
        {
            Assert.That(new GetAttribute("resource").ToString(), Is.EqualTo("GET resource"));
        }

        [Test]
        public void TestToStringWithMultipleResourceUris()
        {
            var attribute = new GetAttribute("first", "second");
            Assert.That(attribute.ToString(),
                Is.EqualTo(string.Format("GET first{0}GET second", Environment.NewLine)));
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
            Assert.That(new GetAttribute("/test").ResourceUris[0], Is.EqualTo("test"));
        }

        [Test]
        public void AllowsEnteringVirtualRootedUriTemplate()
        {
            Assert.That(new GetAttribute("~/test").ResourceUris[0], Is.EqualTo("test"));
        }
    }
}
