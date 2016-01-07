using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseUserDepartmentModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseUserDepartmentAddModel: BaseUserDepartmentModel
    {
        /// <summary>
        /// 城市ID集合
        /// </summary>
        public List<string> ids { get; set; }
    }

}
