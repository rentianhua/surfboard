using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Globalization;

namespace HiiP.Framework.Validation
{
    [ConfigurationElementType(typeof(DateDiffCompareValidatorData))]
    public class DateDiffCompareValidator : PropertyComparisonValidator
    {
        private int dateDiff;
        private ComparisonOperator comparisonOperator;
        private ValueAccess valueAccess;

        public DateDiffCompareValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, int dateDiff)
            : base(valueAccess, comparisonOperator)
        {
            this.valueAccess = valueAccess;
            this.comparisonOperator = comparisonOperator;
            this.dateDiff = dateDiff;
        }

        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            object comparand;
            string valueAccessFailureMessage;

            this.valueAccess.GetValue(currentTarget, out comparand, out valueAccessFailureMessage);

            bool valid = false;

            DateTime sourceTime = DateTime.Parse(objectToValidate.ToString());
            DateTime targetTime = DateTime.Parse(comparand.ToString());

            TimeSpan diff = sourceTime - targetTime;
            int diffDays = Math.Abs(diff.Days);

            if (this.comparisonOperator == ComparisonOperator.Equal)
            {
                valid = diffDays == 0;
            }
            switch (this.comparisonOperator)
            {
                case ComparisonOperator.NotEqual:
                    valid = diffDays != 0;
                    break;
                case ComparisonOperator.GreaterThan:
                    valid = diffDays > dateDiff;
                    break;
                case ComparisonOperator.GreaterThanEqual:
                    valid = diffDays >= dateDiff;
                    break;
                case ComparisonOperator.LessThan:
                    valid = diffDays < dateDiff;
                    break;
                case ComparisonOperator.LessThanEqual:
                    valid = diffDays <= dateDiff;
                    break;
            }

            if (!valid)
            {
                LogValidationResult(validationResults,
                    string.Format(CultureInfo.CurrentUICulture,
                        MessageTemplate),
                    currentTarget,
                    key);
            }
        }
    }
}
