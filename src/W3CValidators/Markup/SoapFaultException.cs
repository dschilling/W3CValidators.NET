namespace W3CValidators.Markup
{
    using System;

    public class SoapFaultException : Exception
    {
        private readonly string _reason;
        private readonly string _messageId;
        private readonly string _errorDetail;

        public SoapFaultException(string reason, string messageId, string errorDetail)
        {
            _reason = reason;
            _messageId = messageId;
            _errorDetail = errorDetail;
        }

        public string Reason
        {
            get { return this._reason; }
        }

        public string MessageId
        {
            get { return this._messageId; }
        }

        public string ErrorDetail
        {
            get { return this._errorDetail; }
        }

        public override string Message
        {
            get 
            {
                return string.Concat(
                    "Reason: ", Reason, Environment.NewLine,
                    "MessageId: ", MessageId, Environment.NewLine,
                    "ErrorDetail: ", ErrorDetail);
            }
        }
    }
}