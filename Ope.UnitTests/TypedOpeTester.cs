using NFluent;
using NUnit.Framework;

namespace Railway.UnitTests
{
    [TestFixture]
    public class TypedOpeTester
    {
        [Test]
        public void ShouldSetSuccessContext()
        {
            //Arrange
            var myBusinessResult = Guid.NewGuid();

            //Act
            Ope<Guid> ope = Ope.Ok(myBusinessResult);

            //Assert
            Check.That(ope.Success).IsTrue();
            Check.That(ope.Result).Equals(myBusinessResult);
        }

        [Test]
        public void ShouldSetErrorMessageContext()
        {
            //Arrange
            Guid myUnSetBusinessResult = default;
            var userErrorMessage = "Some error message";

            //Act
            Ope<Guid> ope = Ope.Error(myUnSetBusinessResult, userErrorMessage);

            //Assert
            Check.That(ope.Success).IsFalse();
            Check.That(ope.UserMessage).Equals(userErrorMessage);
        }

        [Test]
        public void ShouldSetExceptionContext()
        {
            //Arrange
            Guid myUnSetBusinessResult = default;

            var userErrorMessage = "Some error message";
            var exception = new NotImplementedException("Whatever");

            //Act
            Ope<Guid> ope = Ope.Error(myUnSetBusinessResult, new NotImplementedException("Whatever"), userErrorMessage);

            //Assert
            Check.That(ope.Success).IsFalse();
            Check.That(ope.UserMessage).Equals(userErrorMessage);
            Check.That(ope.Exception.Message).Equals(exception.Message);
        }

        [Test]
        public void ShouldSetErrorContextFromAnotherOperation()
        {
            //Arrange
            int defaultResult = default;
            Ope<Guid> baseErrOpe = Ope.Error(Guid.Empty, "Some error Message").TagValue("baseParam", 15);
            
            //Act
            Ope<int> newErrOpe = Ope.Error(defaultResult, baseErrOpe);

            //Assert
            Check.That(newErrOpe.Success).IsFalse();
            Check.That(newErrOpe.UserMessage).Equals(baseErrOpe.UserMessage);
            Check.That(newErrOpe.Tags["baseParam"]).Equals(baseErrOpe.Tags["baseParam"]);
        }
    }
}