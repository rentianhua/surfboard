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
    
}
