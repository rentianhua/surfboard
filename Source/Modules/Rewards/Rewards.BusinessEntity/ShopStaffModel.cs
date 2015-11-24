using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Rewards.BusinessEntity
{
    /// <summary>
    /// 商铺的职员信息
    /// </summary>
    public class ShopStaffModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        public string Shopid { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 职员名称
        /// </summary>
        public string Staffname { get; set; }

        /// <summary>
        /// 性别 1男2女0未知
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }
        
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
    /// 商铺的职员信息
    /// </summary>
    public class ShopStaffQueryModel : QueryModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }
        
        /// <summary>
        /// 职员名称
        /// </summary>
        public string Staffname { get; set; }

        /// <summary>
        /// 性别 1男2女0未知
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }
    }

    /// <summary>
    /// 商铺的职员信息
    /// </summary>
    public class ShopStaffViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Loginname { get; set; }

        /// <summary>
        /// 职员名称
        /// </summary>
        public string Staffname { get; set; }

        /// <summary>
        /// 性别 1男2女0未知
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Shopname { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StaffLoginInfo
    {
        /// <summary>
        /// Loginname
        /// </summary>
        public string Loginname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 职员信息and商户信息
    /// </summary>
    public class ShopStaffInfo
    {
        /// <summary>
        /// 职员信息
        /// </summary>
        public ShopStaffModel StaffModel { get; set; }

        /// <summary>
        /// 职员信息
        /// </summary>
        public ShopModel ShopModel { get; set; }
    }
}
