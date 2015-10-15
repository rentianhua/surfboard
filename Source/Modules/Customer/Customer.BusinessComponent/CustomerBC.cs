#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters;
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
            var mYan = DataAccess.CheckMobile(userInfo.Mobile);
            if (mYan > 0)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "手机号被其他人注册"
                };
            }

            //生成会员名称
            userInfo.Custname = string.Concat("ccn_", DateTime.Now.Year, "_",
                userInfo.Mobile.Substring(userInfo.Mobile.Length - 6));

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
                    var filename = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile\\", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".jpg");
                    var website = ConfigHelper.GetAppSettings("website");
                    var bitmap = BarCodeUtility.CreateBarcode(website + "?innerid=" + userInfo.Innerid, 240, 240);

                    //保存图片到临时文件夹
                    bitmap.Save(filename);

                    //上传图片到七牛云
                    var qinniu = new QiniuUtility();
                    var qrcodeKey = qinniu.PutFile(filename);
                    
                    //删除本地临时文件
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    
                    //上传成功更新会员二维码
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

            #region 注册送积分

            Task.Factory.StartNew(() =>
            {
                DataAccess.ChangePoint(new CustPointModel()
                {
                    Custid = innerid,
                    Createdtime = userInfo.Createdtime,
                    Type = 1,
                    Innerid = Guid.NewGuid().ToString(),
                    Point = 10,
                    Remark = "",
                    Sourceid = 1,
                    Validtime = null
                });
            });

            #endregion

            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "注册成功" : "注册失败"
            };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public JResult CustLogin(CustLoginInfo loginInfo)
        {
            var result = new JResult();
            if (string.IsNullOrWhiteSpace(loginInfo.Mobile))
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

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mRetrievePassword"></param>
        /// <returns></returns>
        public JResult UpdatePassword(CustRetrievePassword mRetrievePassword)
        {
            //密码加密
            var en = new Encryptor();
            mRetrievePassword.NewPassword = en.EncryptMd5(mRetrievePassword.NewPassword);
            var result = DataAccess.UpdatePassword(mRetrievePassword);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "修改成功" : "修改失败"
            };
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
                errcode = result > 0 ?  0 : 400,
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

        #region 会员积分

        /// <summary>
        /// 会员积分变更
        /// </summary>
        /// <param name="model">变更信息</param>
        /// <returns></returns>
        public JResult ChangePoint(CustPointModel model)
        {
            if (model.Point == 0)
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "变更的积分不能为0"
                };
            }
            model.Innerid = Guid.NewGuid().ToString();

            if (model.Type == 0)
            {
                model.Type = 1;
            }

            var result = DataAccess.ChangePoint(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "添加成功" : "添加失败"
            };
        }

        /// <summary>
        /// 获取会员积分记录列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustPointViewModel> GetCustPointLogPageList(CustPointQueryModel query)
        {
            var list = DataAccess.GetCustPointLogPageList(query);
            return list;
        }

        /// <summary>
        /// 积分兑换礼券
        /// </summary>
        /// <param name="model">兑换相关信息</param>
        /// <returns></returns>
        public JResult PointExchangeCoupon(CustPointExChangeCouponModel model)
        {
            if (model == null)
            {
                return _jResult(401, "参数不正确");
            }
            if (string.IsNullOrWhiteSpace(model.Custid))
            {
                return _jResult(402, "会员不存在");
            }
            if (model.Point == 0)
            {
                return _jResult(403, "积分不够");
            }
            if (string.IsNullOrWhiteSpace(model.Cardid))
            {
                return _jResult(404, "礼券不存在");
            }

            //生成随机数
            model.Code = RandomUtility.GetRandomCode();
            //生成二维码位图
            var bitmap = BarCodeUtility.CreateBarcode(model.Code, 240, 240);

            //保存二维码图片到临时文件夹
            var filename = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile\\", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".jpg");
            bitmap.Save(filename);

            //上传图片到七牛云
            var qinniu = new QiniuUtility();
            model.QrCode = qinniu.PutFile(filename);

            //删除本地临时文件
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            //开始兑换
            model.Createdtime = DateTime.Now;
            var result = DataAccess.PointExchangeCoupon(model);
            return _jResult(
                result > 0 ? 0 : 400, 
                result > 0 ? "兑换成功" : "兑换失败");
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        private static JResult _jResult(int errcode, object errmsg)
        {
            return new JResult
            {
                errcode = errcode,
                errmsg = errmsg
            };
        }
    }
}