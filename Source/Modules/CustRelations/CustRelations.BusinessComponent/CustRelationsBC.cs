#region

using System;
using System.Collections.Generic;
using System.Linq;
using CCN.Modules.CustRelations.BusinessEntity;
using CCN.Modules.CustRelations.DataAccess;
using Cedar.Framework.AuditTrail.Interception;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

#endregion

namespace CCN.Modules.CustRelations.BusinessComponent
{
    /// <summary>
    /// </summary>
    public class CustRelationsBC : BusinessComponentBase<CustRelationsDA>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="daObject"></param>
        public CustRelationsBC(CustRelationsDA daObject) : base(daObject)
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
            //如果没有输入条件 返回空
            if (string.IsNullOrWhiteSpace(query.Custname) && string.IsNullOrWhiteSpace(query.Mobile) && string.IsNullOrWhiteSpace(query.Email))
            {
                return new BasePageList<CustViewModel>();
            }
            //手机号小于4位不给搜索
            if (string.IsNullOrWhiteSpace(query.Mobile) || query.Mobile.Trim().Length < 4)
            {
                return new BasePageList<CustViewModel>();
            }

            return DataAccess.GetCustPageList(query);
        }

        /// <summary>
        /// 获取加好友申请
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CustRelationsApplyViewModels> GetCustRelationsPageList(CustRelationsApplyQueryModels query)
        {
            var list = DataAccess.GetCustRelationsPageList(query);
            return list;
        }

        /// <summary>
        /// 根据请求id获取申请信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetRelationsApplyById(string innerid)
        {
            var model = DataAccess.GetRelationsApplyById(innerid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        /// <summary>
        /// 检查是否好友关系
        /// </summary>
        /// <param name="fromid">自己id</param>
        /// <param name="toid">好友id</param>
        /// <returns></returns>
        public JResult CheckRelations(string fromid, string toid)
        {
            var count = DataAccess.CheckRelations(fromid, toid);
            return JResult._jResult(
                count == 0 ? 0 : 1, 
                count == 0 ? "非好友" : "好友");
        }

        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddRelationsApply(CustRelationsApplyModels model)
        {
            if (string.IsNullOrWhiteSpace(model.Fromid) || string.IsNullOrWhiteSpace(model.Toid))
            {
                return new JResult
                {
                    errcode = 401,
                    errmsg = "数据不完整"
                };
            }

            var cRelations = DataAccess.CheckRelations(model.Fromid, model.Toid);
            if (cRelations > 0)
            {
                return new JResult
                {
                    errcode = 201,
                    errmsg = "已经是好友"
                };
            }

            var number = DataAccess.GetApplyNumber(model.Fromid);
            if (number >= 15)
            {
                return new JResult
                {
                    errcode = 202,
                    errmsg = "今天添加好友达到上限"
                };
            }

            var cRelationsApply = DataAccess.CheckRelationsApply(model.Fromid, model.Toid);
            if (cRelationsApply > 0)
            {
                model.Modifiedtime = DateTime.Now;
                DataAccess.UpdateRelationsApply(model);
                return new JResult
                {
                    errcode = 200,
                    errmsg = "重复申请"
                };
            }

            model.Status = 0;
            model.Innerid = Guid.NewGuid().ToString();
            var result = DataAccess.AddRelationsApply(model);
            if (result > 0)
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = model.Innerid
                };
            }
            return new JResult
            {
                errcode = 400,
                errmsg = "申请失败"
            };
        }

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        public JResult HandleRelationsApply(string innerid, int status)
        {
            var model = DataAccess.GetRelationsApplyById(innerid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "申请信息不存在"
                };
            }
            var result = DataAccess.HandleRelationsApply(innerid,  status, model.Fromid, model.Toid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 401,
                errmsg = result > 0 ? "处理成功" : "处理失败"
            };
        }
        
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddFriends(CustRelationsApplyModels model)
        {
            var applyResult = AddRelationsApply(model);
            if (applyResult.errcode != 0)
            {
                return applyResult;
            }

            var handleResult = HandleRelationsApply(applyResult.errmsg.ToString(), 1);
            return handleResult;
        }
        
        /// <summary>
        /// 删除好友的申请
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteApplyById(string innerid)
        {
            var result = DataAccess.DeleteApplyById(innerid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "删除成功" : "删除失败"
            };
        }

        /// <summary>
        /// 删除好友关系
        /// </summary>
        /// <param name="fromid"></param>
        /// <param name="toid"></param>
        /// <returns></returns>
        public JResult DeleteRelations(string fromid, string toid)
        {
            var result = DataAccess.DeleteRelations(fromid, toid);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = result > 0 ? "删除成功" : "删除失败"
            };
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public JResult GetCustRelationsByUserId(string userid)
        {
            var list = DataAccess.GetCustRelationsByUserId(userid).ToList();
            if (!list.Any())
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }

            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        private static JResult _jResult(int errcode, object errmsg)
        {
            return new JResult
            {
                errcode = errcode,
                errmsg = errmsg
            };
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
            return DataAccess.GetHaveCarCustList(query);
        }

        #endregion
    }
}