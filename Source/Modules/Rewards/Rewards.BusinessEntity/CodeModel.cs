using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

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
        /// 礼券id
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
        /// 是否使用
        /// </summary>
        public short? IsUsed { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? Usedtime { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public int Sourceid { get; set; }

        /// <summary>
        /// 一/二维码的图片
        /// </summary>
        public string Qrcode { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? Vstart { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? Vend { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CodeCancelQueryModel : CodeModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CardCancelSummaryModel
    {
        /// <summary>
        /// 礼券id
        /// </summary>
        public string Cardid { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 子标题
        /// </summary>
        public string Titlesub { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 购买价
        /// </summary>
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Logourl { get; set; }
        
        /// <summary>
        /// 起始库存
        /// </summary>
        public int? Maxcount { get; set; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public int? Count { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 已核销数
        /// </summary>
        public int CanedCount { get; set; }

        /// <summary>
        /// 已结算数
        /// </summary>
        public int SettedCount { get; set; }

        /// <summary>
        /// 总计
        /// </summary>
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// 核销查询条件model
    /// </summary>
    public class CardCancelSummaryQueryModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 周期开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 周期结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 核销记录查询条件
    /// </summary>
    public class CodeQueryModel : QueryModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 礼券类型
        /// </summary>
        public int? CardType { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 核销开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 核销结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 我的礼券列表条件
    /// </summary>
    public class MyCodeQueryModel : CodeQueryModel
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 状态
        /// 1.可用，包含有效期未到的
        /// 2.不可用，包含过期的
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 即将过期 1.表示筛选即将过期
        /// </summary>
        public int? IsExpire { get; set; }
    }

    /// <summary>
    /// 核销记录列表model
    /// </summary>
    public class CodeViewListModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        public DateTime? Usedtime { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 购买价
        /// </summary>
        public decimal? Buyprice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? Costprice { get; set; }
    }

    /// <summary>
    /// 我的礼券列表
    /// </summary>
    public class MyCodeViewListModel
    {
        /// <summary>
        /// code id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime? Gettime { get; set; }
        
        /// <summary>
        /// 是否使用
        /// </summary>
        public short? IsUsed { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        public string Logourl { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 购买价
        /// </summary>
        public decimal? BuyPrice { get; set; }

        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Shopname { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? Vstart { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? Vend { get; set; }

        /// <summary>
        /// 使用启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 礼券类型
        /// </summary>
        public string Cardtypename { get; set; }

        /// <summary>
        /// 礼券类型备注
        /// </summary>
        public string CardtypeRemark { get; set; }
    }

    /// <summary>
    /// 我的礼券详情
    /// </summary>
    public class MyCodeViewModel : MyCodeViewListModel
    {
        /// <summary>
        /// 礼券id
        /// </summary>
        public string Cardid { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 商户区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 商户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 使用说明
        /// </summary>
        public string Usedesc { get; set; }
    }

    /// <summary>
    /// code列表汇总
    /// </summary>
    public class CodeListSummaryModel
    {
        /// <summary>
        /// 核销总数
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 核销总价额
        /// </summary>
        public decimal? TotalPrice { get; set; }
    }


    /// <summary>
    /// 核销model
    /// </summary>
    public class CancelModel
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 礼券code
        /// </summary>
        public string Code { get; set; }
    }
    
    /// <summary>
    /// 发放时code model
    /// </summary>
    public class CouponCodeModel
    {
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? Vstart { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? Vend { get; set; }
    }
}
