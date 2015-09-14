using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using NCS.IConnect.Common;

namespace HiiP.Framework.Security.UserManagement.Interface.ExceptionHandling
{
    [global::System.Serializable]
    public class UserManagementException : Exception
    {
        public UserManagementException() { }
        public UserManagementException(string message) : base(message) { }
        public UserManagementException(string message, Exception inner) : base(message, inner) { }
        protected UserManagementException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    }
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class UserManagementSecurityException : ServiceException
    {

        // Methods
        public UserManagementSecurityException(Exception ex)
            : base(ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            this.AssemblyQualifiedName = ex.GetType().AssemblyQualifiedName;

            if (ex.InnerException != null)
                this.InnerException = new UserManagementSecurityException(ex.InnerException);
        }


    }
}