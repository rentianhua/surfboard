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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace HiiP.Framework.Logging.Library
{
    public class HiiPLoggingExceptionFormatter: TextExceptionFormatter
    {
        public HiiPLoggingExceptionFormatter(TextWriter writer, Exception exception)
            : base(writer, exception) 
        { }

        public override void Format()
        {
            base.Format();
        }

        protected override void WriteAdditionalInfo(System.Collections.Specialized.NameValueCollection additionalInformation)
        {
            DateTime time ;
            if (DateTime.TryParse(additionalInformation["TimeStamp"], out time))
            {
                additionalInformation.Set("TimeStamp", time.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));
            }

            base.WriteAdditionalInfo(additionalInformation);
        }

        protected override void WriteDateTime(DateTime utcNow)
        {
            base.Writer.WriteLine(utcNow.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));
        }

        protected override void WriteDescription()
        {
            base.WriteDescription();
        }

        protected override void WriteExceptionType(Type exceptionType)
        {
            base.WriteExceptionType(exceptionType);
        }

        protected override void WriteFieldInfo(System.Reflection.FieldInfo fieldInfo, object value)
        {
            base.WriteFieldInfo(fieldInfo, value);
        }

        protected override void WriteHelpLink(string helpLink)
        {
            base.WriteHelpLink(helpLink);
        }

        protected override void WriteMessage(string message)
        {
            base.WriteMessage(message);
        }

        protected override void WritePropertyInfo(System.Reflection.PropertyInfo propertyInfo, object value)
        {
            base.WritePropertyInfo(propertyInfo, value);
        }

        protected override void WriteSource(string source)
        {
            base.WriteSource(source);
        }

        protected override void WriteStackTrace(string stackTrace)
        {
            base.WriteStackTrace(stackTrace);
        }
    }
}
