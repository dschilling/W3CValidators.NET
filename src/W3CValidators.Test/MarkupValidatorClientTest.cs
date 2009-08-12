// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Test
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using Markup;
    using NUnit.Framework;

    [TestFixture]
    public class MarkupValidatorClientTest
    {
        private static readonly Uri ValidatorUri = new Uri("http://validator.w3.org/check");
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
            _client = new MarkupValidatorClient(ValidatorUri);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Fragment_Method(bool valid)
        {
            var document = Encoding.UTF8.GetString(_examples[valid]);
            var response = _client.Validate(document, null);
            Assert.That(response.Validity, Is.EqualTo(valid));            
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Upload_Method(bool valid)
        {
            var response = _client.Validate(_examples[valid], null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }

        [TestCase(ValidXhtmlUrl, true)]
        [TestCase(InvalidXhtmlUrl, false)]
        public void Client_Should_Return_Correct_Response_By_Uri_Method(string url, bool valid)
        {
            var response = _client.Validate(new Uri(url), null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }
    }
}