namespace Suppliers.API.Tests
{
    using NUnit.Framework;
    using Suppliers.API.ErrorHandling;

    public class ErrorResultTest
    {
        [Test]
        public void ErrorResult_NewInstanceShouldCreateCorrectlyClean()
        {
            var error = new ErrorResult();
            Assert.AreEqual(0, error.Code);
            Assert.IsNull(error.ErrorMessage);
        }

        [Test]
        public void ErrorResult_NewInstanceShouldCreateCorrectlyWithData()
        {
            var error = new ErrorResult
            {
                Code = 1,
                ErrorMessage = "The error message"
            };

            Assert.AreEqual(1, error.Code);
            Assert.IsNotNull(error.ErrorMessage);
            Assert.AreEqual("The error message", error.ErrorMessage);
        }
    }
}