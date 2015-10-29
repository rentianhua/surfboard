using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.CustRelations.BusinessEntity
{
    /// <summary>
    /// 社交圈搜车model
    /// </summary>
    public class CustRelationsCarViewModel : CustViewModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string model_name { get; set; }
        
        /// <summary>
        /// 城市
        /// </summary>
        public string custcityname { get; set; }
        
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public class CustRelationsCarQueryModel : QueryModel
    {
        /// <summary>
        /// 省份id
        /// </summary>
        public int? provid { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int? cityid { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int? brand_id { get; set; }

        /// <summary>
        /// 车系id
        /// </summary>
        public int? series_id { get; set; }

        /// <summary>
        /// 车型id
        /// </summary>
        public int? model_id { get; set; }

        /// <summary>
        /// 会员省份id
        /// </summary>
        public int? custprovid { get; set; }

        /// <summary>
        /// 会员城市id
        /// </summary>
        public int? cuscityid { get; set; }

        /// <summary>
        /// 会员城市id
        /// </summary>
        public string cusarea { get; set; }

    }
}
