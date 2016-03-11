using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Activity.BusinessEntity;
using CCN.Modules.Activity.DataAccess;
using Cedar.Core.ApplicationContexts;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

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
                return JResult._jResult(403, "重复投票");
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
    }
}
