#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Interface
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using NCS.IConnect.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace HiiP.Framework.Logging.Interface.Services
{
    /// <summary>
    /// Logging filter service interface
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IMonitoringLogFilterService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Filter(LogEntry log, out int flag);
    }
}
