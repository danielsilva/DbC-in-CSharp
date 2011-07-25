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
        public void WhenTwoAssertionsAreUsedInALogicalAnd_ReturnsAnObjectOfAndAssertionType() 
        {
            var oneAssertion = Assertion.That(true);
            var otherAssertion = Assertion.That(true);

            var composedAssertion = (oneAssertion & otherAssertion);

            Assert.IsInstanceOf<AndAssertion>(composedAssertion);
        }

        [Test]
        public void WhenTwoAssertionsAreUsedInALogicalAnd_AndTheFirstOneIsFalse_DoNotValidateTheSecond() 
        {
            var oneAssertion = Assertion.That(false).WhenNot("message of the first one.");
            var otherAssertion = Assertion.That(false).WhenNot("message of the second one.");

            IAssertion composed;
            try
            {
                composed = (oneAssertion && otherAssertion);
            }
            catch (DbCException ex)
            {
                Assert.AreEqual("message of the first one.", ex.Message);
            } 
        }

        [Test]
        public void WhenTwoAssertionsAreUsedInALogicalAnd_AndTheFirstOneIsTrue_ValidateTheSecond()
        {
            var oneAssertion = Assertion.That(true).WhenNot("message of the first one.");
            var otherAssertion = Assertion.That(false).WhenNot("message of the second one.");

            var composedAssertion = (oneAssertion && otherAssertion);

            var exception = Assert.Throws<DbCException>(composedAssertion.Validate);
            Assert.AreEqual("message of the second one.", exception.Message);
        }

        [Test]
        public void WhenTwoAssertionsAreComposedWithAThirdAssertion_AndTheFirstTwoAreFalse_DoNotValidateTheThird() 
        {
            var firstAssertion = Assertion.That(false).WhenNot("message of the first assertion.");
            var secondAssertion = Assertion.That(false).WhenNot("message of the second assertion.");
            var thirdAssertion = Assertion.That(false).WhenNot("message of the third assertion.");

            IAssertion composedAssertion;
            try
            {
                composedAssertion = ((firstAssertion & secondAssertion) && thirdAssertion);
            }
            catch (DbCException ex)
            {
                string expectedMessage = "message of the first assertion." + Environment.NewLine + "message of the second assertion.";
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [Test]
        public void WhenTwoAssertionsAreComposedWithAThirdAssertion_AndTheFirstTwoAreFalse_ValidateTheThird()
        {
            var firstAssertion = Assertion.That(true).WhenNot("message of the first assertion.");
            var secondAssertion = Assertion.That(true).WhenNot("message of the second assertion.");
            var thirdAssertion = Assertion.That(false).WhenNot("message of the third assertion.");

            var composedAssertion = ((firstAssertion & secondAssertion) && thirdAssertion);

            var exception = Assert.Throws<DbCException>(composedAssertion.Validate);
            Assert.AreEqual("message of the third assertion.", exception.Message);
        }
    }
}
