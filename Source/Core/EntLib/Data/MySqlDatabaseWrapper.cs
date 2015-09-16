using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Practices.Unity.Utility;
using MySql.Data.Entity;

namespace Cedar.Core.EntLib.Data
{
    public class MySqlDatabaseWrapper : Core.Data.Database
    {
        private string databaseName;
        private ConnectionStringSettings connectionStringSettings;

        protected static DbConnection connection;

        public MySqlConnectionFactory Database
        {
            get;
            private set;
        }

        public MySqlDatabaseWrapper(Func<MySqlConnectionFactory> dataAccessor,
            string databaseName, ConnectionStringSettings connectionStringSettings)
        {
            Guard.ArgumentNotNull(dataAccessor, "dataAccessor");
            Guard.ArgumentNotNullOrEmpty(databaseName, "databaseName");
            Database = dataAccessor();
            this.databaseName = databaseName;
            this.connectionStringSettings = connectionStringSettings;
        }

        public override DbConnection CreateConnection()
        {
            return Database.CreateConnection(connectionStringSettings.ConnectionString);
        }

        public override string DatabaseName
        {
            get
            {
                return databaseName;
            }
        }

        public override int Execute(string sql, dynamic param = null, DbTransaction transaction = null, CommandType commandType = CommandType.Text)
        {
            using (var con = Database.CreateConnection(connectionStringSettings.ConnectionString))
            {
                return param == null
                    ? con.Execute(sql, null, transaction, null, commandType)
                    : con.Execute(sql, (object)param, transaction, null, commandType);
            }
        }

        public override IEnumerable<T> Query<T>(string sql, dynamic param = null, DbTransaction transaction = null, CommandType commandType = CommandType.Text)
        {
            using (var con = Database.CreateConnection(connectionStringSettings.ConnectionString))
            {
                return con.Query<T>(sql, (object)param, transaction, true, null, commandType);
            }
        }

        public override IEnumerable<dynamic> Query(string sql, dynamic param = null, CommandType commandType = CommandType.Text)
        {
            using (var con = Database.CreateConnection(connectionStringSettings.ConnectionString))
            {
                con.Open();
                return con.Query(sql, (object)param, null, true, null, commandType);
            }
        }
    }
}
