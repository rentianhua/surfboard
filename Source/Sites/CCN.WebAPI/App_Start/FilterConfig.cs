using System.Web;
using System.Web.Mvc;
using CCN.WebAPI.Common;

namespace CCN.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
