namespace W3CValidators.NUnit
{
    using System;
    using System.Net;
    using global::NUnit.Framework.Constraints;
    using Markup;

    /// <summary>
    /// A constraint that checks a markup document for validity.
    /// </summary>
    internal class MarkupConstraint : Constraint
    {
        private readonly ValidationOptions _options;
        private readonly MarkupValidatorClient _client;
        private MarkupValidatorResponse _response;

        /// <summary>
        /// Constructs a new MarkupConstraint with the default MarkupValidatorClient settings.
        /// </summary>
        public MarkupConstraint(ValidationOptions options) : this(options, new MarkupValidatorClient()) { }

        /// <summary>
        /// Constructs a new MarkupConstraint with the specified MarkupValidatorClient.
        /// </summary>
        /// <param name="options">flags to modify the behaviour of the validation constraint</param>
        /// <param name="client">the client to use</param>
        public MarkupConstraint(ValidationOptions options, MarkupValidatorClient client)
        {
            _options = options;
            _client = client;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>
        /// True for success, false for failure
        /// </returns>
        public override bool Matches(object actual)
        {
            if (actual == null)
                throw new ArgumentNullException("actual");

            _response = this.GetResponse(actual);

            return _response.Validity;
        }

        private MarkupValidatorResponse GetResponse(object actual)
        {
            var uri = actual as Uri;
            if (uri != null)
                return this.Check(uri);
            var bytes = actual as byte[];
            if (bytes != null)
                return this._client.CheckByUpload(bytes, null);
            var str = actual as string;
            if (str != null)
            {
                if ((this._options & ValidationOptions.DoNotConvertStringToUri) == ValidationOptions.DoNotConvertStringToUri)
                    return this._client.CheckByFragment(str, null);

                try
                {
                    var convertedUri = new Uri(str);
                    return this.Check(convertedUri);
                }
                catch (UriFormatException)
                {
                    return this._client.CheckByFragment(str, null);
                }
            }

            throw new ArgumentOutOfRangeException("actual", "actual must be a Uri, a string, or a byte array");
        }

        private MarkupValidatorResponse Check(Uri uri)
        {
            if ((this._options & ValidationOptions.PrivateDocument) != ValidationOptions.PrivateDocument)
                return this._client.CheckByUri(uri, null);

            using (var webClient = new WebClient())
            {
                var data = webClient.DownloadData(uri);
                return this._client.CheckByUpload(data, null);
            }
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteLine("The document did not contain valid markup.");
            foreach (var error in _response.Errors)
            {
                writer.WriteLine(error);
            }
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            this.WriteDescriptionTo(writer);
        }
    }
}