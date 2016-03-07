using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 拍卖时间区间
    /// </summary>
    public class AuctionTimeModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 序号 
        /// </summary>
        public byte?  no { get; set; }

        /// <summary>
        /// 类型  1：一口价 2：0元拍
        /// </summary>
        public byte? type { get; set; }

        /// <summary>
        /// 开始时间（小时）
        /// </summary>
        public byte? beginhour { get; set; }

        /// <summary>
        /// 结束时间（小时）
        /// </summary>
        public byte? endhour { get; set; }
        
        /// <summary>
        /// 开始时间（分钟）
        /// </summary>
        public byte? beginmin { get; set; }

        /// <summary>
        /// 结束时间（分钟）
        /// </summary>
        public byte? endmin { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdtime { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class AuctionTimeQueryModel : QueryModel { }

    /// <summary>
    /// 显示实体
    /// </summary>
    public class AuctionTimeViewModel : AuctionTimeModel
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime? currenttime { get; set; }

    }
}
