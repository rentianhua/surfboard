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
using HiiP.Framework.Logging.Interface.Constants;
using NCS.IConnect.Common;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.ValidationEntity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HiiP.Framework.Logging.Interface.Services
{
    /// <summary>
    /// Logging View Service Interface
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface ILoggingViewService
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LogIDPairEntity GetInstrumentationLogIDRangeByLogTime(
            [ObjectValidator("Date Time Compare Set For Instrumentation")]
            DateTimeCompare timeEntity);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingViewDataSet RetrieveInstrumentation(LogIDPairEntity logIDPair,
            [ObjectValidator("Date Time Compare Set For Instrumentation")]DateTimeCompare timeEntity,
            [StringLengthValidator(1, RangeBoundaryType.Inclusive, 128, RangeBoundaryType.Inclusive, MessageTemplate = "The User Name is mandatory, and the length must be between 1 and 128.")]string userName, 
            string ipAddress, string moduleId, string functionId, string componentName, string category, string pcName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LogIDPairEntity GetLogIDRangeByLogTime(
            [ObjectValidator("Date Time Compare Set")]
            DateTimeCompare timeEntity, string userName, string machineName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingViewDataSet RetrieveExceptionLog(LogIDPairEntity logIDPair, DateTimeCompare timeEntity, string userName, string category, string severity, string machineName, string logContent, string instanceID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        LoggingViewDataSet GetLogsByID(string logId);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingViewDataSet RetrievePerformanceInformation([ObjectValidator("Date Time Compare Set")]DateTimeCompare timeEntity,
            string functionId, string componentName, 
            [StringLengthValidator(1,RangeBoundaryType.Inclusive, 128, RangeBoundaryType.Inclusive,MessageTemplate="The User Name is mandatory, and the length must be between 1 and 128.")]string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForUser([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string userName);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForRole([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string userRoles);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForOrganization([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string organization);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForOffice([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string office);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForGraphicArea([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity, 
            string geographicArea);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForModule([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string moduleId);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForFunction([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity, 
            string functionId);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForUsersCount([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LoggingUsageDataSet RetrieveUsagesForUsersCountByModule([ObjectValidator("Date Time Compare Set For Usage")]DateTimeCompare timeEntity,
            string moduleId);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string GetExceptionMessageID(string userName, string ipAddress);
    }
}
