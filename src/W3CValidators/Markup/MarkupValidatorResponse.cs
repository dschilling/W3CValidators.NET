// Copyright (c) 2011 Daniel A. Schilling

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
    public class MarkupValidatorResponse
    {
        private readonly XmlHelper _helper;
        private readonly IList<MarkupValidatorAtomicMessage> _errors;
        private readonly IList<MarkupValidatorAtomicMessage> _warnings;
        private readonly IDictionary<string, string> _debug;

        /// <summary>
        /// Constructs a new MarkupValidatorResponse to parse the data in the stream.
        /// </summary>
        /// <param name="stream"></param>
        public MarkupValidatorResponse(Stream stream)
        {
            var doc = new XmlDocument();
            const string soapAlias = "e";
            const string namespaceAlias = "m";
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace(soapAlias, "http://www.w3.org/2003/05/soap-envelope");
            nsmgr.AddNamespace(namespaceAlias, "http://www.w3.org/2005/10/markup-validator");

            doc.Load(stream);

            var faultNode = doc.SelectSingleNode(string.Concat("/", soapAlias, ":Envelope/", soapAlias, ":Body/", soapAlias, ":Fault"), nsmgr);
            if (faultNode != null)
                throw CreateFaultException(faultNode, nsmgr, soapAlias, namespaceAlias);

            var node = doc.SelectSingleNode(string.Concat("/", soapAlias, ":Envelope/", soapAlias, ":Body/", namespaceAlias, ":markupvalidationresponse"), nsmgr);

            _helper = new XmlHelper(node, nsmgr, namespaceAlias);


            _errors = new MarkupValidatorAtomicMessageList(
                _helper.Node.SelectSingleNode(string.Concat("child::", _helper.NamespaceAlias, ":errors"), _helper.NamespaceManager),
                _helper.NamespaceManager,
                _helper.NamespaceAlias,
                "error");
            _warnings = new MarkupValidatorAtomicMessageList(
                _helper.Node.SelectSingleNode(string.Concat("child::", _helper.NamespaceAlias, ":warnings"), _helper.NamespaceManager),
                _helper.NamespaceManager,
                _helper.NamespaceAlias,
                "warning");
            _debug = new Dictionary<string, string>();
            var debugNodes = _helper.Node.SelectNodes(string.Concat("child::", _helper.NamespaceAlias, ":debug"), _helper.NamespaceManager);
            if (debugNodes != null)
                foreach (XmlNode debugNode in debugNodes)
                {
                    _debug.Add(debugNode.Attributes["name"].Value, debugNode.InnerText);
                }
        }

        private static SoapFaultException CreateFaultException(XmlNode node, XmlNamespaceManager namespaceManager, string soapAlias, string validatorAlias)
        {
            var reasonNode = node.SelectSingleNode(string.Concat("child::", soapAlias, ":Reason/", soapAlias, ":Text"), namespaceManager);
            var reason = reasonNode != null ? reasonNode.InnerText : null;

            var messageIdNode = node.SelectSingleNode(string.Concat("child::", soapAlias, ":Detail/", validatorAlias, ":messageid"), namespaceManager);
            var messageId = messageIdNode != null ? messageIdNode.InnerText : null;

            var errorDetailNode = node.SelectSingleNode(string.Concat("child::", soapAlias, ":Detail/", validatorAlias, ":errordetail"), namespaceManager);
            var errorDetail = errorDetailNode != null ? errorDetailNode.InnerText : null;

            return new SoapFaultException(reason, messageId, errorDetail);
        }

        /// <summary>
        /// The address of the document validated.
        /// </summary>
        public Uri Uri
        {
            get { return _helper.GetUri("uri"); }
        }

        /// <summary>
        /// Location of the service which provided the validation result.
        /// </summary>
        public Uri CheckedBy
        {
            get { return _helper.GetUri("checkedby"); }
        }

        /// <summary>
        /// Detected (or forced) Document Type for the validated document.
        /// </summary>
        public string DocType
        {
            get { return _helper["doctype"]; }
        }

        /// <summary>
        /// Detected (or forced) Character Encoding for the validated document.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Charset")]
        public Encoding Charset
        {
            get { return _helper.GetEncoding("charset"); }
        }

        /// <summary>
        /// Whether or not the document validated passed or not formal validation.
        /// </summary>
        public bool Validity
        {
            get { return _helper.GetBool("validity"); }
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