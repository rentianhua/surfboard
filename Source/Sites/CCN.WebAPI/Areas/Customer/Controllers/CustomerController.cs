#region

using System.Web.Mvc;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;

#endregion

namespace CCN.WebAPI.Areas.Customer.Controllers
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
            var d = _service.GetALlCustomers();
            return View(d);
        }
    }
}