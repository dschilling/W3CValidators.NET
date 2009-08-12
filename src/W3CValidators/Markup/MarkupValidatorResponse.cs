// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    // TODO: include <m:Debug> results.

    /// <summary>
    /// A response from the validator service.
    /// </summary>
    public class MarkupValidatorResponse
    {
        private readonly XmlNode _root;
        private readonly XmlNamespaceManager _nsmgr;
        private const string NamespaceAlias = "m";

        internal MarkupValidatorResponse(Stream stream)
        {
            var doc = new XmlDocument();
            _nsmgr = new XmlNamespaceManager(doc.NameTable);
            _nsmgr.AddNamespace("e", "http://www.w3.org/2003/05/soap-envelope");
            _nsmgr.AddNamespace(NamespaceAlias, "http://www.w3.org/2005/10/markup-validator");

            doc.Load(stream);

            _root = doc.SelectSingleNode(string.Format("/e:Envelope/e:Body/{0}:markupvalidationresponse", NamespaceAlias), _nsmgr);
        }

        private string this[string name]
        {
            get
            {
                var node = _root.SelectSingleNode(string.Format("child::{0}:{1}", NamespaceAlias, name), _nsmgr);
                if (node != null && !string.IsNullOrEmpty(node.InnerText))
                    return node.InnerText;
                return null;
            }
        }

        private Uri GetUri(string name)
        {
            var value = this[name];
            if (value == null)
                return null;
            return new Uri(value);
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
        public Encoding Charset
        {
            get
            {
                var value = this["charset"];
                if (value == null)
                    return null;
                return Encoding.GetEncoding(value);
            }
        }

        /// <summary>
        /// Whether or not the document validated passed or not formal validation.
        /// </summary>
        public bool Validity
        {
            get
            {
                var value = this["validity"];
                if (value == null)
                    return false;
                return bool.Parse(value);
            }
        }

        // TODO: implement Error and Warning collections.

        /// <summary>
        /// The list of errors encountered through the validation process.
        /// </summary>
        public IList<MarkupValidatorAtomicMessage> Errors { get { throw new NotImplementedException(); } }

        /// <summary>
        /// The list of warnings encountered through the validation process.
        /// </summary>
        public ICollection<MarkupValidatorAtomicMessage> Warnings { get { throw new NotImplementedException(); } }
    }
}