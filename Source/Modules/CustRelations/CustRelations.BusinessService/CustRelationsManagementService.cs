﻿#region

using System.Collections.Generic;
using CCN.Modules.CustRelations.BusinessComponent;
using CCN.Modules.CustRelations.BusinessEntity;
using CCN.Modules.CustRelations.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;
using Cedar.Framework.Common.BaseClasses;

#endregion

namespace CCN.Modules.CustRelations.BusinessService
{
    /// <summary>
    /// </summary>
    public class CustRelationsManagementService : ServiceBase<CustRelationsBC>, ICustRelationsManagementService
    {
        /// <summary>
        /// </summary>
        public CustRelationsManagementService(CustRelationsBC bc)
            : base(bc)
        {
        }

        #region 好友关系管理

        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustViewModel> GetCustPageList(CustQueryModel query)
        {
            return BusinessComponent.GetCustPageList(query);
        }

        /// <summary>
        /// 获取加好友申请
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustRelationsApplyViewModels> GetCustRelationsPageList(CustRelationsApplyQueryModels query)
        {
            var list = BusinessComponent.GetCustRelationsPageList(query);
            return list;
        }

        /// <summary>
        /// 根据请求id获取申请信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetRelationsApplyById(string innerid)
        {
            return BusinessComponent.GetRelationsApplyById(innerid);
        }

        /// <summary>
        /// 检查是否好友关系
        /// </summary>
        /// <param name="fromid">自己id</param>
        /// <param name="toid">好友id</param>
        /// <returns></returns>
        public JResult CheckRelations(string fromid, string toid)
        {
            return BusinessComponent.CheckRelations(fromid, toid);
        }

        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddRelationsApply(CustRelationsApplyModels model)
        {
            return BusinessComponent.AddRelationsApply(model);
        }

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        public JResult HandleRelationsApply(string innerid, int status)
        {
            return BusinessComponent.HandleRelationsApply(innerid, status);
        }
        
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddFriends(CustRelationsApplyModels model)
        {
            return BusinessComponent.AddFriends(model);
        }

        /// <summary>
        /// 删除好友的申请
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteApplyById(string innerid)
        {
            return BusinessComponent.DeleteApplyById(innerid);
        }

        /// <summary>
        /// 删除好友关系
        /// </summary>
        /// <param name="fromid"></param>
        /// <param name="toid"></param>
        /// <returns></returns>
        public JResult DeleteRelations(string fromid, string toid)
        {
            return BusinessComponent.DeleteRelations(fromid, toid);
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public JResult GetCustRelationsByUserId(string userid)
        {
            var list = BusinessComponent.GetCustRelationsByUserId(userid);
            return list;
        }

        #endregion

        #region 社交圈

        /// <summary>
        /// 社交圈搜车
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustRelationsCarViewModel> GetHaveCarCustList(CustRelationsCarQueryModel query)
        {
            return BusinessComponent.GetHaveCarCustList(query);
        }

        #endregion
    }
}