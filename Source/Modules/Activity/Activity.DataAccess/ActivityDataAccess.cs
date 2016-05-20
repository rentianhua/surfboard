using System;
using System.Collections.Generic;
using System.Data;
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

        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VoteListModel> GetVoteList(VoteQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"activity_vote_info as a ";
            const string fields = "innerid, title, enrollstarttime, enrollendtime, votestarttime, voteendtime,createdtime,(select count(1) from activity_vote_per where voteid=a.innerid) as numper,(select count(1) from activity_vote_log where voteid=a.innerid) as numvote";
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
(select count(1) from activity_vote_per where voteid=a.innerid) as numper,
(select count(1) from activity_vote_log where voteid=a.innerid) as numvote FROM activity_vote_info as a where innerid=@innerid";
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
                @"SELECT innerid, title, enrollstarttime, enrollendtime, votestarttime, voteendtime, votemode, voteflow, prizedeac, attention, createrid, createdtime, modifierid, modifiedtime FROM activity_vote_info as a where innerid=@innerid";
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
            const string tableName = @"activity_vote_per as a ";
            const string fields = "innerid, voteid, fullname, num, picture, mobile, ip, openid, createrid, createdtime, modifierid, modifiedtime,(select count(1) from activity_vote_log where perid=a.innerid) as votenum";
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
                @"SELECT innerid, voteid, fullname, num, picture, files, mobile, ip, remark, openid, createrid, createdtime, modifierid, modifiedtime,(select count(1) from activity_vote_log where perid=a.innerid) as votenum FROM activity_vote_per as a where innerid=@innerid";
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
                @"SELECT innerid, voteid, fullname, num, picture, files, mobile, ip, remark, openid, createrid, createdtime, modifierid, modifiedtime FROM activity_vote_per as a where innerid=@innerid";
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
            const string sql = @"INSERT INTO activity_vote_per
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
                            "SELECT enrollstarttime, enrollendtime FROM activity_vote_info where innerid = @innerid;",
                            new {innerid = model.Voteid}).FirstOrDefault();

                    var nowTime = DateTime.Now;
                    if (nowTime > voteModel?.Enrollendtime || nowTime < voteModel?.Enrollstarttime)
                    {
                        return -1;
                    }

                    var n =
                        conn.Query<int>(
                            "select count(1) from activity_vote_per where voteid=@voteid and openid=@openid;",new
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
                    LoggerFactories.CreateLogger().Write("参赛异常：", TraceEventType.Error, ex);
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
            const string sql = @"select count(1) from activity_vote_per as a where voteid=@voteid and (select count(1) from activity_vote_log where perid=a.innerid and voteid=a.voteid)>@votenum;";
            //const string sql = @"select count(1) from (select count(1),perid from activity_vote_log where voteid=@voteid group by perid having count(1) > @votenum) as tt;";
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Query<int>(sql, new { voteid, votenum }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("GetVotePerRanking异常：", TraceEventType.Error, ex);
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
            const string sql = @"select count(1) from activity_vote_per as a where voteid=@voteid;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Query<int>(sql, new { voteid }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("GetVotePerTotal异常：", TraceEventType.Error, ex);
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
            const string tableName = @"activity_vote_log as a ";
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
            const string sql = @"INSERT INTO activity_vote_log (innerid, voteid, perid, ip, openid, createdtime) VALUES (@innerid, @voteid, @perid, @ip, @openid, @createdtime);";
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var voteModel =
                        conn.Query<VoteModel>(
                            "SELECT votestarttime, voteendtime FROM activity_vote_info where innerid = @innerid;",
                            new { innerid = model.Voteid }).FirstOrDefault();

                    var nowTime = DateTime.Now;
                    if (nowTime > voteModel?.Voteendtime || nowTime < voteModel?.Votestarttime)
                    {
                        return -1;   //不在时间范围内
                    }

                    var loglist =
                        conn.Query<VoteLogModel>("select perid from activity_vote_log where voteid=@voteid and openid=@openid;",
                            new {voteid = model.Voteid, openid = model.Openid}).ToList();

                    var votenum = ConfigHelper.GetAppSettings("votenum");
                    var num = 3;

                    if (!string.IsNullOrWhiteSpace(votenum))
                    {
                        int.TryParse(votenum, out num);
                    }

                    if (loglist.Count >= num)
                    {
                        return -2;  //3次机会用完
                    }

                    if (loglist.Any(x => x.Perid == model.Perid))
                    {
                        return -3;  //不能重复投同一个人
                    }

                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("投票异常：", TraceEventType.Error, ex);
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
            const string sql = @"INSERT INTO activity_activity_vote_log (innerid, voteid, perid, ip, openid, createdtime) VALUES (@innerid, @voteid, @perid, @ip, @openid, @createdtime);";
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
                    LoggerFactories.CreateLogger().Write("投票异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 取消订阅操作
        /// </summary>
        /// <returns></returns>
        public int UnSubscribe(string appid, string openid)
        {
            const string sql = @"update activity_vote_log set invalid=1,modifiedtime=@modifiedtime where openid=@openid;";
            using (var conn = Helper.GetConnection())
            {
                var result = 0;
                try
                {
                    result = conn.Execute(sql, new
                    {
                        openid,
                        modifiedtime = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("取消订阅操作异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        #endregion

        #endregion

        #region 众筹活动

        #region 活动管理

        /// <summary>
        /// 开始抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int StartDraw(StartDrawModel model)
        {
            const string sqlA = @"update activity_crow_info set `status`=4 where flagcode=@flagcode;";
            const string sqlB = @"update activity_crow_player set `iswinning`=1 where flagcode=@flagcode and openid=@openid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlA, new { flagcode = model.Flagcode }, tran);

                    foreach (var item in model.Openids.Split(','))
                    {
                        conn.Execute(sqlB, new { flagcode = model.Flagcode, openid = item }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("开始抽奖记录异常：", TraceEventType.Error, ex);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 结束抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int EndDraw(StartDrawModel model)
        {
            const string sqlA = @"update activity_crow_info set `status`=5 where flagcode=@flagcode;";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sqlA, new { flagcode = model.Flagcode });
                    return 1;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("结束抽奖记录异常：", TraceEventType.Error, ex);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 重置抽奖
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int ResetDraw(string innerid)
        {
            const string sqlA = @"update activity_crow_info set `status`=3 where innerid=@innerid;";
            const string sqlB = @"update activity_crow_player set `iswinning`=0 where flagcode=@flagcode;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    var activityModel = GetCrowdInfoById(innerid);
                    conn.Execute(sqlA, new {innerid}, tran);
                    conn.Execute(sqlB, new {flagcode = activityModel.Flagcode}, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("结束抽奖记录异常：", TraceEventType.Error, ex);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList(CrowdInfoQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"activity_crow_info as a ";
            const string fields = @"innerid,flagcode, title, enrollstarttime, enrollendtime, secrettime, status, type, qrcode, createrid, createdtime,
                (select count(1) from activity_crow_player where flagcode=a.flagcode and isenabled=1) as playernum,
                (select sum(totalfee) from activity_crow_payrecord where flagcode=a.flagcode and ispay=1) as upperedtotal";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("");

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CrowdInfoListModel>(model, query.Echo);
            return list;
        }
        
        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CrowdInfoModel GetCrowdInfoById(string innerid)
        {
            const string sql =
                @"SELECT innerid, title, subtitle, enrollstarttime, enrollendtime, secrettime,uppertotal,uppereach, prize, status, type, qrcode, remark, extend, createrid, createdtime, modifierid, modifiedtime FROM activity_crow_info where innerid=@innerid";
            var model = Helper.Query<CrowdInfoModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取活动详情 flagcode
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public CrowdInfoModel GetCrowdInfoByFlagcode(string flagcode)
        {
            const string sql =
                @"SELECT innerid, title, enrollstarttime, enrollendtime, secrettime, uppertotal, uppereach, status, qrcode, prize FROM activity_crow_info where flagcode=@flagcode";
            var model = Helper.Query<CrowdInfoModel>(sql, new { flagcode }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public CrowdViewModel GetCrowdViewById(string flagcode)
        {
            const string sql =
                @"SELECT innerid, flagcode, title, subtitle, enrollstarttime, enrollendtime, secrettime, uppertotal, uppereach, prize, `status`, `type`, qrcode, remark, extend, 
                (select count(1) from activity_crow_player where flagcode=a.flagcode and isenabled=1) as playernum, 
                (select sum(totalfee) from activity_crow_payrecord where flagcode=a.flagcode and ispay=1) as upperedtotal FROM activity_crow_info as a where a.flagcode=@flagcode";
            var model = Helper.Query<CrowdViewModel>(sql, new { flagcode }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public CrowdProgressModel GetCrowdProgressByFlagcode(string flagcode)
        {
            const string sql =
                "select count(1) as playernum ,(select sum(totalfee) from activity_crow_payrecord where flagcode = @flagcode and ispay = 1) as upperedtotal from activity_crow_player where flagcode = @flagcode and isenabled = 1;";
            var model = Helper.Query<CrowdProgressModel>(sql, new { flagcode }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCrowdInfo(CrowdInfoModel model)
        {
            const string sql = @"INSERT INTO activity_crow_info
                                (innerid, title, subtitle, enrollstarttime, enrollendtime, secrettime,uppertotal,uppereach, prize, status, type,flagcode, qrcode, remark, extend, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @title, @subtitle, @enrollstarttime, @enrollendtime, @secrettime,@uppertotal,@uppereach, @prize, @status, @type,@flagcode, @qrcode, @remark, @extend, @createrid, @createdtime, @modifierid, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    //生成编号
                    var obj = new
                    {
                        p_tablename = "activity_crow_info",
                        p_columnname = "flagcode",
                        p_prefix = "A",
                        p_length = 4,
                        p_hasdate = 0
                    };

                    var args = new DynamicParameters(obj);
                    args.Add("p_value", dbType: DbType.String, direction: ParameterDirection.Output);
                    args.Add("p_errmessage", dbType: DbType.String, direction: ParameterDirection.Output);

                    using (conn.QueryMultiple("sp_automaticnumbering", args, commandType: CommandType.StoredProcedure)) { }

                    model.Flagcode = args.Get<string>("p_value");

                    if (string.IsNullOrWhiteSpace(model.Flagcode))
                    {
                        var msg = args.Get<string>("p_errmessage");
                        LoggerFactories.CreateLogger().Write("活动码生成失败：" + msg, TraceEventType.Error);
                        return -1;
                    }
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("AddCrowdInfo异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCrowdInfo(CrowdInfoModel model)
        {
            var sql = new StringBuilder("update activity_crow_info set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
                LoggerFactories.CreateLogger().Write("UpdateCrowdInfo异常：", TraceEventType.Error, ex);
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public int UpdateCrowdQrCode(string flagcode,string qrcode)
        {
            const string sql = @"update activity_crow_info set qrcode=@qrcode where flagcode = @flagcode;";
            var result = Helper.Execute(sql, new { flagcode, qrcode });
            return result;
        }

        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        public CrowdTotalInfoModel GetCrowdActivityTotal(string flagcode)
        {
            const string sqlSelPid = "select innerid,uppertotal,uppereach from activity_crow_info where `flagcode`=@flagcode;";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    var totalInfo = conn.Query<CrowdTotalInfoModel>(sqlSelPid,new { flagcode }).FirstOrDefault();
                    if (totalInfo == null)
                    {
                        return null;
                    }
                    const string sqlGrade = @"SELECT innerid, totalfee, description, photo FROM activity_crow_grade where flagcode=@flagcode and isenabled=1;";
                    totalInfo.GradeList = conn.Query<CrowdGradeInfo>(sqlGrade,new { totalInfo.Flagcode }).ToList();

                    return totalInfo;
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("GetCrowdActivityTotal异常：", TraceEventType.Error, ex);
                    return null;
                }
            }
        }

        #endregion

        #region 档次管理
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdGradeModel> GetGradePageList(QueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"activity_vote_per as a ";
            const string fields = "innerid, voteid, fullname, num, picture, mobile, ip, openid, createrid, createdtime, modifierid, modifiedtime,(select count(1) from activity_vote_log where perid=a.innerid) as votenum";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder($" a.activity='' ");
            
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CrowdGradeModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public IEnumerable<CrowdGradeModel> GetGradeListByFlagcode(string flagcode)
        {
            const string sql = @"SELECT innerid, flagcode, totalfee, description, isenabled, photo, remark, createrid, createdtime, modifierid, modifiedtime FROM activity_crow_grade where flagcode=@flagcode and isenabled=1;";
            var list = Helper.Query<CrowdGradeModel>(sql, new { flagcode });
            return list;
        }

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CrowdGradeModel GetGradeInfoById(string innerid)
        {
            const string sql =
                @"SELECT innerid, flagcode, totalfee, description, isenabled, photo, remark, createrid, createdtime, modifierid, modifiedtime FROM activity_crow_grade where innerid=@innerid";
            var model = Helper.Query<CrowdGradeModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddGrade(CrowdGradeModel model)
        {
            const string sql = @"INSERT INTO activity_crow_grade
                                (innerid, flagcode, totalfee, description, isenabled, photo, remark, createrid, createdtime)
                                VALUES
                                (@innerid, @flagcode, @totalfee, @description, @isenabled, @photo, @remark, @createrid, @createdtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("AddGrade异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateGrade(CrowdGradeModel model)
        {
            var sql = new StringBuilder("update activity_crow_grad set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
                LoggerFactories.CreateLogger().Write("UpdateGrade异常：", TraceEventType.Error, ex);
            }
            return result;
        }


        #endregion

        #region 参与人管理
        
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdPlayerListModel> GetPlayerPageList(CrowdPlayerQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"activity_crow_player as a ";
            const string fields = @"innerid, wechatnick, wechatheadportrait, mobile, openid, flagcode, isenabled, iswinning, 
                                    (select      count(1) from activity_crow_payrecord where flagcode=a.flagcode and openid=a.openid and ispay=1) as paynum,
                                    (select sum(totalfee) from activity_crow_payrecord where flagcode=a.flagcode and openid=a.openid and ispay=1) as totalfee";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime desc " : query.Order;

            var sqlWhere = new StringBuilder($" a.flagcode='{query.Flagcode}' ");

            if (!string.IsNullOrWhiteSpace(query.Wechatnick))
            {
                sqlWhere.Append($" and a.wechatnick like '%{query.Wechatnick}%'");
            }
            if (query.Iswinning == 1)
            {
                sqlWhere.Append(" and a.iswinning=1");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CrowdPlayerListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public IEnumerable<CrowdPlayerSecretModel> GetPlayerListByFlagcode(string flagcode)
        {
            const string sql = @"SELECT innerid, openid, wechatnick, wechatheadportrait, mobile, iswinning,
            (select sum(totalfee) from activity_crow_payrecord where flagcode=@flagcode and openid=a.openid and ispay=1) as totalfee 
            FROM activity_crow_player as a where flagcode=@flagcode and isenabled=1 order by totalfee desc;";
            var list = Helper.Query<CrowdPlayerSecretModel>(sql, new { flagcode });
            return list;
        }

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CrowdPlayerModel GetPlayerInfoById(string innerid)
        {
            const string sql =
                @"SELECT innerid, flagcode, totalfee,ispay, description, isenabled, photo, remark, createrid, createdtime, modifierid, modifiedtime FROM activity_crow_player where innerid=@innerid";
            var model = Helper.Query<CrowdPlayerModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 根据openid获取Player详情 info
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public CrowdPlayerModel GetPlayerInfoById(string flagcode, string openid)
        {
            const string sql =
                @"SELECT innerid, flagcode, openid, mobile, wechatnick, wechatheadportrait, isenabled, remark, createrid, createdtime, modifierid, modifiedtime FROM activity_crow_player where flagcode=@flagcode and openid=@openid;";
            var model = Helper.Query<CrowdPlayerModel>(sql, new { flagcode, openid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 根据openid获取Player详情 view
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CrowdPlayerViewModel GetPlayerViewById(string innerid)
        {
            const string sql =
                @"SELECT innerid, flagcode, openid, mobile, wechatnick, wechatheadportrait, isenabled, remark, createrid, createdtime, modifierid, modifiedtime,
                (select      count(1) from activity_crow_payrecord where flagcode=a.flagcode and openid=a.openid and ispay=1) as paynum,
                (select sum(totalfee) from activity_crow_payrecord where flagcode=a.flagcode and openid=a.openid and ispay=1) as totalfee
                FROM activity_crow_player as a where innerid=@innerid;";
            var model = Helper.Query<CrowdPlayerViewModel>(sql, new { innerid }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPlayer(CrowdPlayerModel model)
        {
            const string sql = @"INSERT INTO activity_crow_player
                                (innerid, flagcode, gradeid, totalfee, openid, mobile, wechatnick, wechatheadportrait, orderno, ispay, isenabled, remark, createrid, createdtime)
                                VALUES
                                (@innerid, @flagcode, @gradeid, @totalfee, @openid, @mobile, @wechatnick, @wechatheadportrait, @orderno, @ispay, @isenabled, @remark, @createrid, @createdtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("AddPlayer异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// UpdatePlayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePlayer(CrowdPlayerModel model)
        {
            var sql = new StringBuilder("update activity_crow_player set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where orderno = @orderno");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
                LoggerFactories.CreateLogger().Write("UpdatePlayer：", TraceEventType.Error, ex);
            }
            return result;
        }

        #region 添加订单

        /// <summary>
        /// 获取Player支付记录列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public IEnumerable<CrowdPayRecordListModel> GetPayRecordListWithPlayer(string flagcode, string openid)
        {
            const string sql = @"SELECT innerid, totalfee, orderno, createdtime FROM activity_crow_payrecord where flagcode=@flagcode and openid=@openid and ispay=1 order by createdtime desc;";
            var list = Helper.Query<CrowdPayRecordListModel>(sql, new {flagcode, openid });
            return list;
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPlayerPay(CrowdPayRecordModel model)
        {
            const string sql = @"INSERT INTO activity_crow_payrecord
                                (innerid, flagcode, totalfee, openid, orderno, ispay, remark, createdtime, modifiedtime)
                                VALUES
                                (@innerid, @flagcode, @totalfee, @openid, @orderno, @ispay, @remark, @createdtime, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("AddPlayerPay异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPlayerPayEx(CrowdPayRecordModel model)
        {
            const string sql = @"INSERT INTO activity_crow_payrecord
                                (innerid, flagcode, totalfee, openid, orderno, ispay, remark, createdtime, modifiedtime)
                                VALUES
                                (@innerid, @flagcode, @totalfee, @openid, @orderno, @ispay, @remark, @createdtime, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                
                var tran = conn.BeginTransaction();
                try
                {
                    //检查是否已经保存过粉丝信息
                    const string sqlSel = @"SELECT 1 FROM activity_crow_player where flagcode=@flagcode and openid=@openid;";
                    var i = conn.Query<int>(sqlSel, new { model.Flagcode, model.Openid }).FirstOrDefault();
                    if (i != 1)
                    {
                        const string sqlPlayer = @"INSERT INTO activity_crow_player
                                (innerid, flagcode, openid, mobile, wechatnick, wechatheadportrait, isenabled, remark, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @flagcode, @openid, @mobile, @wechatnick, @wechatheadportrait, @isenabled, @remark, @createrid, @createdtime, @modifierid, @modifiedtime);";
                        conn.Execute(sqlPlayer, model.Player, tran);
                    }
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("AddPlayerPayEx异常：", TraceEventType.Error, ex);
                    return 0;
                }
            }
        }
        
        #endregion

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public int DoPay(string orderNo)
        {
            const string sql = @"update activity_crow_payrecord set ispay=1 where orderno = @orderno;";
            var result = Helper.Execute(sql, new { orderNo });
            return result;
        }

        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public PayNotifyModel GetPlayerByOrderNo(string orderNo)
        {
            const string sql = @"select a.totalfee,a.remark,b.wechatnick,b.wechatheadportrait from  activity_crow_payrecord as a inner join activity_crow_player as b on a.openid=b.openid and a.flagcode=b.flagcode where a.orderno = @orderno;";
            var model = Helper.Query<PayNotifyModel>(sql, new { orderNo }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public int GetPaidTotal(string flagcode,string openid)
        {
            const string sql = @"select ifnull(sum(a.totalfee),0) as totalfee from activity_crow_payrecord as a where a.flagcode=@flagcode and a.openid=@openid and a.ispay=1;";
            var total = Helper.Query<int>(sql, new { flagcode, openid }).FirstOrDefault();
            return total;
        }

        
        #endregion

        #endregion
    }
}