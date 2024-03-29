﻿using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Activity.BusinessEntity
{
    /// <summary>
    /// 投票活动--投票记录表
    /// </summary>
    public class VoteLogModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 投票活动id
        /// </summary>
        public string Activityid { get; set; }
        
        /// <summary>
        /// 参赛人id
        /// </summary>
        public string Perid { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoteLogQueryModel : QueryModel
    {
        /// <summary>
        /// 投票活动id
        /// </summary>
        public string Activityid { get; set; }

        /// <summary>
        /// 参赛人id
        /// </summary>
        public string Perid { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Openid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoteLogListModel
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public int IsInvalid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string Modifiedtime { get; set; }

    }
}
