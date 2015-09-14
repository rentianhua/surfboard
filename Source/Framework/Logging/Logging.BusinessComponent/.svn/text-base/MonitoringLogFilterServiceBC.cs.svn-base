#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Business Component
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

using HiiP.Framework.Logging.DataAccess;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace HiiP.Framework.Logging.BusinessComponent
{
    public class MonitoringLogFilterServiceBC
    {
        public bool Filter(LogEntry log)
        {
            var da = new MonitoringLogFilterDA("MonitoringLogFilter");
            return da.Filter(log);
        }
    }
}
