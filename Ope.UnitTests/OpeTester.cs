using NFluent;
using NUnit.Framework;

namespace Railway.UnitTests
{
    [TestFixture] // Base unit test class
    public class OpeTester
    {
        [Test]
        public void ShouldSetSuccessContext()
        {
            //Arrange
            var myBusinessResult = Guid.NewGuid();

            //Act
            OpeBase ope = Ope.Ok(myBusinessResult);

            //Assert
            Check.That(ope.Success).IsTrue();
        }

        [Test]
        public void ShouldSetErrorMessageContext()
        {
            //Arrange
            var userErrorMessage = "Some error message";

            //Act
            Ope ope = Ope.Error(userErrorMessage);

            //Assert
            Check.That(ope.Success).IsFalse();
            Check.That(ope.UserMessage).Equals(userErrorMessage);
        }

        [Test]
        public void ShouldSetExceptionContext()
        {
            //Arrange
            var userErrorMessage = "Some error message";
            var exception = new Exception("Whatever");

            //Act
            var ope = Ope.Error(exception, userErrorMessage);

            //Assert
            Check.That(ope.Success).IsFalse();
            Check.That(ope.UserMessage).Equals(userErrorMessage);
            Check.That(ope.Exception.Message).Equals(exception.Message);
        }

        [Test]
        public void ShouldSetErrorContextFromAnotherOperation()
        {
            //Arrange
            Ope<Guid> baseErrOpe = Ope.Error(Guid.Empty, "Some error Message").TagValue("baseParam", 15);
            
            //Act
            Ope newErrOpe = Ope.Error(baseErrOpe);

            //Assert
            Check.That(newErrOpe.Success).IsFalse();
            Check.That(newErrOpe.UserMessage).Equals(baseErrOpe.UserMessage);
            Check.That(newErrOpe.Tags["baseParam"]).Equals(baseErrOpe.Tags["baseParam"]);
        }

        [Test]
        public void ShouldSetErrorContextFromAnotherOperationAndOverrideUserMessage()
        {
            //Arrange
            Ope<Guid> baseErrOpe = Ope.Error(Guid.Empty, "Some error Message").TagValue("baseParam", 15);

            //Act
            Ope newErrOpe = Ope.Error(baseErrOpe).WithMessage("Some global other message");

            //Assert
            Check.That(newErrOpe.Success).IsFalse();
            Check.That(newErrOpe.UserMessage).Equals("Some global other message");
            Check.That(newErrOpe.Tags["baseParam"]).Equals(baseErrOpe.Tags["baseParam"]);
        }

        [Test]
        public void ShouldLogFailedOperationInfos()
        {
            //Arrange
            int userId = 1357;
            var userRoles = new[] { "IT_ADMIN", "READER" };
            var opeError = Ope.Error("User details retrievement fails").Tag(userId).Tag(userRoles);

            //Act
            var errOpeInfos = opeError.ToString();

            //Assert
            Check.That(!string.IsNullOrEmpty(errOpeInfos)).IsTrue();
            Check.That(errOpeInfos).Contains(opeError.UserMessage);
            Check.That(errOpeInfos).Contains(opeError.Tags["userId"]);
            Check.That(errOpeInfos).Contains(opeError.Tags["userRoles"]);

        }
    }
}