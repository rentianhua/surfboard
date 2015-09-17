#region

using System.Collections.Generic;
using CCN.Modules.Customer.DataAccess;
using Cedar.Core.IoC;
using Cedar.Framework.Common.Server.BaseClasses;

#endregion

namespace CCN.Modules.Customer.BusinessComponent
{
    /// <summary>
    /// </summary>
    public class CustomerBC : BusinessComponentBase<CustomerDA>
    {
        /// <summary>
        /// </summary>
        /// <param name="da"></param>
        public CustomerBC(CustomerDA da)
            : base(da)
        {
            
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //[AuditTrailCallHandler("GetALlCustomers")]
        public List<dynamic> GetALlCustomers()
        {
            return DataAccess.GetALlCustomers();
        }
    }
}