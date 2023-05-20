using NFluent;
using NUnit.Framework;

namespace Railway.UnitTests
{
    [TestFixture]
    public class TagsTester
    {
        [Test]
        public void ShouldTagNamedParameter()
        {
            //Arrange
            var myBusinessParam = Guid.NewGuid();

            //Act
            var ope = Ope.Error("Something goes wrong").Tag(myBusinessParam);

            //Assert
            Check.That(ope.Tags).ContainsKey("myBusinessParam");
            Check.That(ope.Tags["myBusinessParam"]).Equals(myBusinessParam.ToString());
        }

        [Test]
        public void ShouldTagNamedParameterOnTypedOpe()
        {
            //Arrange
            var myBusinessParam = Guid.NewGuid();

            //Act
            var ope = Ope.Error<bool>("Something goes wrong").Tag(myBusinessParam);

            //Assert
            Check.That(ope.Tags).ContainsKey("myBusinessParam");
            Check.That(ope.Tags["myBusinessParam"]).Equals(myBusinessParam.ToString());
        }
    }
}