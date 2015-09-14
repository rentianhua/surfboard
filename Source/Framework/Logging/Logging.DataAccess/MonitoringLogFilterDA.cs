#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Data Access
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Logging.BusinessEntity;
using HiiP.Framework.Logging.Interface.Services;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Logging.Library.Constants;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System.Diagnostics;

namespace HiiP.Framework.Logging.DataAccess
{
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class MonitoringLogFilterDA : LogFilter
    {
        private static readonly CacheManager _cacheManager= CacheFactory.GetCacheManager();

        private const string filterCacheKey = "filterCacheKey";

        //private readonly MonitoringLogFlagDA _da;

        #region Construction
        
        public MonitoringLogFilterDA(string name)
            : base(name)
        {
        }

// ReSharper disable UnusedParameter.Local
        public MonitoringLogFilterDA(NameValueCollection nameValueParis)
// ReSharper restore UnusedParameter.Local
            : this("MonitoringLogFilter")
        {
            //_cacheManager = CacheFactory.GetCacheManager();
            //_da = new MonitoringLogFlagDA();
        }
        #endregion

        public override bool Filter(LogEntry log)
        {
            MonitoringLogEntry logEntry = log as MonitoringLogEntry;
            LoggingFilterDataSet data ;

            //When the log type is not MonitoringLogEntry, the filter will return true
            if (logEntry == null)
            {
                return true;
            }

            bool isError = (log.Severity == TraceEventType.Error) || !string.IsNullOrEmpty(logEntry.InstanceID);
            if (isError)
            {
                return true;
            }

            //Jiang Jin Nan, 21 May 2010, 3rd Party System mornitoring.
            if (logEntry.Component == ComponentType.ThirdPartySystem)
            {
                logEntry.Flag = FilterFlag.InstrumentationFlag;
                return true;
            }

            if ((logEntry.Categories.Count != 0) && (!(logEntry.Categories.Contains(LoggingCategories.Monitoring))))
            {
                return true;
            }

            if (_cacheManager == null)
            {
                throw new HiiP.Framework.Common.BusinessException("Uable to create cache storage.");
            }
            
            object obj = _cacheManager.GetData(filterCacheKey) ;
            data = obj as LoggingFilterDataSet;
            if (obj == null || data == null)
            {
                var da = new MonitoringLogFlagDA();

                data = da.GetAllFilters();
                _cacheManager.Add(filterCacheKey, data, CacheItemPriority.None, null,
                    new AbsoluteTime(new TimeSpan(0, 0, 10)));
            }

            int instrumentationFlag = 0;
            int monitoringFlag = 0;
            int usageFlag = 0;

            if (data != null)
            {
                LoggingFilterDataSet cacheDs = data ;
                //Instrumentation Flag
                instrumentationFlag = GetFlag(cacheDs, FilterCategory.Instrumentation.ToString(), logEntry.UserID);
                //Monitoring Flag
                monitoringFlag = GetFlag(cacheDs, FilterCategory.Monitoring.ToString(), logEntry.UserID);
                //Usage Flag
                usageFlag = GetFlag(cacheDs, FilterCategory.Usage.ToString(), string.Empty);
            }

            int flag = 0;
            if (instrumentationFlag == 1)
            {
                flag = flag | FilterFlag.InstrumentationFlag;
            }
            if (monitoringFlag == 1)
            {
                flag = flag | FilterFlag.MonitoringFlag;
            }
            if (logEntry.Component == ComponentType.ExternalSystem)
            {
                usageFlag = 0;
            }
            if (usageFlag == 1)
            {
                flag = flag | FilterFlag.UsageFlag;
            }

            logEntry.Flag = flag;
            if (instrumentationFlag == 1 || monitoringFlag == 1 || usageFlag == 1)
            {
                return true;
            }
            return false;
        }

        private int GetFlag(LoggingFilterDataSet cacheDs, string category, string userID)
        {
            int filterFlag = 0;

            if (cacheDs != null)
            {
                var filterResult = cacheDs.T_IC_LOGGING_FILTER
                                              .Where(i => i.CATEGORY.Equals(category)
                                                           && i.USER_ID.Equals(userID))
                                              .Select(i => i.FLAG).FirstOrDefault();

// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (filterResult != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    filterFlag = filterResult;
                }
            }


            return filterFlag;

        }
    }
}
