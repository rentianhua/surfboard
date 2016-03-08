using System.Web.Http;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Resource.ApiControllers
{
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Activity")]
    public class ActivityController : ApiController
    {
        private readonly IActivityManagementService _activityservice;

        public ActivityController()
        {
            _activityservice = ServiceLocatorFactory.GetServiceLocator().GetService<IActivityManagementService>();
        }

        #region 投票活动

        /// <summary>
        /// 获取投票活动列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVoteList")]
        public BasePageList<VoteListModel> GetVoteList([FromBody]VoteQueryModel query)
        {
            return _activityservice.GetVoteList(query);
        }

        /// <summary>
        /// 获取投票活动详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVoteViewById")]
        public JResult GetVoteViewById(string id)
        {
            return _activityservice.GetVoteViewById(id);
        }

        /// <summary>
        /// 获取投票活动详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVoteInfoById")]
        public JResult GetVoteInfoById(string id)
        {
            return _activityservice.GetVoteInfoById(id);
        }

        #endregion

        #region 投票活动参赛人员
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVotePerList")]
        public BasePageList<VotePerListModel> GetVotePerList([FromBody]VotePerQueryModel query)
        {
            return _activityservice.GetVotePerList(query);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVotePerViewById")]
        public JResult GetVotePerViewById(string id)
        {
            return _activityservice.GetVotePerViewById(id);
        }

        /// <summary>
        /// 获取投票活动的参赛人员详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVotePerInfoById")]
        public JResult GetVotePerInfoById(string id)
        {
            return _activityservice.GetVotePerInfoById(id);
        }

        /// <summary>
        /// 参赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddVotePer")]
        public JResult AddVotePer([FromBody]VotePerModel model)
        {
            return _activityservice.AddVotePer(model);
        }

        #endregion

        #region 投票日志

        /// <summary>
        /// 获取参赛人的投票列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVoteLogList")]
        public BasePageList<VoteLogListModel> GetVoteLogList([FromBody]VoteLogQueryModel query)
        {
            return _activityservice.GetVoteLogList(query);
        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddVoteLog")]
        public JResult AddVoteLog([FromBody]VoteLogModel model, int number)
        {
            return _activityservice.AddVoteLog(model,number);
        }
        #endregion
    }
}
