using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// code model
    /// </summary>
    public class CodeModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// custid
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime? Gettime { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? Usedtime { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public int Sourceid { get; set; }

        /// <summary>
        /// 一/二维码的图片
        /// </summary>
        public string Qrcode { get; set; }
    }
}
