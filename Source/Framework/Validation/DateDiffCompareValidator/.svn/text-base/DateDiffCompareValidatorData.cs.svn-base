using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;

namespace HiiP.Framework.Validation
{
    public class DateDiffCompareValidatorData : PropertyComparisonValidatorData
    {
        public DateDiffCompareValidatorData()
            : base()
        { }

        public DateDiffCompareValidatorData(string name)
            : base(name)
        { }

        private const string DateDiffPropertyName = "dateDiff";

        [ConfigurationProperty(DateDiffPropertyName)]
        public int DateDiff
        {
            get { return (int)this[DateDiffPropertyName]; }
            set { this[DateDiffPropertyName] = value; }
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            if (string.IsNullOrEmpty(base.PropertyToCompare))
            {
                throw new ConfigurationErrorsException("The property to compare is null");
            }

            PropertyInfo propertyInfo = ownerType.GetProperty(PropertyToCompare);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException
                    (ownerType.Name + "has errors");
            }

            return new DateDiffCompareValidator
                (memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo),
                base.ComparisonOperator,
                DateDiff);
        }
    }
}
