using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CCN.Modules.Activity.BusinessEntity;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Dapper;

namespace CCN.Modules.Activity.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityDataAccess : DataAccessBase
    {
        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VoteListModel> GetVoteList(VoteQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"vote_info as a ";
            const string fields = "innerid, title, enrollstarttime, enrollendtime, votestarttime, voteendtime,createdtime,(select count(1) from vote_per where voteid=a.innerid) as numper,(select count(1) from vote_log where voteid=a.innerid) as numvote";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder(" 1=1 ");
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<VoteListModel>(model, query.Echo);
            return list;
        }
        
        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteViewModel GetVoteViewById(string id)
        {
            const string sql =
                @"SELECT innerid, title, enrollstarttime, enrollendtime, votestarttime, voteendtime, votemode, voteflow, prizedeac, attention, createrid, createdtime, modifierid, modifiedtime, 
(select count(1) from vote_per where voteid=a.innerid) as numper,
(select count(1) from vote_log where voteid=a.innerid) as numvote FROM vote_info as a where innerid=@innerid";
            var model = Helper.Query<VoteViewModel>(sql, new {innerid = id}).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取投票活动详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteModel GetVoteInfoById(string id)
        {
            const string sql =
                @"SELECT innerid, title, enrollstarttime, enrollendtime, votestarttime, voteendtime, votemode, voteflow, prizedeac, attention, createrid, createdtime, modifierid, modifiedtime FROM vote_info as a where innerid=@innerid";
            var model = Helper.Query<VoteModel>(sql, new { innerid = id }).FirstOrDefault();
            return model;
        }

        #endregion

        #region 投票活动参赛人员
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VotePerListModel> GetVotePerList(VotePerQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"vote_per as a ";
            const string fields = "innerid, voteid, fullname, num, picture, mobile, ip, openid, createrid, createdtime, modifierid, modifiedtime,(select count(1) from vote_log where perid=a.innerid) as votenum";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder($" a.voteid='{query.Voteid}' ");

            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and mobile like '%{query.Mobile}%'");
            }

            if (query.Num != null)
            {
                sqlWhere.Append($" and num={query.Num}");
            }

            if (!string.IsNullOrWhiteSpace(query.Fullname))
            {
                sqlWhere.Append($" and fullname like '%{query.Fullname}%'");
            }

            if (!string.IsNullOrWhiteSpace(query.Openid))
            {
                sqlWhere.Append($" and openid like '%{query.Openid}%'");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<VotePerListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VotePerViewModel GetVotePerViewById(string id)
        {
            const string sql =
                @"SELECT innerid, voteid, fullname, num, picture, files, mobile, ip, remark, openid, createrid, createdtime, modifierid, modifiedtime,(select count(1) from vote_log where perid=a.innerid) as votenum FROM vote_per as a where innerid=@innerid";
            var model = Helper.Query<VotePerViewModel>(sql, new { innerid = id }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VotePerModel GetVotePerInfoById(string id)
        {
            const string sql =
                @"SELECT innerid, voteid, fullname, num, picture, files, mobile, ip, remark, openid, createrid, createdtime, modifierid, modifiedtime FROM vote_per as a where innerid=@innerid";
            var model = Helper.Query<VotePerModel>(sql, new { innerid = id }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddVotePer(VotePerModel model)
        {
            const string sql = @"INSERT INTO vote_per
                                (innerid, voteid, fullname, picture, files, mobile, ip, remark, openid, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @voteid, @fullname, @picture, @files, @mobile, @ip, @remark, @openid, @createrid, @createdtime, @modifierid, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var voteModel =
                        conn.Query<VoteModel>(
                            "SELECT enrollstarttime, enrollendtime FROM vote_info where innerid = @innerid;",
                            new {innerid = model.Voteid}).FirstOrDefault();

                    var nowTime = DateTime.Now;
                    if (nowTime > voteModel?.Enrollendtime || nowTime < voteModel?.Enrollstarttime)
                    {
                        return -1;
                    }

                    var n =
                        conn.Query<int>(
                            "select count(1) from vote_per where voteid=@voteid and openid=@openid;",new
                            {
                                voteid = model.Voteid,
                                openid = model.Openid
                            })
                            .FirstOrDefault();

                    if (n > 0)
                    {
                        return -2;
                    }

                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("参赛异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 获取参赛者排名
        /// </summary>
        /// <param name="voteid"></param>
        /// <param name="votenum"></param>
        /// <returns></returns>
        public int GetVotePerRanking(string voteid, int votenum)
        {
            const string sql = @"select count(1) from vote_per as a where voteid=@voteid and (select count(1) from vote_log where perid=a.innerid and voteid=a.voteid)>@votenum;";
            //const string sql = @"select count(1) from (select count(1),perid from vote_log where voteid=@voteid group by perid having count(1) > @votenum) as tt;";
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Query<int>(sql, new { voteid, votenum }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("GetVotePerRanking异常：", TraceEventType.Information, ex);
                    result = -1;
                }

                return result;
            }
        }

        /// <summary>
        /// 获取参赛者总人数
        /// </summary>
        /// <param name="voteid"></param>
        /// <returns></returns>
        public int GetVotePerTotal(string voteid)
        {
            const string sql = @"select count(1) from vote_per as a where voteid=@voteid;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Query<int>(sql, new { voteid }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("GetVotePerTotal异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        #endregion

        #region 投票日志

        /// <summary>
        /// 获取参赛人的投票列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VoteLogListModel> GetVoteLogList(VoteLogQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"vote_log as a ";
            const string fields = "innerid, voteid, perid, ip, openid, createdtime";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder($" perid='{query.Perid}' ");

            if (!string.IsNullOrWhiteSpace(query.Voteid))
            {
                sqlWhere.Append($" and voteid='{query.Voteid}'");
            }            

            if (!string.IsNullOrWhiteSpace(query.Openid))
            {
                sqlWhere.Append($" and openid like '{query.Openid}'");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<VoteLogListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddVoteLog(VoteLogModel model)
        {
            const string sql = @"INSERT INTO vote_log (innerid, voteid, perid, ip, openid, createdtime) VALUES (@innerid, @voteid, @perid, @ip, @openid, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var voteModel =
                        conn.Query<VoteModel>(
                            "SELECT votestarttime, voteendtime FROM vote_info where innerid = @innerid;",
                            new { innerid = model.Voteid }).FirstOrDefault();

                    var nowTime = DateTime.Now;
                    if (nowTime > voteModel?.Voteendtime || nowTime < voteModel?.Votestarttime)
                    {
                        return -1;
                    }

                    var n =
                        conn.Query<int>("select count(1) from vote_log where voteid=@voteid and openid=@openid;",
                            new {voteid = model.Voteid, openid = model.Openid}).FirstOrDefault();
                    if (n > 0)
                    {
                        return -2;
                    }
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("投票异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 作弊投票
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public int AddVoteLog(VoteLogModel model, int number)
        {
            const string sql = @"INSERT INTO vote_log (innerid, voteid, perid, ip, openid, createdtime) VALUES (@innerid, @voteid, @perid, @ip, @openid, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                var result = 0;
                try
                {
                    for (var i = 0; i < number; i++)
                    {
                        result = conn.Execute(sql, new
                        {
                            innerid = Guid.NewGuid().ToString(),
                            voteid = model.Voteid,
                            perid = model.Perid,
                            ip = model.IP,
                            openid = model.Openid,
                            createdtime = model.Createdtime
                        });
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("投票异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        #endregion
    }
}