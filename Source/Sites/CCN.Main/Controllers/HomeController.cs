#region

using System.Collections.Generic;
using System.Web.Mvc;

#endregion

namespace Smartac.SR.Main.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        /// <summary>
        ///     顶部导航
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult NavBar()
        {
            var links = new Dictionary<string, string>(4);
            links.Add("Smart Rewards", Url.Action("Index", "Home"));
            links.Add("Smart Access", "#");
            links.Add("APP", "#");
            links.Add("Smart Space", "#");
            ViewBag.Links = links;

            //var validMenu = new List<TbResourceMenuModel>();
            //if (MySession != null)
            //{
            //    //获取所有可用菜单
            //    validMenu = (from menu in MySession.pmlist
            //                 where menu.IsEnabled.Equals(1)
            //                 orderby menu.ResourceKey, menu.SortNo
            //                 select menu).ToList();
            //}
            //ViewData["SiteMap"] = validMenu;

            //var fileName = Localization.GetInstance().LanguageKey.Equals("en-US") ? "Log_En.config" : "Log_Zh.config";
            //var path = string.Concat("/Content/ChangeLog/", fileName);
            //var reader = XmlHelper.CreateReader(Server.MapPath(path));
            //var doc = new XmlDocument();
            //doc.Load(reader);
            //ViewData["ChangeLog"] = doc;

            //ViewData["MultipleLanguage"] =
            //    _baseDao.LanguageGet(Localization.GetInstance().LanguageKey).OrderBy(l => l.Value).ToList();

            return PartialView();
        }
    }
}