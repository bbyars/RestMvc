using System.Linq;
using NUnit.Framework;
using RestMvc.Attributes;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class ReflectionExtensionsTest
    {
        public class EmptyController {}

        public void Unannotated() { }

        [Get("test")]
        public void Index() { }

        [Post("Test")]
        public void Create() { }

        [Get("test/{id}")]
        public void Show() { }

        public class Multiple
        {
            [Get("first", "second")]
            public void Test() { }
        }

        [Test]
        public void ControllerNameShouldBeTypeNameIfNoSuffix()
        {
            Assert.That(GetType().GetControllerName(), Is.EqualTo("ReflectionExtensionsTest"));
        }

        [Test]
        public void ControllerNameShouldStripOutSuffix()
        {
            Assert.That(typeof(EmptyController).GetControllerName(), Is.EqualTo("Empty"));
        }

        [Test]
        public void GetAttributeReturnsNullIfMissingAttribute()
        {
            var method = GetType().GetMethod("Unannotated");
            Assert.That(method.GetResourceActionAttribute(), Is.Null);
        }

        [Test]
        public void GetAttributeReturnsCorrectAttribute()
        {
            var method = GetType().GetMethod("Index");
            Assert.That(method.GetResourceActionAttribute(), Is.EqualTo(new GetAttribute("test")));
        }

        [Test]
        public void GetResourceActionAttributesReturnsAllAnnotatedMethods()
        {
            var actions = GetType().GetResourceActions().Select(action => action.Name).ToArray();
            Assert.That(actions, Is.EqualTo(new[] {"Index", "Create", "Show"}));
        }

        [Test]
        public void TypeWithNoAnnotationsShouldHaveNoResourceUris()
        {
            Assert.That(typeof(EmptyController).GetResourceUris(), Is.EqualTo(new string[0]));
        }

        [Test]
        public void ShouldIgnoreCaseWhenSelectingResourceUris()
        {
            Assert.That(GetType().GetResourceUris(), Is.EqualTo(new[] {"test", "test/{id}"}));
        }

        [Test]
        public void ShouldFindAllUrisInOneAttribute()
        {
            Assert.That(typeof(Multiple).GetResourceUris(), Is.EqualTo(new[] {"first", "second"}));
        }

        [Test]
        public void ShouldSupportNoMethodsForMissingResourceUri()
        {
            Assert.That(GetType().GetSupportedMethods(""), Is.EqualTo(new string[0]));
        }

        [Test]
        public void AllMethodsShouldBeUnsupportedForMissingResourceUri()
        {
            Assert.That(GetType().GetUnsupportedMethods(""),
                Is.EqualTo(new[] {"GET", "POST", "PUT", "DELETE"}));
        }

        [Test]
        public void ShouldDetectSetOfMethodsForResourceUri()
        {
            Assert.That(GetType().GetSupportedMethods("Test"), Is.EqualTo(new[] {"GET", "POST"}));
        }

        [Test]
        public void SupportedMethodsShouldBeCaseInsensitive()
        {
            Assert.That(GetType().GetSupportedMethods("test"), Is.EqualTo(new[] {"GET", "POST"}));
        }

        [Test]
        public void SupportedMethodsForSecondResourceUriOnType()
        {
            Assert.That(GetType().GetSupportedMethods("test/{id}"), Is.EqualTo(new[] {"GET"}));
        }

        [Test]
        public void ShouldSupportMethodForAllUrisOnAttribute()
        {
            Assert.That(typeof(Multiple).GetSupportedMethods("first"), Is.EqualTo(new[] {"GET"}));
            Assert.That(typeof(Multiple).GetSupportedMethods("second"), Is.EqualTo(new[] {"GET"}));
        }

        [Test]
        public void GetActionShouldReturnNullForInvalidRequestUri()
        {
            Assert.That(GetType().GetAction("GET", ""), Is.Null);
        }

        [Test]
        public void GetActionShouldReturnNullForInvalidHttpMethod()
        {
            Assert.That(GetType().GetAction("PUT", "test"), Is.Null);
        }

        [Test]
        public void GetActionShouldReturnCorrectMethod()
        {
            Assert.That(GetType().GetAction("GET", "test").Name, Is.EqualTo("Index"));
        }

        [Test]
        public void GetActionShouldSupportMultipleUris()
        {
            Assert.That(typeof(Multiple).GetAction("GET", "second").Name, Is.EqualTo("Test"));
        }
    }
}
