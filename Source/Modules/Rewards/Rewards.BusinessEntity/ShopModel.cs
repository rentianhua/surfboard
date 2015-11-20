using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class ShopModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 商户名
        /// </summary>
        public string Shopname { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 所在地：身份
        /// </summary>
        public int? Provid { get; set; }

        /// <summary>
        /// 所在地：城市
        /// </summary>
        public int? Cityid { get; set; }

        /// <summary>
        /// 所在地：区/县
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 个人签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Qrcode { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ShopViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 商户名
        /// </summary>
        public string Shopname { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }
        
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 所在地：身份
        /// </summary>
        public string ProvName { get; set; }

        /// <summary>
        /// 所在地：城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 所在地：区/县
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ShopQueryModel :QueryModel
    {
        /// <summary>
        /// 商户名
        /// </summary>
        public string Shopname { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 所在地：身份
        /// </summary>
        public int? Provid { get; set; }

        /// <summary>
        /// 所在地：城市
        /// </summary>
        public int? Cityid { get; set; }
        
        /// <summary>
        /// 所在地：区/县
        /// </summary>
        public string Area { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemShop
    {
        /// <summary>
        /// shopid
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 商户名
        /// </summary>
        public string Text { get; set; }
    }
}
