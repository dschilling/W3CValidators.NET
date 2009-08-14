namespace W3CValidators.NUnit
{
    using global::NUnit.Framework.Constraints;

    /// <summary>
    /// Helper class that supplies constraints for verifying validity of documents with the W3C validators.
    /// </summary>
    public static class IsValid
    {
        /// <summary>
        /// Returns a constraint that checks a document for valid markup.
        /// </summary>
        public static IResolveConstraint Markup()
        {
            return new MarkupConstraint();
        }
    }
}