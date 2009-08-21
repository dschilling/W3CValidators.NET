namespace W3CValidators.Markup
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when the validator service sends us a soap fault message.
    /// </summary>
    [Serializable]
    public class SoapFaultException : Exception
    {
        private readonly string _reason;
        private readonly string _messageId;
        private readonly string _errorDetail;

        /// <summary>
        /// Creates a new SoapFaultException.
        /// </summary>
        public SoapFaultException(string reason, string messageId, string errorDetail)
        {
            _reason = reason;
            _messageId = messageId;
            _errorDetail = errorDetail;
        }

        protected SoapFaultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            _reason = info.GetString("Reason");
            _messageId = info.GetString("MessageId");
            _errorDetail = info.GetString("ErrorDetail");
        }

        /// <summary>
        /// A short message describing in plain words what went wrong.
        /// </summary>
        public string Reason
        {
            get { return this._reason; }
        }

        /// <summary>
        /// A token that identifies the problem.
        /// </summary>
        public string MessageId
        {
            get { return this._messageId; }
        }

        /// <summary>
        /// A detailed message that describes what went wrong.
        /// </summary>
        public string ErrorDetail
        {
            get { return this._errorDetail; }
        }

        /// <summary>
        /// Returns a message describing the exception.
        /// </summary>
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("Reason", _reason);
            info.AddValue("MessageId", _messageId);
            info.AddValue("ErrorDetail", _errorDetail);

            base.GetObjectData(info, context);
        }
    }
}