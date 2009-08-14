namespace W3CValidators.NUnit.Test
{
    using System;
    using System.IO;
    using global::NUnit.Framework;

    [TestFixture]
    public class MarkupConstraintTest
    {
        private const string ValidXhtmlUrl = "http://raw-data.org/sites/default/files/validXhtml11.html";
        private const string InvalidXhtmlUrl = "http://raw-data.org/sites/default/files/invalidXhtml11.html";

        [Test]
        public void Constraint_Should_Convert_String_To_Uri()
        {
            Assert.That(ValidXhtmlUrl, IsValid.Markup());
        }

        [Test]
        public void Constraint_Should_Accept_Uri()
        {
            Assert.That(new Uri(ValidXhtmlUrl), IsValid.Markup());
        }

        [Test]
        public void Constraint_Should_Not_Convert_String_If_Asked_Not_To()
        {
            Assert.That(() =>
                Assert.That(ValidXhtmlUrl, IsValid.Markup(ValidationOptions.DontConvertStringToUri)),
            Throws.InstanceOf<AssertionException>());
        }

        [Test]
        public void Constraint_Should_Be_Able_To_Validate_Private_Document()
        {
            var htmlPath = Path.GetFullPath(Path.Combine(".", "validXhtml11.html"));
            // create a file:// uri, which will be inaccessable to the validator service.
            var uri = new Uri(htmlPath);
            Assert.That(uri, IsValid.Markup(ValidationOptions.PrivateDocument));
        }

        [Test]
        public void Constraint_Should_Not_Be_Able_To_Validate_Private_Document_If_Not_Asked_To()
        {
            Assert.That(() =>
            {
                var htmlPath = Path.GetFullPath(Path.Combine(".", "validXhtml11.html"));
                // create a file:// uri, which will be inaccessable to the validator service.
                var uri = new Uri(htmlPath);
                Assert.That(uri, IsValid.Markup());
            },
            Throws.InstanceOf<ArgumentNullException>());
        }
    }
}