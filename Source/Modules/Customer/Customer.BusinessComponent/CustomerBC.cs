﻿#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CCN.Modules.Customer.BusinessEntity;
using CCN.Modules.Customer.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        #region 用户模块

        /// <summary>
        /// 会员注册检查Email是否被注册
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>0：未被注册，非0：Email被注册</returns>
        public int CheckEmail(string email)
        {
            return DataAccess.CheckEmail(email);
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：被注册</returns>
        public int CheckMobile(string mobile)
        {
            return DataAccess.CheckMobile(mobile);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>

        public JResult CustRegister(CustModel userInfo)
        {
            LoggerFactories.CreateLogger().Write("注册：" + JsonConvert.SerializeObject(userInfo, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), TraceEventType.Information);
            var mYan = DataAccess.CheckMobile(userInfo.Mobile);
            if (mYan > 0)
            {
                return new JResult
                {
                    errcode = 402,
                    errmsg = "手机号被其他人注册"
                };
            }

            if (string.IsNullOrWhiteSpace(userInfo.Custname))
            {
                //生成会员名称
                userInfo.Custname = string.Concat("ccn_", DateTime.Now.Year, "_",
                    userInfo.Mobile.Substring(userInfo.Mobile.Length - 6));
            }
            
            if (!string.IsNullOrWhiteSpace(userInfo.Wechat?.Openid))
            {
                //检查openid是否被其他手机号注册
                var m = DataAccess.CustInfoByOpenid(userInfo.Wechat.Openid);
                if (m != null) //openid已绑定其他手机号（如果绑定自己手机号，CheckMobile接口就过滤掉）
                {
                    return JResult._jResult(403, "openid已绑定其他手机号");
                }

                //填充会员
                var wechat = DataAccess.CustWechatByOpenid(userInfo.Wechat.Openid);
                if (wechat != null)
                {
                    userInfo.Custname = wechat.Nickname;
                }
            }

            //密码加密
            userInfo.Password = Encryptor.EncryptAes(userInfo.Password);

            if (userInfo.Type == null)
            {
                userInfo.Type = 1; //这版只有车商
            }
            
            userInfo.Status = 1; //初始化状态[1.正常]
            userInfo.AuthStatus = 0; //初始化认证状态[0.未提交认证]
            userInfo.Createdtime = DateTime.Now;
            userInfo.Totalpoints = 0;
            userInfo.Level = 0;

            var innerid = Guid.NewGuid().ToString();
            userInfo.Innerid = innerid;

            var result = DataAccess.CustRegister(userInfo);

            #region 生成二维码

            Task.Run(() =>
            {
                try
                {
                    //生成二维码位图
                    var bitmap = BarCodeUtility.CreateBarcode(userInfo.Mobile, 240, 240);
                    var filename = QiniuUtility.GetFileName(Picture.cust_qrcode);
                    var stream = BarCodeUtility.BitmapToStream(bitmap);
                    //上传图片到七牛云
                    var qinniu = new QiniuUtility();
                    var qrcode = qinniu.Put(stream, "", filename);
                    stream.Dispose();
                    //上传成功更新会员二维码
                    if (!string.IsNullOrWhiteSpace(qrcode))
                    {
                        DataAccess.UpdateQrCode(innerid, qrcode);
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                    LoggerFactories.CreateLogger().Write("CustRegister接口异常", TraceEventType.Error, ex);
                }
            });
            #endregion
            
            return new JResult
            {
                errcode = result > 0 ? 0 : 404,
                errmsg = result > 0 ? innerid : ""
            };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public JResult CustLogin(CustLoginInfo loginInfo)
        {
            LoggerFactories.CreateLogger().Write("登录："+JsonConvert.SerializeObject(loginInfo, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), TraceEventType.Information);
            if (string.IsNullOrWhiteSpace(loginInfo.Mobile))
            {
                return JResult._jResult(403, "帐户名不能为空");
            }
            if (string.IsNullOrWhiteSpace(loginInfo.Password))
            {
                return JResult._jResult(404, "密码不能为空");
            }

            loginInfo.Password = Encryptor.EncryptAes(loginInfo.Password);

            var userInfo = DataAccess.CustLogin(loginInfo);
            if (userInfo == null)
            {
                return JResult._jResult(401, "帐户名或登录密码不正确");
            }
            if (userInfo.Status == 2)
            {
                return JResult._jResult(402, "帐户被冻结");
            }

            userInfo.Password = "";

            return JResult._jResult(0, userInfo);
        }

        /// <summary>
        /// 判断是否会员
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public JResult IsCustByOpenid(string openid)
        {
            var userInfo = DataAccess.GetCustByOpenid(openid);
            if (userInfo == null || userInfo.Status != 1)
            {
                return JResult._jResult(400, "否");
            }
            return JResult._jResult(0, "是");
        }

        /// <summary>
        /// 用户登录(openid登录)
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>用户信息</returns>
        public JResult CustLoginByOpenid(string openid)
        {
            var userInfo = DataAccess.CustLoginByOpenid(openid);
            if (userInfo == null)
            {
                return JResult._jResult(405, "会员不存在");
            }
            if (userInfo.Status == 2)
            {
                return JResult._jResult(402, "帐户被冻结");
            }
            userInfo.Password = "";
            return JResult._jResult(0, userInfo);
        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public JResult GetCustById(string innerid)
        {
            var model = DataAccess.GetCustById(innerid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            model.Password = "";
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        /// <summary>
        /// 获取会员详情（根据手机号）
        /// </summary>
        /// <param name="mobile">会员手机号</param>
        /// <returns></returns>
        public JResult GetCustByMobile(string mobile)
        {
            var model = DataAccess.GetCustByMobile(mobile);
            if (model != null)
            {
                model.Password = "";
            }
            
            return JResult._jResult(model);
        }

        /// <summary>
        /// 手机+验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public JResult GetCustLoginByMobile(string mobile)
        {
            var model = DataAccess.GetCustByMobile(mobile);
            if (model == null)
            {
                return JResult._jResult(401, "帐户名或登录密码不正确");
            }
            if (model.Status == 2)
            {
                return JResult._jResult(402, "帐户被冻结");
            }
            model.Password = "";
            return JResult._jResult(model);
        }
        
        /// <summary>
        /// 根据carid获取会员基本信息
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>用户信息</returns>
        public JResult CustInfoByCarid(string carid)
        {
            var model = DataAccess.CustInfoByCarid(carid);
            if (model != null)
            {
                model.Password = "";
            }
            
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            //if (!string.IsNullOrWhiteSpace(query?.Mobile) && query.Mobile.Trim().Length >= 4)

            return DataAccess.GetCustPageList(query);

            //var list = new BasePageList<CustModel>
            //{
            //    aaData = null,
            //    iTotalRecords = 0,
            //    iTotalDisplayRecords = 0,
            //    sEcho = 0
            //};
            //return list;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        public JResult UpdatePassword(CustRetrievePassword mRetrievePassword)
        {

            var model = DataAccess.GetCustByMobile(mRetrievePassword.Mobile);
            if (model == null)
            {
                return JResult._jResult(403, "账户不存在");
            }

            if (model.Status == 2)
            {
                return JResult._jResult(404, "账户被冻结");
            }

            //密码加密
            mRetrievePassword.NewPassword = Encryptor.EncryptAes(mRetrievePassword.NewPassword);
            var result = DataAccess.UpdatePassword(mRetrievePassword);
            return new JResult
            {
                errcode = result > 0 ? 0 : 405,
                errmsg = result > 0 ? "修改成功" : "修改失败"
            };
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCustInfo(CustModel model)
        {
            var newModel = new CustModel
            {
                Innerid = model.Innerid,
                Custname = model.Custname,
                Telephone = model.Telephone,
                Email = model.Email,
                Headportrait = model.Headportrait,
                Provid = model.Provid,
                Cityid = model.Cityid,
                Area = model.Area,
                Sex = model.Sex,
                Brithday = model.Brithday,
                QQ = model.QQ,
                Signature = model.Signature
            };

            var result = DataAccess.UpdateCustInfo(newModel);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "修改成功" : "修改失败"
            };
        }

        /// <summary>
        /// 修改会员状态(冻结和解冻)
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCustStatus(string innerid, int status)
        {
            var result = DataAccess.UpdateCustStatus(innerid, status);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改会员类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult UpdateCustType(string innerid)
        {
            var result = DataAccess.UpdateCustType(innerid);
            return JResult._jResult(result);
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

            int result;
            var m = DataAccess.GetCustAuthByCustid(model.Custid);
            if (m == null)
            {
                result = DataAccess.AddAuthentication(model);
            }
            else
            {
                model.Innerid = m.Innerid;
                result = DataAccess.UpdateAuthentication(model);
            }

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
            model.AuditPer = null;
            model.AuditDesc = null;
            model.AuditTime = null;
            model.AuditResult = null;
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
        /// <param name="model">会员相关信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CustomerBC.AuditAuthentication")]
        public JResult AuditAuthentication(CustAuthenticationModel model)
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

            model.AuditPer = operid;
            model.AuditTime = DateTime.Now;

            var result = DataAccess.AuditAuthentication(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "审核成功" : "审核失败"
            };
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult CancelAuditAuthentication(string custid)
        {
            var result = DataAccess.CancelAuditAuthentication(custid);
            return JResult._jResult(result);
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
                    errmsg = ""
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
            var model = DataAccess.GetCustAuthByCustid(custid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        #endregion

        #region 会员点赞

        /// <summary>
        /// 给会员点赞
        /// </summary>
        /// <param name="model">粉丝信息</param>
        /// <returns></returns>
        public JResult CustPraise(CustLaudator model)
        {
            if (string.IsNullOrWhiteSpace(model?.Openid) || string.IsNullOrWhiteSpace(model.Tocustid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var m = DataAccess.GetCustLaudatorByOpenid(model.Openid, model.Tocustid);
            if (m != null) //说明点赞人信息已经存在
            {
                model.Innerid = m.Innerid; //
                //说明之前的信息是匿名的,现在非匿名，需要更新信息 或者 修改了信息后赞人，库中数据也更新
                //if (string.IsNullOrWhiteSpace(m.Nickname) && !m.Nickname.Equals(model.Nickname)) 
                if (!m.Nickname.Equals(model.Nickname) && !string.IsNullOrWhiteSpace(m.Nickname))
                {
                    var uModel = new CustLaudator
                    {
                        Accountid = model.Accountid,
                        Openid = model.Openid,
                        Nickname = model.Nickname,
                        Sex = model.Sex,
                        Photo = model.Photo,
                        Remarkname = model.Remarkname,
                        Area = model.Area,
                        Subscribe_time = model.Subscribe_time,
                        Subscribe = model.Subscribe,
                        Country = model.Country,
                        Province = model.Province,
                        City = model.City
                    };
                    DataAccess.UpdateLaudator(uModel);
                }
                if (m.IsPraise > 0)
                {
                    return JResult._jResult(402, "重复点赞");
                }
            }
            else
            {
                model.Innerid = Guid.NewGuid().ToString();
                model.Createdtime = DateTime.Now;
                var addresult = DataAccess.AddLaudator(model);
                if (addresult == 0)
                {
                    return JResult._jResult(403, "点赞人信息保存失败");
                }
            }

            var rel = new CustLaudatorRelation
            {
                Laudatorid = model.Innerid,
                Tocustid = model.Tocustid,
                Carid = model.Carid,
                Createdtime = DateTime.Now
            };
            var res = DataAccess.SaveLaudatorRelation(rel);
            return JResult._jResult(
                res > 0 ? 0 : 400,
                res > 0 ? "赞成功" : "赞失败");
        }

        /// <summary>
        /// 根据会员id获取所有点赞人列表
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public JResult GetLaudatorListByCustid(string custid)
        {
            var list = DataAccess.GetLaudatorListByCustid(custid);
            return JResult._jResult(0, list);
        }

        /// <summary>
        /// 判断是否点赞
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult RepeatPraise(string custid, string openid)
        {
            var list = DataAccess.GetLaudatorListByCustid(custid).ToList();
            if (!list.Any())
            {
                return JResult._jResult(0, "");
            }
            if (list.Any(item => item.Openid.Equals(openid)))
            {
                return JResult._jResult(400, "");
            }
            
            return JResult._jResult(00, "");
        }

        #endregion

        #region 会员标签


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public JResult AddTag(CustTagModel model)
        {
            model.Isenabled = 1;
            var result = DataAccess.AddTag(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "添加成功" : "添加失败"
            };
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public JResult UpdateTag(CustTagModel model)
        {
            model.Modifiedtime = DateTime.Now;
            var result = DataAccess.UpdateTag(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "修改成功" : "修改失败"
            };
        }
        /// <summary>
        /// 修改标签状态
        /// </summary>
        /// <param name="innerid">标签ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateTagStatus(string innerid, int status)
        {
            var result = DataAccess.UpdateTagStatus(innerid, status);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "修改成功" : "修改失败"
            };
        }
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public JResult DeleteTag(string innerid)
        {
            var result = DataAccess.DeleteTag(innerid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "删除成功" : "删除失败"
            };
        }

        /// <summary>
        /// 获取标签详情
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public JResult GetTagById(string innerid)
        {
            var model = DataAccess.GetTagById(innerid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "暂无数据"
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustTagModel> GetTagPageList(CustTagQueryModel query)
        {
            return DataAccess.GetTagPageList(query);
        }

        /// <summary>
        /// 打标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoTagRelation(CustTagRelation model)
        {
            if (model == null)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "参数不正确"
                };
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.DoTagRelation(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "打标签成功" : "打标签失败"
            };
        }

        /// <summary>
        /// 删除标签关系
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DelTagRelation(string innerid)
        {
            var result = DataAccess.DelTagRelation(innerid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "删除标签成功" : "删除标签失败"
            };
        }

        /// <summary>
        /// 获取会员拥有的标签
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public JResult GetTagRelation(string custid)
        {
            var list = DataAccess.GetTagRelation(custid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取会员该标签的操作者
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        public JResult GetTagRelationWithOper(string custid, string tagid)
        {
            var list = DataAccess.GetTagRelationWithOper(custid, tagid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 数据清理

        /// <summary>
        /// 清除所有数据(除基础数据)
        /// </summary>
        /// <returns></returns>
        public JResult DeleteAll()
        {
            var result = DataAccess.DeleteAll();
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "清除成功" : "清除失败"
            };
        }
        
        /// <summary>
        /// 删除会员所有信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public JResult DeleteCustomer(string mobile)
        {
            /*
            //图片
            1.会员的二维码 cust_info
            2.会员认证信息的图片 cust_authentication
            3.会员的所有的车辆的图片 car_picture
            4.礼券的二维码 coupon_code
            */

            var picModel = DataAccess.GetCustomerAllPicture(mobile);
            if (picModel != null)
            {
                var qiniu = new QiniuUtility();
                if (!string.IsNullOrWhiteSpace(picModel.Qrcode))
                {
                    qiniu.DeleteFile(picModel.Qrcode);
                }
                if (!string.IsNullOrWhiteSpace(picModel.AuthPic))
                {
                    foreach (var item in picModel.AuthPic.Split(','))
                    {
                        qiniu.DeleteFile(item);
                    }
                }
                if (picModel.CarPicList.Any())
                {
                    foreach (var item in picModel.CarPicList)
                    {
                        qiniu.DeleteFile(item);
                    }
                }
                if (picModel.CodeList.Any())
                {
                    foreach (var item in picModel.CodeList)
                    {
                        qiniu.DeleteFile(item);
                    }
                }
            }
            
            var result = DataAccess.DeleteCustomer(mobile);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "删除成功" : "删除失败"
            };
        }

        #endregion

        #region 微信信息
        /// <summary>
        /// 获取cust_wechat信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CustWeChatViewModel> GetCustWeChatList(CustWeChatQueryModel query)
        {
            return DataAccess.GetCustWeChatList(query);
        }

        /// <summary>
        /// 更新绑定openid
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult BindOpenid(string custid, string openid)
        {
            if (string.IsNullOrWhiteSpace(custid) || string.IsNullOrWhiteSpace(openid))
            {
                return JResult._jResult(401,"参数不完整");
            }

            var wechat = DataAccess.CustWechatByOpenid(openid);
            if (wechat == null)
            {
                return JResult._jResult(402, "没有微信信息");
            }

            var result = DataAccess.UpdateOpenid(custid, openid);
            return JResult._jResult(result);
        }

        #endregion

        #region 入驻公司


        /// <summary>
        /// 公司列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CompanyModel> GetCompanyPageList(CompanyQueryModel query)
        {
            return DataAccess.GetCompanyPageList(query);
        }

        /// <summary>
        /// 获取公司详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCompanyById(string innerid)
        {
            var model = DataAccess.GetCompanyById(innerid);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 企业评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoComment(CommentModel model)
        {
            var chkresult = DataAccess.CheckComment(model.Mobile, model.Companyid);

            if (chkresult > 0)
            {
                return JResult._jResult(402,"不能重复评论");
            }

            var custModel = DataAccess.GetCustByMobile(model.Mobile);
            if (custModel != null)
            {
                //设置会员头像
                if (!string.IsNullOrWhiteSpace(custModel.Headportrait))
                {
                    model.Headportrait = custModel.Headportrait;
                }
                else
                {
                    //设置会员的微信头像
                    if (!string.IsNullOrWhiteSpace(custModel.Wechat?.Photo))
                    {
                        model.Headportrait = custModel.Wechat?.Photo;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(model.Headportrait))
            {
                //随机头像
                var randomNumber = new Random(Guid.NewGuid().GetHashCode()).Next(1, 278);
                model.Headportrait = string.Concat("commentheadportrait_", randomNumber, ".jpg");
                
                #region 修改文件名

                //var num = 1;
                //var folder = new DirectoryInfo(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile"));
                //foreach (var item in folder.GetFiles())
                //{
                //    var file = "";
                //    if (num < 10)
                //    {
                //        file = "00" + num;
                //    }
                //    else if (num >= 10 && num < 100)
                //    {
                //        file = "0" + num;
                //    }
                //    else
                //    {
                //        file = num.ToString();
                //    }
                //    File.Move(folder + "\\" + item.Name, folder + "\\" + "commentheadportrait_" + file + ".jpg");
                //    num ++;
                //}

                #endregion
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.DoComment(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 企业点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult DoPraise(PraiseModel model)
        {
            var chkresult = DataAccess.CheckPraise(model.IP, model.Companyid);

            if (chkresult > 0)
            {
                return JResult._jResult(402, "不能重复点赞");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.DoPraise(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CommentListModel> GetCommentPageList(CommentQueryModel query)
        {
            return DataAccess.GetCommentPageList(query);
        }

        
        /// <summary>
        /// 导入公司
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public JResult ImportCompany(string file)
        {
            file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile\\", file);
            var dt = ExcelHelper.ReadExcelToDataSet(file,true)?.Tables[0];
            var result = 0;
            if (!(dt?.Rows.Count > 0))
                return JResult._jResult(result);

            //dt = ClearEmpty(dt);
            result = DataAccess.ImportCompany(dt);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 导入公司
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable ClearEmpty(DataTable dt)
        {
            var resultDt = new DataTable();
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                resultDt.Columns.Add(dt.Columns[i].ColumnName);//有重载的方法，可以加入列数据的类型
            }
            foreach (var dr in dt.Rows.Cast<DataRow>().Where(dr => dr["CompanyName"].ToString().Trim().Length > 0))
            {
                resultDt.ImportRow(dr);
            }
            return resultDt;
        }

        #endregion
    }
}