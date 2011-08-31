namespace W3CValidators.Test.Markup
{
    using W3CValidators.Markup;
    using global::NUnit.Framework;

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