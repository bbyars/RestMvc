using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.UnitTests.Assertions
{
    [TestFixture]
    public class ThrowsTest
    {
        [Test]
        public void ShouldFailIfNoExceptionThrown()
        {
            try
            {
                CustomAssert.That(delegate { }, Throws<Exception>.Where(ex => ex, Is.Not.Null));
                Assert.Fail("Should have failed assertion");
            }
            catch (AssertionException ex)
            {
                Assert.That(ex.Message, Text.Contains("Should have thrown Exception"));
            }
        }

        [Test]
        public void ShouldPassIfExceptionThrown()
        {
            CustomAssert.That(delegate { throw new Exception(); },
                Throws<Exception>.Where(ex => ex, Is.Not.Null));
        }

        [Test]
        public void ShouldFailIfAssertionFails()
        {
            try
            {
                CustomAssert.That(delegate { throw new Exception(); },
                    Throws<Exception>.Where(ex => ex, Is.Null));
                Assert.Fail("Should have failed assertion");
            }
            catch (AssertionException ex)
            {
                Assert.That(ex.Message, Text.Contains("Expected: null"));
            }
        }

        [Test]
        public void ShouldFailIfChainedAssertionFails()
        {
            try
            {
                CustomAssert.That(delegate { throw new Exception(); },
                    Throws<Exception>.Where(ex => ex, Is.Not.Null).And(ex => ex, Is.Null));
                Assert.Fail("Should have failed assertion");
            }
            catch (AssertionException ex)
            {
                Assert.That(ex.Message, Text.Contains("Expected: null"));
            }
        }
    }
}
