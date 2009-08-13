// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// The base class for MarkupValidationResponse, MarkupValidatorAtomicMessageList, and
    /// MarkupValidatorAtomicMessage.  Exists to make parsing the SOAP response easier.
    /// </summary>
    public abstract class MarkupValidationResponseBase
    {
        private readonly XmlNode _node;
        private readonly XmlNamespaceManager _nsmgr;
        private readonly string _namespaceAlias;

        /// <summary>
        /// Constructs a new MarkupValidationResponseBase.  Used by MarkupValidatorAtomicMessageList
        /// and MarkupValidatorAtomicMessage.
        /// </summary>
        /// <param name="node">
        /// The root node of this element.  This might be a &lt;m:errors&gt; tag, a &lt;m:error&gt;
        /// tag,  a &lt;m:warnings&gt; tag, or a &lt;m:warning&gt; tag.
        /// </param>
        /// <param name="nsmgr">
        /// The namespace manager that is used to navigate this xml document.
        /// </param>
        /// <param name="namespaceAlias">
        /// The xml namespace alias that corresponds to http://www.w3.org/2005/10/markup-validator.
        /// </param>
        internal protected MarkupValidationResponseBase(XmlNode node, XmlNamespaceManager nsmgr, string namespaceAlias)
        {
            _node = node;
            _nsmgr = nsmgr;
            _namespaceAlias = namespaceAlias;
        }

        /// <summary>
        /// Constructs a new MarkupValidationResponseBase.  Used by MarkupValidationResponse.
        /// </summary>
        /// <param name="stream">The stream to read the SOAP response from.</param>
        internal protected MarkupValidationResponseBase(Stream stream)
        {
            var doc = new XmlDocument();
            _namespaceAlias = "m";
            _nsmgr = new XmlNamespaceManager(doc.NameTable);
            _nsmgr.AddNamespace("e", "http://www.w3.org/2003/05/soap-envelope");
            _nsmgr.AddNamespace(_namespaceAlias, "http://www.w3.org/2005/10/markup-validator");

            doc.Load(stream);

            _node = doc.SelectSingleNode(string.Concat("/e:Envelope/e:Body/", _namespaceAlias, ":markupvalidationresponse"), _nsmgr);
        }

        /// <summary>
        /// The root node of this element.  This might be a &lt;m:markupvalidationresponse&gt;
        /// tag, a &lt;m:errors&gt; tag, a &lt;m:error&gt; tag,  a &lt;m:warnings&gt; tag, or a
        /// &lt;m:warning&gt; tag.
        /// </summary>
        internal protected XmlNode Node
        {
            get { return _node; }
        }

        /// <summary>
        /// The namespace manager that is used to navigate this xml document.
        /// </summary>
        internal protected XmlNamespaceManager NamespaceManager
        {
            get { return _nsmgr; }
        }

        /// <summary>
        /// The xml namespace alias that corresponds to http://www.w3.org/2005/10/markup-validator.
        /// </summary>
        internal protected string NamespaceAlias
        {
            get { return _namespaceAlias; }
        }

        /// <summary>
        /// Gets a simple string element contained within the tag named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>a string, or null if empty</returns>
        internal protected string this[string name]
        {
            get
            {
                var childNode = _node.SelectSingleNode(string.Concat("child::", _namespaceAlias, ":", name), _nsmgr);
                if (childNode != null && !string.IsNullOrEmpty(childNode.InnerText))
                    return childNode.InnerText;
                return null;
            }
        }

        /// <summary>
        /// Gets the value contained within the <paramref name="name"/> tag and converts it to a Uri.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>a Uri, or null if empty</returns>
        /// <exception cref="T:System.UriFormatException"/>
        internal protected Uri GetUri(string name)
        {
            var value = this[name];
            if (value == null)
                return null;
            return new Uri(value);
        }

        /// <summary>
        /// Gets the value contained within the <paramref name="name"/> tag and converts it to an Encoding.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>an Encoding, or null if empty</returns>
        /// <exception cref="T:System.ArgumentException"/>
        internal protected Encoding GetEncoding(string name)
        {
            var value = this[name];
            if (value == null)
                return null;
            return Encoding.GetEncoding(value);
        }

        /// <summary>
        /// Gets the value contained within the <paramref name="name"/> tag and converts it to a bool.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>a bool</returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        /// <exception cref="T:System.FormatException"/>
        internal protected bool GetBool(string name)
        {
            var value = this[name];
            return bool.Parse(value);
        }

        /// <summary>
        /// Gets the value contained within the <paramref name="name"/> tag and converts it to an int.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>an int</returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        /// <exception cref="T:System.FormatException"/>
        internal protected int GetInt(string name)
        {
            var value = this[name];
            return int.Parse(value);
        }
    }
}