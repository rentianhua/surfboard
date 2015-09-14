#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Library
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Logging.Library.Constants;
using HiiP.Framework.Logging.Library.Properties;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Logging;

namespace HiiP.Framework.Logging.Library
{
    public class MonitoringPopulateLogEntry
    {
        #region Private method
        public MonitoringLogEntry PopulateDefaultLogEntry()
        {
            MonitoringLogEntry logEntry = new MonitoringLogEntry();
            logEntry.EventId = MonitoringLogEntryDefaults.EventId;
            logEntry.Priority = MonitoringLogEntryDefaults.Priority;
            logEntry.ExtendedActivityId = AppContext.Current.ActivityID;
            logEntry.UserName = AppContext.Current.UserName;
            logEntry.UserID = AppContext.Current.UserID;
            logEntry.IpAddress = AppContext.Current.IPAddress;
            logEntry.UserRoles = AppContext.Current.UserRoles;
            logEntry.UserGraphicArea = AppContext.Current.GraphicArea;
            //logEntry.Organization = AppContext.Current.Organization;
            logEntry.Office = AppContext.Current.Office;
            return logEntry;
        }

        private void PopulatePreLogEntry(MonitoringLogEntry logEntry)
        {
            logEntry.Severity = TraceEventType.Start;
            logEntry.Title = "TracerEnter";

        }

        private void PopulatePostLogEntry(MonitoringLogEntry logEntry)
        {
            logEntry.Severity = TraceEventType.Stop;
            logEntry.Title = "TracerExit";
        }
        #endregion

        #region Public method
        public MonitoringLogEntry PopulateLogEntryForTrace(bool start)
        {
            MonitoringLogEntry logEntry = PopulateDefaultLogEntry();

            if (start)
            {
                PopulatePreLogEntry(logEntry);
            }
            else
            {
                PopulatePostLogEntry(logEntry);
            }
            return logEntry;
        }

        public void PopulateLogEntryForCallHandler(IMethodInvocation input, 
            //List<string> categories, 
            //bool includeParameters, bool includeCallStack, 
            IMethodReturn result,
            long? tracingStartTicks, long? tracingEndTicks,
            long elapsedMilliseconds,
            string messageFormat,
            string moduleId,
            string functionId,
            ComponentType component)
        {
            MonitoringLogEntry logEntry = PopulateDefaultLogEntry();

            using (BackgroundWorker worker = new BackgroundWorker())
            {

                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    try
                    {
                        AppContext.SetToCallContext(AppContext.Current.ToDictionary());
                        CategoryFormatter formatter = new CategoryFormatter(input.MethodBase);
                        logEntry = eDoWork.Argument as MonitoringLogEntry;
                        //foreach (string category in categories)
                        //{
                        //    logEntry.Categories.Add(formatter.FormatCategory(category));
                        //}
                        logEntry.Categories.Add(formatter.FormatCategory(LoggingCategories.Monitoring));

                        logEntry.Component = component;

                        if (!Logger.ShouldLog(logEntry))
                        {
                            return;
                        }

                        //When do the instrumentation log , we should log the parameter values information, otherwise not
                        if ((logEntry.Flag & FilterFlag.InstrumentationFlag) == FilterFlag.InstrumentationFlag)
                        {
                            //if (includeParameters)
                            {
                                logEntry.ParameterValues = GenerateValuesInfo(input.Arguments, true);
                            }
                        }

                        //if (includeCallStack)
                        //{
                        //    logEntry.CallStack = Environment.StackTrace;
                        //}

                        logEntry.TypeName = input.Target.GetType().FullName;

                        logEntry.MethodName = string.Concat(input.MethodBase.DeclaringType.FullName, ".", input.MethodBase.Name);

                        if (result == null)
                        {
                            PopulatePreLogEntry(logEntry);
                        }
                        else
                        {
                            PopulatePostLogEntry(logEntry);
                            if ((logEntry.Flag & FilterFlag.InstrumentationFlag) == FilterFlag.InstrumentationFlag)
                            {
                                int depth = 0;
                                object returnedValue = result.ReturnValue;
                                logEntry.ReturnValue = GenerateValueInfo("ReturnValue", returnedValue, (returnedValue == null) ? null : returnedValue.GetType(), ref depth);
                            }
                        }

                        logEntry.TracingStartTicks = tracingStartTicks;
                        logEntry.TracingEndTicks = tracingEndTicks;
                        logEntry.SecondsElapsed = GetSecondsElapsed(elapsedMilliseconds);
                        logEntry.Message = string.Format(Resources.Culture, messageFormat,
                                    logEntry.ExtendedActivityId,
                                    logEntry.MethodName,
                                    logEntry.TracingEndTicks,
                                    logEntry.SecondsElapsed);


                        logEntry.FunctionId = functionId;

                        logEntry.ModuleId = moduleId;


                        //if (result.ReturnValue != null && includeParameters)
                        //{
                        //    logEntry.ReturnValue = result.ReturnValue.ToString();
                        //}
                        if (result != null && result.Exception != null)
                        {
                            logEntry.Exception = result.Exception.ToString();
                        }

                        LogWriter logWriter = Logger.Writer;
                        logWriter.Write(logEntry);

                    }
                    catch (Exception ex)
                    {
                        //Swallow exception, because it is logging should not impact the business function
                        HiiPLoggingExceptionHandler handler = new HiiPLoggingExceptionHandler("Server Exception",
                            100, TraceEventType.Error,
                            "Instrumtation logging of RowUpdated failed",
                            0,
                            typeof(HiiPLoggingExceptionFormatter),
                            ComponentType.StoredProcedure,
                            Logger.Writer);
                        handler.HandleException(ex, Guid.NewGuid());
                    }

                };

                worker.RunWorkerAsync(logEntry);
            }
        }
        
