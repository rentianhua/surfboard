#region Copyright(C) 2009 NCS Pte. Ltd. All rights reserved.
// ==============================================================================
// Copyright(C) 2009 NCS Pte Ltd
//
// SYSTEM NAME			: HiiP
// COMPONENT ID			: HiiP.Framework.Settings
// COMPONENT DESC		: 
//
// CREATED DATE/BY	    : 13 03 2009 / He Jiang Yan
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
using HiiP.Infrastructure.Interface;

namespace HiiP.Framework.Settings.Interface.Services
{

    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class ParameterEntity
    {
        public const string AlertKey = ParameterUtil.AlertPath;
        public const string MOTDKey = ParameterUtil.MessageOfTheDayPath;
        public const string ToDoListKey = ParameterUtil.ToDoListPath;
        public const string MyAppointmentsKey = ParameterUtil.MyAppointmentPath;
        public const string MyReportKey = ParameterUtil.MyReportPath;
        public const string MyDashboardKey = ParameterUtil.MyDashboardPath;

        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public int Value { get; set; }

        public ParameterEntity(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
