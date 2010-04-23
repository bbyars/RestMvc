using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Attributes;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class ReflectionExtensionsTest
    {
        public void Unannotated() {}

        [Get("resource")]
        public void GetResource() {}

        [Post("resource")]
        public void PostResource() {}

        [Get("list")]
        public void GetList() {}

        [Test]
        public void GetAttributeReturnsNullIfMissingAttribute()
        {
            var method = GetType().GetMethod("Unannotated");
            Assert.That(method.GetResourceActionAttribute(), Is.Null);
        }

        [Test]
        public void GetAttributeReturnsCorrectAttribute()
        {
            var method = GetType().GetMethod("GetResource");
            Assert.That(method.GetResourceActionAttribute(), Is.EqualTo(new GetAttribute("resource")));
        }

        [Test]
        public void GetResourceActionAttributesReturnsAllAnnotatedMethods()
        {
            var actions = GetType().GetResourceActions().Select(action => action.Name).ToArray();
            Assert.That(actions, Is.EqualTo(new[] {"GetResource", "PostResource", "GetList"}));
        }
    }
}
