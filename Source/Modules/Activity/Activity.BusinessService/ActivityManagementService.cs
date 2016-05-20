using System;
using System.Diagnostics;
using System.Threading;
using CCN.Modules.Activity.BusinessComponent;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.Interface;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Activity.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityManagementService : ServiceBase<ActivityBusinessComponent>, IActivityManagementService
    {
        private readonly object _obj = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        public ActivityManagementService(ActivityBusinessComponent bc) : base(bc)
        {

        }

        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<VoteListModel> GetVoteList(VoteQueryModel query)
        {
            return BusinessComponent.GetVoteList(query);
        }

        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVoteViewById(string id)
        {
            return BusinessComponent.GetVoteViewById(id);
        }

        /// <summary>
        /// 获取投票活动详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVoteInfoById(string id)
        {
            return BusinessComponent.GetVoteInfoById(id);
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
            return BusinessComponent.GetVotePerList(query);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVotePerViewById(string id)
        {
            return BusinessComponent.GetVotePerViewById(id);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JResult GetVotePerInfoById(string id)
        {
            return BusinessComponent.GetVotePerInfoById(id);
        }

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddVotePer(VotePerModel model)
        {
            return BusinessComponent.AddVotePer(model);
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
            return BusinessComponent.GetVoteLogList(query);
        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddVoteLog(VoteLogModel model)
        {
            return BusinessComponent.AddVoteLog(model);
        }
        /// <summary>
        /// 作弊投票
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public JResult AddVoteLog(VoteLogModel model, int number)
        {
            return BusinessComponent.AddVoteLog(model, number);
        }

        /// <summary>
        /// 取消订阅操作
        /// </summary>
        /// <returns></returns>
        public JResult UnSubscribe(string appid, string openid)
        {
            return BusinessComponent.UnSubscribe(appid, openid);
        }
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
            return BusinessComponent.StartDraw(model);
        }

        /// <summary>
        /// 结束抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult EndDraw(StartDrawModel model)
        {
            return BusinessComponent.EndDraw(model);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList(CrowdInfoQueryModel query)
        {
            return BusinessComponent.GetCrowdActivityPageList(query);
        }

        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCrowdInfoById(string innerid)
        {
            return BusinessComponent.GetCrowdInfoById(innerid);
        }

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetCrowdViewById(string flagcode)
        {
            return BusinessComponent.GetCrowdViewById(flagcode);
        }
        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetCrowdProgressByFlagcode(string flagcode)
        {
            return BusinessComponent.GetCrowdProgressByFlagcode(flagcode);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCrowdInfo(CrowdInfoModel model)
        {
            return BusinessComponent.AddCrowdInfo(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCrowdInfo(CrowdInfoModel model)
        {
            return BusinessComponent.UpdateCrowdInfo(model);
        }

        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        public JResult GetCrowdActivityTotal(string flagcode)
        {
            return BusinessComponent.GetCrowdActivityTotal(flagcode);
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
            return BusinessComponent.GetGradePageList(query);
        }

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetGradeListByFlagcode(string flagcode)
        {
            return BusinessComponent.GetGradeListByFlagcode(flagcode);
        }

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetGradeInfoById(string innerid)
        {
            return BusinessComponent.GetGradeInfoById(innerid);
        }

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddGrade(CrowdGradeModel model)
        {
            return BusinessComponent.AddGrade(model);
        }

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateGrade(CrowdGradeModel model)
        {
            return BusinessComponent.UpdateGrade(model);
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
            return BusinessComponent.GetPlayerPageList(query);
        }

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult GetPlayerListByFlagcode(string flagcode)
        {
            return BusinessComponent.GetPlayerListByFlagcode(flagcode);
        }

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetPlayerInfoById(string innerid)
        {
            return BusinessComponent.GetPlayerInfoById(innerid);
        }
        /// <summary>
        /// 根据openid获取Player详情 view
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetPlayerViewById(string innerid)
        {
            return BusinessComponent.GetPlayerViewById(innerid);
        }
        /// <summary>
        /// 获取Player支付记录列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JResult GetPayRecordListWithPlayer(string flagcode, string openid)
        {
            return BusinessComponent.GetPayRecordListWithPlayer(flagcode, openid);
        }
        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddPlayer(CrowdPlayerModel model)
        {
            return BusinessComponent.AddPlayer(model);
        }
        /// <summary>
        /// UpdatePlayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdatePlayer(CrowdPlayerModel model)
        {
            return BusinessComponent.UpdatePlayer(model);
        }

        /// <summary>
        /// 获取活动信息及用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public JResult GetActivityAndPaidTotal(string flagcode, string openid)
        {
            return BusinessComponent.GetActivityAndPaidTotal(flagcode, openid);
        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public JResult DoPay(string orderNo)
        {

            return BusinessComponent.DoPay(orderNo);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        public JResult CrowdGenerateQrCode(string flagcode)
        {
            return BusinessComponent.CrowdGenerateQrCode(flagcode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult CrowdUnifiedOrder(CrowdUnifiedOrderModel model)
        {
            lock (_obj)
            {
                var mu = new Mutex(false, "MyMutex");
                mu.WaitOne();
                try
                {
                    return BusinessComponent.CrowdUnifiedOrder(model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("微信支付下单异常", TraceEventType.Error, ex);
                    return JResult._jResult(400, "微信支付下单异常");
                }
                finally
                {
                    mu.ReleaseMutex();
                }
            }
        }

        #endregion
    }
}
