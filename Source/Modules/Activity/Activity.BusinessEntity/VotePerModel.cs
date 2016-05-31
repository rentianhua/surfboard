using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Activity.BusinessEntity
{
    /// <summary>
    /// 投票活动--参赛人员model
    /// </summary>
    public class VotePerModel
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
        /// 姓名
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int? Num { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 成品图片
        /// </summary>
        public string Picture { get; set; }
        
        /// <summary>
        /// 其他文件
        /// </summary>
        public string Files { get; set; }

        /// <summary>
        /// 联系手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsAudit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string Modifierid { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotePerQueryModel : QueryModel
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
        /// 姓名
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int? Num { get; set; }

        /// <summary>
        /// 联系手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 是否前端访问
        /// </summary>
        public int Isfront { get; set; } = 1;
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotePerListModel : VotePerModel
    {
        /// <summary>
        /// 投票数
        /// </summary>
        public int? Votenum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotePerViewModel : VotePerModel
    {
        /// <summary>
        /// 投票数
        /// </summary>
        public int Votenum { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Ranking { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotePerBackViewModel : VotePerModel
    {
        /// <summary>
        /// 投票数
        /// </summary>
        public int Votenum { get; set; }

        /// <summary>
        /// 假票数
        /// </summary>
        public int FakeVotenum { get; set; }

        /// <summary>
        /// 无效票数
        /// </summary>
        public int InvalidVotenum { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Ranking { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotePerAuditModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string perid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string openid { get; set; }
        
    }
}
