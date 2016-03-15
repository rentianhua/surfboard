using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Auction.BusinessEntity
{
    /// <summary>
    /// 认证报告实体
    /// </summary>
    public class AuctionCarInspectionModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string carid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string inspectiondetailid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int intactcount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string modifierid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime modifiedtime { get; set; }

    }

    /// <summary>
    /// 认证项实体
    /// </summary>
    public class AuctionCarInspectionItemModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name_zh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name_en { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? isenabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string modifierid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime modifiedtime { get; set; }

    }

    /// <summary>
    /// 返回所用认证项
    /// </summary>
    public class AuctionAllCarInspection : AuctionCarInspectionItemModel
    {
        /// <summary>
        /// 认证明细项
        /// </summary>
        public IEnumerable<AuctionCarInspectionDetailModel> auctioncarinspectiondetail { get; set; }
    }

    /// <summary>
    /// 认证项明细实体
    /// </summary>
    public class AuctionCarInspectionDetailModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string inspectionid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name_zh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name_en { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? inspectioncount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? isenabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string defaultvalue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string fieldid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string modifierid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime modifiedtime { get; set; }
    }
}
