//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright ?Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
	internal sealed class ValidatedControlItem : IValidationIntegrationProxy
	{
		public ValidatedControlItem(ValidationProvider validationProvider, Control control)
		{
			this.validationProvider = validationProvider;
			this.control = control;
			this.control.Validating += this.OnValidating;
			this.isValid = true;
			this.validatedPropertyName = ValidationProvider.DefaultValidatedProperty;
		}

		public void Dispose()
		{
			if (this.control != null)
			{
				this.control.Validating -= this.OnValidating;
				this.control = null;
			}
			this.validator = null;
		}

		internal bool GetValue(out object value, out string failureMessage)
		{
			ValidationIntegrationHelper helper = new ValidationIntegrationHelper(this);
			return helper.GetValue(out value, out failureMessage);
		}

		private void OnValidating(object source, CancelEventArgs e)
		{
			if (this.PerformValidation)
			{
				this.validationProvider.PerformValidation(this);
				e.Cancel = !this.IsValid;
			}
		}

		private ValidationProvider validationProvider;
		private Control control;
		private Validator validator;
		private bool isValid;

		public ValidationProvider ValidationProvider
		{
			get { return validationProvider; }
		}

		public Control Control
		{
			get { return control; }
		}

		public Validator Validator
		{
			get
			{
				if (validator == null)
				{
					validator = new ValidationIntegrationHelper(this).GetValidator();
				}

				return validator;
			}
		}

		public bool IsValid
		{
			get { return isValid; }
			set { isValid = value; }
		}

		#region extender properties

		private bool performValidation;
		public bool PerformValidation
		{
			get { return performValidation; }
			set { performValidation = value; }
		}

		private string sourcePropertyName;
		public string SourcePropertyName
		{
			get { return sourcePropertyName; }
			set { sourcePropertyName = value; }
		}

		private string validatedPropertyName;
		public string ValidatedPropertyName
		{
			get { return validatedPropertyName; }
			set { validatedPropertyName = value; }
		}

		#endregion

		#region IValidationIntegrationProxy Members

		void IValidationIntegrationProxy.PerformCustomValueConversion(ValueConvertEventArgs e)
		{
			this.validationProvider.PerformCustomValueConversion(e);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304", Justification = "Warning on call to System.Type.InvokeMember")]
        object IValidationIntegrationProxy.GetRawValue()
        {
            if (ValidationProvider.DefaultValidatedProperty.Equals(this.validatedPropertyName))
            {
                return this.control.Text;
            }

            try
            {
                return this.control.GetType().InvokeMember(this.validatedPropertyName,
                                                           BindingFlags.GetProperty | BindingFlags.Public |
                                                           BindingFlags.Instance,
                                                           null,
                                                           this.control,
                                                           null);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentUICulture,
                                  Resources.ExceptionValidatedControlPropertyNotFound,
                                  this.validatedPropertyName,
                                  this.control.Name),
                    e);
            }

        }

	    MemberValueAccessBuilder IValidationIntegrationProxy.GetMemberValueAccessBuilder()
		{
			return new PropertyMappedControlValueAccessBuilder();
		}

		bool IValidationIntegrationProxy.ProvidesCustomValueConversion
		{
			get { return this.validationProvider.ProvidesCustomValueConversion; }
		}

		string IValidationIntegrationProxy.Ruleset
		{
			get { return this.validationProvider.RulesetName; }
		}

		ValidationSpecificationSource IValidationIntegrationProxy.SpecificationSource
		{
			get { return this.validationProvider.SpecificationSource; }
		}

		string IValidationIntegrationProxy.ValidatedPropertyName
		{
			get { return this.SourcePropertyName; }
		}

		Type IValidationIntegrationProxy.ValidatedType
		{
			get { return this.validationProvider.GetSourceType(); }
		}

		#endregion

		internal void ClearValidation()
		{
			this.validator = null;
			this.isValid = true;
		}
	}
}
