using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HiiP.Framework.Validation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public sealed class CompareTodayValidatorAttribute : ValidatorAttribute
    {
        private ComparisonOperator _comparisonOperator;

        public CompareTodayValidatorAttribute(ComparisonOperator comparisonOperator)
        {
            this._comparisonOperator = comparisonOperator;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new CompareTodayValidator(_comparisonOperator);
        }
    }
}
