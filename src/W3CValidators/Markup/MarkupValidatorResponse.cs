// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// A response from the validator service.
    /// </summary>
    public class MarkupValidatorResponse : MarkupValidatorResponseBase
    {
        private readonly IList<MarkupValidatorAtomicMessage> _errors;
        private readonly IList<MarkupValidatorAtomicMessage> _warnings;
        private readonly IDictionary<string, string> _debug;

        /// <summary>
        /// Constructs a new MarkupValidatorResponse to parse the data in the stream.
        /// </summary>
        /// <param name="stream"></param>
        public MarkupValidatorResponse(Stream stream)
            : base(stream)
        {
            _errors = new MarkupValidatorAtomicMessageList(
                this.Node.SelectSingleNode(string.Concat("child::", NamespaceAlias, ":errors"), this.NamespaceManager),
                this.NamespaceManager,
                NamespaceAlias,
                "error");
            _warnings = new MarkupValidatorAtomicMessageList(
                this.Node.SelectSingleNode(string.Concat("child::", NamespaceAlias, ":warnings"), this.NamespaceManager),
                this.NamespaceManager,
                NamespaceAlias,
                "warning");
            _debug = new Dictionary<string, string>();
            var debugNodes = this.Node.SelectNodes(string.Concat("child::", NamespaceAlias, ":debug"), this.NamespaceManager);
            foreach (XmlNode debugNode in debugNodes)
            {
                _debug.Add(debugNode.Attributes["name"].Value, debugNode.InnerText);
            }
        }

        /// <summary>
        /// The address of the document validated.
        /// </summary>
        public Uri Uri
        {
            get { return this.GetUri("uri"); }
        }

        /// <summary>
        /// Location of the service which provided the validation result.
        /// </summary>
        public Uri CheckedBy
        {
            get { return this.GetUri("checkedby"); }
        }

        /// <summary>
        /// Detected (or forced) Document Type for the validated document.
        /// </summary>
        public string DocType
        {
            get { return this["doctype"]; }
        }

        /// <summary>
        /// Detected (or forced) Character Encoding for the validated document.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Charset")]
        public Encoding Charset
        {
            get { return GetEncoding("charset"); }
        }

        /// <summary>
        /// Whether or not the document validated passed or not formal validation.
        /// </summary>
        public bool Validity
        {
            get { return GetBool("validity"); }
        }

        /// <summary>
        /// The list of errors encountered through the validation process.
        /// </summary>
        public IList<MarkupValidatorAtomicMessage> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// The list of warnings encountered through the validation process.
        /// </summary>
        public ICollection<MarkupValidatorAtomicMessage> Warnings
        {
            get { return _warnings; }
        }

        /// <summary>
        /// Contains debug messages if the Debug option was enabled.
        /// </summary>
        public IDictionary<string, string> Debug
        {
            get { return _debug; }
        }
    }
}