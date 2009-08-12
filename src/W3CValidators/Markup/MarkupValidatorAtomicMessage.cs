// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;

    /// <summary>
    /// A single error or warning.
    /// </summary>
    public class MarkupValidatorAtomicMessage
    {
        /// <summary>
        /// Within the source code of the validated document, refers to the line where the error or
        /// warning was detected.
        /// </summary>
        public int Line { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Within the source code of the validated document, refers to the column of the line
        /// where the error or warning was detected.
        /// </summary>
        public int Col { get { throw new NotImplementedException(); } }

        /// <summary>
        /// The actual error or warning message.
        /// </summary>
        public string Message { get { throw new NotImplementedException(); } }

        /// <summary>
        /// The number/identifier of the error or warning, as addressed internally by the
        /// validator.
        /// </summary>
        public string MessageId { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Explanation for the error or warning. Given as an HTML fragment.
        /// </summary>
        public string Explanation { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Snippet of the source where the error or warning was found. Given as an HTML fragment.
        /// </summary>
        public string Source { get { throw new NotImplementedException(); } }
    }
}