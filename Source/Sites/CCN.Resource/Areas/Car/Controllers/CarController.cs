﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using CCN.Resource.Main.Common;
using System.Net;
using CCN.Modules.Car.Interface;
using System.IO;

namespace CCN.Resource.Areas.Car.Controllers
{
    public class CarController : DefaultController
    {
        // GET: Car/Car
        public ActionResult CarList(string custid)
        {
            if ((CustModel)Session["CustModel"] != null)
            {
                custid = ((CustModel)Session["CustModel"]).Innerid;
            }
            if (string.IsNullOrWhiteSpace(custid))
            {
                return Redirect("/Customer/Customer/CustomerList");
            }

            var custservice = ServiceLocatorFactory.GetServiceLocator().GetService<ICustomerManagementService>();
            var jresult = custservice.GetCustById(custid);
            if (jresult != null && jresult.errcode == 0)
            {
                var custModel = (CustViewModel)jresult.errmsg;
                ViewBag.custname = "[" + custModel.Custname + "]";
            }

            ViewBag.custid = custid;
            return View();
        }

        public ActionResult CarEdit(string custid, string carid)
        {
            if (string.IsNullOrWhiteSpace(custid))
            {
                return Redirect("/Customer/Customer/CustomerList");
            }

            ViewBag.custid = custid;
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        /// <summary>
        /// 车辆列表（没有录车功能）
        /// </summary>
        /// <returns></returns>
        public ActionResult CarShowList()
        {
            if (ADMIN != UserInfo.innerid)
            {
                ViewBag.userid = UserInfo.innerid;
            }
            return View();
        }

        /// <summary>
        /// 车辆详情
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public ActionResult CarView(string carid)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            return View();
        }

        /// <summary>
        /// 车(主)贷列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CarLoanList()
        {
            return View();
        }

        /// <summary>
        /// 车(主)贷编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CarLoanEdit(string id)
        {
            ViewBag.id = string.IsNullOrWhiteSpace(id) ? "" : id;
            ViewBag.UserName = UserInfo.username;
            ViewBag.UserNo = UserInfo.no;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        #region 金融

        /// <summary>
        /// 申请金融方案列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceProgrammeList()
        {
            if (ADMIN != UserInfo.innerid)
            {
                ViewBag.userid = UserInfo.innerid;
            }
            return View();
        }

        /// <summary>
        /// 金融方案申请新增
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceProgrammeAdd()
        {
            ViewBag.userid = UserInfo.innerid;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        /// <summary>
        /// 金融方案申请编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceProgrammeEdit(string innerid)
        {
            if (ADMIN == UserInfo.innerid)
            {
                ViewBag.isadmin = 1;
            }
            ViewBag.innerid = string.IsNullOrWhiteSpace(innerid) ? "" : innerid;
            ViewBag.userid = UserInfo.innerid;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        #endregion

        #region 神秘车源

        /// <summary>
        /// 神秘车源列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CarSupplierList()
        {
            return View();
        }

        /// <summary>
        /// 神秘车源编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult CarSupplierEdit(string carid)
        {
            ViewBag.carid = string.IsNullOrWhiteSpace(carid) ? "" : carid;
            ViewBag.uptoken = QiniuUtility.GetToken();
            return View();
        }

        #endregion

        #region 下载
        public ActionResult DownZip(List<PicModel> piclist)
        {
            var res = new JsonResult();
            foreach (var item in piclist)
            {
                if (!string.IsNullOrWhiteSpace(item.imgsrc))
                {
                    var url = ConfigHelper.GetAppSettings("GETURL");
                    var savepath = "d:\\kplxpic\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "\\" + item.imgsrc;
                    //验证并创建目录
                    CheckPath(savepath);

                    url = url + item.imgsrc;
                    WebClient web = new WebClient();
                    web.DownloadFile(url, savepath);
                }
            }

            return res;
        }

        /// <summary>
        /// 检查路径，创建目录
        /// </summary>
        static void CheckPath(string path)
        {
            path = path.Substring(0, path.LastIndexOf('\\'));
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
        }

        public class PicModel
        {
            public string imgsrc { get; set; }
        }
        #endregion
    }
}