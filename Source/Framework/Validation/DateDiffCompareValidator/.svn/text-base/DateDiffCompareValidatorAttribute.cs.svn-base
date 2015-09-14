using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Configuration;
using System.Reflection;

namespace HiiP.Framework.Validation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public sealed class DateDiffCompareValidatorAttribute : PropertyComparisonValidatorAttribute
    {
        private string propertyToCompare;
        private ComparisonOperator comparisonOperator;
        private int dateDiff;

        public DateDiffCompareValidatorAttribute(string propertyToCompare, ComparisonOperator comparisonOperator, int dateDiff)
            :base(propertyToCompare, comparisonOperator)
        {
            if (propertyToCompare == null)
            {
                throw new ArgumentNullException("propertyToCompare");
            }

            this.propertyToCompare = propertyToCompare;
            this.comparisonOperator = comparisonOperator;
            this.dateDiff = dateDiff;
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            if (string.IsNullOrEmpty(this.propertyToCompare))
            {
                throw new ConfigurationErrorsException("The property to compare is null.");
            }

            PropertyInfo propertyInfo = ownerType.GetProperty(this.propertyToCompare);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException
                    (ownerType.Name + "has errors");
            }

            return new DateDiffCompareValidator
                (memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo),
                this.comparisonOperator,
                this.dateDiff);
        }
    }
}
