using System;

namespace CCN.Modules.Customer.BusinessEntity
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class CustModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 用户名
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
        /// 头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 认证状态[0.未提交认证，1.提交认证(待审核)，2.审核通过，3.审核不过，4.重新填写(重新提交认证)]
        /// </summary>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 类型1.车商，2,个人
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int Totalpoints { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 用户微信信息
        /// </summary>
        public CustWechat Wechat { get; set; }
    }

    /// <summary>
    /// 用户微信信息
    /// </summary>
    public class CustWechat
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 微信公众号id
        /// </summary>
        public string Accountid { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Cex { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifiedtime { get; set; }
    }

    /// <summary>
    /// 用户认证信息
    /// </summary>
    public class CustAuthenticationModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string Idcard { get; set; }

        /// <summary>
        /// 身份证上名称
        /// </summary>
        public string Idname { get; set; }

        /// <summary>
        /// 手持身份证照片
        /// </summary>
        public string Idpicture { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 公司法人
        /// </summary>
        public string Legalperson { get; set; }

        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string Organizationcode { get; set; }

        /// <summary>
        /// 组织机构代码证照片
        /// </summary>
        public string Organizationpicture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
    }
    
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class CustLoginInfo
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
        /// 登录时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}