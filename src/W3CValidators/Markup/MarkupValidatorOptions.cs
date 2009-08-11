// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System.Text;

    public class MarkupValidatorOptions
    {
        public Encoding Charset { get; set; }
        public string DocType { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
    }
}