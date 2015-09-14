#region

using System.Collections.Generic;
using Smartac.SR.Modules.Customer.BusinessComponent;
using Smartac.SR.Modules.Customer.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.AuditTrail.Interception;

#endregion

namespace Smartac.SR.Modules.Customer.BusinessService
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