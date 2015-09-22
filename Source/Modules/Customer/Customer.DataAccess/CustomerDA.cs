#region

using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using Cedar.Core.EntLib.Data;
using MySql.Data.MySqlClient;

#endregion

namespace CCN.Modules.Customer.DataAccess
{
    /// <summary>
    /// </summary>
    public class CustomerDA : CustomerDataAccessBase
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetALlCustomers()
        {
            var factoy = new DatabaseWrapperFactory().GetDatabase("mysqldb");
            var d = factoy.Query("select * from base_carbrand").ToList();
            return d;
        }
    }
}