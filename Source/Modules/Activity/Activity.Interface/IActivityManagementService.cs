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
    }
}
