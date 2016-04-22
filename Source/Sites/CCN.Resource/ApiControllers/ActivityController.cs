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
            return _activityservice.AddVoteLog(model, number);
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
        [HttpPost]
        [Route("GetCrowdActivityPageList")]
        public BasePageList<CrowdInfoListModel> GetCrowdActivityPageList([FromBody]CrowdInfoQueryModel query)
        {
            return _activityservice.GetCrowdActivityPageList(query);
        }

        /// <summary>
        /// 获取活动详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdInfoById")]
        public JResult GetCrowdInfoById(string innerid)
        {
            return _activityservice.GetCrowdInfoById(innerid);
        }
        
        /// <summary>
        /// 获取活动详情 view
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrowdViewById")]
        public JResult GetCrowdViewById(string flagcode)
        {
            return _activityservice.GetCrowdViewById(flagcode);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCrowdInfo")]
        public JResult AddCrowdInfo([FromBody]CrowdInfoModel model)
        {
            return _activityservice.AddCrowdInfo(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCrowdInfo")]
        public JResult UpdateCrowdInfo([FromBody]CrowdInfoModel model)
        {
            return _activityservice.UpdateCrowdInfo(model);
        }
        
        #endregion

        #region 档次管理
        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGradePageList")]
        public BasePageList<CrowdGradeModel> GetGradePageList([FromBody]QueryModel query)
        {
            return _activityservice.GetGradePageList(query);
        }

        /// <summary>
        /// 获取档次列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGradeListByFlagcode")]
        public JResult GetGradeListByFlagcode(string flagcode)
        {
            return _activityservice.GetGradeListByFlagcode(flagcode);
        }

        /// <summary>
        /// 获取档次详情 info
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGradeInfoById")]
        public JResult GetGradeInfoById(string innerid)
        {
            return _activityservice.GetGradeInfoById(innerid);
        }

        /// <summary>
        /// 添加档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGrade")]
        public JResult AddGrade([FromBody]CrowdGradeModel model)
        {
            return _activityservice.AddGrade(model);
        }

        /// <summary>
        /// 修改档次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGrade")]
        public JResult UpdateGrade([FromBody]CrowdGradeModel model)
        {
            return _activityservice.UpdateGrade(model);
        }

        #endregion

        #region 参与人管理

        /// <summary>
        /// 获取投票活动的参赛人员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPlayerPageList")]
        public BasePageList<CrowdPlayerListModel> GetPlayerPageList([FromBody]CrowdPlayerQueryModel query)
        {
            return _activityservice.GetPlayerPageList(query);
        }

        /// <summary>
        /// 根据openid获取Player详情 view
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPlayerViewById")]
        public JResult GetPlayerViewById(string innerid)
        {
            return _activityservice.GetPlayerViewById(innerid);
        }

        /// <summary>
        /// 获取Player支付记录列表
        /// </summary>
        /// <param name="flagcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayRecordListWithPlayer")]
        public JResult GetPayRecordListWithPlayer(string flagcode, string openid)
        {
            return _activityservice.GetPayRecordListWithPlayer(flagcode, openid);
        }

        #endregion

        [HttpGet]
        [Route("CrowdGenerateQrCode")]
        public JResult CrowdGenerateQrCode(string flagcode)
        {
            return _activityservice.CrowdGenerateQrCode(flagcode);
        }

        [HttpPost]
        [Route("CrowdUnifiedOrder")]
        public JResult CrowdUnifiedOrder([FromBody]CrowdUnifiedOrderModel model)
        {
            return _activityservice.CrowdUnifiedOrder(model);
        }

        #endregion
    }
}
