//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
	/// <summary>
	/// Provides a mechanism to specify automatic or user requested validation for controls.
	/// </summary>
	/// <remarks>
	/// This component is an extender provider that adds properties to controls that allow 
	/// the designer of the application to specify how to validate the controls' values using
	/// the Validation Application Block.
	/// <para/>
	/// Validation can be automatic through the <see cref="Control.Validating"/> event, or initiated 
	/// via code by invoking the <see cref="ValidationProvider.PerformValidation(Control)"/> method; in 
	/// both cases the control needs to have been properly configured through the validation provider.
	/// <para/>
	/// An instance of <see cref="ErrorProvider"/> can be specified for the component; in this case
	/// the validation errors resulting from validating a control are set on the error provider as
	/// a properly formatted error message for the control.
	/// <para/>
	/// A validation provider can be enabled or disabled through the <see cref="ValidationProvider.Enabled"/>
	/// property. When it is disabled no validation will occur and the validaton provider will be considered
	/// valid, and any error messages posted to the <see cref="ErrorProvider"/> will be cleared. 
	/// If re-enabled, it will continue to be valid and no error messages will be posted until validation
	/// is triggered again.
	/// </remarks>
	/// <seealso cref="ValidationProvider.IsValid"/>
	/// <seealso cref="ValidationProvider.Enabled"/>
	/// <seealso cref="ValidationProvider.PerformValidation(Control)"/>
	/// <seealso cref="Validator"/>
	/// <seealso cref="ValidationResults"/>
	[ToolboxItemFilter("System.Windows.Forms")]
	[ToolboxBitmap(typeof(ValidationProvider))]
	[ProvideProperty("PerformValidation", typeof(Control))]
	[ProvideProperty("SourcePropertyName", typeof(Control))]
	[ProvideProperty("ValidatedProperty", typeof(Control))]
	public class ValidationProvider : Component, IExtenderProvider
	{
		private Type sourceType;
		private Hashtable items;
		internal const string DefaultValidatedProperty = "Text";

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationProvider"/> class.
		/// </summary>
		public ValidationProvider()
		{
			this.items = new Hashtable();
		}

		/// <summary>
		/// Occurs when validation for a control has been performed.
		/// </summary>
		/// <remarks>
		/// An instance of the <see cref="ValidationPerformedEventArgs"/> class holds the information
		/// about the validated control and the outcome of the validation.
		/// </remarks>
		/// <see cref="ValidationPerformedEventArgs"/>
		public event EventHandler<ValidationPerformedEventArgs> ValidationPerformed;

		/// <summary>
		/// Occurs when conversion for a control's value must be performed.
		/// </summary>
		/// <remarks>
		/// An instance of the <see cref="ValueConvertEventArgs"/> class holds the information
		/// about the value that needs conversion.
		/// </remarks>
		/// <see cref="ValueConvertEventArgs"/>
		public event EventHandler<ValueConvertEventArgs> ValueConvert;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033", Justification = "Implementation for IExtenderProvider.")]
		bool IExtenderProvider.CanExtend(object extendee)
		{
			return extendee is Control;
		}

		/// <summary>
		/// Invokes the validation process for a control.
		/// </summary>
		/// <remarks>
		/// This method allows for programmatic invocation of the validation process. The control needs to be already
		/// configured for validation with the validation provider through the extender properties.
		/// </remarks>
		/// <param name="control">The control to validate.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is not registered with the validation provider
		/// by specifying the extended properties.</exception>
		/// <exception cref="InvalidOperationException">when the <see cref="ValidationProvider.SourceTypeName"/> has not 
		/// been specified or is invalid.</exception>
		/// <exception cref="InvalidOperationException">when the source property name for the control to validate has not 
		/// been specified or is invalid.</exception>
		public void PerformValidation(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ValidatedControlItem existingValidatedControlItem = this.GetExistingValidatedControlItem(control);
			if (existingValidatedControlItem == null)
			{
				throw new ArgumentException(Resources.ExceptionControlNotExtended, "control");
			}

			PerformValidation(existingValidatedControlItem);
		}

		/// <summary>
		/// Releases managed resources.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; 
		/// <see langword="false"/> to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					foreach (ValidatedControlItem validatedControlItem in this.items.Values)
					{
						if (this.errorProvider != null)
						{
							this.errorProvider.SetError(validatedControlItem.Control, null);
						}
						validatedControlItem.Dispose();
					}
					this.items.Clear();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		internal void PerformValidation(ValidatedControlItem validatedControlItem)
		{
			if (this.enabled)
			{
				Validator validator = validatedControlItem.Validator;
				if (validator != null)
				{
					ValidationResults validationResults = validator.Validate(validatedControlItem);
					validatedControlItem.IsValid = validationResults.IsValid;

					if (this.errorProvider != null)
					{
						string errorProviderMessage = null;

						if (!validationResults.IsValid)
						{
							errorProviderMessage = FormatErrorMessage(validationResults);
						}

						this.errorProvider.SetError(validatedControlItem.Control, errorProviderMessage);
					}

					if (this.ValidationPerformed != null)
					{
						this.ValidationPerformed(this,
							new ValidationPerformedEventArgs(validatedControlItem.Control, validationResults));
					}
				}
			}
		}

		internal string FormatErrorMessage(ValidationResults validationResults)
		{
			StringBuilder sb = new StringBuilder();

			IEnumerator<ValidationResult> validationResultEnumerator
				= ((IEnumerable<ValidationResult>)validationResults).GetEnumerator();

			if (validationResultEnumerator.MoveNext())
			{
				do
				{
					sb.AppendFormat(CultureInfo.CurrentUICulture,
						this.ValidationResultFormat,
						validationResultEnumerator.Current.Message,
						validationResultEnumerator.Current.Key,
						validationResultEnumerator.Current.Tag);

					if (validationResultEnumerator.MoveNext())
					{
						sb.AppendLine();
					}
					else
					{
						break;
					}
				}
				while (true);
			}

			return sb.ToString();
		}

		internal ValidatedControlItem EnsureValidatedControlItem(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ValidatedControlItem existingItem
				= (ValidatedControlItem)this.items[control];
			if (existingItem == null)
			{
				existingItem = new ValidatedControlItem(this, control);
				this.items[control] = existingItem;
			}
			return existingItem;
		}

		internal ValidatedControlItem GetExistingValidatedControlItem(Control control)
		{
			return (ValidatedControlItem)this.items[control];
		}

		internal ValidatedControlItem GetExistingValidatedControlItem(string sourcePropertyName)
		{
			foreach (ValidatedControlItem validatedControlItem in this.items.Values)
			{
				if (validatedControlItem.SourcePropertyName.Equals(sourcePropertyName))
				{
					return validatedControlItem;
				}
			}

			return null;
		}

		internal Type GetSourceType()
		{
			if (this.sourceType == null)
			{
				if (string.IsNullOrEmpty(this.sourceTypeName))
				{
					throw new InvalidOperationException(Resources.ExceptionNoSourceTypeName);
				}
				this.sourceType = Type.GetType(this.sourceTypeName, false);
				if (this.sourceType == null)
				{
					throw new InvalidOperationException(Resources.ExceptionInvalidSourceTypeName);
				}
			}

			return this.sourceType;
		}

		#region extended properties support

		/// <summary>
		/// Gets the indication of whether automatic validation should be performed for 
		/// <paramref name="control"/> when it fires the <see cref="Control.Validating"/> event.
		/// </summary>
		/// <param name="control">The configured <see cref="Control"/>.</param>
		/// <returns><see langword="true"/> when the control should be automatically validated; 
		/// otherwise, <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		[SRDescription("DescriptionPerformValidation")]
		[SRCategory("CategoryValidation")]
		[DefaultValue(false)]
		public bool GetPerformValidation(Control control)
		{
			return this.EnsureValidatedControlItem(control).PerformValidation;
		}

		/// <summary>
		/// Sets the indication of whether automatic validation should be performed for 
		/// <paramref name="control"/> when it fires the <see cref="Control.Validating"/> event.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to configure.</param>
		/// <param name="performValidation"><see langword="true"/> if the control should be automatically validated; 
		/// otherwise, <see langword="false"/>.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		public void SetPerformValidation(Control control, bool performValidation)
		{
			ValidatedControlItem validatedControlItem = this.EnsureValidatedControlItem(control);
			validatedControlItem.PerformValidation = performValidation;
			ClearValidation(validatedControlItem);
		}

		/// <summary>
		/// Gets the name of the property on the type specified by <see cref="ValidationProvider.SourceTypeName"/> for which
		/// the validation specification should be retrieved to validate the value for <paramref name="control"/>.
		/// </summary>
		/// <param name="control">The configured <see cref="Control"/>.</param>
		/// <returns>The property name.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		[TypeConverter(typeof(RequiredIdentifierConverter))]
		[SRDescription("DescriptionSourcePropertyName")]
		[SRCategory("CategoryValidation")]
        [DefaultValue(null)]
		public string GetSourcePropertyName(Control control)
		{
			return this.EnsureValidatedControlItem(control).SourcePropertyName;
		}

		/// <summary>
		/// Sets the name of the property on the type specified by <see cref="ValidationProvider.SourceTypeName"/> for which
		/// the validation specification should be retrieved to validate the value for <paramref name="control"/>.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to configure.</param>
		/// <param name="sourcePropertyName">The property name.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		public void SetSourcePropertyName(Control control, string sourcePropertyName)
		{
			ValidatedControlItem validatedControlItem = this.EnsureValidatedControlItem(control);
			validatedControlItem.SourcePropertyName = sourcePropertyName;
			ClearValidation(validatedControlItem);
		}

		/// <summary>
		/// Gets the name of the property to use when extracting the value from <paramref name="control"/>.
		/// </summary>
		/// <param name="control">The configured <see cref="Control"/>.</param>
		/// <returns>The name of the property.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		[SRDescription("DescriptionValidatedProperty")]
		[SRCategory("CategoryValidation")]
		[DefaultValue(DefaultValidatedProperty)]
		public string GetValidatedProperty(Control control)
		{
			return this.EnsureValidatedControlItem(control).ValidatedPropertyName;
		}

		/// <summary>
		/// Sets the name of the property to use when extracting the value from <paramref name="control"/>.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to configure.</param>
		/// <param name="validatedPropertyName">The name of the property.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="control"/> is null.</exception>
		public void SetValidatedProperty(Control control, string validatedPropertyName)
		{
			ValidatedControlItem validatedControlItem = this.EnsureValidatedControlItem(control);
			validatedControlItem.ValidatedPropertyName = validatedPropertyName;
			ClearValidation(validatedControlItem);
		}

		#endregion

		/// <summary>
		/// Gets an indication of whether the values for the controls managed by the <see cref="ValidationProvider"/> have
		/// been successfully validated.
		/// </summary>
		/// <value>
		/// <see langword="true"/> when the values for all the managed controls have been successfully validated; 
		/// <see langword="false"/> otherwise.
		/// </value>
		/// <remarks>
		/// This property reflects the outcome of validations that were performed, so if there is a managed control with an invalid
		/// value for which validation has not been performed the value for this property might be <see langword="true"/>.
		/// </remarks>
		[Browsable(false)]
		public bool IsValid
		{
			get
			{
				foreach (ValidatedControlItem validatedControlItem in this.items.Values)
				{
					if (!validatedControlItem.IsValid)
					{
						return false;
					}
				}
				return true;
			}
		}

		private ErrorProvider errorProvider;
		/// <summary>
		/// Gets or sets the <see cref="ErrorProvider"/> to which formatted validation errors should be posted
		/// when validating a <see cref="Control"/>.
		/// </summary>
		[SRDescription("DescriptionErrorProvider")]
		[SRCategory("CategoryValidation")]
		public ErrorProvider ErrorProvider
		{
			get { return errorProvider; }
			set { errorProvider = value; }
		}

		private string sourceTypeName;
		/// <summary>
		/// Gets or sets the name for the type to use as the source of specifications for the validations
		/// performed by the <see cref="ValidationProvider"/>.
		/// </summary>
		[TypeConverter(typeof(RequiredIdentifierConverter))]
		[SRDescription("DescriptionSourceType")]
		[SRCategory("CategoryValidation")]
		public string SourceTypeName
		{
			get { return sourceTypeName; }
			set
			{
				sourceTypeName = value;
				this.ClearValidation();
			}
		}

		private ValidationSpecificationSource specificationSource = ValidationSpecificationSource.Both;
		/// <summary>
		/// Gets or sets the <see cref="ValidationSpecificationSource"/> indicating where to get validation specifications from.
		/// </summary>
		[DefaultValue(ValidationSpecificationSource.Both)]
		[SRDescription("DescriptionSpecificationSource")]
		[SRCategory("CategoryValidation")]
		public ValidationSpecificationSource SpecificationSource
		{
			get { return specificationSource; }
			set
			{
				specificationSource = value;
				this.ClearValidation();
			}
		}

		private string rulesetName;
		/// <summary>
		/// Gets or sets the name of the ruleset to use when retrieving validation specifications.
		/// </summary>
		[TypeConverter(typeof(RequiredIdentifierConverter))]
		[SRDescription("DescriptionRuleset")]
		[SRCategory("CategoryValidation")]
		public string RulesetName
		{
			get { return rulesetName != null ? rulesetName : string.Empty; }
			set
			{
				rulesetName = value;
				this.ClearValidation();
			}
		}

		private string validationResultFormat = @"{0}";
		/// <summary>
		/// Gets or sets the format to use when formatting validation results to post to an <see cref="ErrorProvider"/>.
		/// </summary>
		/// <remarks>
		/// The value is a standard string formatting template. The supplied format items are:
		/// <list type="table">
		/// <item><term>{0}</term><description>The <see cref="ValidationResult"/> message.</description></item>
		/// <item><term>{1}</term><description>The <see cref="ValidationResult"/> key.</description></item>
		/// <item><term>{2}</term><description>The <see cref="ValidationResult"/> tag.</description></item>
		/// </list>
		/// </remarks>
		/// <seealso cref="String.Format(string, object)"/>
		[DefaultValue("{0}")]
		[SRDescription("DescriptionValidationResultFormat")]
		[SRCategory("CategoryValidation")]
		public string ValidationResultFormat
		{
			get { return validationResultFormat; }
			set { validationResultFormat = value; }
		}

		private bool enabled = true;
		/// <summary>
		/// The indication of whether the <see cref="ValidationProvider"/> will perform validations.
		/// </summary>
		/// <remarks>
		/// No validation will be performed if the <see cref="ValidationProvider"/> is not enabled, even
		/// when requested through the <see cref="ValidationProvider.PerformValidation(Control)"/> method.
		/// <para/>
		/// All existing errors will be cleared if set to <see langword="false"/>, but will not be restated
		/// if set back to <see langword="true"/>.
		/// </remarks>
		[DefaultValue(true)]
		[SRDescription("DescriptionEnabled")]
		[SRCategory("CategoryValidation")]
		public bool Enabled
		{
			get { return enabled; }
			set
			{
				enabled = value;
				if (enabled == false)
				{
					// clear status and validation errors
					foreach (ValidatedControlItem validatedControlItem in this.items.Values)
					{
						validatedControlItem.IsValid = true;

						if (this.errorProvider != null)
						{
							this.errorProvider.SetError(validatedControlItem.Control, null);
						}
					}
				}
			}
		}

		internal void PerformCustomValueConversion(ValueConvertEventArgs e)
		{
			if (this.ValueConvert != null)
			{
				this.ValueConvert(this, e);
			}
		}

		internal bool ProvidesCustomValueConversion
		{
			get { return this.ValueConvert != null; }
		}

		internal void ClearValidation()
		{
			if (!this.DesignMode)
			{
				foreach (ValidatedControlItem validatedControlItem in this.items.Values)
				{
					ClearValidation(validatedControlItem);
				}
			}
		}

		internal void ClearValidation(ValidatedControlItem validatedControlItem)
		{
			if (!this.DesignMode)
			{
				if (this.errorProvider != null)
				{
					this.errorProvider.SetError(validatedControlItem.Control, null);
				}

				validatedControlItem.ClearValidation();
			}
		}
	}
}