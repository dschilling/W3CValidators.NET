// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Various parameters that affect the way the validator processes the request.
    /// </summary>
    public class MarkupValidatorOptions
    {
        /// <summary>
        /// Character encoding override.  Specify the character encoding to use when parsing the
        /// document.  Note that this parameter is ignored when using the "fragment" method.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Charset")]
        public Encoding Charset { get; set; }

        /// <summary>
        /// Document Type override.  Specify the Document Type (DOCTYPE) to use when parsing the
        /// document
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// When set to true, will output some extra debugging information on the validated
        /// resource (such as HTTP headers) and validation process (such as parser used, parse mode
        /// etc.).
        /// </summary>
        public bool Debug { get; set; }

        internal IDictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("output", "soap12");

            if (this.Charset != null)
                dictionary.Add("charset", this.Charset.WebName);
            if (this.DocType != null)
                dictionary.Add("doctype", this.DocType);
            if (this.Debug)
                dictionary.Add("debug", "1");
            return dictionary;
        }
    }
}