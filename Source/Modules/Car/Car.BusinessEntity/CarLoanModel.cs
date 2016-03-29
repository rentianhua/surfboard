using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车贷实体
    /// </summary>
    public class CarLoanModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 期限（月数）
        /// </summary>
        public int? term { get; set; }

        /// <summary>
        /// 贷款说明
        /// </summary>
        public string instruction { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modifiedid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string modifiedtime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string createdtime { get; set; }
    }

    /// <summary>
    /// 车贷查询实体
    /// </summary>
    public class CarLoanQueryModel : QueryModel
    { }

    /// <summary>
    /// 车贷显示实体
    /// </summary>
    public class CarLoanViewModel : CarLoanModel
    { }
}
