using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 角色对应的权限
    /// </summary>
    public class BaseRoleMenuModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string roleid { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string menuid { get; set; }
    }
}
