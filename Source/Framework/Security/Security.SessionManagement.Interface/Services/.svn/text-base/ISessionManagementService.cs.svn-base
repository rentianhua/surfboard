#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME       :   Housing Integrated Information Program
// COMPONENT ID      :    HiiP.Framework.Security.SessionManagement.Interface.Services/ISessionManagementService
// COMPONENT DESC:  
//
// CREATED DATE/BY:   22/09/2008/Jiang Nan
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System.ServiceModel;
using NCS.IConnect.Common;
using HiiP.Infrastructure.Interface.BusinessEntities;
using System;
using HiiP.Framework.Security.AccessControl.Interface;
using HiiP.Framework.Security.AccessControl.BusinessEntity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace HiiP.Framework.Security.SessionManagement.Interface.Services
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface ISessionManagementService
    {

        // Get sessions according to session criteria
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        SessionInfo[] GetActiveSessions(SessionCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        void KillSessions(Guid[] ids);
    }
}
