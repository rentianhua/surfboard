using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Core.Data
{
    /// <summary>
    /// A abstract database which is used to perform basic data access operation.
    /// </summary>
    public abstract class Database
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public abstract string DatabaseName
        {
            get;
        }

        /// <summary>
        /// Gets the database provider factory.
        /// </summary>
        /// <value>
        /// The database provider factory.
        /// </value>
        public abstract DbProviderFactory DbProviderFactory
        {
            get;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The number of rows affected.</returns>
        public abstract int ExecuteNonQuery(DbCommand command, DbTransaction transaction = null);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns>The number of rows affected.</returns>
        public abstract int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>The number of rows affected.</returns>
        public abstract int ExecuteNonQuery(CommandType commandType, string commandText);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns>The number of rows affected.</returns>
        public abstract int ExecuteNonQuery(DbTransaction transaction, string storedProcedureName, params object[] parameterValues);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        public abstract IDataReader ExecuteReader(DbCommand command, DbTransaction transaction = null);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public abstract IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public abstract IDataReader ExecuteReader(CommandType commandType, string commandText);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(CommandType commandType, string commandText);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(DbCommand command);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(DbCommand command, DbTransaction transaction);

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The <see cref="T:System.Data.DataSet" />.</returns>
        public abstract DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction = null);

        /// <summary>
        /// Gets the stored proc command.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns></returns>
        public abstract DbCommand GetStoredProcCommand(string procedureName);

        /// <summary>
        /// Gets the stored proc command.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public abstract DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues);

        /// <summary>
        /// Discovers the parameters.
        /// </summary>
        /// <param name="cmd">The command.</param>
        public abstract void DiscoverParameters(DbCommand cmd);

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// <para>Adds a new instance of a <see cref="T:System.Data.Common.DbParameter" /> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="T:System.Data.DbType" /> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="T:System.Data.ParameterDirection" /> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value" />.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="T:System.Data.DataRowVersion" /> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        public abstract void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value);
        
        /// <summary>
        /// Adds a new In <see cref="T:System.Data.Common.DbParameter" /> object to the given <paramref name="command" />.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="T:System.Data.DbType" /> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>  
        public abstract void AddInParameter(DbCommand command, string name, DbType dbType);
       
        /// <summary>
        /// Adds a new In <see cref="T:System.Data.Common.DbParameter" /> object to the given <paramref name="command" />.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="T:System.Data.DbType" /> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public abstract void AddInParameter(DbCommand command, string name, DbType dbType, object value);
        
        /// <summary>
        /// Adds a new In <see cref="T:System.Data.Common.DbParameter" /> object to the given <paramref name="command" />.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="T:System.Data.DbType" /> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="T:System.Data.DataRowVersion" /> values.</para></param>
        public abstract void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion);
        
        /// <summary>
        /// Adds the out parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="size">The size.</param>
        public abstract void AddOutParameter(DbCommand command, string name, DbType dbType, int size);
        
        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract object GetParameterValue(DbCommand command, string name);
        
        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public abstract void SetParameterValue(DbCommand command, string name, object value);
        
        /// <summary>
        /// Gets a DbDataAdapter with Standard update behavior.
        /// </summary>
        /// <returns>A <see cref="T:System.Data.Common.DbDataAdapter" />.</returns>
        /// <seealso cref="T:System.Data.Common.DbDataAdapter" />
        /// <devdoc>
        /// Created this new, public method instead of modifying the protected, abstract one so that there will be no
        /// breaking changes for any currently derived Database class.
        /// </devdoc>
        public abstract DbDataAdapter GetDataAdapter();
        
        /// <summary>
        /// Wraps around a derived class's implementation of the GetStoredProcCommandWrapper method and adds functionality for
        /// using this method with UpdateDataSet.  The GetStoredProcCommandWrapper method (above) that takes a params array 
        /// expects the array to be filled with VALUES for the parameters. This method differs from the GetStoredProcCommandWrapper 
        /// method in that it allows a user to pass in a string array. It will also dynamically discover the parameters for the 
        /// stored procedure and set the parameter's SourceColumns to the strings that are passed in. It does this by mapping 
        /// the parameters to the strings IN ORDER. Thus, order is very important.
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="sourceColumns"><para>The list of DataFields for the procedure.</para></param>
        /// <returns><para>The <see cref="T:System.Data.Common.DbCommand" /> for the stored procedure.</para></returns>
        public abstract DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName, params string[] sourceColumns);
        
        /// <summary>
        /// Builds DBMS specific parameter name.
        /// </summary>
        /// <param name="parameterName">The parameter name without any prefix symbol.</param>
        /// <returns>The DBMS specific parameter name.</returns>
        public abstract string BuildParameterName(string parameterName);
        
        /// <summary>
        /// Build a <see cref="T:System.Data.Common.DbCommand" /> based on the SQL text.
        /// </summary>
        /// <param name="query">The SQL text.</param>
        /// <returns>The <see cref="T:System.Data.Common.DbCommand" />.</returns>
        public abstract DbCommand GetSqlStringCommand(string query);
    }
}
