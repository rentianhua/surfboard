using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerTotalModel
    {
        /// <summary>
        /// 内部id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 当前积分
        /// </summary>
        public int Currpoint { get; set; }

        /// <summary>
        /// 当前礼券数
        /// </summary>
        public int Currpouponnum { get; set; }

        /// <summary>
        /// 剩余刷新次数
        /// </summary>
        public int Refreshnum { get; set; }

        /// <summary>
        /// 剩余置顶次数
        /// </summary>
        public int Topnum { get; set; }
    }
}
