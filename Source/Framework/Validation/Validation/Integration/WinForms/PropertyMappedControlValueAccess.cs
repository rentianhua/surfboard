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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
	/// <summary>
	/// Returns the value from a control.
	/// </summary>
	internal class PropertyMappedControlValueAccess : ValueAccess
	{
		private string propertyName;

		public PropertyMappedControlValueAccess(string propertyName)
		{
			this.propertyName = propertyName;
		}

		public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
		{
			value = null;
			valueAccessFailureMessage = null;

			ValidatedControlItem validatedControlItem = source as ValidatedControlItem;

			if (validatedControlItem == null)
			{
				throw new InvalidOperationException(Resources.ExceptionValueAccessRequiresValidatedControlItem);
			}

			if (!this.propertyName.Equals(validatedControlItem.SourcePropertyName))
			{
				validatedControlItem = validatedControlItem.ValidationProvider.GetExistingValidatedControlItem(this.propertyName);
				if (validatedControlItem == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture,
							Resources.ExceptionValueAccessPropertyNotFound,
							this.propertyName));
				}
			}

			return validatedControlItem.GetValue(out value, out valueAccessFailureMessage);
		}

		public override string Key
		{
			get { return this.propertyName; }
		}
	}
}