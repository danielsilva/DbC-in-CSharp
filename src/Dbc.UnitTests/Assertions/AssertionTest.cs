using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DbC;

namespace DbC.UnitTests
{
    [TestFixture]
    public class AssertionTest
    {
        [Test]
        public void WhenAnAssertionIsValid_DoNotThrowsException() 
        {
            var assertion = Assertion.That(true);
            Assert.DoesNotThrow(assertion.Validate);
        }

        [Test]
        public void WhenAnAssertionIsInvalid_AndDoesNotHaveMessage_ThrowsExceptionWithDefaultMessage()
        {
            var assertion = Assertion.That(false);
            
            var exception = Assert.Throws<DbCException>(assertion.Validate);
            Assert.AreEqual("DbC rule violated.", exception.Message);
        }

        [Test]
        public void WhenAnAssertionIsInvalid_AndHasMessage_ThrowsExceptionWithSpecifiedMessage() 
        {
            var assertion = Assertion.That(false).WhenNot("An error ocurred. Rule violated.");
            
            var exception = Assert.Throws<DbCException>(assertion.Validate);
            Assert.AreEqual("An error ocurred. Rule violated.", exception.Message);
        }

        [Test]
        public void WhenTwoAssertionsAreUsedInALogicalAnd_ReturnsAnObjectOfAssertionType() 
        {
            var oneAssertion = Assertion.That(true);
            var otherAssertion = Assertion.That(true);

            var composedAssertion = (oneAssertion & otherAssertion);

            Assert.IsInstanceOf<AndAssertion>(composedAssertion);
        }
    }
}
