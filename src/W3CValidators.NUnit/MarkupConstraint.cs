namespace W3CValidators.NUnit
{
    using System;
    using global::NUnit.Framework.Constraints;
    using Markup;

    /// <summary>
    /// A constraint that checks a markup document for validity.
    /// </summary>
    internal class MarkupConstraint : Constraint
    {
        private readonly MarkupValidatorClient _client;
        private MarkupValidatorResponse _response;

        /// <summary>
        /// Constructs a new MarkupConstraint with the default MarkupValidatorClient settings.
        /// </summary>
        public MarkupConstraint() : this(new MarkupValidatorClient()) { }

        /// <summary>
        /// Constructs a new MarkupConstraint with the specified MarkupValidatorClient.
        /// </summary>
        /// <param name="client">the client to use</param>
        public MarkupConstraint(MarkupValidatorClient client)
        {
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
            if (actual is Uri)
                _response = _client.CheckByUri((Uri)actual, null);
            else if (actual is byte[])
                _response = _client.CheckByUpload((byte[])actual, null);
            else if (actual is string)
                _response = _client.CheckByFragment((string)actual, null);
            else
                throw new ArgumentOutOfRangeException("actual", "actual must be a Uri, a string, or a byte array");

            return _response.Validity;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}