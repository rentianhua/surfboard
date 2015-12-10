using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Base.BusinessEntity
{
    /// <summary>
    /// 菜单基本实体
    /// </summary>
    public class BaseMenuModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        public string parentid { get; set; }

        /// <summary>
        /// 菜单级别
        /// </summary>
        public int? level { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public int? openmode { get; set; }

        /// <summary>
        /// 是否启用 1启用 0禁用
        /// </summary>
        public int? isenabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 是否有子菜单
        /// </summary>
        public int? submenu { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modifiedtime { get; set; }
    }

    /// <summary>
    /// 菜单展现实体
    /// </summary>
    public class MenuViewMode : BaseMenuModel
    {

    }

    /// <summary>
    /// 菜单查询实体
    /// </summary>
    public class MenuQueryModel : QueryModel
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 菜单级别
        /// </summary>
        public int? level { get; set; }
    }

   
    public class XmlConfig
    {
        public List<MenuGroup> menugroups { get; set; }
    }

    /// <summary>
    /// 一级菜单
    /// </summary>
    public class MenuGroup
    {
        public string name { get; set; }
        public string icon { get; set; }
        public int submenu { get; set; }
        public string url { get; set; }
        public List<paramsItem> MenuArray { get; set; }
    }

    /// <summary>
    /// 二级菜单
    /// </summary>
    public class paramsItem
    {
        public string name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string info { get; set; }
        public int submenu { get; set; }
        public List<paramsSub> MenuArray { get; set; }
    }

    /// <summary>
    /// 三级菜单
    /// </summary>
    public class paramsSub
    {
        public string name { get; set; }
        public string url { get; set; }
        public string info { get; set; }
        public string icon { get; set; }
    }
}
