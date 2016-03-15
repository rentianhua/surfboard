using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// app广告管理model
    /// </summary>
    public class BaseBannerModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Picurl { get; set; }

        /// <summary>
        /// 转跳链接
        /// </summary>
        public string Linkurl { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? Enabletime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? Disabletime { get; set; }

        /// <summary>
        /// 转跳链接
        /// </summary>
        public short? Sort { get; set; }

        /// <summary>
        /// 是否启用[1.启用，0.禁用],默认1
        /// </summary>
        public short? Isenabled { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string Modifierid { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseBannerPageListModel : BaseBannerModel
    {

    }

    /// <summary>
    /// app广告管理model
    /// </summary>
    public class BaseBannerListModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Picurl { get; set; }

        /// <summary>
        /// 转跳链接
        /// </summary>
        public string Linkurl { get; set; }
    }


    /// <summary>
    /// app广告管理model
    /// </summary>
    public class BaseBannerQueryModel : QueryModel
    {

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否启用[1.启用，0.禁用],默认1
        /// </summary>
        public short? Isenabled { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? Enabletime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? Disabletime { get; set; }

    }
}
