using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 贷款申请实体
    /// </summary>
    public class FinanceProgrammeModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal? amount { get; set; }

        /// <summary>
        /// 车龄
        /// </summary>
        public int? coty { get; set; }

        /// <summary>
        /// 里程
        /// </summary>
        public decimal? mileage { get; set; }

        /// <summary>
        /// 贷款期限（月）
        /// </summary>
        public string loanterm { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal? interestrate { get; set; }

        /// <summary>
        /// 客户地区 1shenf
        /// </summary>
        public int? customerpro { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string applicant { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? applytime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modifiedid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdtime { get; set; }

        /// <summary>
        /// 身份证图片
        /// </summary>
        public string identitypic { get; set; }

        /// <summary>
        /// 驾驶证图片
        /// </summary>
        public string driverspic { get; set; }

        /// <summary>
        /// 银行卡图片
        /// </summary>
        public string bankpic { get; set; }
        
    }

    /// <summary>
    /// 金融方案查询实体
    /// </summary>
    public class FinanceProgrammeQueryModel : QueryModel
    {
        /// <summary>
        /// 联系电话
        /// </summary>
        public string mobile { get; set; }
    }

    /// <summary>
    /// 金融方案展示实体
    /// </summary>
    public class FinanceProgrammeViewModel : FinanceProgrammeModel
    {
        /// <summary>
        /// 方案明细
        /// </summary>
        public IEnumerable<FinanceProgrammeDetailModel> financeprogrammedetail { get; set; }
    }

    /// <summary>
    /// 经融方案详情
    /// </summary>
    public class FinanceProgrammeDetailModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 金融ID
        /// </summary>
        public string financeid { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public decimal? interestrate { get; set; }
        /// <summary>
        /// 类型 方案一 方案二 方案三
        /// </summary>
        public int? type { get; set; }

        /// <summary>
        /// 状态 1保存 2提交 3通过 4未通过 5结束
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 图片信息（json格式）
        /// </summary>
        public string describepic { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modifiedid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdtime { get; set; }

    }
}
