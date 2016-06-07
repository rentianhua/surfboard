using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Customer.BusinessEntity
{
    /// <summary>
    /// 投诉建议
    /// </summary>
    public class SiteAdviceModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 投诉或建议
        /// </summary>
        public string Advice { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
    }
}
