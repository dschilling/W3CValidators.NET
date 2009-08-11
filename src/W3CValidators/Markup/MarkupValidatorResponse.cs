namespace W3CValidators.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MarkupValidatorResponse
    {
        public Uri Uri { get { throw new NotImplementedException(); } }
        public Uri CheckedBy { get { throw new NotImplementedException(); } }
        public string DocType { get { throw new NotImplementedException(); } }
        public Encoding Charset { get { throw new NotImplementedException(); } }
        public bool Validity { get { throw new NotImplementedException(); } }
        public ICollection<MarkupValidatorAtomicMessage> Errors { get { throw new NotImplementedException(); } }
        public ICollection<MarkupValidatorAtomicMessage> Warnings { get { throw new NotImplementedException(); } }
    }
}