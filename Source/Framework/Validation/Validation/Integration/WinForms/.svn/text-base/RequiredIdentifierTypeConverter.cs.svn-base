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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
	internal class RequiredIdentifierConverter : StringConverter
	{
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string stringValue = value as string;
			if (stringValue == null)
				return false;

			return stringValue.Length > 0;
		}
	}
}
