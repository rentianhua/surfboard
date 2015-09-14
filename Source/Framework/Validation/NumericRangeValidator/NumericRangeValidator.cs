using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HiiP.Framework.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class NumericRangeValidator : Validator
    {
        private RangeBoundaryType lowerBoundType;
        private RangeBoundaryType upperBoundType;

        private int lowerBound;
        private int upperBound;

        private int maxLength;

        private string mandatoryMessageTemplate=string.Empty;
        private string invalidMessageTemplate=string.Empty;
        private string rangeMessageTemplate=string.Empty;
        private string maxLengthMessageTemplate=string.Empty;

        public NumericRangeValidator(NameValueCollection attributes)
            :base(null, null)
        {
            lowerBoundType = (RangeBoundaryType)Enum.Parse(typeof(RangeBoundaryType), attributes.Get("lowerBoundType"));
            upperBoundType = (RangeBoundaryType)Enum.Parse(typeof(RangeBoundaryType), attributes.Get("upperBoundType"));

            int lower;
            if (int.TryParse(attributes.Get("lowerBound"), out lower))
            {
                lowerBound = lower;
            }
            else
            {
                lowerBound = int.MinValue;
            }

            int upper;
            if (int.TryParse(attributes.Get("upperBound"), out upper))
            {
                upperBound = upper;
            }
            else
            {
                upperBound = int.MaxValue;
            }

            int max;
            if (int.TryParse(attributes.Get("maxLength"), out max))
            {
                maxLength = max;
            }
            else
            {
                maxLength = int.MaxValue;
            }

            if (attributes.Get("mandatoryMessageTemplate") != null)
            {
                this.mandatoryMessageTemplate = attributes.Get("mandatoryMessageTemplate").ToString();
            }

            if (attributes.Get("invalidMessageTemplate") != null)
            {
                this.invalidMessageTemplate = attributes.Get("invalidMessageTemplate").ToString();
            }

            if (attributes.Get("rangeMessageTemplate") != null)
            {
                this.rangeMessageTemplate = attributes.Get("rangeMessageTemplate").ToString();
            }

            if (attributes.Get("maxLengthMessageTemplate") != null)
            {
                this.maxLengthMessageTemplate = attributes.Get("maxLengthMessageTemplate").ToString();
            }
        }

        public NumericRangeValidator(int lowerBound, RangeBoundaryType lowerBoundType,
            int upperBound, RangeBoundaryType upperBoundType, int maxLength,
            string mandatoryMessageTemplate, string invalidMessageTemplate, string rangeMessageTemplate,
            string maxLengthMessageTemplate)
            : base(null, null)
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

        protected override string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }

        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            //Mandatory check
            if (objectToValidate.ToString().Length == 0)
            {
                LogValidationResult(validationResults,
                                                  string.Format(CultureInfo.CurrentUICulture,
                                                      mandatoryMessageTemplate),
                                                  currentTarget,
                                                  key);
                return;
            }

            //Numeric check
            if(!Regex.IsMatch(objectToValidate.ToString(), @"^\d*$"))
            {
                LogValidationResult(validationResults,
                                                 string.Format(CultureInfo.CurrentUICulture,
                                                     invalidMessageTemplate),
                                                 currentTarget,
                                                 key);
                return;
            }

            //MaxLength check
            if (objectToValidate.ToString().Length > this.maxLength)
            {
                LogValidationResult(validationResults,
                                   string.Format(CultureInfo.CurrentUICulture,
                                       maxLengthMessageTemplate),
                                   currentTarget,
                                   key);
                return;
            }

            bool lowerValid = false;
            bool upperValid = false;
            int compared =int.Parse(objectToValidate.ToString());

            if (lowerBoundType.Equals(RangeBoundaryType.Exclusive))
            {
                lowerValid = compared > lowerBound;
            }
            else if (lowerBoundType.Equals(RangeBoundaryType.Inclusive))
            {
                lowerValid = compared >= lowerBound;
            }

            if (upperBoundType.Equals(RangeBoundaryType.Exclusive))
            {
                upperValid = compared < upperBound;
            }
            else if (upperBoundType.Equals(RangeBoundaryType.Inclusive))
            {
                upperValid = compared <= upperBound;
            }

            if (!lowerValid || !upperValid)
            {
                LogValidationResult(validationResults,
                    string.Format(CultureInfo.CurrentUICulture,
                        rangeMessageTemplate),
                    currentTarget,
                    key);
            }
        }
    }
}
