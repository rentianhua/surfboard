#region

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CCN.Modules.Customer.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.BaseClasses;
using Dapper;
using MySql.Data.MySqlClient;

#endregion

namespace CCN.Modules.Customer.DataAccess
{
    /// <summary>
    /// </summary>
    public class CustomerDA : CustomerDataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerDA()
        {

        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetALlCustomers()
        {
            var d = Helper.Query("select * from base_carbrand where id=@id", new { id = "" }).ToList();
            return d;
        }


        #region 用户模块

        /// <summary>
        /// 会员注册检查用户名是否被注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckUserName(string username)
        {
            const string sql = @"select count(1) as count from `cust_info` where username=@username;";
            var result = Helper.Execute<int>(sql, new { username });
            return result;
        }

        /// <summary>
        /// 会员注册检查手机号是否被注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>0：未被注册，非0：用户名被注册</returns>
        public int CheckMobile(string mobile)
        {
            const string sql = @"select count(1) as count from `cust_info` where mobile=@mobile;";
            var result = Helper.Execute<int>(sql, new { mobile });
            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public int CustRegister(CustModel userInfo)
        {
            var result = 0;
            //插入账户基本信息
            const string sql = @"INSERT INTO `cust_info`(`innerid`,`username`,`password`,`mobile`,`telephone`,`email`,`headportrait`,qrcode,`status`,authstatus,`type`,`realname`,`totalpoints`,`level`,`createdtime`,`modifiedtime`)
                        VALUES (@innerid,@username,@password,@mobile,@telephone,@email,@headportrait,@qrcode,@status,@authstatus,@type,@realname,@totalpoints,@level,@createdtime,@modifiedtime);";
            
            try
            {
                Helper.Execute(sql, userInfo);

                //插入微信信息
                if (userInfo.Wechat != null)
                {
                    const string sqlwechat = @"INSERT INTO cust_wechat(`innerid`,`custid`,`accountid`,`openid`,`nickname`,`headportrait`,`sex`,`country`,`province`,`city`,`createdtime`,`modifiedtime`)
                        VALUES(uuid(),@custid,@accountid,@openid,@nickname,@headportrait,@sex,@country,@province,@city,@createdtime,@modifiedtime);";
                    Helper.Execute(sqlwechat, userInfo.Wechat);
                }
            }
            catch (Exception ex)
            {
                result = 401;
            }

            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录账户</param>
        /// <returns>用户信息</returns>
        public CustModel CustLogin(CustLoginInfo loginInfo)
        {
            const string sql = "select * from `cust_info` where (username=@username or mobile=@mobile) and password=@password;";
            var custModel = Helper.Query<CustModel>(sql, new
            {
                username = loginInfo.Username,
                mobile = loginInfo.Mobile,
                password = loginInfo.Password
            }).FirstOrDefault();

            //获取微信信息
            if (custModel != null)
            {
                custModel.Wechat = Helper.Query<CustWechat>("select * from cust_wechat where custid=@custid;", new
                {
                    custid = custModel.Innerid
                }).FirstOrDefault();
            }

            return custModel;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="qrcode"></param>
        /// <returns>用户信息</returns>
        public int UpdateQrCode(string innerid,string qrcode)
        {
            const string sql = "update cust_info set qrcode=@qrcode where innerid=@innerid;";
            var custModel = Helper.Execute(sql, new {qrcode, innerid});
            return custModel;
        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="innerid">会员id</param>
        /// <returns></returns>
        public CustModel GetCustById(string innerid)
        {
            const string sql = "select * from cust_info where innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CustModel>(sql, new {innerid}).FirstOrDefault();
                //获取微信信息
                if (custModel != null)
                {
                    custModel.Wechat = Helper.Query<CustWechat>("select * from cust_wechat where custid=@custid;", new
                    {
                        custid = innerid
                    }).FirstOrDefault();
                }
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustModel> GetCustPageList(CustQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_info";
            const string fields = "innerid, username, mobile, telephone, email, headportrait, status, authstatus, autherid, authtime, authdesc, type, realname, totalpoints, level, createdtime, modifiedtime";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Status != null
                ? $" and status={query.Status}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and mobile like '%{query.Mobile}%'");
            }
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustModel>(model, query.Echo);
            return list;
        }

        #endregion

        #region 用户认证

        /// <summary>
        /// 用户添加认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int AddAuthentication(CustAuthenticationModel model)
        {
            const string sqlU = "update cust_info set authstatus=1 where innerid=@innerid;";
            const string sqlI = @"INSERT INTO `cust_authentication`
                                (`innerid`,`custid`,`idcard`,`idname`,`idpicture`,`company`,`legalperson`,`organizationcode`,`organizationpicture`,`createdtime`,`modifiedtime`)
                                VALUES
                                (uuid(),@custid,@idcard,@idname,@idpicture,@company,@legalperson,@organizationcode,@organizationpicture,@createdtime,@modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlU, new {innerid = model.Custid}, tran);
                    conn.Execute(sqlI, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            } 
        }

        /// <summary>
        /// 用户修改认证信息
        /// </summary>
        /// <param name="model">认证信息</param>
        /// <returns></returns>
        public int UpdateAuthentication(CustAuthenticationModel model)
        {
            var sqlStr = new StringBuilder("update `cust_authentication` set ");
            sqlStr.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sqlStr.Append(" where innerid = @innerid");

            const string sqlU = "update cust_info set authstatus=1 where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlU, new { innerid = model.Custid }, tran);
                    conn.Execute(sqlStr.ToString(), model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="info">会员相关信息</param>
        /// <param name="operid">操作人id</param>
        /// <returns></returns>
        public int AuditAuthentication(CustModel info,string operid)
        {
            const string sql = "update cust_info set authstatus=@authstatus,autherid=@autherid,authdesc=@authdesc,authtime=@authtime where innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new
                {
                    innerid = info.Innerid,
                    authstatus = info.AuthStatus,
                    autherid = operid,
                    authtime = info.AuthTime,
                    authdesc = info.AuthDesc
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取会员认证信息 by innerid
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public CustAuthenticationModel GetCustAuthById(string innerid)
        {
            const string sql = "select * from cust_authentication where innerid=@innerid;";

            try
            {
                var custModel = Helper.Query<CustAuthenticationModel>(sql, new { innerid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员认证信息 by custid
        /// </summary>
        /// <param name="custid">会员id</param>
        /// <returns></returns>
        public CustAuthenticationModel GetCustAuthByCustid(string custid)
        {
            const string sql = "select * from cust_authentication where custid=@custid;";

            try
            {
                var custModel = Helper.Query<CustAuthenticationModel>(sql, new { custid }).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 会员标签


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public int AddTag(CustTagModel model)
        {
            const string sql = @"INSERT INTO cust_tag(innerid, tagname, hotcount, isenabled, createdtime, modifiedtime) VALUES (uuid(), @tagname, @hotcount, @isenabled, @createdtime, @modifiedtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="model">标签信息</param>
        /// <returns></returns>
        public int UpdateTag(CustTagModel model)
        {
            const string sql = @"UPDATE cust_tag SET tagname = @tagname,isenabled = @isenabled,modifiedtime = @modifiedtime WHERE innerid = @innerid;";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public int DeleteTag(string innerid)
        {
            const string sql = @"delete from cust_tag WHERE innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取标签详情
        /// </summary>
        /// <param name="innerid">标签id</param>
        /// <returns></returns>
        public CustTagModel GetTagById(string innerid)
        {
            const string sql = @"select innerid, tagname, hotcount, isenabled, createdtime, modifiedtime from cust_tag WHERE innerid=@innerid;";

            try
            {
                return Helper.Query<CustTagModel>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustTagModel> GetTagPageList(CustTagQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_tag";
            const string fields = "innerid, tagname, hotcount, isenabled, createdtime, modifiedtime";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.Isenabled != null
                ? $" and status={query.Isenabled}"
                : "");

            if (!string.IsNullOrWhiteSpace(query.Tagname))
            {
                sqlWhere.Append($" and tagname like '%{query.Tagname}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustTagModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 打标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DoTag(CustTagRelation model)
        {
            const string sql = @"INSERT INTO cust_tag_relation(innerid,tagid,fromid,toid,createdtime) VALUES (@innerid,@tagid,@fromid,@toid,@createdtime);";
            const string sqlH = @"UPDATE cust_tag SET hotcount=@hotcount WHERE innerid = @innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    conn.Execute(sql, model, tran);
                    
                    
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        #endregion
    }
}