using CCN.Modules.Activity.BusinessComponent;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.Interface;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Activity.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityManagementService : ServiceBase<ActivityBusinessComponent>, IActivityManagementService
    {
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

        #endregion
    }
}
