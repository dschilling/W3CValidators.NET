using System;
using System.Configuration;

namespace W3CValidators
{
    public class ValidatorConfigSection : ConfigurationSection
    {
        /// <summary>
        /// The URL for the W3C validator.  This is used by <c>MarkupValidatorClient</c>'s
        /// parameterless constructor. Default value is http://validator.w3.org/check
        /// </summary>
        [ConfigurationProperty("markupValidatorUri", DefaultValue = "http://validator.w3.org/check", IsRequired = false)]
        public Uri MarkupValidatorUri
        {
            get { return (Uri)this["markupValidatorUri"]; }
            set { this["markupValidatorUri"] = value; }
        }
    }
}