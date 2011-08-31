// Copyright (c) 2011 Daniel A. Schilling

namespace W3CValidators.Test.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading;
    using W3CValidators.Markup;
    using global::NUnit.Framework;

    [TestFixture]
    public class MarkupValidatorClientTest
    {
        private const string ValidXhtmlUrl = "http://raw-data.org/sites/default/files/validXhtml11.html";
        private const string InvalidXhtmlUrl = "http://raw-data.org/sites/default/files/invalidXhtml11.html";

        private readonly IDictionary<bool, byte[]> _examples = new Dictionary<bool, byte[]>();
        private MarkupValidatorClient _client;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            using (var c = new WebClient())
            {
                _examples[true] = c.DownloadData(ValidXhtmlUrl);
                _examples[false] = c.DownloadData(InvalidXhtmlUrl);
            }
        }

        [SetUp]
        public void SetUp()
        {
            // The W3C says (on http://validator.w3.org/docs/api.html):
            //     Note: If you wish to call the validator programmatically for a batch of
            //     documents, please make sure that your script will sleep for at least 1 second
            //     between requests. The Markup Validation service is a free, public service for
            //     all, your respect is appreciated. thanks.
            if (Equals(MarkupValidatorClient.ConfiguredValidator, MarkupValidatorClient.PublicValidator))
            {
                // Just in case throttling is broken, we'll add this additional delay to ensure we
                // don't abuse the free service.
                Thread.Sleep(1000);
            }

            _client = new MarkupValidatorClient();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Fragment_Method(bool valid)
        {
            var document = Encoding.UTF8.GetString(_examples[valid]);
            var response = _client.CheckByFragment(document, null);
            Assert.That(response.Validity, Is.EqualTo(valid));            
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Upload_Method(bool valid)
        {
            var response = _client.CheckByUpload(_examples[valid], null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }

        [TestCase(ValidXhtmlUrl, true)]
        [TestCase(InvalidXhtmlUrl, false)]
        public void Client_Should_Return_Correct_Response_By_Uri_Method(string url, bool valid)
        {
            var response = _client.CheckByUri(new Uri(url), null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }

        [Test]
        public void Client_Should_Return_Correct_Response_By_Uri_Method_False()
        {
            Client_Should_Return_Correct_Response_By_Uri_Method(InvalidXhtmlUrl, false);
        }
    }
}