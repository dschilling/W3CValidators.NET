namespace W3CValidators.Test
{
    using Markup;
    using NUnit.Framework;

    [TestFixture]
    public class MarkupValidatorResponseFaultTest
    {
        [Test]
        public void Test()
        {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("W3CValidators.Test.fault.xml"))
            {
                Assert.That(() =>
                {
                    new MarkupValidatorResponse(stream);
                },
                Throws.InstanceOf<SoapFaultException>());
            }
        }
    }
}