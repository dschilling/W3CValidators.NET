// Copyright (c) 2011 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// The base class for MarkupValidatorResponse, MarkupValidatorAtomicMessageList, and
    /// MarkupValidatorAtomicMessage.  Exists to make parsing the SOAP response easier.
    /// </summary>
    internal class XmlHelper
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
        /// <param name="namespaceManager">
        /// The namespace manager that is used to navigate this xml document.
        /// </param>
        /// <param name="namespaceAlias">
        /// The xml namespace alias that corresponds to http://www.w3.org/2005/10/markup-validator.
        /// </param>
        internal XmlHelper(XmlNode node, XmlNamespaceManager namespaceManager, string namespaceAlias)
        {
            _node = node;
            _nsmgr = namespaceManager;
            _namespaceAlias = namespaceAlias;
        }

        /// <summary>
        /// The root node of this element.  This might be a &lt;m:markupvalidationresponse&gt;
        /// tag, a &lt;m:errors&gt; tag, a &lt;m:error&gt; tag,  a &lt;m:warnings&gt; tag, or a
        /// &lt;m:warning&gt; tag.
        /// </summary>
        internal XmlNode Node
        {
            get { return _node; }
        }

        /// <summary>
        /// The namespace manager that is used to navigate this xml document.
        /// </summary>
        internal XmlNamespaceManager NamespaceManager
        {
            get { return _nsmgr; }
        }

        /// <summary>
        /// The xml namespace alias that corresponds to http://www.w3.org/2005/10/markup-validator.
        /// </summary>
        internal string NamespaceAlias
        {
            get { return _namespaceAlias; }
        }

        /// <summary>
        /// Gets a simple string element contained within the tag named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The tag to get the value of.</param>
        /// <returns>a string, or null if empty</returns>
        internal string this[string name]
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
        internal Uri GetUri(string name)
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
        internal Encoding GetEncoding(string name)
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
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool")]
        internal bool GetBool(string name)
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
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        internal int GetInt(string name)
        {
            var value = this[name];
            return int.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}