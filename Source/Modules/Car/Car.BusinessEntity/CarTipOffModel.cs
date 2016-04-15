using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆举报信息
    /// </summary>
    public class CarTipOffModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tipoffname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tipoffphone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Handlecontent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Handledtime { get; set; }
    }

    /// <summary>
    /// 车辆举报信息
    /// </summary>
    public class CarTipQueryModel: QueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Carid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Tipoffphone { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int? Status { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartCreatedtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndCreatedtime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CarTipHandleModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Innerid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Handlecontent { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Handledtime { get; set; }
        
        /// <summary>
        /// 0,删除，2.处理了
        /// </summary>
        public int? Status { get; set; }
    }
}