        #region PopulateLogEntryStoredProcedure
        public void PopulateLogEntryStoredProcedure(long? tracingStartTicks, long? tracingEndTicks,
            long elapsedMilliseconds,
            string executionMethod,
            string messageFormat,
            string moduleId,
            string functionId,
            EventArgs e)
        {
            LogWriter logWriter = Logger.Writer;
            if (!logWriter.IsTracingEnabled())
            {
                return;
            }

            MonitoringLogEntry logEntry = PopulateDefaultLogEntry();
            using (BackgroundWorker worker = new BackgroundWorker())
            {

                RowUpdatedEventArgs updatedEvent = e as RowUpdatedEventArgs;
                string[] rows = new string[0];
                if (updatedEvent != null)
                {
                    if (updatedEvent.StatementType == StatementType.Batch
                        && updatedEvent.RowCount > 0)
                    {
                        rows = new string[updatedEvent.RowCount];
                    DataRow[] tempRows ;
                        tempRows = new DataRow[updatedEvent.RowCount];
                        updatedEvent.CopyToRows(tempRows);

                        for (int i = 0; i < tempRows.Length; i++)
                        {
                            DataRow row = tempRows[i];
                            StringBuilder batchBuilder = new StringBuilder();

                            BuildRowInfo(batchBuilder, row);
                            rows[i] = batchBuilder.ToString();
                        }
                    }
                    else if (updatedEvent.Row != null)
                    {
                        rows = new string[1];
                        StringBuilder builder = new StringBuilder();

                        BuildRowInfo(builder, updatedEvent.Row);
                        rows[0] = builder.ToString();
                    }
                }
                else
                {
                    RowUpdatingEventArgs updatingEvent = e as RowUpdatingEventArgs;
                    if (updatingEvent != null)
                    {
                        rows = new string[1];
                        StringBuilder builder = new StringBuilder();

                        BuildRowInfo(builder, updatingEvent.Row);
                        rows[0] = builder.ToString();
                    }
                }
                worker.DoWork += delegate(object oDoWork, DoWorkEventArgs eDoWork)
                {
                    try
                    {
                        AppContext.SetToCallContext(AppContext.Current.ToDictionary());
                        logEntry = eDoWork.Argument as MonitoringLogEntry;

                        logEntry.Component = ComponentType.StoredProcedure;

                        logEntry.FunctionId = functionId;
                        logEntry.ModuleId = moduleId;
                        //logEntry.Categories.Add("Trace");

                        //TODO: modified by xiaofeng. Should here hardcode the category? Can it be re-used by exception logging and security logging?
                        logEntry.Categories.Add(LoggingCategories.Monitoring);

                        if (!Logger.ShouldLog(logEntry))
                        {
                            return;
                        }
                        logEntry.TracingStartTicks = tracingStartTicks;
                        logEntry.TracingEndTicks = tracingEndTicks;
                        logEntry.SecondsElapsed = GetSecondsElapsed(elapsedMilliseconds);
                        logEntry.MethodName = executionMethod;

                        logEntry.Message = string.Format(Resources.Culture, messageFormat,
                                                         logEntry.ExtendedActivityId, logEntry.MethodName, logEntry.TracingEndTicks,
                                                         logEntry.SecondsElapsed);
                        PopulateStoredProcedueDetail(logEntry, e, rows);
                    }
                    catch (Exception ex)
                    {
                        //Swallow exception, because it is logging should not impact the business function
                        HiiPLoggingExceptionHandler handler = new HiiPLoggingExceptionHandler("Server Exception",
                            100, TraceEventType.Error,
                            "Instrumtation logging of RowUpdated failed",
                            0,
                            typeof(HiiPLoggingExceptionFormatter),
                            ComponentType.StoredProcedure,
                            Logger.Writer);
                        handler.HandleException(ex, Guid.NewGuid());
                    }

                };

                worker.RunWorkerAsync(logEntry);
            }
        }

