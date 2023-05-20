using NFluent;
using NUnit.Framework;

namespace Railway.UnitTests
{
    [TestFixture]
    public class OpeRunnerTester
    {
        private static Task<float> Multiply(float a, float b) => Task.FromResult(a * b);

        [Test]
        public void ShouldRunSuccessOperation()
        {
            Ope<string> mathOpe = Ope.Run(() => Multiply(100, 3), "Your operation fails")
                .Then((res) => res + 22)
                .Then((res) => res.ToString());

            Check.That(mathOpe.Success).IsTrue();
            Check.That(mathOpe.Result).Equals("322");
        }

        [Test]
        public void ShouldRunOperation()
        {
            var mathOpe = Ope.Run(() => Multiply(100, 3), "Your operation fails")
                .Then(res => res + 22).WithMessage("Addition fails")
                .Then(res => throw new Exception("ERR"));

            Check.That(mathOpe.Success).IsFalse();
            Check.That(mathOpe.Exception).IsNotNull();
            Check.That(mathOpe.Tags.ContainsKey(OpeExtensions.InputParamTagKey)).IsTrue();
            Check.That(mathOpe.Tags[OpeExtensions.InputParamTagKey]).Equals("322");
        }
    }
}
