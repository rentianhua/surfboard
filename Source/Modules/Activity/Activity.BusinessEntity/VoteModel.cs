using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Activity.BusinessEntity
{
    /// <summary>
    /// 投票活动信息表
    /// </summary>
    public class VoteModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 报名开始时间
        /// </summary>
        public DateTime? Enrollstarttime { get; set; }

        /// <summary>
        /// 报名结束时间
        /// </summary>
        public DateTime? Enrollendtime { get; set; }

        /// <summary>
        /// 投票开始时间
        /// </summary>
        public DateTime? Votestarttime { get; set; }

        /// <summary>
        /// 投票结束时间
        /// </summary>
        public DateTime? Voteendtime { get; set; }

        /// <summary>
        /// 投票方式
        /// </summary>
        public string Votemode { get; set; }

        /// <summary>
        /// 投票流程
        /// </summary>
        public string Voteflow { get; set; }

        /// <summary>
        /// 奖项说明
        /// </summary>
        public string Prizedeac { get; set; }

        /// <summary>
        /// 注意事项
        /// </summary>
        public string Attention { get; set; }

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
    public class VoteViewModel : VoteModel
    {
        /// <summary>
        /// 参赛人数
        /// </summary>
        public int Numper { get; set; }

        /// <summary>
        /// 总投票数
        /// </summary>
        public int Numvote { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoteListModel : VoteModel
    {
        /// <summary>
        /// 参赛人数
        /// </summary>
        public int Numper { get; set; }

        /// <summary>
        /// 总投票数
        /// </summary>
        public int Numvote { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoteQueryModel : QueryModel
    {
        
    }
}