        private void PopulateStoredProcedueDetail(MonitoringLogEntry logEntry, EventArgs e,string[] rows)
        {
            LogWriter logWriter = Logger.Writer;
            if ((logEntry.Flag & FilterFlag.InstrumentationFlag) != FilterFlag.InstrumentationFlag)
            {
                logWriter.Write(logEntry);
                return;
            }

            RowUpdatingEventArgs updatingEvent = e as RowUpdatingEventArgs;
            if (updatingEvent != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("This is for row updating");
                BuildCommandInfo(builder, updatingEvent.Command);

                if (updatingEvent.Errors != null)
                {
                    builder.AppendLine("Row errors: " + updatingEvent.Errors.Message);
                }

                logEntry.ParameterValues = builder.ToString() + ((rows.Length == 0) ? "" : rows[0]);
                PopulatePostLogEntry(logEntry);

                logWriter.Write(logEntry);
            }
            else
            {
                RowUpdatedEventArgs updatedEvent = e as RowUpdatedEventArgs;
                if (updatedEvent != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("This is for row updated");

                    BuildCommandInfo(builder, updatedEvent.Command);

                    if (updatedEvent.Errors != null)
                    {
                        builder.AppendLine("Row errors: " + updatedEvent.Errors.Message);
                    }

                    if (updatedEvent.StatementType != StatementType.Batch)
                    {
                        logEntry.ParameterValues = builder.ToString() + ((rows.Length == 0) ? "" : rows[0]);
                        PopulatePostLogEntry(logEntry);
                        logWriter.Write(logEntry);
                    }
                    else
                    {
                        builder.AppendLine("Batch row count :" + updatedEvent.RowCount);
                        if (rows != null)
                        {
                            foreach (string batchRow in rows)
                            {

                                logEntry.ParameterValues = builder.ToString() + batchRow;
                                PopulatePostLogEntry(logEntry);

                                logWriter.Write(logEntry);
                            }
                        }
                    }
                }
            }
        }

        private void BuildCommandInfo(StringBuilder builder, IDbCommand command)
        {
            builder.AppendLine("Stored procedure name: " + command.CommandText);

            builder.AppendLine("Parameters:");
            foreach (DbParameter param in command.Parameters)
            {
                builder.AppendLine(string.Format("{0}({1}) = {2};" , param.ParameterName, param.SourceColumn , param.Value ));
            }

        }
        private void BuildRowInfo(StringBuilder builder, DataRow row)
        {
            if (row==null)
            {
                return;
            }
            builder.AppendLine("Row State :" + row.RowState );
            builder.AppendLine("Columns: ");
            foreach (DataColumn column in row.Table.Columns)
            {
                object value ;
                if (row.HasVersion(DataRowVersion.Current))
                {
                    value = row[column];
                }
                else
                {
                    value = row[column, DataRowVersion.Original];
                }

                builder.AppendLine(string.Format("{0} = {1};", column.ColumnName, value));

            }

        }
        #endregion

