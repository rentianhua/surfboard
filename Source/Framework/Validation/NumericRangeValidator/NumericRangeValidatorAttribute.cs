using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HiiP.Framework.Validation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public sealed class NumericRangeValidatorAttribute : ValidatorAttribute
    {
        private RangeBoundaryType lowerBoundType;
        private RangeBoundaryType upperBoundType;

        private int lowerBound;
        private int upperBound;

        private int maxLength;

        private string mandatoryMessageTemplate;
        private string invalidMessageTemplate;
        private string rangeMessageTemplate;
        private string maxLengthMessageTemplate;

        public NumericRangeValidatorAttribute(int lowerBound, RangeBoundaryType lowerBoundType,
            int upperBound, RangeBoundaryType upperBoundType, int maxLength,
            string mandatoryMessageTemplate, string invalidMessageTemplate,
            string rangeMessageTemplate, string maxLengthMessageTemplate)
        {
            this.lowerBound = lowerBound;
            this.lowerBoundType = lowerBoundType;
            this.upperBound = upperBound;
            this.upperBoundType = upperBoundType;
            this.maxLength = maxLength;
            this.mandatoryMessageTemplate = mandatoryMessageTemplate;
            this.invalidMessageTemplate = invalidMessageTemplate;
            this.rangeMessageTemplate = rangeMessageTemplate;
            this.maxLengthMessageTemplate = maxLengthMessageTemplate;
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            return new NumericRangeValidator(lowerBound, lowerBoundType,
                upperBound, upperBoundType, maxLength, mandatoryMessageTemplate,
                invalidMessageTemplate, rangeMessageTemplate, maxLengthMessageTemplate);
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new NumericRangeValidator(lowerBound, lowerBoundType,
                upperBound, upperBoundType, maxLength, mandatoryMessageTemplate,
                invalidMessageTemplate, rangeMessageTemplate, maxLengthMessageTemplate);
        }
    }
}
