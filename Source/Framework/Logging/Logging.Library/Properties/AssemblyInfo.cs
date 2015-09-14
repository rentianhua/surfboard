using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;


[assembly: AssemblyTitle("HiiP.Framework.Logging.Library")]
[assembly: AssemblyDescription("HiiP Framework Logging Library")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: AssemblyFileVersion("2.0.11020.0")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7286c031-3587-4945-9686-2a096912e8ed")]


[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringTracer.#PeekLogicalOperationStack()")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringTracer.#StopLogicalOperation()")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringTracer.#StartLogicalOperation(System.String)")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringTracer.#SetActivityId(System.Guid)")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "HiiP.Framework.Logging.Library.Utility.#GetLoggingConnectionStringName()")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerAttribute.#Categories")]
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerAttribute")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringLogEntry.#IpAddress", MessageId = "Ip")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerAttribute.#ModuleID", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerAttribute.#FunctionID", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerData+MonitoringCallHandlerAssembler")]
[module: SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Scope = "type", Target = "HiiP.Framework.Logging.Library.SecurityLogger")]
[module: SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#PopulateDefaultLogEntry()")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#PopulatePreLogEntry(HiiP.Framework.Logging.Library.MonitoringLogEntry)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#GetSecondsElapsed(System.Nullable`1<System.Int64>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#GetSecondsElapsed(System.Nullable`1<System.Int64>)", MessageId = "System.Convert.ToDecimal(System.Object)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#PopulatePostLogEntry(HiiP.Framework.Logging.Library.MonitoringLogEntry)")]
[module: SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringPopulateLogEntry.#PopulateLogEntryForCallHandler(Microsoft.Practices.EnterpriseLibrary.PolicyInjection.IMethodInvocation,System.Collections.Generic.List`1<System.String>,System.Boolean,System.Boolean,Microsoft.Practices.EnterpriseLibrary.PolicyInjection.IMethodReturn)")]
[module: SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandler.#Categories")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringLogEntry.#Flag", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.Library.HiiPLoggingExceptionFormatter.#WriteAdditionalInfo(System.Collections.Specialized.NameValueCollection)", MessageId = "System.DateTime.ToString(System.String)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.Library.HiiPLoggingExceptionFormatter.#WriteDateTime(System.DateTime)", MessageId = "System.DateTime.ToString(System.String)")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.Library.SecurityLogger.#WriteLogWhenLoginSuccessful(System.Nullable`1<System.Int64>,System.Nullable`1<System.Int64>,System.Int64)", MessageId = "Login")]
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "HiiP.Framework.Logging.Library.MonitoringCallHandlerAttribute")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringLogEntry.#InstanceID", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.HiiPLoggingExceptionHandler.#WriteToLog(System.String,System.Collections.IDictionary,System.String)", MessageId = "Instance")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.Library.MonitoringLogEntry.#UserID", MessageId = "ID")]

[module: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "HiiP.Framework.Logging.Library.Utility")]

[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "type", Target = "HiiP.Framework.Logging.Library.FilterFlag", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.Library.FilterFlag.#InstrumentationFlag", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.Library.FilterFlag.#MonitoringFlag", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.Library.FilterFlag.#UsageFlag", MessageId = "Flag")]
