using System;
using System.Collections.Generic;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Customer.BusinessEntity
{
    /// <summary>
    /// C用户基本信息
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员名
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        /// 固话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }
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
        public int? Countyid { get; set; }

        /// <summary>
        /// 性别 1男2女
        /// </summary>
        public short? Sex { get; set; }
        
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Brithday { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 个人签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int? Totalpoints { get; set; }
        
        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
        
        /// <summary>
        /// 添加人
        /// </summary>
        public string Createrid { get; set; }
        
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// c用户注册
    /// </summary>
    public class UserRegModel : UserModel
    {

        /// <summary>
        /// 验证码
        /// </summary>
        public string VCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserQueryModel : QueryModel
    {
        /// <summary>
        /// 会员名
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        /// 固话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

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
        public int? Countyid { get; set; }

        /// <summary>
        /// 性别 1男2女
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Brithday { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 个人签名
        /// </summary>
        public string Signature { get; set; }
        
        /// <summary>
        /// 总积分
        /// </summary>
        public int? Totalpoints { get; set; }
        
        /// <summary>
        /// 注册时间-start
        /// </summary>
        public DateTime? CreatedtimeS { get; set; }

        /// <summary>
        /// 注册时间-end
        /// </summary>
        public DateTime? CreatedtimeE { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserListModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员名
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }
        
        /// <summary>
        /// 性别 1男2女
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Brithday { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
        
    }

    /// <summary>
    /// C用户登录信息
    /// </summary>
    public class UserLoginInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VCode { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }

    /// <summary>
    /// C修改密码model
    /// </summary>
    public class UserRetrievePassword
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VCode { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class UserViewModel : UserModel
    {
        /// <summary>
        /// 会员所在省份
        /// </summary>
        public string ProvName { get; set; }

        /// <summary>
        /// 会员所在城市
        /// </summary>
        public string CityName { get; set; }
        
        /// <summary>
        /// 会员所在区县
        /// </summary>
        public string CountyName { get; set; }
    }
}