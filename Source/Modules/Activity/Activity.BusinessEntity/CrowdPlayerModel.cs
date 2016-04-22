using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Activity.BusinessEntity
{
    /// <summary>
    /// 众筹活动--参与人员
    /// </summary>
    public class CrowdPlayerModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 活动码
        /// </summary>
        public string Flagcode { get; set; }

        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Wechatnick { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Wechatheadportrait { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public short? Iswinning { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public short? Isenabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
    public class CrowdPlayerListModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 活动码
        /// </summary>
        public string Flagcode { get; set; }

        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Wechatnick { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Wechatheadportrait { get; set; }

        /// <summary>
        /// 支付次数
        /// </summary>
        public int PayNum { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public int Totalfee { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public short? Isenabled { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public short? Iswinning { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CrowdPlayerViewModel: CrowdPlayerModel
    {
        /// <summary>
        /// 支付次数
        /// </summary>
        public int PayNum { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public int Totalfee { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CrowdPlayerSecretModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }
        
        /// <summary>
        /// 总金额
        /// </summary>
        public int Totalfee { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Wechatnick { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Wechatheadportrait { get; set; }
        
        /// <summary>
        /// 是否中介
        /// </summary>
        public short Iswinning { get; set; }
    }

    /// <summary>
    /// 众筹活动--参与人员Query
    /// </summary>
    public class CrowdPlayerQueryModel: QueryModel
    {
        /// <summary>
        /// 活动码
        /// </summary>
        public string Flagcode { get; set; }

        /// <summary>
        /// 档次id
        /// </summary>
        public string Gradeid { get; set; }
        
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Wechatnick { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public short? Isenabled { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public short? Iswinning { get; set; }
    }


    /// <summary>
    /// 众筹活动--参与人员--支付记录表
    /// </summary>
    public class CrowdPayRecordModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 活动码
        /// </summary>
        public string Flagcode { get; set; }

        /// <summary>
        /// 档次金额
        /// </summary>
        public int Totalfee { get; set; }

        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string Openid { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        public string Orderno { get; set; }

        /// <summary>
        /// 是否支付 1已支付，0未支付
        /// </summary>
        public short Ispay { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CrowdPlayerModel Player { get; set; }
    }

    /// <summary>
    /// 众筹活动--参与人员--支付记录表
    /// </summary>
    public class CrowdPayRecordListModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }
        
        /// <summary>
        /// 档次金额
        /// </summary>
        public int Totalfee { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Orderno { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CrowdPayInfoModel
    {
        /// <summary>
        /// 当前粉丝已支付金额
        /// </summary>
        public int Totalfee { get; set; }

        /// <summary>
        /// 活动金额总的上限(单位分)
        /// </summary>
        public int Uppertotal { get; set; }

        /// <summary>
        /// 每人的上限(单位分)
        /// </summary>
        public int Uppereach { get; set; }

        /// <summary>
        /// 当前参与人数
        /// </summary>
        public int PlayerNum { get; set; }

        /// <summary>
        /// 已筹到的金额(单位分)
        /// </summary>
        public int Upperedtotal { get; set; }

        /// <summary>
        /// 活动奖品(车辆编号)
        /// </summary>
        public string CarNo { get; set; }

        /// <summary>
        /// 活动状态,1未开始,2参与阶段,3待开奖,4抽奖中,5抽奖结束
        /// </summary>
        public short Status { get; set; }
        
        /// <summary>
        /// 参与活动的二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 活动的主题
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// 支付通知model
    /// </summary>
    public class PayNotifyModel
    {
        /// <summary>
        /// 支付金额
        /// </summary>
        public int Totalfee { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Wechatnick { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Wechatheadportrait { get; set; }
    }
}
