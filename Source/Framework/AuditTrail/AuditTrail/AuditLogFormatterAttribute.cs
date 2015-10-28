using System;
using Cedar.Core;
using Cedar.Framework.AuditTrail.Base;
using Cedar.Framework.AuditTrail.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Cedar.Framework.AuditTrail
{
    /// <summary>
    ///     This attribute is used to specify the AuditLogFormatter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AuditLogFormatterAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Cedar.Framework.AuditTrail.AuditLogFormatterAttribute" /> class.
        /// </summary>
        /// <param name="formatterType">Type of the formatter.</param>
        public AuditLogFormatterAttribute(Type formatterType)
        {
            Guard.ArgumentNotNull(formatterType, "formatterType");
            if (typeof(IAuditLogFormatter).IsAssignableFrom(formatterType))
            {
                FormatterType = formatterType;
                return;
            }
            throw new ArgumentException(ResourceUtility.Format("Resources.ExceptionInvalidAuditLogFormatterType:{0}",
                formatterType.FullName));
        }

        /// <summary>
        ///     Gets the type of the AuditLogFormatter.
        /// </summary>
        /// <value>
        ///     The type of the formatter.
        /// </value>
        public Type FormatterType { get; private set; }
    }
}