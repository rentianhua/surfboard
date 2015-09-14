#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :   HiiP.Framework.Security.SessionManagement.Interface.Services/ISessionService
// COMPONENT DESC:  
//
// CREATED DATE/BY:   22/09/2008/Jiang Nan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using HiiP.Framework.Security.AccessControl.Interface;

namespace HiiP.Framework.Security.SessionManagement.Interface.Services
{
    /// <summary>
    /// Contract for Session Service.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
   public  interface ISessionService
    {
       /// <summary>
       /// Kill one or more active session.
       /// </summary>
        /// <param name="ids"></param>
       [OperationContract]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
        void KillSessions(Guid[] ids);

       /// <summary>
       /// Get active sessions.
       /// </summary>
       /// <returns>Session list.</returns>
       [OperationContract(Name = "GetAllActiveSessions")]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
       SessionInfo[] GetActiveSessions();

       /// <summary>
       /// Get active sessions.
       /// </summary>
       /// <param name="userName">User name.</param>
       /// <returns>Session list.</returns>
       [OperationContract(Name = "GetActiveSessionsByUserName")]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
       SessionInfo[] GetActiveSessions(string userName);

       /// <summary>
       /// Get active sessions.
       /// </summary>
       /// <param name="userName">User Name</param>
       /// <param name="ipAddress">IP adress</param>
       /// <param name="hostName">Host name</param>
       /// <returns>Session list.</returns>

       [OperationContract(Name = "GetActiveSessionsByClientInfo")]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
       SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName);

       /// <summary>
       /// Get active sessions.
       /// </summary>
       /// <param name="userName">User name</param>
       /// <param name="startTimeFrom">Session start time from.</param>
       /// <param name="startTimeTill">Session start time till</param>
       /// <returns>Session list.</returns>
       [OperationContract(Name = "GetActiveSessionsByUserNameAndStartTime")]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
       SessionInfo[] GetActiveSessions(string userName, DateTime startTimeFrom, DateTime startTimeTill);
       /// <summary>
       /// Get active sessions.
       /// </summary>
      /// <param name="userName">User Name</param>
       /// <param name="ipAddress">IP adress</param>
       /// <param name="hostName">Host name</param>
       /// <param name="startTimeFrom">Session start time from.</param>
       /// <param name="startTimeTill">Session start time till.</param>
       /// <param name="lastActivityTimeFrom">Last activity time from.</param>
       /// <param name="lastActivityTimeTill">Last activity time till.</param>
       /// <returns>Session list.</returns>
       [OperationContract(Name = "GetActiveSessions")]
       [FaultContract(typeof(ValidationFault))]
       [FaultContract(typeof(ServiceException))]
       SessionInfo[] GetActiveSessions(string userName, string ipAddress, string hostName, DateTime startTimeFrom, DateTime startTimeTill, DateTime lastActivityTimeFrom, DateTime lastActivityTimeTill);
 
    }
}
