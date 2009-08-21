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

            _client = new MarkupValidatorClient(MarkupValidatorClient.PublicValidator);
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

        [Test]
        public void Client_Should_Wait_One_Second_Between_Requests_By_Fragment_Method()
        {
            var document = Encoding.UTF8.GetString(_examples[true]);
            Client_Should_Wait_One_Second_Between_Requests(client => client.CheckByFragment(document, null));
        }

        [Test]
        public void Client_Should_Wait_One_Second_Between_Requests_By_Upload_Method()
        {
            Client_Should_Wait_One_Second_Between_Requests(client => client.CheckByUpload(_examples[true], null));
        }

        [Test]
        public void Client_Should_Wait_One_Second_Between_Requests_By_Uri_Method()
        {
            Client_Should_Wait_One_Second_Between_Requests(client => client.CheckByUri(new Uri(ValidXhtmlUrl), null));
        }

        /// <remarks>
        /// This test really only applies to http://validator.w3.org/check.  When pointed at other
        /// validators, the client should be allowed to go full speed ahead.
        /// </remarks>
        private void Client_Should_Wait_One_Second_Between_Requests(Action<MarkupValidatorClient> check)
        {
            var stopWatch = new Stopwatch();
            int[] requestCount = { 0 };

            this._client.ResponseReceived += (sender, e) =>
            {
                if (requestCount[0] == 1)
                    stopWatch.Start();
            };
            this._client.SendingRequest += (sender, e) =>
            {
                if (requestCount[0] == 1)
                {
                    stopWatch.Stop();
                    // If the assertion fails, an exception will be thrown and we will never
                    // execute the 2nd "too early" request.  In other words, even if our code is
                    // broken, this test will not abuse the w3c validator service.
                    Assert.That(stopWatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(950)); // about 50 ms of grace
                }
                requestCount[0]++;
            };

            check(_client);
            check(_client);
        }
    }
}