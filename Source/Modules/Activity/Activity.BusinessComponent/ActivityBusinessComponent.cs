using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Newtonsoft.Json;
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

            model.Ranking = DataAccess.GetVotePerRanking(model.Voteid, model.Votenum) + 1;

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
            if (string.IsNullOrWhiteSpace(model?.Voteid) 
                || string.IsNullOrWhiteSpace(model.Fullname) 
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
            var result = DataAccess.AddVotePer(model);

            if (result == -1)
            {
                return JResult._jResult(402, "不在参赛时间范围内");
            }

            if (result == -2)
            {
                return JResult._jResult(403, "不能重复报名");
            }

            return JResult._jResult(result);
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
            if (string.IsNullOrWhiteSpace(model?.Voteid)
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
                return JResult._jResult(403, "三次机会已用完");
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
            if (string.IsNullOrWhiteSpace(model?.Voteid)
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

        #endregion

        #endregion

        #region 众筹活动

        #region 活动管理

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList(QueryModel query)
        {
            return DataAccess.GetCrowdActivityPageList(query);
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
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCrowdInfo(CrowdInfoModel model)
        {
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
            var result = DataAccess.UpdateCrowdInfo(model);
            return JResult._jResult(result);
        }

        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        public JResult GetCrowdActivityTotal(string flagcode)
        {
            var result = DataAccess.GetCrowdActivityTotal(flagcode);
            return JResult._jResult(result);
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
        /// <param name="activityid"></param>
        /// <returns></returns>
        public JResult GetGradeListByActivityId(string activityid)
        {
            var list = DataAccess.GetGradeListByActivityId(activityid);
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
            if (string.IsNullOrWhiteSpace(model?.Activityid) || model.Totalfee == 0)
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
            if (string.IsNullOrWhiteSpace(model?.Activityid) || string.IsNullOrWhiteSpace(model.Innerid) ||
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
        public BasePageList<CrowdPlayerModel> GetPlayerPageList(CrowdPlayerQueryModel query)
        {
            return DataAccess.GetPlayerPageList(query);
        }

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public JResult GetPlayerListByActivityId(string activityid)
        {
            var list = DataAccess.GetPlayerListByActivityId(activityid);
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
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayer(CrowdPlayerModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Activityid) || 
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
            if (string.IsNullOrWhiteSpace(model?.Activityid) ||
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
        /// 获取用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public JResult GetPaidTotal(string flagcode, string openid)
        {
            var total = DataAccess.GetPaidTotal(flagcode, openid);
            return JResult._jResult(total);
        }
        
        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public JResult DoPay(string orderNo)
        {
            var result = DataAccess.DoPay(orderNo);
            return JResult._jResult(result);
        }

        #region 添加订单


        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayerPay(CrowdPayRecordModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Activityid) ||
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
            model.Innerid = Guid.NewGuid().ToString();
            model.Createdtime = DateTime.Now;
            model.Player.Innerid = Guid.NewGuid().ToString();
            model.Player.Createdtime = DateTime.Now;            
            var list = DataAccess.AddPlayerPayEx(model);
            return JResult._jResult(list);
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

            DataAccess.AddPlayerPayEx(new CrowdPayRecordModel
            {
                Innerid = Guid.NewGuid().ToString(),
                Createdtime = DateTime.Now,
                Activityid = model.activityid,
                Openid = model.openid,
                Totalfee = int.Parse(model.total_fee),
                Orderno = outTradeNo,
                Player = new CrowdPlayerModel
                {
                    Innerid = Guid.NewGuid().ToString(),
                    Createdtime = DateTime.Now,
                    Activityid = model.activityid,
                    Mobile = model.mobile,
                    Openid = model.openid,
                    Wechatnick = model.wechatnick,
                    Wechatheadportrait = model.wechatheadportrait
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
            //var url = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appid}&redirect_uri={activityurl}&response_type=code&scope=snsapi_userinfo&state=#wechat_redirect";
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
            }
            catch (Exception ex)
            {
                // ignored
                LoggerFactories.CreateLogger().Write("CustRegister接口异常", TraceEventType.Error, ex);
            }
            return null;
        }
        #endregion
    }
}
