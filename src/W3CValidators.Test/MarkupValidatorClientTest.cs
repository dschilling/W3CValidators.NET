// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Threading;
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
            // The W3C says (on http://validator.w3.org/docs/api.html):
            //     Note: If you wish to call the validator programmatically for a batch of
            //     documents, please make sure that your script will sleep for at least 1 second
            //     between requests. The Markup Validation service is a free, public service for
            //     all, your respect is appreciated. thanks.
            Thread.Sleep(1000);

            _client = new MarkupValidatorClient(ValidatorUri);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Fragment_Method(bool valid)
        {
            var document = Encoding.UTF8.GetString(_examples[valid]);
            var response = _client.Check(document, null);
            Assert.That(response.Validity, Is.EqualTo(valid));            
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Client_Should_Return_Correct_Response_By_Upload_Method(bool valid)
        {
            var response = _client.Check(_examples[valid], null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }

        [TestCase(ValidXhtmlUrl, true)]
        [TestCase(InvalidXhtmlUrl, false)]
        public void Client_Should_Return_Correct_Response_By_Uri_Method(string url, bool valid)
        {
            var response = _client.Check(new Uri(url), null);
            Assert.That(response.Validity, Is.EqualTo(valid));
        }

        /// <remarks>
        /// This test really only applies to http://validator.w3.org/check.  When pointed at other
        /// validators, the client should be allowed to go full speed ahead.
        /// </remarks>
        [Test]
        public void Client_Should_Wait_One_Second_Between_Requests()
        {
            if (!Equals(ValidatorUri, MarkupValidatorClient.PublicValidator))
                throw new IgnoreException("This test only applies to http://validator.w3.org/check.");

            var stopWatch = new Stopwatch();
            int[] requestCount = { 0 };

            _client.ResponseRecieved += (sender, e) =>
            {
                if (requestCount[0] == 1)
                    stopWatch.Start();
            };
            _client.SendingRequest += (sender, e) =>
            {
                if (requestCount[0] == 1)
                    stopWatch.Stop();
                requestCount[0]++;
            };

            _client.Check(new Uri(ValidXhtmlUrl), null);
            _client.Check(new Uri(ValidXhtmlUrl), null);

            Assert.That(stopWatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(1000));
        }
    }
}