#region

using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
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
            //var factoy = new DatabaseWrapperFactory().GetDatabase("roledb");

            //var i = factoy.ExecuteNonQuery(new SqlCommand("select * from sr_tag_tag"));

            return new List<dynamic>();
        }
    }
}