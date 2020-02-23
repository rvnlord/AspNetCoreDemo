using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string _allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            _allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            var strings = value.ToString().Split('@');
            return string.Equals(strings[1], _allowedDomain, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}