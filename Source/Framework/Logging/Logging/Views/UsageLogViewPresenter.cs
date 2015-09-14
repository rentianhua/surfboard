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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.ServiceProxy;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.Interface.Constants;
using HiiP.Framework.Logging.Interface.ValidationEntity;

namespace HiiP.Framework.Logging
{
    public partial class UsageLogViewPresenter : Presenter<IUsageLogView>
    {
        #region Get related data from service layer

        internal LoggingUsageDataSet GetUsageForUserData(DateTimeCompare timeEntity, string userName)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForUser(timeEntity, userName);
                }
            }
        }

        internal LoggingUsageDataSet GetUsageForRoleData(DateTimeCompare timeEntity, string roleId)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForRole(timeEntity, roleId);
                }
            }
        }

        //Unused method
        //internal LoggingUsageDataSet GetUsageForOrganization(DateTimeCompare timeEntity, string organization)
        //{
        //    Guid id = Utility.SetContextValues();
        //    using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
        //    {
        //        using (LoggingViewProxy proxy = new LoggingViewProxy())
        //        {
        //            return proxy.RetrieveUsagesForOrganization(timeEntity, organization);    
        //        }
        //    }
        //}


        internal LoggingUsageDataSet GetUsageForOffice(DateTimeCompare timeEntity, string office)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForOffice(timeEntity, office);
                }
            }
        }

        //Unused method
        //internal LoggingUsageDataSet GetUsageForGraphicAreaData(DateTimeCompare timeEntity, string geographicArea)
        //{
        //    Guid id = Utility.SetContextValues();
        //    using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
        //    {
        //        using (LoggingViewProxy proxy = new LoggingViewProxy())
        //        {
        //            return proxy.RetrieveUsagesForGraphicArea(timeEntity, geographicArea);
        //        }
        //    }
        //}

        internal LoggingUsageDataSet GetUsageForModuleData(DateTimeCompare timeEntity, string moduleId)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForModule(timeEntity, moduleId);
                }   
            }
        }

        internal LoggingUsageDataSet GetUsageForFunctionData(DateTimeCompare timeEntity, string functionId)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForFunction(timeEntity, functionId);    
                }
            }
        }

        internal LoggingUsageDataSet GetAllCountOfUsers(DateTimeCompare timeEntity)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForUsersCount(timeEntity);
                }
            }
        }

        internal LoggingUsageDataSet GetCountOfUsersByModule(DateTimeCompare timeEntity, string moduleId)
        {
            Guid id = Utility.SetContextValues();
            using (new MonitoringTracer(id, FunctionNames.LoggingModuleID, FunctionNames.UsageFunctionID, ComponentType.Screen))
            {
                using (LoggingViewProxy proxy = new LoggingViewProxy())
                {
                    return proxy.RetrieveUsagesForUsersCountByModule(timeEntity, moduleId);
                }
            }
        }

        #endregion
    }
}

