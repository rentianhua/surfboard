using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using HiiP.Framework.Security.AccessControl.Interface.Properties;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// A static facade class to get Web seal related configuration informaiton.
    /// </summary>
    public static class WebSealSettings
    {
        /// <summary>
        /// Get authentication related settings.
        /// </summary>
        /// <returns>A WebSealAuthenticationSection object including all of authentication related settings.</returns>
        public static WebSealAuthenticationSection GetWebSealAuthenticationSetting()
        {
            return ConfigurationManager.GetSection(string.Format("{0}/{1}", Resources.WebSealConfigGroupName,
                Resources.WebSealAuthenticationConfigSectionName)) as WebSealAuthenticationSection;
        }

        /// <summary>
        /// Get all of the security token retrieval based field settings.
        /// </summary>
        /// <returns>A FormsFieldCollection object containing all of the form field settings.</returns>
        public static FormsFieldCollection GetFormsFieldSetting()
        {
            FormsFieldSection formFiedlsSection= ConfigurationManager.GetSection(string.Format("{0}/{1}", Resources.WebSealConfigGroupName,
                 Resources.FormFieldsConfigSectionName)) as FormsFieldSection;
            return (formFiedlsSection==null)? new FormsFieldCollection(): formFiedlsSection.FormsFields;
        }

        /// <summary>
        /// Get all of the keys of eBusiness response .
        /// </summary>
        /// <returns>A FormsFieldCollection object containing all of the form field settings.</returns>
        public static string GetSessionExpiredExceptionKeySetting()
        {
            ExceptionKeysSection exceptionKey = ConfigurationManager.GetSection(string.Format("{0}/{1}", 
                Resources.WebSealConfigGroupName,
                 Resources.ExceptionKeysSectionName)) as ExceptionKeysSection;

            if (null==exceptionKey)
            {
                return "";
            }
            return exceptionKey.SessionExpiredExceptionKey.Key;
        }

        /// <summary>
        /// Get all of the message of eBusiness response .
        /// </summary>
        /// <returns>A FormsFieldCollection object containing all of the form field settings.</returns>
        public static string GetSessionExpiredExceptionMessageSetting()
        {
            ExceptionKeysSection exceptionKey = ConfigurationManager.GetSection(string.Format("{0}/{1}",
                Resources.WebSealConfigGroupName,
                 Resources.ExceptionKeysSectionName)) as ExceptionKeysSection;

            if (null == exceptionKey)
            {
                return "";
            }
            return exceptionKey.SessionExpiredExceptionKey.Message;
        }

        /// <summary>
        /// Get all of the keys of eBusiness response .
        /// </summary>
        /// <returns>A FormsFieldCollection object containing all of the form field settings.</returns>
        public static string GetPasswordExpiredExceptionKeySetting()
        {
            ExceptionKeysSection exceptionKey = ConfigurationManager.GetSection(string.Format("{0}/{1}",
                Resources.WebSealConfigGroupName,
                 Resources.ExceptionKeysSectionName)) as ExceptionKeysSection;
            if (null == exceptionKey)
            {
                return "";
            }
            return exceptionKey.PasswordExpiredExceptionKey.Key;
        }

        /// <summary>
        /// Get all of the keys of eBusiness response .
        /// </summary>
        /// <returns>A FormsFieldCollection object containing all of the form field settings.</returns>
        public static string GetADAccountExpiredExceptionKeySetting()
        {
            ExceptionKeysSection exceptionKey = ConfigurationManager.GetSection(string.Format("{0}/{1}",
                Resources.WebSealConfigGroupName,
                 Resources.ExceptionKeysSectionName)) as ExceptionKeysSection;
            if (null == exceptionKey)
            {
                return "";
            }
            return exceptionKey.ADAccountExpiredExceptionKey.Key;
        }

        /// <summary>
        /// Get the name of the User Name form field.
        /// </summary>
        /// <returns>The name of the User Name form field.</returns>
        public static string GetUserNameFieldName()
        {
            FormsFieldCollection formFiedlsSection = GetFormsFieldSetting();
            foreach (FormsFieldElement element in formFiedlsSection)
            {
                if (element.FieldType == Resources.UserNameFormFieldType)
                {
                    return element.Name;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the name of Password form field.
        /// </summary>
        /// <returns>The name of Password form field.</returns>
        public static string GetPasswordFieldName()
        {
            FormsFieldCollection formFiedlsSection = GetFormsFieldSetting();
            foreach (FormsFieldElement element in formFiedlsSection)
            {
                if (element.FieldType == Resources.PasswordFormFieldType)
                {
                    return element.Name;
                }
            }

            return string.Empty;
        }
    }
}
