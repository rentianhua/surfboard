using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 部门表
    /// </summary>
    public class BaseDepartmentModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 区域ID    
        /// </summary>
        public string areaid { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public int? code { get; set; }

    }

    /// <summary>
    /// 部门查询实体
    /// </summary>
    public class BaseDepartmentQueryModel : QueryModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 区域ID    
        /// </summary>
        public string areaid { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public int? code { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseDepartmentViewModel: BaseDepartmentModel
    { }
}
