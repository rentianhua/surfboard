#region Copyright(C) 2009 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2009 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Infrastructure/Library
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 07/08/2009/He Jiang Yan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.IO;
using System.ServiceModel;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// EventTopicExceptionFormatter
    /// </summary>
    public class ValidationFaultExceptionFormatter : TextExceptionFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFaultExceptionFormatter"/> class.
        /// </summary>
        /// <param name="writer">The stream to write formatting information to.</param>
        /// <param name="exception">The exception to format.</param>
        public ValidationFaultExceptionFormatter(TextWriter writer, Exception exception) : base(writer, exception)
        {
        }

        /// <summary>
        /// Writes and formats the exception and all nested inner exceptions to the <see cref="T:System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="exceptionToFormat">The exception to format.</param>
        /// <param name="outerException">The outer exception. This
        /// value will be null when writing the outer-most exception.</param>
        protected override void WriteException(Exception exceptionToFormat, Exception outerException)
        {
            base.WriteException(exceptionToFormat,null);
            
            FaultException<ValidationFault> ex = exceptionToFormat as FaultException<ValidationFault>;
            if (null==ex)
            {
                return;
            }
            string s = BuildValidationFaultMessage(ex);
            base.WriteMessage(s);
        }

        internal static string BuildValidationFaultMessage(FaultException<ValidationFault> ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("");
            builder.AppendLine("ValidationDetail:");
            foreach (ValidationDetail validationResult in ex.Detail.Details)
            {
                builder.AppendLine(string.Format(CultureInfo.CurrentCulture, Resources.ValidationFault, validationResult.Key, validationResult.Message, validationResult.Tag));
            }

            builder.AppendLine("");
            return builder.ToString();
        }

        /// <summary>
        /// Writes the additional info.
        /// </summary>
        /// <param name="additionalInformation">The additional information.</param>
        protected override void WriteAdditionalInfo(System.Collections.Specialized.NameValueCollection additionalInformation)
        {
            DateTime time ;
            if (DateTime.TryParse(additionalInformation["TimeStamp"], out time))
            {
                additionalInformation.Set("TimeStamp", time.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));
            }

            base.WriteAdditionalInfo(additionalInformation);
        }

        /// <summary>
        /// Writes the date time.
        /// </summary>
        protected override void WriteDateTime(DateTime utcNow)
        {
            base.Writer.WriteLine(utcNow.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));
        }

    }
}