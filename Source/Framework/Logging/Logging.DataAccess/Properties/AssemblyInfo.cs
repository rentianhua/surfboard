using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

[assembly: AssemblyTitle("HiiP.Framework.Logging.DataAccess")]
[assembly: AssemblyDescription("HiiP Framework Logging DataAccess")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: AssemblyFileVersion("3.0.20603.0")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2770199a-e664-491a-9545-6305b0a1a6d4")]


[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.AuditLogViewDA.#GetAppID(System.String)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#GetInstrumentationLogIDRangeByLogTime(HiiP.Framework.Logging.Interface.ValidationEntity.DateTimeCompare)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#GetLogIDRangeByLogTime(HiiP.Framework.Logging.Interface.ValidationEntity.DateTimeCompare)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#RetrieveInstrumentation(HiiP.Framework.Logging.Interface.ValidationEntity.LogIDPairEntity,System.String,System.String,System.String,System.String,System.String,System.String,System.String)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#GetLogsByID(System.String)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#RetrieveExceptionLog(HiiP.Framework.Logging.Interface.ValidationEntity.LogIDPairEntity,System.String,System.String,System.String,System.String,System.String,System.String)", MessageId = "ID")]
[module: SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#GetIDRangeByLogTime(HiiP.Framework.Logging.Interface.ValidationEntity.DateTimeCompare,System.String)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#RetrieveInstrumentation(HiiP.Framework.Logging.Interface.ValidationEntity.LogIDPairEntity,System.String,System.String,System.String,System.String,System.String,System.String,System.String)", MessageId = "System.Int64.ToString")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.LoggingViewDA.#RetrieveExceptionLog(HiiP.Framework.Logging.Interface.ValidationEntity.LogIDPairEntity,System.String,System.String,System.String,System.String,System.String,System.String)", MessageId = "System.Int64.ToString")]
[module: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFilterDA.#Filter(Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry)", MessageId = "System.Int32.Parse(System.String)")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFilterDA.#.ctor(System.Collections.Specialized.NameValueCollection)", MessageId = "nvParis")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "type", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFlagDA", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFlagDA.#GetFlagForLoggingFilter(HiiP.Framework.Logging.Library.FilterCategory,System.String)", MessageId = "Flag")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFilterDA.#GetFlag(HiiP.Framework.Logging.BusinessEntity.LoggingFilterDataSet,System.String,System.String)")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFlagDA.#GetAllFilters()")]

[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "HiiP.Framework.Logging.DataAccess.MonitoringLogFilterDA.#.ctor(System.Collections.Specialized.NameValueCollection)", MessageId = "nameValueParis")]
