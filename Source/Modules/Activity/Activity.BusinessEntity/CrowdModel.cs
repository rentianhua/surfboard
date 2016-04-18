using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Activity.BusinessEntity
{

    /// <summary>
    /// 众筹活动model
    /// </summary>
    public class CrowdInfoModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Enrollstarttime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Enrollendtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Secrettime { get; set; }

        /// <summary>
        /// 活动金额总的上限(单位分)
        /// </summary>
        public int? Uppertotal { get; set; }

        /// <summary>
        /// 每人的上限(单位分)
        /// </summary>
        public int? Uppereach { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        /// 活动码
        /// </summary>
        public string Flagcode { get; set; }

        /// <summary>
        /// 参与活动的二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Extend { get; set; }
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
    /// 众筹活动model
    /// </summary>
    public class CrowdInfoListModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Enrollstarttime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Enrollendtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Secrettime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        /// 参与活动的二维码
        /// </summary>
        public string QrCode { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        public int Playernum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CrowdTotalInfoModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string Activityid { get; set; }

        /// <summary>
        /// 活动金额总的上限(单位分)
        /// </summary>
        public int Uppertotal { get; set; }

        /// <summary>
        /// 每人的上限(单位分)
        /// </summary>
        public int Uppereach { get; set; }

        /// <summary>
        /// 档次list
        /// </summary>
        public List<CrowdGradeInfo> GradeList { get; set; }
    }

    /// <summary>
    /// 众筹活动model
    /// </summary>
    public class CrowdUnifiedOrderModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string activityid { get; set; }

        /// <summary>
        /// 活动id
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 支付说明
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string wechatnick { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string wechatheadportrait { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string code { get; set; }
    }
}
