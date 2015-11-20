using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 基础数据代码类型
    /// </summary>
    public class BaseCodeTypeModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }
        /// <summary>
        /// 代码类型key
        /// </summary>
        public string Typekey { get; set; }
        /// <summary>
        /// 代码类型名称
        /// </summary>
        public string Typename { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int? Isenabled { get; set; }
    }
    /// <summary>
    /// 基础数据代码类型展示信息
    /// </summary>
    public class BaseCodeTypeListModel : BaseCodeTypeModel
    {
    }
    /// <summary>
    /// 基础数据代码类型查询条件
    /// </summary>
    public class BaseCodeTypeQueryModel : QueryModel {
        /// <summary>
        /// 代码类型名称
        /// </summary>
        public string Typename { get; set; }
        /// <summary>
        /// 代码类型key
        /// </summary>
        public string Typekey { get; set; }
    }
}
