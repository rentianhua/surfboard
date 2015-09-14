#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
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
using HiiP.Framework.Logging.Interface.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Collections.Specialized;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.ApplicationContexts.CallHandlers;
using HiiP.Framework.Common;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.ServiceModel.Channels;
using System.Net;
using HiiP.Framework.Common.Client;

namespace HiiP.Framework.Logging.ServiceProxy
{
    public class MonitoringLogFilterServiceProxy : ServiceProxyBase<IMonitoringLogFilterService>, IMonitoringLogFilterService
    {    

        protected MonitoringLogFilterServiceProxy(string endpointName):base(endpointName)
        {
            
        }

        public MonitoringLogFilterServiceProxy()
        {
            base.WrapObject(new MonitoringLogFilterServiceProxy(EndpointNames.LoggingFilterEndpoint));
        }

        public bool Filter(LogEntry log, out int flag)
        {
            return this.Proxy.Filter(log, out flag);             
        }
    }
}
