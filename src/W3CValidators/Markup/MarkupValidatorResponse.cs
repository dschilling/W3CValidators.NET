// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class MarkupValidatorResponse
    {
        private readonly XmlNode _root;
        private readonly XmlNamespaceManager _nsmgr;
        private const string NamespaceAlias = "v";

        public MarkupValidatorResponse(Stream stream)
        {
            var doc = new XmlDocument();
            _nsmgr = new XmlNamespaceManager(doc.NameTable);
            _nsmgr.AddNamespace("e", "http://www.w3.org/2003/05/soap-envelope");
            _nsmgr.AddNamespace(NamespaceAlias, "http://www.w3.org/2005/10/markup-validator");

            doc.Load(stream);

            _root = doc.SelectSingleNode("/e:Envelope/e:Body/v:markupvalidationresponse", _nsmgr);
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

        public Uri Uri
        {
            get { return this.GetUri("uri"); }
        }

        public Uri CheckedBy
        {
            get { return this.GetUri("checkedby"); }
        }

        public string DocType
        {
            get { return this["doctype"]; }
        }

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

        public ICollection<MarkupValidatorAtomicMessage> Errors { get { throw new NotImplementedException(); } }
        public ICollection<MarkupValidatorAtomicMessage> Warnings { get { throw new NotImplementedException(); } }
    }
}