        public decimal GetSecondsElapsed(Nullable<long> milliseconds)
        {
            decimal result = Convert.ToDecimal(milliseconds) / 1000m;
            return Math.Round(result, 6);
        }

        #region GenerateValueInfo APIs
        private string GenerateValuesInfo(IParameterCollection arguments, bool isParameter)
        {
            StringBuilder builder = new StringBuilder();

            switch (arguments.Count)
            {
                case 0:
                    builder.AppendLine(string.Format("No {0} in the method.",isParameter?"parameter":"output"));
                    break;
                case 1:
                    builder.AppendLine(string.Format("There is 1 {0} in the method: ",isParameter?"parameter":"output"));
                    break;
                default:
                    builder.AppendLine("There are " + arguments.Count + string.Format(" {0} in the method: ", isParameter ? "parameters" : "outputs"));
                    break;
            }

            for (int i = 0; i < arguments.Count; ++i)
            {
                ParameterInfo paramter = arguments.GetParameterInfo(i);

                int depth = 0;
                builder.AppendLine(GenerateValueInfo(paramter.Name, arguments[i], paramter.ParameterType,ref depth));
            }

            return builder.ToString();
        }

        private static string GetMessageTemplateString(int depth)
        {
            string indent = new string('\t',depth);
            return string.Format("{4}Name: {0}, type :{1},{3}{4}and value is: {3}{4}\"{2}\".",
                "{0}","{1}","{2}",
                Environment.NewLine,
                indent);
        }

        private string GenerateValueInfo(string name, object value, Type valueType, ref int depth)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (null == value)
            {
                return string.Format(GetMessageTemplateString(depth), name,
                                     (valueType == null) ? "" : valueType.FullName, Resources.NullString);
            }

            if (value.GetType().IsValueType || typeof (string).IsAssignableFrom(value.GetType()))
            {
                return GenerateNormalValueInfo(name, value, depth);
            }
            if (typeof (DataSet).IsAssignableFrom(value.GetType()))
            {
                return GenerateDataSetValueInfo(name, value, depth);
            }
            if (typeof (DataTable).IsAssignableFrom(value.GetType()))
            {
                return GenerateDataTableValueInfo(name, value, depth);
            }
            if (typeof (DbCommand).IsAssignableFrom(value.GetType()))
            {
                return GenerateDbCommandValueInfo(name, value, depth);
            }
            if (typeof (byte[]).IsAssignableFrom(value.GetType()))
            {
                return string.Format(GetMessageTemplateString(depth), name, valueType.FullName,
                                     "its length is " + ((byte[]) value).Length);
            }
            if (value.GetType().IsSerializable)
            {
                try
                {
                    return GenerateSerializableValueInfo(name, value, depth);
                }
                catch (Exception ex)
                {
                    try
                    {
                        return GenerateOtherValueInfo(ex, name, value, ref depth);
                    }
                    catch (Exception secondEx)
                    {
                        return GenerateValueInfoWithToString(ex, secondEx, name, value, depth);
                    }
                }
            }

