// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System.Collections.Generic;
    using System.Text;

    public class MarkupValidatorOptions
    {
        public Encoding Charset { get; set; }
        public string DocType { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }

        internal IDictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("output", "soap12");

            if (this.Charset != null)
                dictionary.Add("charset", this.Charset.WebName);
            if (this.DocType != null)
                dictionary.Add("doctype", this.DocType);
            if (this.Verbose)
                dictionary.Add("verbose", "1");
            if (this.Debug)
                dictionary.Add("debug", "1");
            return dictionary;
        }
    }
}