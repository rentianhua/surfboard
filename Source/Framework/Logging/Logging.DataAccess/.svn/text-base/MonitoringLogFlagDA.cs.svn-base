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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;
using System.Data.Common;
using HiiP.Framework.Logging.Library;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using HiiP.Framework.Common;
using System.Transactions;
using HiiP.Framework.Logging.BusinessEntity;

namespace HiiP.Framework.Logging.DataAccess
{
    public class MonitoringLogFlagDA
    {
        #region Variable
        private const string GetAllFiltersSP = "P_IC_LOGGING_FILTER_GET_ALL";

        private string _connectionStringName;

        private Database _database;
        #endregion

        #region Construction
        public MonitoringLogFlagDA()
        {
            _connectionStringName = Utility.GetLoggingConnectionStringName();
            _database = DatabaseFactory.CreateDatabase(_connectionStringName);
        }
        #endregion

        #region Public method

        public LoggingFilterDataSet GetAllFilters()
        {
            LoggingFilterDataSet ds ;
            if (Transaction.Current == null)
            {
                ds = Execute();
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    ds = Execute();
                    scope.Complete();
                }
            }
            return ds;
        }

        private LoggingFilterDataSet Execute()
        {
            LoggingFilterDataSet ds = new LoggingFilterDataSet();
            DbCommand dbCommand = _database.GetStoredProcCommand(GetAllFiltersSP);
            _database.LoadDataSet(dbCommand, ds, ds.T_IC_LOGGING_FILTER.TableName);

            return ds;
        }

        #endregion
    }
}
