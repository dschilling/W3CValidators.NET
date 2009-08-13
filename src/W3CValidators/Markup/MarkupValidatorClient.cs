// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// Communicates with a W3C Markup Validator service.
    /// </summary>
    public class MarkupValidatorClient
    {
        /// <summary>
        /// The location of W3C's free public markup validator service.
        /// </summary>
        public static readonly Uri PublicValidator = new Uri("http://validator.w3.org/check");
        private static readonly object ThrottleLock = new object();

        private readonly Uri _validator;

        /// <summary>
        /// Creates a new MarkupValidatorClient instance pointing at W3C's public validator:
        /// http://validator.w3.org/check.
        /// </summary>
        public MarkupValidatorClient()
            : this(PublicValidator)
        {}

        /// <summary>
        /// Creates a new MarkupValidatorClient instance pointing at the specified validator.  See
        /// http://validator.w3.org/docs/install.html for instructions on installing your own copy
        /// of the validator.
        /// </summary>
        /// <param name="validator">the location of the validator service</param>
        public MarkupValidatorClient(Uri validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Asks the validator service to download and validate the document at specified public
        /// uri.  This is the "uri" method.
        /// </summary>
        /// <param name="documentUri">the location of the document to be validated</param>
        /// <param name="options">configuration options</param>
        /// <returns>a MarkupValidatorResponse object</returns>
        public MarkupValidatorResponse CheckByUri(Uri documentUri, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var queryStrings = options.ToDictionary();
            queryStrings.Add("uri", documentUri.ToString());
            var request = WebRequest.Create(AppendQueryString(_validator, queryStrings));

            return ThrottledParseResponse(request);
        }

        /// <summary>
        /// Uploads a document to the validator service for validation.  This is the
        /// "uploaded_file" method.
        /// </summary>
        /// <param name="documentData">the document to upload</param>
        /// <param name="options">configuration options</param>
        /// <returns>a MarkupValidatorResponse object</returns>
        public MarkupValidatorResponse CheckByUpload(byte[] documentData, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var request = this.ConstructPostRequest(
                options,
                writer => writer.Write("uploaded_file", "document.html", "text/html", documentData));

            return ThrottledParseResponse(request);
        }

        /// <summary>
        /// Posts a document to the validator service for validation.  This is the "fragment"
        /// method.
        /// </summary>
        /// <param name="documentFragment">the document to upload</param>
        /// <param name="options">configuration options</param>
        /// <returns>a MarkupValidatorResponse object</returns>
        public MarkupValidatorResponse CheckByFragment(string documentFragment, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var request = this.ConstructPostRequest(
                options,
                writer => writer.Write("fragment", documentFragment));

            return ThrottledParseResponse(request);
        }

        private WebRequest ConstructPostRequest(MarkupValidatorOptions options, Action<MultipartFormDataWriter> writePayload)
        {
            var request = WebRequest.Create(this._validator);
            var boundary = Guid.NewGuid().ToString();
            request.Method = "POST";
            request.ContentType = string.Concat("multipart/form-data; boundary=", boundary);

            using (var contents = new MemoryStream())
            {
                using (var writer = new MultipartFormDataWriter(contents, boundary))
                {
                    foreach (var pair in options.ToDictionary())
                    {
                        writer.Write(pair.Key, pair.Value);
                    }

                    writePayload(writer);
                }

                contents.Flush();
                request.ContentLength = contents.Length;

                using (var requestStream = request.GetRequestStream())
                {
                    contents.WriteTo(requestStream);
                }
            }
            return request;
        }

        private static Uri AppendQueryString(Uri baseUri, ICollection<KeyValuePair<string, string>> queryStrings)
        {
            if (queryStrings.Count <= 0)
                return baseUri;

            var pieces = new string[queryStrings.Count];
            var i = 0;
            foreach (var pair in queryStrings)
            {
                pieces[i] = string.Concat(
                    HttpUtility.UrlEncode(pair.Key),
                    "=",
                    HttpUtility.UrlEncode(pair.Value));
                i++;
            }

            var uriString = string.Concat(
                baseUri.ToString(),
                "?",
                string.Join("&", pieces));

            return new Uri(uriString);
        }

        private MarkupValidatorResponse ThrottledParseResponse(WebRequest request)
        {
            if (!Equals(this._validator, PublicValidator))
                return this.ParseResponse(request);

            lock (ThrottleLock)
            {
                Thread.Sleep(1000);
                return this.ParseResponse(request);
            }
        }

        private MarkupValidatorResponse ParseResponse(WebRequest request)
        {
            this.OnSendingRequest();

            using (var response = request.GetResponse())
            {
                this.OnResponseRecieved();
                using (var stream = response.GetResponseStream())
                    return new MarkupValidatorResponse(stream);
            }
        }

        /// <summary>
        /// Fires just before the client sends its request to the service.
        /// </summary>
        public event EventHandler SendingRequest;

        private void OnSendingRequest()
        {
            if (this.SendingRequest != null)
                this.SendingRequest(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires just after the response has been recieved from the service.
        /// </summary>
        public event EventHandler ResponseRecieved;

        private void OnResponseRecieved()
        {
            if (this.ResponseRecieved != null)
                this.ResponseRecieved(this, EventArgs.Empty);
        }
    }
}