            try
            {
                return GenerateOtherValueInfo(null, name, value, ref depth);
            }
            catch (Exception ex)
            {
                return GenerateValueInfoWithToString(null, ex, name, value, depth);
            }

        }

        private string GenerateNormalValueInfo(string name, object value, int depth)
        {
            string valueString = (value??string.Empty).ToString();
            return string.Format(GetMessageTemplateString(depth), name, (value==null)?"UnknownType":value.GetType().FullName, valueString);
        }

        private string GenerateDataSetValueInfo(string name, object value, int depth)
        {
            DataSet ds = value as DataSet;
            if (null == ds)
            {
                return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, Resources.NullString);
            }

            //XmlWriterSettings settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (XmlWriter writer = XmlWriter.Create(ms, settings))
            //    {
            //        ds.WriteXml(writer);
            //    }

            //    string xml = Encoding.UTF8.GetString(ms.ToArray());
            //    return string.Format(GetMessageTemplateString(),
            //       name, value.GetType().FullName,
            //       xml);
            //}

            //Unable to use GetXml to store the detail because of performance. 
            //For example, big dataset will make string big, so that outOfMemory
            string indent = new string('\t',depth);
            StringBuilder builderValue = new StringBuilder();
            foreach (DataTable table in ds.Tables)
            {
                builderValue.AppendLine(string.Format("{2}There are {0} records in table '{1}'", 
                    table.Rows.Count, table.TableName,
                   indent));
            }


            return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, builderValue.ToString());
        }

        private string GenerateDataTableValueInfo(string name, object value, int depth)
        {
            DataTable table = value as DataTable;
            if (null == table)
            {
                return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, Resources.NullString);
            }

            DataSet ds;
            if (table.DataSet == null)
            {
                 ds = new DataSet(table.TableName + "DataSet");
                ds.Tables.Add(table.Clone());
            }
            else
            {
                ds = table.DataSet;
            }
            return GenerateDataSetValueInfo(name, ds, depth);
        }

        private string GenerateDbCommandValueInfo(string name, object value, int depth)
        {
            DbCommand command = value as DbCommand;
            if (null == command)
            {
                return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, Resources.NullString);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("");
            builder.AppendLine("\tStoredProcedure Name: " + command.CommandText);
            builder.AppendLine("\tParameters: ");
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                builder.AppendLine(string.Format("\t{0} = {1}" ,command.Parameters[i].ParameterName, command.Parameters[i].Value));
            }

            return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, builder.ToString());
        }

        private string GenerateSerializableValueInfo(string name, object value, int depth)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    DataContractSerializer ser =
                         new DataContractSerializer(value.GetType());
                    ser.WriteObject(writer, value);
                }
                string xml = Encoding.UTF8.GetString(ms.ToArray());
                return string.Format(GetMessageTemplateString(depth),
                   name, value.GetType().FullName,
                   xml);
            }

        }

        private string GenerateOtherValueInfo(Exception ex, string name, object value, ref int depth)
        {
            StringBuilder builder = new StringBuilder();
            if (ex != null)
            {
                builder.AppendLine("Because encoutering error : ");
                builder.AppendLine(ex.ToString());
                builder.AppendLine("");
                builder.AppendLine("Have to populate parameter info as:");
            }

            if (depth++ > 1)
            {
                //Just support one level parameter. For example, parameter->its property. That is all.
                builder.AppendLine(string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, value.ToString()));
                return builder.ToString();
            }

            ConstructorInfo constructor = value.GetType().GetConstructor(Type.EmptyTypes);
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (constructor == null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
            {
                //No parameterless constructor
                builder.AppendLine(string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, value.ToString()));
                return builder.ToString();
            }
// ReSharper restore HeuristicUnreachableCode

            PropertyInfo[] properties = value.GetType().GetProperties();

            if (properties.Length==0)
            {
                //No public readable property
                builder.AppendLine(string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, value.ToString()));
                return builder.ToString();
            }

            foreach (PropertyInfo property in properties)
            {
                try
                {
                    builder.AppendLine(this.GenerateValueInfo(property.Name, property.GetValue(value, null), property.DeclaringType, ref depth));
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                    //Swallow the exception, 
                }
            }
            builder.AppendLine();
            return string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, builder.ToString());
        }

        private string GenerateValueInfoWithToString(Exception firstEx, Exception secondEx, string name, object value,int depth)
        {
            StringBuilder builder = new StringBuilder();
            if (firstEx != null)
            {
                builder.AppendLine("Because encoutering error : ");
                builder.AppendLine(firstEx.ToString());
                builder.AppendLine("");

                if (secondEx!=null)
                {
                    builder.AppendLine("and");
                    builder.AppendLine("");
                    builder.AppendLine(secondEx.ToString());
                    builder.AppendLine("");
                }

                builder.AppendLine("Have to populate parameter info as:");
            }

            builder.AppendLine(string.Format(GetMessageTemplateString(depth), name, value.GetType().FullName, value.ToString()));
            return builder.ToString();
        }
        #endregion

        #endregion

    }
}
