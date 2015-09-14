#region

using System.Web.Mvc;
using Smartac.SR.Modules.Customer.Interface;
using Cedar.Core.IoC;

#endregion

namespace Smartac.SR.Main.Areas.Customer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManagementService _service;

        public CustomerController()
        {
            _service = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
        }

        // GET: Customer/Customer
        public ActionResult Index()
        {
            _service.GetALlCustomers();
            return View();
        }
    }
}