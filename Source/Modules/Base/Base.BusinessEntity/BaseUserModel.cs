using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class BaseUserModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 状态 0禁用 1启用
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modifiedtime { get; set; }

    }

    /// <summary>
    /// 用户查询实体
    /// </summary>
    public class BaseUserQueryModel : QueryModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 状态 0禁用 1启用
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modifiedtime { get; set; }
    }
}
