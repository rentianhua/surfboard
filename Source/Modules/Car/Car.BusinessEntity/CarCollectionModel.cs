using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 会员收藏车辆model
    /// </summary>
    public class CarCollectionModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

    }

    /// <summary>
    /// 会员收藏车辆model
    /// </summary>
    public class CarCollectionQueryModel:QueryModel
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string Custid { get; set; }

    }

    /// <summary>
    /// 会员收藏车辆model
    /// </summary>
    public class CarCollectionViewListModel : CarInfoListViewModel
    {
        /// <summary>
        /// 收藏备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime? Collectiontime { get; set; }

    }
}
