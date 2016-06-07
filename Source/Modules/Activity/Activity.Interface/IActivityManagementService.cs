using CCN.Modules.Activity.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Activity.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivityManagementService
    {
        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<VoteListModel> GetVoteList(VoteQueryModel query);

        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetVoteViewById(string id);

        /// <summary>
        /// 获取投票活动详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetVoteInfoById(string id);

        #endregion

        #region 投票活动参赛人员

        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<VotePerListModel> GetVotePerList(VotePerQueryModel query);

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetVotePerViewById(string id);

        /// <summary>
        /// 获取投票活动的参赛人员详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JResult GetVotePerInfoById(string id);

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddVotePer(VotePerModel model);

        /// <summary>
        /// 作弊投票
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        JResult AddVoteLog(VoteLogModel model, int number);

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        JResult AuditPer(VotePerAuditModel model);

        #endregion

        #region 投票日志

        /// <summary>
        /// 获取参赛人的投票列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<VoteLogListModel> GetVoteLogList(VoteLogQueryModel query);

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddVoteLog(VoteLogModel model);

        /// <summary>
        /// 取消订阅操作
        /// </summary>
        /// <returns></returns>
        JResult UnSubscribe(string appid, string openid);

        #endregion

        #region 众筹活动

        #region 活动管理

        /// <summary>
        /// 开始抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult StartDraw(StartDrawModel model);

        /// <summary>
        /// 结束抽奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult EndDraw(StartDrawModel model);

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CrowdInfoListModel> GetCrowdActivityPageList(CrowdInfoQueryModel query);

        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetCrowdInfoById(string innerid);

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        JResult GetCrowdViewById(string flagcode);

        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        JResult GetCrowdProgressByFlagcode(string flagcode);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddCrowdInfo(CrowdInfoModel model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateCrowdInfo(CrowdInfoModel model);
        
        /// <summary>
        /// 获取活动的信息及档次list信息
        /// </summary>
        /// <returns></returns>
        JResult GetCrowdActivityTotal(string flagcode);

        #endregion

        #region 档次管理

        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CrowdGradeModel> GetGradePageList(QueryModel query);

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        JResult GetGradeListByFlagcode(string flagcode);

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetGradeInfoById(string innerid);

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddGrade(CrowdGradeModel model);

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdateGrade(CrowdGradeModel model);


        #endregion

        #region 参与人管理

        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        BasePageList<CrowdPlayerListModel> GetPlayerPageList(CrowdPlayerQueryModel query);

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        JResult GetPlayerListByFlagcode(string flagcode);

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetPlayerInfoById(string innerid);

        /// <summary>
        /// 根据openid获取Player详情 view
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetPlayerViewById(string innerid);

        /// <summary>
        /// 获取Player支付记录列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        JResult GetPayRecordListWithPlayer(string flagcode, string openid);

        /// <summary>
        /// 添加Player
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult AddPlayer(CrowdPlayerModel model);

        /// <summary>
        /// UpdatePlayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult UpdatePlayer(CrowdPlayerModel model);

        /// <summary>
        /// 获取活动信息及用户已支付总金额
        /// </summary>
        /// <param name="flagcode">活动码</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        JResult GetActivityAndPaidTotal(string flagcode, string openid);

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        JResult DoPay(string orderNo);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        JResult CrowdGenerateQrCode(string flagcode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult CrowdUnifiedOrder(CrowdUnifiedOrderModel model);

        #endregion
    }
}
