using Cedar.Framework.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆悬赏
    /// </summary>
    public class CarReward
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public int brand_id { get; set; }

        /// <summary>
        /// 车系
        /// </summary>
        public int series_id { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public int model_id { get; set; }

        /// <summary>
        /// 里程区间
        /// </summary>
        public string mileage { get; set; }

        /// <summary>
        /// 车龄区间
        /// </summary>
        public string coty { get; set; }

        /// <summary>
        /// 价格区间
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int colorid { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int provid { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public int cityid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string usermobile { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string qrcode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string createdid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modifiedtime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 预留字段1
        /// </summary>
        public string selffield1 { get; set; }
    }

    /// <summary>
    /// 车辆悬赏查询实体
    /// </summary>
    public class CarRewardQueryModel : QueryModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string innerid { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public int brand_id { get; set; }

        /// <summary>
        /// 车系
        /// </summary>
        public int series_id { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public int model_id { get; set; }

        /// <summary>
        /// 里程区间
        /// </summary>
        public string mileage { get; set; }

        /// <summary>
        /// 车龄区间
        /// </summary>
        public string coty { get; set; }

        /// <summary>
        /// 价格区间
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int colorid { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int provid { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public int cityid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string usermobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string createdid { get; set; }

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
    /// 车辆悬赏显示实体
    /// </summary>
    public class CarRewardViewModel : CarReward
    { }
}
