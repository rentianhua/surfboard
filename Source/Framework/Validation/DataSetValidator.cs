using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Collections.Specialized;

namespace HiiP.Framework.Validation
{
    public interface IValidationData
    {
        void SetData(DataRow dataRow);
    }

    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class DataSetValidator : Validator<DataSet>
    {
        private string _tableName = string.Empty;
        private string _validatorTypeName = string.Empty;
        private string _ruleSet = string.Empty;

        public DataSetValidator(NameValueCollection attributes)
            : base(null, null)
        {
            this._tableName = attributes.Get("TableName");
            this._validatorTypeName = attributes.Get("ValidatorTypeName");
        }

        public DataSetValidator(string tableName, string validatorTypeName, string ruleSet)
            : this(tableName, validatorTypeName, ruleSet, null, null)
        {
        }

        public DataSetValidator(string tableName, string validatorTypeName, string ruleSet, string messageTemplate)
            : this(tableName, validatorTypeName, ruleSet, messageTemplate, null)
        {
        }

        public DataSetValidator(string tableName, string validatorTypeName, string ruleSet, string messageTemplate, string tag)
            : base(messageTemplate, tag)
        {
            this._tableName = tableName;
            this._validatorTypeName = validatorTypeName;
            this._ruleSet = ruleSet;
        }

        protected override void DoValidate(DataSet dataSet, object currentTarget, string key, ValidationResults validationResults)
        {
            ValidationResults dataSetVR = new ValidationResults();
            Type validatorType = Type.GetType(this._validatorTypeName);

            IValidationData data = Activator.CreateInstance(validatorType) as IValidationData;
            bool isValid = true;
            foreach (DataRow row in dataSet.Tables[this._tableName].Rows)
            {
                data.SetData(row);
                Validator validator = ValidationFactory.CreateValidator(validatorType, this._ruleSet);
                dataSetVR = validator.Validate(data);
                if (!dataSetVR.IsValid)
                {
                    isValid = false;
                    validationResults.AddAllResults(dataSetVR);
                }
            }
            if (!isValid)
            {
                string message = this.MessageTemplate;
                this.LogValidationResult(validationResults, message, currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return "DataSet validation"; }
        }
    }
}
