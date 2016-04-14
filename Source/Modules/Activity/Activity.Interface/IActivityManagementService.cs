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

        #endregion

        #region 众筹活动


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
        /// <param name="activityid"></param>
        /// <returns></returns>
        JResult GetGradeListByActivityId(string activityid);

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
        BasePageList<CrowdPlayerModel> GetPlayerPageList(CrowdPlayerQueryModel query);

        /// <summary>
        /// 获取Player列表
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        JResult GetPlayerListByActivityId(string activityid);

        /// <summary>
        /// 获取Player详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        JResult GetPlayerInfoById(string innerid);

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
        /// 确认支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        JResult DoPay(string orderNo);
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        JResult CrowdUnifiedOrder(CrowdUnifiedOrderModel model);

        #endregion
    }
}
