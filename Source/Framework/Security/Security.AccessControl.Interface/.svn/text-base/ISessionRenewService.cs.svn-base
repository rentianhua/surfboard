using System;
using System.ServiceModel;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;

namespace HiiP.Framework.Security.AccessControl.Interface
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface ISessionRenewService
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        SessionStatus RefreshSession(string sessionID);        

        /// <summary>
        /// Close the current session.
        /// </summary>
        /// <param name="sessionID">An ID which uniquely identifies the session.</param>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void CloseSession(Guid sessionID);
    }
}
