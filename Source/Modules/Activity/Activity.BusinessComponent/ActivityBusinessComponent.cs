using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.Logging;
using Cedar.Foundation.SMS.Common;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace CCN.Modules.Activity.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityBusinessComponent : BusinessComponentBase<ActivityDataAccess>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        public ActivityBusinessComponent(ActivityDataAccess da) : base(da)
        {

        }

        #region 投票活动

        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VoteListModel> GetVoteList(VoteQueryModel query)
        {
            return DataAccess.GetVoteList(query);
        }

        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVoteViewById(string id)
        {
            var model = DataAccess.GetVoteViewById(id);
            return JResult._jResult(model);
        }
        
        /// <summary>
        /// 获取投票活动详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVoteInfoById(string id)
        {
            var model = DataAccess.GetVoteInfoById(id);
            return JResult._jResult(model);
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
            return DataAccess.GetVotePerList(query);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVotePerViewById(string id)
        {
            var model = DataAccess.GetVotePerViewById(id);

            if (model == null)
            {
                return JResult._jResult(400, "");
            }

            model.Ranking = DataAccess.GetVotePerRanking(model.Activityid, model.Votenum) + 1;

            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVotePerInfoById(string id)
        {
            var model = DataAccess.GetVotePerInfoById(id);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddVotePer(VotePerModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Activityid) 
                || string.IsNullOrWhiteSpace(model.Fullname)
                || string.IsNullOrWhiteSpace(model.Openid)
                || string.IsNullOrWhiteSpace(model.Picture) 
                || string.IsNullOrWhiteSpace(model.Mobile))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createrid = ApplicationContext.Current.UserId;
            model.Createdtime = DateTime.Now;
            model.Modifiedtime = null;
            model.Modifierid = "";
            model.IsAudit = 0;//初始化没有审核
            var result = DataAccess.AddVotePer(model);

            if (result == -1)
            {
                return JResult._jResult(402, "不在参赛时间范围内");
            }

            if (result == -2)
            {
                return JResult._jResult(403, "不能重复报名");
            }

            var task = new Task(() =>
            {
                CustomApi.SendText(ConfigHelper.GetAppSettings("APPID"), model.Openid, "车王大赛报名成功，我们的小编会尽快审核的资料！");
                //发送手机
                var sms = new SMSMSG();
                sms.PostSms(ConfigHelper.GetAppSettings("NotifyMobile"), $"车王大赛有新人报名，姓名：{model.Fullname},手机号：{model.Mobile}，请尽快审核！");
            });
            task.Start();

            return JResult._jResult(result);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public JResult AuditPer(VotePerAuditModel model)
        {            
            var re = DataAccess.AuditPer(model.perid, model.result);
            if (re == 0)
            {
                return JResult._jResult(400, "审核失败");
            }

            CustomApi.SendText(ConfigHelper.GetAppSettings("APPID"), model.openid,
                model.result != 0 
                ? "您的车王大赛报名已经审核通过啦，赶快分享给你的好友帮你投票吧！"
                : "您的车王大赛报名审核没过，请您重新报名！");

            return JResult._jResult(0, "审核成功");
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
            return DataAccess.GetVoteLogList(query);
        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddVoteLog(VoteLogModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Activityid)
                || string.IsNullOrWhiteSpace(model.Perid) 
                || string.IsNullOrWhiteSpace(model.Openid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var result = DataAccess.AddVoteLog(model);
            
            if (result == -1)
            {
                return JResult._jResult(402, "不在投票时间范围内");
            }

            if (result == -2)
            {
                return JResult._jResult(403, "每人只能投一次");
            }

            if (result == -3)
            {
                return JResult._jResult(404, "不能重复投同一个人");
            }

            return JResult._jResult(result);
        }

        /// <summary>
        /// 作弊投票
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public JResult AddVoteLog(VoteLogModel model, int number)
        {
            if (string.IsNullOrWhiteSpace(model?.Activityid)
                || string.IsNullOrWhiteSpace(model.Perid))
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.IP = "";
            model.Openid = "";
            var result = DataAccess.AddVoteLog(model, number);

            return JResult._jResult(result);
        }

        /// <summary>
        /// 取消订阅操作
        /// </summary>
        /// <returns></returns>
        public JResult UnSubscribe(string appid, string openid)
        {
            var result = DataAccess.UnSubscribe(appid, openid);
            return JResult._jResult(result);
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
        public JResult StartDraw(StartDrawModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) || string.IsNullOrWhiteSpace(model.Openids))
            {
                return JResult._jResult(401, "参数不完整");
            }
            var result = DataAccess.StartDraw(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 结束抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult EndDraw(StartDrawModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode))
            {
                return JResult._jResult(401,"参数不完整");
            }
            var result = DataAccess.EndDraw(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList(CrowdInfoQueryModel query)
        {
            var list = DataAccess.GetCrowdActivityPageList(query);
            if (list.aaData.Any())
            {
                var nowtime = DateTime.Now;
                foreach (var model in list.aaData)
                {
                    if (model.Status != 1)
                        continue;
                                        
                    if (nowtime > model.Enrollstarttime && nowtime <model.Enrollendtime)
                    {
                        //参与时间
                        model.Status = 2;
                    }
                    else if (nowtime > model.Enrollendtime && nowtime < model.Secrettime)
                    {
                        //待开奖时间
                        model.Status = 3;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCrowdInfoById(string innerid)
        {
            var model = DataAccess.GetCrowdInfoById(innerid);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetCrowdViewById(string flagcode)
        {
            var model = DataAccess.GetCrowdViewById(flagcode);
            if (model.Status != 1)
                return JResult._jResult(model);
            var nowtime = DateTime.Now;

            if (nowtime > model.Enrollstarttime && nowtime < model.Enrollendtime)
            {
                //参与时间
                model.Status = 2;
            }
            else if (nowtime > model.Enrollendtime && nowtime < model.Secrettime)
            {
                //待开奖时间
                model.Status = 3;
            }
            return JResult._jResult(model);
        }
        
        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetCrowdProgressByFlagcode(string flagcode)
        {
            var model = DataAccess.GetCrowdProgressByFlagcode(flagcode);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCrowdInfo(CrowdInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Title) ||
                model.Secrettime == null ||
                model.Enrollstarttime == null ||
                model.Enrollendtime == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            if (model.Enrollstarttime.Value > model.Enrollendtime.Value
                || model.Enrollendtime.Value > model.Secrettime.Value)
            {
                return JResult._jResult(402, "时间顺序不正确");
            }
            if (model.Enrollendtime.Value.AddMinutes(10) > model.Secrettime.Value)
            {
                return JResult._jResult(402, "时间顺序不正确");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Status = 1;
            model.Createdtime = DateTime.Now;
            model.Createrid = ApplicationContext.Current.UserId;
            var result = DataAccess.AddCrowdInfo(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCrowdInfo(CrowdInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Title) ||
                model.Secrettime == null ||
                model.Enrollstarttime == null ||
                model.Enrollendtime == null)
            {
                return JResult._jResult(401, "参数不完整");
            }

            if (model.Enrollstarttime.Value > model.Enrollendtime.Value
                || model.Enrollendtime.Value > model.Secrettime.Value)
            {
                return JResult._jResult(402, "时间顺序不正确");
            }
            if (model.Enrollendtime.Value.AddMinutes(10) > model.Secrettime.Value)
            {
                return JResult._jResult(402, "时间顺序不正确");
            }
            var result = DataAccess.UpdateCrowdInfo(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        public JResult GetCrowdActivityTotal(string flagcode)
        {
            var model = DataAccess.GetCrowdActivityTotal(flagcode);
            if (model.Status != 1)
                return JResult._jResult(model);
            var nowtime = DateTime.Now;

            if (nowtime > model.Enrollstarttime && nowtime < model.Enrollendtime)
            {
                //参与时间
                model.Status = 2;
            }
            else if (nowtime > model.Enrollendtime && nowtime < model.Secrettime)
            {
                //待开奖时间
                model.Status = 3;
            }
            return JResult._jResult(model);
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
            return DataAccess.GetGradePageList(query);
        }

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetGradeListByFlagcode(string flagcode)
        {
            var list = DataAccess.GetGradeListByFlagcode(flagcode);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetGradeInfoById(string innerid)
        {
            var list = DataAccess.GetGradeInfoById(innerid);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddGrade(CrowdGradeModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) || model.Totalfee == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Createrid = ApplicationContext.Current.UserId;
            var list = DataAccess.AddGrade(model);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateGrade(CrowdGradeModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) || string.IsNullOrWhiteSpace(model.Innerid) ||
                model.Totalfee == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.Createdtime = null;
            model.Createrid = null;
            model.Modifiedtime = DateTime.Now;
            model.Modifierid = ApplicationContext.Current.UserId;
            var list = DataAccess.UpdateGrade(model);
            return JResult._jResult(list);
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
            return DataAccess.GetPlayerPageList(query);
        }

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetPlayerListByFlagcode(string flagcode)
        {
            var list = DataAccess.GetPlayerListByFlagcode(flagcode);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetPlayerInfoById(string innerid)
        {
            var model = DataAccess.GetPlayerInfoById(innerid);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 根据openid获取Player详情 view
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetPlayerViewById(string innerid)
        {
            var model = DataAccess.GetPlayerViewById(innerid);
            return JResult._jResult(model);
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayer(CrowdPlayerModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) || 
                string.IsNullOrWhiteSpace(model.Mobile))
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Createrid = ApplicationContext.Current.UserId;
            var list = DataAccess.AddPlayer(model);
            return JResult._jResult(list);
        }

        /// <summary>
        /// UpdatePlayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdatePlayer(CrowdPlayerModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) ||
                string.IsNullOrWhiteSpace(model.Openid))
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.Modifiedtime = DateTime.Now;
            model.Modifierid = ApplicationContext.Current.UserId;
            var list = DataAccess.UpdatePlayer(model);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 获取活动信息及用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public JResult GetActivityAndPaidTotal(string flagcode, string openid)
        {
            var activityModel = DataAccess.GetCrowdInfoByFlagcode(flagcode);
            if (activityModel == null)
            {
                return JResult._jResult(402, "活动不存在");
            }
            var model = new CrowdPayInfoModel
            {
                CarNo = activityModel.Prize,
                QrCode = activityModel.QrCode,
                Title = activityModel.Title,
                Uppertotal = activityModel.Uppertotal ?? 0,
                Uppereach = activityModel.Uppereach ?? 0
            };

            if (activityModel.Status == 1)
            {
                var nowtime = DateTime.Now;
                if (nowtime > activityModel.Enrollstarttime && nowtime < activityModel.Enrollendtime)
                {
                    //参与时间
                    model.Status = 2;
                }
                else if (nowtime > activityModel.Enrollendtime && nowtime < activityModel.Secrettime)
                {
                    //待开奖时间
                    model.Status = 3;
                }
            }
            else
            {
                model.Status = activityModel.Status ?? 0;
            }

            //获取粉丝已支付金额
            var total = DataAccess.GetPaidTotal(flagcode, openid);
            model.Totalfee = total;

            var progress = DataAccess.GetCrowdProgressByFlagcode(flagcode);
            model.PlayerNum = progress.PlayerNum;
            model.Upperedtotal = progress.Upperedtotal;

            return JResult._jResult(0, model);
        }
        
        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public JResult DoPay(string orderNo)
        {
            var result = DataAccess.DoPay(orderNo);

            if (result <= 0) return JResult._jResult(result);

            var player = DataAccess.GetPlayerByOrderNo(orderNo);
            var url = ConfigHelper.GetAppSettings("nodejssiteurl2") + "api/bit";
            var param = new Dictionary<string, string>
            {
                {"wechatnick", player.Wechatnick},
                {"wechatheadportrait", player.Wechatheadportrait},
                {"fee", player.Totalfee.ToString()},
                {"remark", player.Remark}
            };
            var nodeRes = DynamicWebService.SendPost(url, param, "post");
            LoggerFactories.CreateLogger().Write($"参与支付通知结果 ： {url},result:{nodeRes}", TraceEventType.Information);
            return JResult._jResult(result);
        }

        #region 订单

        /// <summary>
        /// 获取Player支付记录列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult GetPayRecordListWithPlayer(string flagcode, string openid)
        {
            var list = DataAccess.GetPayRecordListWithPlayer(flagcode, openid);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayerPay(CrowdPayRecordModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode) ||
                string.IsNullOrWhiteSpace(model.Openid) || model.Totalfee == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            var list = DataAccess.AddPlayerPay(model);
            return JResult._jResult(list);
        }

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayerPayEx(CrowdPayRecordModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Flagcode)
                || string.IsNullOrWhiteSpace(model.Openid)
                || string.IsNullOrWhiteSpace(model.Orderno)
                || model.Totalfee == 0)
            {
                return JResult._jResult(401, "参数不完整");
            }

            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Player.Innerid = Guid.NewGuid().ToString();
            model.Player.Createdtime = DateTime.Now;
            model.Player.Isenabled = 1;
            var result = DataAccess.AddPlayerPayEx(model);
            
            return JResult._jResult(result);
        }

        #endregion

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult CrowdUnifiedOrder(CrowdUnifiedOrderModel model)
        {
            /*
            if (string.IsNullOrWhiteSpace(model?.code) 
                || string.IsNullOrWhiteSpace(model.total_fee))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var appId = ConfigHelper.GetAppSettings("APPID");
            var appSecret = ConfigHelper.GetAppSettings("AppSecret");
            //Stopwatch stopwatch = new Stopwatch();
            var token = OAuthApi.GetAccessToken(appId, appSecret, model.code);
            var userInfo = OAuthApi.GetUserInfo(token.access_token, token.openid);

            model.openid = token.openid;
            model.wechatnick = userInfo.nickname;
            model.wechatheadportrait = userInfo.headimgurl;
            */
            
            if (string.IsNullOrWhiteSpace(model?.openid) || string.IsNullOrWhiteSpace(model.total_fee))
            {
                return JResult._jResult(401, "参数不完整");
            }

            var activityModel = DataAccess.GetCrowdInfoByFlagcode(model.flagcode);
            var nowTime = DateTime.Now;
            if (activityModel == null)
            {
                return JResult._jResult(403, "活动不存在");
            }
            if (nowTime < activityModel.Enrollstarttime || nowTime > activityModel.Enrollendtime)
            {
                return JResult._jResult(404, "不在参与时间范围内");
            }

            var outTradeNo = "AT" + DateTime.Now.ToString("yyyyMMddHHmmss") + RandomUtility.GetRandom(4);

            if (string.IsNullOrWhiteSpace(model.body))
            {
                model.body = "活动经费";
            }

            const string attach = "kplx_activity_crowd";
            var jspayurl = ConfigHelper.GetAppSettings("payurl") + "jspay";
            var json = "{\"out_trade_no\":\"" + outTradeNo + "\",\"total_fee\":\"" + model.total_fee + "\",\"body\":\"" + model.body + "\",\"attach\":\"" + attach + "\",\"openid\":\"" + model.openid + "\"}";
            var orderresult = DynamicWebService.ExeApiMethod(jspayurl, "post", json, false);

            if (string.IsNullOrWhiteSpace(orderresult))
            {
                return JResult._jResult(402, "JsPay下单失败");
            }

            AddPlayerPayEx(new CrowdPayRecordModel
            {
                Flagcode = model.flagcode,
                Openid = model.openid,
                Totalfee = int.Parse(model.total_fee),
                Orderno = outTradeNo,
                Player = new CrowdPlayerModel
                {
                    Flagcode = model.flagcode,
                    Mobile = model.mobile,
                    Openid = model.openid,
                    Wechatnick = model.wechatnick,
                    Wechatheadportrait = model.wechatheadportrait,
                    Remark = model.remark
                }
            });
            
            LoggerFactories.CreateLogger().Write($"WxPay Result--众筹: {orderresult}", TraceEventType.Information);

            var jobj = JObject.Parse(orderresult);
            return jobj["errcode"].ToString() == "0"
                ? JResult._jResult(0, jobj["errmsg"].ToString())
                : JResult._jResult(402, "JsPay下单失败");
        }

        /// <summary>
        /// 成功活动二维码
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult CrowdGenerateQrCode(string flagcode)
        {
            var appid = ConfigHelper.GetAppSettings("APPID");
            var activityurl = ConfigHelper.GetAppSettings("activityurl") + "?flag=" + flagcode;
            var url = OAuthApi.GetAuthorizeUrl(appid, activityurl, "", OAuthScope.snsapi_base);
            LoggerFactories.CreateLogger().Write("url："+ url, TraceEventType.Information);
            try
            {
                //生成二维码位图
                var bitmap = BarCodeUtility.CreateBarcode(url, 240, 240);
                var filename = string.Concat("AQ", Guid.NewGuid().ToString().Replace("-", ""));
                var stream = BarCodeUtility.BitmapToStream(bitmap);
                //上传图片到七牛云
                var qinniu = new QiniuUtility();
                var qrcode = qinniu.Put(stream, "", filename);
                stream.Dispose();
                //上传成功更新活动二维码
                if (!string.IsNullOrWhiteSpace(qrcode))
                {
                    DataAccess.UpdateCrowdQrCode(flagcode,qrcode);
                }
                return JResult._jResult(0,"生成成功");
            }
            catch (Exception ex)
            {
                // ignored
                LoggerFactories.CreateLogger().Write("CustRegister接口异常", TraceEventType.Error, ex);
                return JResult._jResult(400, "生成失败");
            }
        }
        #endregion
    }
}
