#region

using System.Collections.Generic;

#endregion

namespace Smartac.SR.Modules.Customer.Interface
{
    /// <summary>
    /// </summary>
    public interface ICustomerManagementService
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetALlCustomers();
    }
}