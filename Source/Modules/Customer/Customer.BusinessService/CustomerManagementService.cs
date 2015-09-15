#region

using System.Collections.Generic;
using CCN.Modules.Customer.BusinessComponent;
using CCN.Modules.Customer.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.AuditTrail.Interception;

#endregion

namespace CCN.Modules.Customer.BusinessService
{
    /// <summary>
    /// </summary>
    public class CustomerManagementService : ServiceBase<CustomerBC>, ICustomerManagementService
    {
        /// <summary>
        /// </summary>
        public CustomerManagementService(CustomerBC bc)
            : base(bc)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AuditTrailCallHandler("GetALlCustomers")]
        public List<dynamic> GetALlCustomers()
        {
            return BusinessComponent.GetALlCustomers();
        }
    }
}