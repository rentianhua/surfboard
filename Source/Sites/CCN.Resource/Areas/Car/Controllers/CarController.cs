using System;
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
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Checksums;

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
            var currenttime = DateTime.Now.ToString("yyyyMMddhhmmss");
            try
            {
                foreach (var item in piclist)
                {
                    if (!string.IsNullOrWhiteSpace(item.imgsrc))
                    {
                        var url = ConfigHelper.GetAppSettings("GETURL");
                        var savepath = "d:\\kplxpic\\" + currenttime + "\\" + item.imgsrc;
                        //验证并创建目录
                        CheckPath(savepath);

                        url = url + item.imgsrc;
                        WebClient web = new WebClient();
                        web.DownloadFile(url, savepath);
                    }
                }
            }
            catch
            {
                return Json(new { errcode = 1 }, "text/html; charset=UTF-8");
            }
            CreateZip("d:\\kplxpic\\" + currenttime, "d:\\kplxpic\\" + currenttime+".zip");
            return Json(new { errcode = 0 }, "text/html; charset=UTF-8");
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

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationZipFilePath"></param>
        public static void CreateZip(string sourceFilePath, string destinationZipFilePath)
        {
            if (sourceFilePath[sourceFilePath.Length - 1] != Path.DirectorySeparatorChar)
                sourceFilePath += Path.DirectorySeparatorChar;
            ZipOutputStream zipStream = new ZipOutputStream(global::System.IO.File.Create(destinationZipFilePath));
            zipStream.SetLevel(6);  // 压缩级别 0-9
            CreateZipFiles(sourceFilePath, zipStream);
            zipStream.Finish();
            zipStream.Close();
            sourceFilePath = sourceFilePath.Substring(0, sourceFilePath.LastIndexOf('\\'));
            Directory.Delete(sourceFilePath,true);
        }
        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file))                     //如果当前是文件夹，递归
                {
                    CreateZipFiles(file, zipStream);
                }
                else                                            //如果是文件，开始压缩
                {
                    FileStream fileStream = global::System.IO.File.OpenRead(file);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(sourceFilePath.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);
                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }




        #endregion
    }
}