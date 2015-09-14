using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;

namespace HiiP.Framework.Security.AccessControl.Interface
{
    /// <summary>
    /// Service Contract for HiiP login service.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface ILoginService
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="hostName"></param>
        /// <param name="userInfo"></param>
        /// <param name="sessionData"></param>
        /// <returns>A bool value indicating if the login user is a valid active membership user.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        bool Login(string ipAddress, string hostName, out MembershipUserInfo userInfo, out SessionData sessionData);
    }
}