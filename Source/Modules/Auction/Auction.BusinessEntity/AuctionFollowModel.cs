using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionFollowModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 拍卖车辆id
        /// </summary>
        public string Auctionid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string Userid { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public short? Isdelete { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
        
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? Deletedtime { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public AuctionFollowModel()
        {
            Createdtime = DateTime.Now;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuctionFollowQueryModel :QueryModel
    {
        /// <summary>
        /// 拍卖车辆id
        /// </summary>
        public string Auctionid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string Userid { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public short? Isdelete { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? Deletedtime { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public decimal? minmileage { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public decimal? maxmileage { get; set; }

        /// <summary>
        /// 车辆上牌日期
        /// </summary>
        public DateTime? register_date { get; set; }

        /// <summary>
        /// 城市id(所在地)
        /// </summary>
        public int? cityid { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class FollowaAuctionListModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 拍卖车辆id
        /// </summary>
        public string Auctionid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string Userid { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

    }
}
