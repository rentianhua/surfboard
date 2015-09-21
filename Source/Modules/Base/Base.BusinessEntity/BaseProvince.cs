
namespace CCN.Modules.Base.BusinessEntity
{
    public class BaseProvince
    {
        /// <summary>
        /// id
        /// </summary>
        public int Innerid { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
