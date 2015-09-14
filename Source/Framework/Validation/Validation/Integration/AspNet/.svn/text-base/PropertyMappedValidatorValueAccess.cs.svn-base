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
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet
{
	internal class PropertyMappedValidatorValueAccess : ValueAccess
	{
		private string propertyName;

		public PropertyMappedValidatorValueAccess(string propertyName)
		{
			this.propertyName = propertyName;
		}

		public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
		{
			value = null;
			valueAccessFailureMessage = null;

			PropertyProxyValidator validator = source as PropertyProxyValidator;

			if (this.propertyName.Equals(validator.PropertyName))
			{
				return validator.GetValue(out value, out valueAccessFailureMessage);
			}
			else
			{
				foreach (BaseValidator siblingValidator in validator.Page.Validators)
				{
					PropertyProxyValidator siblingPropertyProxyValidator = siblingValidator as PropertyProxyValidator;

					// the right source for the value must:
					// - be a proxy validtor
					// - belonging to the same naming container as the target of the validation
					// - mapped to the required property
					// - with matching source type name
					if (siblingPropertyProxyValidator != null
						&& this.propertyName.Equals(siblingPropertyProxyValidator.PropertyName)
						&& validator.NamingContainer == siblingPropertyProxyValidator.NamingContainer
						&& (validator.SourceTypeName != null
							&& validator.SourceTypeName.Equals(siblingPropertyProxyValidator.SourceTypeName)))
					{
						return siblingPropertyProxyValidator.GetValue(out value, out valueAccessFailureMessage);
					}
				}
			}

			valueAccessFailureMessage = string.Format(CultureInfo.CurrentUICulture,
				Resources.ErrorNonMappedProperty,
				this.propertyName);
			return false;
		}

		public override string Key
		{
			get { return this.propertyName; }
		}
	}
}
