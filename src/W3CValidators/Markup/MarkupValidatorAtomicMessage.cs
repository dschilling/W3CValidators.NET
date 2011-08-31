// Copyright (c) 2011 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Xml;

    /// <summary>
    /// A single error or warning.
    /// </summary>
    public class MarkupValidatorAtomicMessage
    {
        private readonly XmlHelper _helper;

        internal MarkupValidatorAtomicMessage(XmlNode node, XmlNamespaceManager namespaceManager, string namespaceAlias)
        {
            _helper = new XmlHelper(node, namespaceManager, namespaceAlias);
        }

        /// <summary>
        /// Within the source code of the validated document, refers to the line where the error or
        /// warning was detected.
        /// </summary>
        public int Line
        {
            get { return _helper.GetInt("line"); }
        }

        /// <summary>
        /// Within the source code of the validated document, refers to the column of the line
        /// where the error or warning was detected.
        /// </summary>
        public int Col
        {
            get { return _helper.GetInt("col"); }
        }

        /// <summary>
        /// The actual error or warning message.
        /// </summary>
        public string Message
        {
            get { return _helper["message"]; }
        }

        /// <summary>
        /// The number/identifier of the error or warning, as addressed internally by the
        /// validator.
        /// </summary>
        public string MessageId
        {
            get { return _helper["messageid"]; }
        }

        /// <summary>
        /// Explanation for the error or warning. Given as an HTML fragment.
        /// </summary>
        public string Explanation
        {
            get { return _helper["explanation"]; }
        }

        /// <summary>
        /// Snippet of the source where the error or warning was found. Given as an HTML fragment.
        /// </summary>
        public string Source
        {
            get { return _helper["source"]; }
        }

        /// <summary>
        /// Creates a succinct string describing the problem.
        /// </summary>
        public override string ToString()
        {
            return this.ToString(null);
        }

        /// <summary>
        /// Creates a succinct string describing the problem.
        /// </summary>
        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, "Line: {0}; Col: {1}; Message: {2}; Source: {3};", Line, Col, Message, Source);
        }
    }
}