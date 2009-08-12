// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web;

    // TODO: throttle down to 1 request per second if pointed at http://validator.w3.org/check

    /// <summary>
    /// Communicates with a W3C Markup Validator service.
    /// </summary>
    public class MarkupValidatorClient
    {
        private readonly Uri _validator;

        /// <summary>
        /// Creates a new MarkupValidatorClient instance pointing at W3C's public validator:
        /// http://validator.w3.org/check.
        /// </summary>
        public MarkupValidatorClient()
            : this(new Uri("http://validator.w3.org/check"))
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
        public MarkupValidatorResponse Check(Uri documentUri, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var queryStrings = options.ToDictionary();
            queryStrings.Add("uri", documentUri.ToString());
            var request = WebRequest.Create(AppendQueryString(_validator, queryStrings));

            return ParseResponse(request);
        }

        /// <summary>
        /// Uploads a document to the validator service for validation.  This is the
        /// "uploaded_file" method.
        /// </summary>
        /// <param name="documentData">the document to upload</param>
        /// <param name="options">configuration options</param>
        /// <returns>a MarkupValidatorResponse object</returns>
        public MarkupValidatorResponse Check(byte[] documentData, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var request = this.ConstructPostRequest(
                options,
                writer => writer.Write("uploaded_file", "document.html", "text/html", documentData));

            return ParseResponse(request);
        }

        /// <summary>
        /// Posts a document to the validator service for validation.  This is the "fragment"
        /// method.
        /// </summary>
        /// <param name="documentFragment">the document to upload</param>
        /// <param name="options">configuration options</param>
        /// <returns>a MarkupValidatorResponse object</returns>
        public MarkupValidatorResponse Check(string documentFragment, MarkupValidatorOptions options)
        {
            if (options == null)
                options = new MarkupValidatorOptions();

            var request = this.ConstructPostRequest(
                options,
                writer => writer.Write("fragment", documentFragment));

            return ParseResponse(request);
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

        private static MarkupValidatorResponse ParseResponse(WebRequest request)
        {
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
                return new MarkupValidatorResponse(stream);
        }
    }
}