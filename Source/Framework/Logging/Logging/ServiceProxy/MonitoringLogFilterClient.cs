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
using System.Collections.Specialized;
using System.Text;

using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.Library;
using HiiP.Infrastructure.Interface;
using HiiP.Framework.Logging.Library.Constants;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Diagnostics;
using HiiP.Framework.Common.ApplicationContexts;

namespace HiiP.Framework.Logging.ServiceProxy
{
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class MonitoringLogFilterClient : LogFilter
    {
        private CacheManager _cacheManager;

        private const string FlagCacheKey = "_flagCacheKey";
        private const string ResultCacheKey = "_resultCacheKey";

        #region Construction
        public MonitoringLogFilterClient(string name)
            : base(name)
        { }

// ReSharper disable UnusedParameter.Local
        public MonitoringLogFilterClient(NameValueCollection nameValueParis)
// ReSharper restore UnusedParameter.Local
            : this("MonitoringLogFilter")
        {
            _cacheManager = CacheFactory.GetCacheManager();
        }
        #endregion

        public override bool Filter(LogEntry log)
        {
            MonitoringLogEntry logEntry = log as MonitoringLogEntry;
            if (logEntry == null)
            {
                return true;
            }

            bool isError = (log.Severity==TraceEventType.Error) || !string.IsNullOrEmpty(logEntry.InstanceID);
            if (isError)
            {
                return true;
            }

            int entryFlag ;
            bool filterResult ;

            if (_cacheManager==null)
            {
                throw new HiiP.Framework.Common.BusinessException("Uable to create cache storage.");   
            }

            int? flag = _cacheManager.GetData(FlagCacheKey) as int?;
            bool? result = _cacheManager.GetData(ResultCacheKey) as bool?;

            if (result == null ||
                flag == null)
            {
                using (MonitoringLogFilterServiceProxy proxy = new MonitoringLogFilterServiceProxy())
                {
                    var standardEntry = CreateStandardEntry();
                    result = proxy.Filter(standardEntry, out entryFlag);
                    flag = entryFlag;
                }
                //Cache the flag and result always in client side until the user re-login
                _cacheManager.Add(FlagCacheKey, flag, CacheItemPriority.None, null, new AbsoluteTime(ParameterUtil.DefaultCacheTimeSpan));
                _cacheManager.Add(ResultCacheKey, result, CacheItemPriority.None, null, new AbsoluteTime(ParameterUtil.DefaultCacheTimeSpan));
            }

            //Get data from cache
            //if (flag != null)
            {
                //flag is always not null
                entryFlag = flag.Value;
            }
            //if (result != null)
            {
                //result is always not null
                filterResult = result.Value;
            }

            logEntry.Flag = entryFlag;

            bool usageLogOnly = (flag == FilterFlag.UsageFlag);
            //SecurityLogEntry's Component will be BusinessService
            bool isClientComponent = (logEntry.Component.Equals(ComponentType.Screen) 
                        || logEntry.Component.Equals(ComponentType.ServiceProxy));

            //Remove the service call when only usage logging is turned on.
            if ( usageLogOnly && isClientComponent)
            {
                //Need to check component here, because of SecurityLogger
                return false;
            }
            return filterResult;
        }

        private MonitoringLogEntry CreateStandardEntry()
        {
            MonitoringLogEntry log = new MonitoringLogEntry();

            log.Component = ComponentType.Screen;
            log.Severity = TraceEventType.Information;
            log.Categories.Add(LoggingCategories.Monitoring);
            log.UserID = AppContext.Current.UserID;
            log.UserName = AppContext.Current.UserName;

            return log;
        }
    }
}
