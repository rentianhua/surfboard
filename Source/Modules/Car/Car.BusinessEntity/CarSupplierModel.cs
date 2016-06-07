using System;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class CarSupplierModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Suppliername { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 供应商简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Contactsphone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createrid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Modifierid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modifiedtime { get; set; }
    }

    /// <summary>
    /// 下拉框model
    /// </summary>
    public class CarSupplierDdlModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Suppliername { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarSupplierQueryModel : QueryModel
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Suppliername { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Contactsphone { get; set; }
    }

}
