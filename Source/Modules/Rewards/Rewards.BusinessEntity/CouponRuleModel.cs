using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class CouponRuleModel
    {
        /// <summary>
        /// Innerid
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 动作类型[1.认证,2.结案,...]
        /// </summary>
        public int Actiontype { get; set; }

        /// <summary>
        /// 礼券id
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// 奖励数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? Opertime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SendCouponModel
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 动作类型[1.认证,2.结案,...]
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 礼券来源
        /// </summary>
        public int Sourceid { get; set; }
    }
}
