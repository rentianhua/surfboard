using System;
using System.Collections.Generic;
using Cedar.Framework.Common.BaseClasses;

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
        /// 会员名
        /// </summary>
        public string Custname { get; set; }

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
        public int? Status { get; set; }

        /// <summary>
        /// 认证状态[0.未提交认证，1.提交认证(待审核)，2.审核通过，3.审核不过，4.重新填写(重新提交认证)]
        /// </summary>
        public int? AuthStatus { get; set; }

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
        /// 会员等级
        /// </summary>
        public int? Level { get; set; }
        
        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 类型1.车商，2,个人
        /// </summary>
        public int? Type { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
        
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string VCode { get; set; }

        /// <summary>
        /// 用户微信信息
        /// </summary>
        public CustWechat Wechat { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string RecommendedId { get; set; }

        /// <summary>
        /// 车辆数量
        /// </summary>
        public int? CarNum { get; set; }

        /// <summary>
        /// VIP有效期
        /// </summary>
        public DateTime? expirestime { get; set; }
    }

    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class CustQueryModel : QueryModel
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public string innerid { get; set; }
        /// <summary>
        /// 会员名
        /// </summary>
        public string Custname { get; set; }

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
        /// 用户状态[1.正常，2.冻结]
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 认证状态[0.未提交认证，1.提交认证(待审核)，2.审核通过，3.审核不过，4.重新填写(重新提交认证)]
        /// </summary>
        public int? AuthStatus { get; set; }

        /// <summary>
        /// 所在地：身份
        /// </summary>
        public int Provid { get; set; }

        /// <summary>
        /// 所在地：城市
        /// </summary>
        public int Cityid { get; set; }

        /// <summary>
        /// 所在地：区/县
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 性别 1男2女
        /// </summary>
        public short Sex { get; set; }

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
        /// 类型1.车商，2,个人
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int? Totalpoints { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int? Level { get; set; }

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
        /// 性别 1.男2女
        /// </summary>
        public short Sex { get; set; }

        /// <summary>
        /// 是不删除(1:删除;0:未删除)
        /// </summary>
        public short Isdel { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarkname { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Subscribe_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Subscribe { get; set; }

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
        public DateTime? Createdtime { get; set; }

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
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string Idcard { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string Enterprisename { get; set; }

        /// <summary>
        /// 工商营业执照注册号
        /// </summary>
        public string Licencecode { get; set; }

        /// <summary>
        /// 营业执照所在地
        /// </summary>
        public string Licencearea { get; set; }

        /// <summary>
        /// 组织机构代码号
        /// </summary>
        public string Organizationcode { get; set; }
        
        /// <summary>
        /// 税务登记证号
        /// </summary>
        public string Taxcode { get; set; }

        /// <summary>
        /// 相关证件图片
        /// </summary>
        public string Relevantpicture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 审核人id
        /// </summary>
        public string AuditPer { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 审核描述
        /// </summary>
        public string AuditDesc { get; set; }

        /// <summary>
        /// 审核结果 [1.审核通过，0.审核不过]
        /// </summary>
        public int? AuditResult { get; set; }

    }
    
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class CustLoginInfo
    {
        /// <summary>
        /// 会员
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
    /// 修改密码model
    /// </summary>
    public class CustRetrievePassword
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
    /// 修改密码model
    /// </summary>
    public class CustModifyPassword
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }

    /// <summary>
    /// 会员相关总数信息
    /// </summary>
    public class CustTotalModel
    {
        /// <summary>
        /// 内部id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 当前积分
        /// </summary>
        public int Currpoint { get; set; }

        /// <summary>
        /// 当前礼券数
        /// </summary>
        public int Currpouponnum { get; set; }

        /// <summary>
        /// 剩余刷新次数
        /// </summary>
        public int Refreshnum { get; set; }

        /// <summary>
        /// 剩余置顶次数
        /// </summary>
        public int Topnum { get; set; }
    }

    /// <summary>
    /// 会员相关总数变更记录
    /// </summary>
    public class CustTotalRecordModel
    {
        /// <summary>
        /// 内部id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 类型：1.刷新次数，2.置顶数次，3.积分调整
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Spare1 { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? Creatertime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustViewModel : CustModel
    {
        /// <summary>
        /// 会员所在城市
        /// </summary>
        public string CityName { get; set; }


        /// <summary>
        /// 会员所在省份
        /// </summary>
        public string ProvName { get; set; }

        /// <summary>
        /// 会员总数信息
        /// </summary>
        public CustTotalModel TotalInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeleteCustAllPic
    {
        /// <summary>
        /// 会员二维码
        /// </summary>
        public string Qrcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AuthPic { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<string> CarPicList { get; set; }

        /// <summary>
        /// code列表
        /// </summary>
        public List<string> CodeList { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CustWxPayModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单号二维码
        /// </summary>
        public string OrderNoQrCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
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
    /// 新粉丝重新绑定会员model
    /// </summary>
    public class CustRebindFansModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string Openid { get; set; }
    }
}