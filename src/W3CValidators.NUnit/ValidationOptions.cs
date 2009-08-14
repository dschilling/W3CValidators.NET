namespace W3CValidators.NUnit
{
    using System;

    /// <summary>
    /// Various flags to modify the behaviour of the validation constraint.
    /// </summary>
    [Flags]
    public enum ValidationOptions
    {
        /// <summary>
        /// Create a constraint with the normal options, i.e. uri's will be sent to the validator
        /// by the uri method (not the upload method), and strings that contain valid uri's will
        /// be converted into uri's.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Indicates that the uri will probably not be accessible to the validator (maybe it's on
        /// localhost), so instead of sending the uri to the validator, we'll download the actual
        /// document and sent it instead.
        /// </summary>
        PrivateDocument = 1,

        /// <summary>
        /// If a string is supplied that can be converted to a uri, interpret it as a uri instead
        /// of document data.
        /// </summary>
        DontConvertStringToUri = 2
    }
}