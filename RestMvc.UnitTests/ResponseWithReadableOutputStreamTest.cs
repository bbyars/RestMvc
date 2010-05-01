using System.Web;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class ResponseWithReadableOutputStreamTest
    {
        [Test]
        public void StringWriteShouldBeReadable()
        {
            var response = new ResponseWithReadableOutputStream(new Mock<HttpResponseBase>().Object);
            response.Write("test");
            Assert.That(response.OutputText, Is.EqualTo("test"));
        }

        [Test]
        public void CharWriteShouldBeReadable()
        {
            var response = new ResponseWithReadableOutputStream(new Mock<HttpResponseBase>().Object);
            response.Write('A');
            Assert.That(response.OutputText, Is.EqualTo("A"));
        }

        [Test]
        public void SlicedWriteShouldBeReadable()
        {
            var response = new ResponseWithReadableOutputStream(new Mock<HttpResponseBase>().Object);
            response.Write("test".ToCharArray(), 1, 2);
            Assert.That(response.OutputText, Is.EqualTo("es"));
        }

        [Test]
        public void ObjectWriteShouldBeReadable()
        {
            var response = new ResponseWithReadableOutputStream(new Mock<HttpResponseBase>().Object);
            response.Write(1);
            Assert.That(response.OutputText, Is.EqualTo("1"));
        }
    }
}
