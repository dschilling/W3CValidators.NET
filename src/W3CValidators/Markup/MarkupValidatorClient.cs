// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;

    public class MarkupValidatorClient
    {
        private readonly Uri _validator;

        public MarkupValidatorClient(Uri validator)
        {
            _validator = validator;
        }

        public MarkupValidatorResponse Check(Uri documentUri, MarkupValidatorOptions options)
        {
            var queryStrings = options != null
                ? options.ToDictionary()
                : new Dictionary<string, string>(1);
            queryStrings.Add("uri", documentUri.ToString());
            var request = WebRequest.Create(AppendQueryString(_validator, queryStrings));
            return ParseResponse(request);
        }

        public MarkupValidatorResponse Check(byte[] documentData, MarkupValidatorOptions options)
        {
            throw new NotImplementedException();
        }

        public MarkupValidatorResponse Check(string documentFragment, MarkupValidatorOptions options)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}