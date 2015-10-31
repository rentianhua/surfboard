#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CCN.Modules.CustRelations.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.BaseClasses;
using Dapper;
using MySql.Data.MySqlClient;

#endregion

namespace CCN.Modules.CustRelations.DataAccess
{
    /// <summary>
    /// </summary>
    public class CustRelationsDA : CustRelationsDataAccessBase
    {

        #region 好友申请

        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustViewModel> GetCustPageList(CustQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_info as a 
                                        left join cust_wechat as c on a.innerid=c.custid
                                        left join wechat_friend as d on d.openid=c.openid";
            var fields = $"a.innerid,a.custname,a.mobile,a.`level`,a.headportrait,d.photo ,(select count(1) from car_info where custid=a.innerid) as carnum,(select count(1) from cust_relations where userid='{query.Oneselfid}' and frientsid=a.innerid) as isfriends";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.custname asc,a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            //手机号查询
            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and a.mobile like '%{query.Mobile}%'");
            }

            //会员名查询
            if (!string.IsNullOrWhiteSpace(query.Custname))
            {
                sqlWhere.Append($" and a.custname like '%{query.Custname}%'");
            }

            //email查询
            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                sqlWhere.Append($" and a.email like '%{query.Email}%'");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取加好友申请
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustRelationsApplyViewModels> GetCustRelationsPageList(CustRelationsApplyQueryModels query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"cust_relations_apply as a 
                                        inner join cust_info as b on a.fromid=b.innerid 
                                        left join cust_wechat as c on a.fromid=c.custid
                                        left join wechat_friend as d on d.openid=c.openid
                                        left join base_code as bc1 on a.sourceid=bc1.codevalue and bc1.typekey='cust_source'";
            const string fields = " a.innerid ,a.fromid as Frientsid,a.status as ApplyStatus,bc1.codename as Source,b.custname, b.mobile, b.email, b.headportrait, b.`status`, b.authstatus, b.brithday, b.totalpoints, b.`level`, b.createdtime,d.photo ,(select count(1) from car_info where custid=a.fromid) as carnum ";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.Toid))
            {
                sqlWhere.Append($" and a.toid='{query.Toid}'");
            }

            sqlWhere.Append(query.Status != null
                ? $" and a.status={query.Status}"
                : "");
            
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustRelationsApplyViewModels>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 根据请求id获取申请信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CustRelationsApplyModels GetRelationsApplyById(string innerid)
        {
            const string sql = "select * from cust_relations_apply where innerid=@innerid;";
            try
            {
                return Helper.Query<CustRelationsApplyModels>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据请求id获取申请信息
        /// </summary>
        /// <param name="fromid"></param>
        /// <param name="toid"></param>
        /// <returns></returns>
        public int CheckRelationsApply(string fromid, string toid)
        {
            const string sql = "select count(1) as count from cust_relations_apply where fromid=@fromid and toid=@toid and `status`=0;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new {fromid, toid});
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 检查是否已建立关系
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="frientsid"></param>
        /// <returns></returns>
        public int CheckRelations(string userid, string frientsid)
        {
            const string sql = "select count(1) as count from cust_relations where userid=@userid and frientsid=@frientsid;";
            try
            {
                return Helper.ExecuteScalar<int>(sql, new { userid, frientsid });
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRelationsApply(CustRelationsApplyModels model)
        {
            int result;
            const string sql = "insert into cust_relations_apply (innerid, fromid, toid, status, createdtime, modifiedtime, remark, sourceid) values (uuid(), @fromid, @toid, @status, @createdtime, @modifiedtime, @remark, @sourceid);";
            try
            {
                result = Helper.Execute(sql, model);
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 修改好友申请By id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateRelationsApplyById(CustRelationsApplyModels model)
        {
            int result;
            const string sql = "update cust_relations_apply set remark=@remark, sourceid=@sourceid where innerid=@innerid;";
            try
            {
                result = Helper.Execute(sql, model);
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 修改好友申请[重复申请] by fromid and toid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateRelationsApply(CustRelationsApplyModels model)
        {
            int result;
            const string sql = "update cust_relations_apply set remark=@remark, sourceid=@sourceid where fromid=@fromid and toid=@toid and `status`=0;";
            try
            {
                result = Helper.Execute(sql, model);
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 删除好友的申请
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteApplyById(string innerid)
        {
            const string sql = "delete from cust_relations_apply where innerid=@innerid;";
            try
            {
                Helper.Execute(sql, new {innerid});
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        public int HandleRelationsApply(string innerid, int status,string fromid ,string toid)
        {
            var result = 1;
            const string sqlUpdate = "update cust_relations_apply set `status`=@status where innerid=@innerid;";
            const string sqlInsert = "insert into cust_relations (innerid, userid, frientsid, createdtime) values (uuid(), @userid, @frientsid, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlUpdate, new {innerid, status}, tran);

                    //接受
                    if (status == 1)
                    {
                        const string sqlR = "select count(1) as count from cust_relations where userid=@userid and frientsid=@frientsid;";
                        var isR = conn.Query<int>(sqlR, new {userid = fromid, frientsid = toid}).FirstOrDefault();
                        if (isR == 0)
                        {
                            conn.Execute(sqlInsert, new { userid = fromid, frientsid = toid, createdtime = DateTime.Now }, tran);
                            conn.Execute(sqlInsert, new { userid = toid, frientsid = fromid, createdtime = DateTime.Now }, tran);
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 删除好友关系
        /// </summary>
        /// <param name="fromid"></param>
        /// <param name="toid"></param>
        /// <returns></returns>
        public int DeleteRelations(string fromid, string toid)
        {
            const string sql2 =
                "delete from cust_relations where (userid=@fromid and frientsid=@toid) or (userid=@toid and frientsid=@fromid);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql2, new {fromid, toid}, tran);
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
        /// 获取好友列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public IEnumerable<CustRelationsViewModel> GetCustRelationsByUserId(string userid)
        {
            const string sql = @"select frientsid ,b.custname, b.mobile, b.email, b.headportrait, b.`status`, b.authstatus, b.brithday, b.totalpoints, b.`level`, b.createdtime,d.photo ,(select count(1) from car_info where custid=a.frientsid) as carnum
                                from cust_relations as a
                                inner join cust_info as b on a.frientsid=b.innerid 
                                left join cust_wechat as c on a.frientsid=c.custid
                                left join wechat_friend as d on d.openid=c.openid
                                where a.userid=@userid order by b.custname asc;";
            try
            {
                return Helper.Query<CustRelationsViewModel>(sql, new { userid });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 社交圈

        /// <summary>
        /// 社交圈搜车
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustRelationsCarViewModel> GetHaveCarCustList(CustRelationsCarQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    inner join cust_info as b on a.custid=b.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join cust_wechat as c on a.custid=c.custid
                                    left join wechat_friend as d on d.openid=c.openid
                                    left join base_city as ct on b.cityid=ct.innerid ";
            const string fields = "b.innerid, b.custname, b.mobile, b.email, b.headportrait,d.photo ,c3.modelname as model_name,ct.cityname as custcityname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("1=1");

            if (query.model_id != null)
            {
                sqlWhere.Append($" and a.model_id='{query.model_id}'");
            }
            //会员所在省份
            if (query.custprovid != null)
            {
                sqlWhere.Append($" and b.provid={query.custprovid}");
            }
            //会员所在城市
            if (query.cuscityid != null)
            {
                sqlWhere.Append($" and b.cityid={query.cuscityid}");
            }
            
            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CustRelationsCarViewModel>(model, query.Echo);
            return list;
        }

        #endregion
    }
}