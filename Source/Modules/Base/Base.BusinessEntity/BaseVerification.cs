using System;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseVerification
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 手机号/emial
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 接受类型 1.手机，2.邮件
        /// </summary>
        public int TType { get; set; }

        /// <summary>
        /// 验证码值
        /// </summary>
        public string Vcode { get; set; }

        /// <summary>
        /// 效期[单位：分] ,默认20分钟
        /// </summary>
        public int Valid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Createdtime { get; set; }

        /// <summary>
        /// 验证码长度，默认4位
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 验证码类型 [1.纯数字，2.数据+字母，...]，默认纯数字
        /// </summary>
        public int VType { get; set; }

        /// <summary>
        /// 发送结果 1.成功，0.失败
        /// </summary>
        public int Result { get; set; }

        public BaseVerification(){
            Length = 4;
            Valid = 20;
            VType = 1;
            TType = 1;
            Result = 1;
        }
    }
}
