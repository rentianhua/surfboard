using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    public class CarInfoModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 聚合数据车辆id
        /// </summary>
        public int Carid { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 指导价
        /// </summary>
        public string pic_url { get; set; }

        /// <summary>
        /// 省份id
        /// </summary>
        public int provid { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int cityid { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public int brand_id { get; set; }

        /// <summary>
        /// 车系id
        /// </summary>
        public int series_id { get; set; }

        /// <summary>
        /// 车型id
        /// </summary>
        public int model_id { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public float price { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public int mileageid { get; set; }

        /// <summary>
        /// 车辆上牌日期
        /// </summary>
        public DateTime? register_date { get; set; }

        /// <summary>
        /// 注册年份
        /// </summary>
        public int reg_year { get; set; }

        /// <summary>
        /// 变速箱id
        /// </summary>
        public int gearid { get; set; }

        /// <summary>
        /// 车龄[和上牌时间冲突，备用]
        /// </summary>
        public int carageid { get; set; }

        /// <summary>
        /// 排量[车型基础数据中有，备用]
        /// </summary>
        public int literid { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int colorid { get; set; }

        /// <summary>
        /// 车身结构
        /// </summary>
        public int carshructid { get; set; }

        /// <summary>
        /// 排放标准
        /// </summary>
        public int dischargeid { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string contactor { get; set; }

        /// <summary>
        /// 商家id
        /// </summary>
        public int dealer_id { get; set; }

        /// <summary>
        /// 卖家类型，1表示个人，2表示商家
        /// </summary>
        public int seller_type { get; set; }

        /// <summary>
        /// 状态[1.待发布，2.待审核，3.saling，4.已售，5.已存档]
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// createdtime
        /// </summary>
        public DateTime? createdtime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? modifiedtime { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? post_time { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? audit_time { get; set; }

        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime? sold_time { get; set; }

        /// <summary>
        /// 存档时间
        /// </summary>
        public DateTime? keep_time { get; set; }

        /// <summary>
        /// 车源估值
        /// </summary>
        public float eval_price { get; set; }

        /// <summary>
        /// 明年评估价格
        /// </summary>
        public float next_year_eval_price { get; set; }

        /// <summary>
        /// 性价比
        /// </summary>
        public float vpr { get; set; }

        /// <summary>
        /// 交强险到期时间
        /// </summary>
        public DateTime? tlci_date { get; set; }

        /// <summary>
        /// 商业险到期时间
        /// </summary>
        public DateTime? audit_date { get; set; }
        
        /// <summary>
        /// 车源详细信息链接(che300)
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string custid { get; set; }

        #region code name

        /// <summary>
        /// 省份
        /// </summary>
        public string provname { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string cityname { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string brand_name { get; set; }

        /// <summary>
        /// 车系
        /// </summary>
        public string series_name { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string model_name { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public string mile_age { get; set; }

        /// <summary>
        /// 变速箱类型
        /// </summary>
        public string gear_type { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        public string liter { get; set; }

        /// <summary>
        /// 车龄
        /// </summary>
        public string caragename { get; set; }

        /// <summary>
        /// 车身结构
        /// </summary>
        public string carshructName { get; set; }

        /// <summary>
        /// 排放标准
        /// </summary>
        public string dischargeName { get; set; }
        
        #endregion
    }
}
