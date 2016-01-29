using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Customer.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        public string OperName { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public string OriginalRegistCapi { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 公司状态
        /// </summary>
        public string CompanyStatus { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        public string Spare1 { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        public string Spare2 { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 添加人id
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CompanyListModel : CompanyModel
    {
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseNum { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CompanyViewModel : CompanyModel
    {
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseNum { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CompanyQueryModel : QueryModel
    {
        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

    }
    
    /// <summary>
    /// 评论model
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public string Companyid { get; set; }

        /// <summary>
        /// 评论人手机号
        /// </summary>
        public long Mobile { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Headportrait { get; set; }

        /// <summary>
        /// 评论分数
        /// </summary>
        public float? Score { get; set; }

        /// <summary>
        /// 评论人ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Commentdesc { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommentListModel : CommentModel
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string CompanyName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommentQueryModel : QueryModel
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string Companyid { get; set; }
    }

    /// <summary>
    /// 点赞model
    /// </summary>
    public class PraiseModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public string Companyid { get; set; }
        
        /// <summary>
        /// 点赞人ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 点赞人地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 备用字段
        /// </summary>
        public string Spare1 { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        public string Spare2 { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }
    }
}
