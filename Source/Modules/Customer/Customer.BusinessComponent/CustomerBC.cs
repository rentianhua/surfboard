#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.DataAccess;
using Cedar.AuditTrail.Interception;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
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

        #region 用户模块

        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckUserName(string username)
        {
            return DataAccess.CheckUserName(username);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckMobile(string mobile)
        {
            return DataAccess.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            //密码加密
            var en = new Encryptor();
            userInfo.Password = en.EncryptMd5(userInfo.Password);
            userInfo.Type = 1; //这版只有车商
            userInfo.Status = 1; //初始化状态[1.正常]
            userInfo.AuthStatus = 0; //初始化认证状态[0.未提交认证]
            userInfo.Createdtime = DateTime.Now;

            var innerid = Guid.NewGuid().ToString();
            userInfo.Innerid = innerid;

            if (userInfo.Wechat != null)
            {
                userInfo.Wechat.Custid = userInfo.Innerid;
                userInfo.Wechat.Createdtime = DateTime.Now;
            }

            var result = DataAccess.CustRegister(userInfo);

            #region 生成二维码
            
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var filename = "D:\\" + Guid.NewGuid() + ".jpg";
                    var website = ConfigHelper.GetAppSettings("website");
                    var bitmap = BarCodeUtility.CreateBarcode(website + "?innerid=" + userInfo.Innerid, 240, 240);

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    bitmap.Save(filename);

                    var qinniu = new QiniuUtility();
                    var qrcodeKey = qinniu.PutFile(filename);
                    if (!string.IsNullOrWhiteSpace(qrcodeKey))
                    {
                        DataAccess.UpdateQrCode(innerid, qrcodeKey);
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            });
            #endregion

            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public JResult CustLogin(CustLoginInfo loginInfo)
        {
            var result = new JResult();
            if (string.IsNullOrWhiteSpace(loginInfo.Username) && string.IsNullOrWhiteSpace(loginInfo.Mobile))
            {
                result.errcode = 403;
                result.errmsg = "帐户名不能为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(loginInfo.Password))
            {
                result.errcode = 404;
                result.errmsg = "密码不能为空";
                return result;
            }

            var en = new Encryptor();
            loginInfo.Password = en.EncryptMd5(loginInfo.Password);
            
            var userInfo = DataAccess.CustLogin(loginInfo);
            if (userInfo == null)
            {
                result.errcode = 401;
                result.errmsg = "帐户名或登录密码不正确";
            }
            else if (userInfo.Status == 2)
            {
                result.errcode = 402;
                result.errmsg = "帐户被冻结";
            }
            else
            {
                result.errcode = 0;
                result.errmsg = userInfo;
            }
            return result;
        }


        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public JResult GetCustById(string innerid)
        {
            var list = DataAccess.GetCustById(innerid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "没有数据"
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            return DataAccess.GetCustPageList(query);
        }


        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult AddAuthentication(CustAuthenticationModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Custid))
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "会员id不能空"
                };
            }
            model.Createdtime = DateTime.Now;
            model.Modifiedtime = null;
            var result = DataAccess.AddAuthentication(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "申请认证成功" : "申请认证失败"
            };
        }

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public JResult UpdateAuthentication(CustAuthenticationModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Custid))
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "会员id不能空"
                };
            }
            model.Modifiedtime = DateTime.Now;
            var result = DataAccess.UpdateAuthentication(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "修改认证信息成功" : "修改认证信息失败"
            };
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CustomerBC.AuditAuthentication")]
        public JResult AuditAuthentication(CustModel info)
        {
            var operid = ApplicationContext.Current.UserId;
            if (string.IsNullOrWhiteSpace(operid))
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "操作人信息不存在"
                };
            }
            var result = DataAccess.AuditAuthentication(info, operid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "审核成功" : "审核失败"
            };
        }


        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCustAuthById(string innerid)
        {
            var list = DataAccess.GetCustAuthById(innerid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "没有数据"
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult GetCustAuthByCustid(string custid)
        {
            var list = DataAccess.GetCustAuthByCustid(custid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "没有数据"
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

    }
}