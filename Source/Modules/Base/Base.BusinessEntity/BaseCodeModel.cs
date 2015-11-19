using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 代码值
    /// </summary>
    public class BaseCodeModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 代码值
        /// </summary>
        public string CodeValue { get; set; }

        /// <summary>
        /// 代码名称
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 代码类型key
        /// </summary>
        public string Typekey { get; set; }
     

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 代码值
    /// </summary>
    public class BaseCodeSelectModel:BaseCodeModel 
    {
        /// <summary>
        /// 代码类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
    /// <summary>
    /// 基础数据代码值查询条件
    /// </summary>
    public class BaseCodeQueryModel:QueryModel {
        /// <summary>
        /// 代码类型key
        /// </summary>
        public string Typekey { get; set; }
        /// <summary>
        /// 代码名称
        /// </summary>
        public string CodeName { get; set; }
    }
}
