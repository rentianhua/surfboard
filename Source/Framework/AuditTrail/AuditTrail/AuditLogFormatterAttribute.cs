﻿using System;
using Cedar.Core;
using Cedar.Framwork.AuditTrail.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Cedar.Framwork.AuditTrail
{
    /// <summary>
    /// This attribute is used to specify the AuditLogFormatter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AuditLogFormatterAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the AuditLogFormatter.
        /// </summary>
        /// <value>
        /// The type of the formatter.
        /// </value>
        public Type FormatterType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cedar.Framwork.AuditTrail.AuditLogFormatterAttribute" /> class.
        /// </summary>
        /// <param name="formatterType">Type of the formatter.</param>
        public AuditLogFormatterAttribute(Type formatterType)
        {
            Guard.ArgumentNotNull(formatterType, "formatterType");
            if (typeof(IAuditLogFormatter).IsAssignableFrom(formatterType))
            {
                this.FormatterType = formatterType;
                return;
            }
            throw new ArgumentException(ResourceUtility.Format(Resources.ExceptionInvalidAuditLogFormatterType,
                new object[]
                {
                    formatterType.FullName
                }));
        }
    }
}
