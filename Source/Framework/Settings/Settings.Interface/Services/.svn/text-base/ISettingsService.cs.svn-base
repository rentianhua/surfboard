#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==============================================================================
// Copyright(C) 2008 NCS Pte Ltd
//
// SYSTEM NAME			: HiiP
// COMPONENT ID			: HiiP.Framework.Settings
// COMPONENT DESC		: 
//
// CREATED DATE/BY	    : 15 Sep 2008 / Yang Jian Hua
//
// REVISION HISTORY     :
// DATE/BY  ISSUE#/SR#/CS/PM#/OTHERS    DESCRIPTION OF CHANGE
// 
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Security;

using HiiP.Framework.Logging.Library;
using HiiP.Framework.Settings.BusinessEntity;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

using NCS.IConnect.Common;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Data;
using NCS.IConnect.Validation.Validators;

namespace HiiP.Framework.Settings.Interface.Services
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface ISettingsService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        LoggingFilterDS RetrieveLoggingFilter(string category);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        bool IsUserExists(string category, string userId);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //string GetSectionXml(string sectionName);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void SaveSection(string sectionName,string sectionXml, string configSectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        void UpdateLoggingFilter(LoggingFilterDS ds);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, string> GetAllUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        void SaveParameterValue([ObjectCollectionValidator(typeof(ParameterEntity),"Hierarchy RuleSet")]IList<ParameterEntity> parameters);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        DataTable RetrieveMessages(string category, string severity, string messageValue);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        DataTable RetrieveMessage(string category, string id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ValidationFault))]
        void UpdateMessage([DataTableOperationValidator(AllowedOperation.Update)]DataTable messages);

    }

}
