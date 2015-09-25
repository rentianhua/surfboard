#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Practices.Unity.Utility;

#endregion

namespace Cedar.Framework.Common.Server.BaseClasses
{
    public class MySqlDbHelper : MarshalByRefObject
    {
        //private Dictionary<CacheKey, DbParameterCollection> cachedParameters = new Dictionary<CacheKey, DbParameterCollection>();

        /// <summary>
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MySqlDbHelper(string connectionStringName)
        {
            Guard.ArgumentNotNullOrEmpty(connectionStringName, "connectionStringName");
            ConnectionStringName = connectionStringName;
            ConnectionStringSettings cnnStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            ConnectionString = cnnStringSettings.ConnectionString;
            Factory = DbProviderFactories.GetFactory(cnnStringSettings.ProviderName);
        }

        /// <summary>
        /// </summary>
        protected string ConnectionStringName { get; private set; }

        /// <summary>
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// </summary>
        public DbProviderFactory Factory { get; private set; }

        //protected abstract void DeriveParameters(DbCommand discoveryCommand);

        //protected virtual int UserParametersStartIndex()
        //{
        //    return 1;
        //}

        //public virtual string BuildParameterName(string name)
        //{
        //    if (name.StartsWith("@"))
        //    {
        //        return name;
        //    }
        //    return "@" + name;
        //}

        //protected virtual void AssignParameters(DbCommand command, object[] parameters)
        //{
        //    CacheKey key = new CacheKey { Database = this.ConnectionStringName, Procedure = command.CommandText };
        //    if (!cachedParameters.ContainsKey(key))
        //    {
        //        this.DeriveParameters(command);
        //        lock (cachedParameters)
        //        {
        //            cachedParameters[key] = command.Parameters;
        //        }
        //    }
        //    int parameterIndexShift = this.UserParametersStartIndex();
        //    for (int i = 0; i < parameters.Length; i++)
        //    {
        //        IDataParameter parameter = command.Parameters[i + parameterIndexShift];
        //        command.Parameters[this.BuildParameterName(parameter.ParameterName)].Value = parameters[i] ?? DBNull.Value;
        //    }
        //}

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string sql, object parameters, CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.ExecuteScalar<T>(sql, parameters, null, null, commandType);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters,
            CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.QueryAsync<T>(sql, parameters, null, null, commandType);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string sql, object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.Query<T>(sql, parameters, null, true, null, commandType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> Query(string sql, object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.Query(sql, parameters, null, true, null, commandType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteAsync(string sql, object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.ExecuteAsync(sql, parameters, null, null, commandType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual int Execute(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = Factory.CreateConnection())
            {
                return connection.Execute(sql, parameters, null, null, commandType);
            }
        }
    }

    //internal class CacheKey
    //{
    //    public string Database { get; set; }
    //    public string Procedure { get; set; }
    //    public override int GetHashCode()
    //    {
    //        return this.Database.GetHashCode() ^ this.Procedure.GetHashCode();
    //    }
    //}
}