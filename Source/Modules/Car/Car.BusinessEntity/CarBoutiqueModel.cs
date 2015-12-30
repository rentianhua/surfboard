using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Car.BusinessEntity
{
    #region 精品店基本信息
    
    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Enterprisename { get; set; }

        /// <summary>
        /// 公司LOGO
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        public string Tempid { get; set; }

        /// <summary>
        /// 自定义排序字段
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 自定义信息{"website":"公司网址","wechatnumber":"微信公众号","wechatqrcode":"微信二维码","":"","":""}
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueListModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Enterprisename { get; set; }

        /// <summary>
        /// 公司LOGO
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        public string Tempid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueQueryModel : QueryModel
    {
        /// <summary>
        /// 省份id
        /// </summary>
        public int? Provid { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int? Cityid { get; set; }
    }

    #endregion

    #region 精品店模板信息

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueTempModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Tempname { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 模板链接
        /// </summary>
        public string Pageurl { get; set; }

        /// <summary>
        /// 预览链接
        /// </summary>
        public string Previewurl { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueTempListModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Tempname { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 模板链接
        /// </summary>
        public string Pageurl { get; set; }

        /// <summary>
        /// 预览链接
        /// </summary>
        public string Previewurl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiqueTempQueryModel : QueryModel
    {
        
    }

    #endregion

    #region 精品店模板信息

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiquePlateSetModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Tempname { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 模板链接
        /// </summary>
        public string Pageurl { get; set; }

        /// <summary>
        /// 预览链接
        /// </summary>
        public string Previewurl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiquePlateSetListModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Tempname { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduces { get; set; }

        /// <summary>
        /// 模板链接
        /// </summary>
        public string Pageurl { get; set; }

        /// <summary>
        /// 预览链接
        /// </summary>
        public string Previewurl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarBoutiquePlateSetQueryModel : QueryModel
    {

    }

    #endregion
}
