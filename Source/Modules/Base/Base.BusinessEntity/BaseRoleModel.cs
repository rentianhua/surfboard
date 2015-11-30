using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class BaseRoleModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? isenabled { get; set; }
        
    }

    /// <summary>
    /// 角色显示实体
    /// </summary>
    public class BaseRoleViewModel: BaseRoleModel
    {
        
    }

    /// <summary>
    /// 角色查询实体
    /// </summary>
    public class BaseRoleQueryModel : QueryModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? isenabled { get; set; }
    }
}
