using System;
using System.Collections.Generic;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 车辆图片
    /// </summary>
    public class AuctionCarPictureModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 类型id code
        /// </summary>
        public int Typeid { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否封面 1.是，0不是
        /// </summary>
        public int IsCover { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public AuctionCarPictureModel()
        {
            Createdtime = DateTime.Now;
        }

    }
    
    /// <summary>
    /// 批量处理图片(添加+删除)
    /// </summary>
    public class AuctionPictureListModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 删除列表(id集合)
        /// </summary>
        public List<string> DelIds { get; set; }

        /// <summary>
        /// 添加列表(七牛key集合)
        /// </summary>
        public List<string> AddPaths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuctionPictureListModel ()
        {
            DelIds = new List<string>();
            AddPaths = new List<string>();
        }
    }
}
