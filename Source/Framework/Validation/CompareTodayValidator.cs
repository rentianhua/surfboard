using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Specialized;

namespace HiiP.Framework.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class CompareTodayValidator : Validator<DateTime>
    {
        private ComparisonOperator operation;

        public CompareTodayValidator(NameValueCollection attributes)
            : base(null, null)
        {
            operation = (ComparisonOperator)Enum.Parse(typeof(ComparisonOperator), attributes.Get("Operator"));
        }

        public CompareTodayValidator(ComparisonOperator operation)
            : this(operation, null, null)
        {
        }

        public CompareTodayValidator(ComparisonOperator operation, string messageTemplate)
            : this(operation, messageTemplate, null)
        {
        }

        public CompareTodayValidator(ComparisonOperator operation, string messageTemplate, string tag)
            : base(messageTemplate, tag)
        {
            this.operation = operation;
        }


        protected override void DoValidate(DateTime objectToValidate, object currentTarget, string key, Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults validationResults)
        {
            bool valid = true;
            switch (operation)
            {
                case ComparisonOperator.GreaterThan:
                    if (objectToValidate <= DateTime.Today) valid = false;
                    break;
                case ComparisonOperator.LessThan:
                    if (objectToValidate >= DateTime.Today) valid = false;
                    break;
                case ComparisonOperator.GreaterThanEqual:
                    if (objectToValidate < DateTime.Today) valid = false;
                    break;
                case ComparisonOperator.LessThanEqual:
                    if (objectToValidate > DateTime.Today) valid = false;
                    break;

            }
            if (!valid)
            {
                string message = string.Format(this.MessageTemplate,key,operation);
                this.LogValidationResult(validationResults, message, currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return "Must be later than current date."; }
        }
    }
}
