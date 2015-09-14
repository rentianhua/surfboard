using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HiiP.Framework.Validation
{
    public class DataSetValidatorAttribute : ValidatorAttribute
    {
        private string _validatorType = string.Empty;
        private string _tableName = string.Empty;

        public DataSetValidatorAttribute(string tableName, string validatorType)
        {
            this._tableName = tableName;
            this._validatorType = validatorType;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DataSetValidator(_tableName, _validatorType, this.Ruleset);
        }
    }
}
