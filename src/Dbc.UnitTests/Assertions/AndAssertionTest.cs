using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DbC.UnitTests
{
    [TestFixture]
    public class AndAssertionTest
    {
        [Test]
        public void WhenComposedOfTwoTrueAssertions_ThenValidated_DoNotThrowsException() 
        {
            var trueAssertion = Assertion.That(true);
            var anotherTrueAssertion = Assertion.That(true);

            var andAssertion = new AndAssertion(trueAssertion, anotherTrueAssertion);

            Assert.DoesNotThrow(andAssertion.Validate);
        }

        [Test]
        public void WhenComposedOfTwoFalseAssertions_ThenValidated_ThrowsExceptionWithBothMessages()
        {
            var falseAssertion = Assertion.That(false).WhenNot("falseAssertion's message");
            var anotherFalseAssertion = Assertion.That(false).WhenNot("anotherFalseAssertion's message");

            var andAssertion = new AndAssertion(falseAssertion, anotherFalseAssertion);
            
            var exception = Assert.Throws<DbCException>(andAssertion.Validate);
            var expectedMessage = "falseAssertion's message" + Environment.NewLine + "anotherFalseAssertion's message";
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void WhenComposedOfATrueAssertionAndAFalseAssertion_ThenValidated_ThrowsExceptionWithFalseAssertionMessage() 
        {
            var trueAssertion = Assertion.That(true).WhenNot("trueAssertion's message");
            var falseAssertion = Assertion.That(false).WhenNot("falseAssertion's message");
            

            var andAssertion = new AndAssertion(trueAssertion, falseAssertion);

            var exception = Assert.Throws<DbCException>(andAssertion.Validate);
            var expectedMessage = "falseAssertion's message";
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
