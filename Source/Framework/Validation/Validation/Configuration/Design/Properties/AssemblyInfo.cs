//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright  Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Permissions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: ComVisible(false)]

[assembly: AssemblyTitle("Enterprise Library Validation Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Validation Application Block Design")]
[assembly: AssemblyVersion("3.1.0.0")]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests")]

[assembly: Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationDesignManager(typeof(Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidationConfigurationDesignManager))]
