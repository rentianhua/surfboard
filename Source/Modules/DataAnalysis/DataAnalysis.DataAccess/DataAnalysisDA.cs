using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.DataAnalysis.BusinessEntity;
using Cedar.Framework.Common.BaseClasses;
using Dapper;
using System.Data;

namespace CCN.Modules.DataAnalysis.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAnalysisDA : DataAnalysisDataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DataAnalysisDA()
        {

        }

        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetLocalByMonthTop10()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select count(*) as cnt,month(post_time) as monthflg,model_id from car_info_bak 
                            where cityid=125 AND month(post_time)=10 group by monthflg,model_id order by cnt desc
                            limit 0, 10; ");

            var list = Helper.Query<dynamic>(sql.ToString());
            return list;
        }

        #region 买家分布

        /// <summary>
        /// 获取不同年龄段买家分布
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAgeArea()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "First";
            da1.value = "1";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "Second";
            da2.value = "2";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "Third";
            da3.value = "3";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "Fourth";
            da4.value = "2";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "Fifth";
            da5.value = "1";
            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);

            return list;
        }

        /// <summary>
        /// 获取买家性别比例
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetGenterPer()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "Male";
            da1.value = "87";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "Female";
            da2.value = "13";

            list.Add(da1);
            list.Add(da2);

            return list;
        }

        /// <summary>
        /// 职业分布
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetOccupationPer()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "工人";
            da1.value = "27.62";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "技术人员";
            da2.value = "22.97";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "销售人员";
            da3.value = "11.76";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "专业人士";
            da4.value = "6.24";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "文职/办事人员";
            da5.value = "4.42";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "教师";
            da6.value = "1.89";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "在校学生";
            da7.value = "0.87";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "其他";
            da8.value = "24.23";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);

            return list;
        }

        #endregion

        #region 2015年交易额交易量折线图

        /// <summary>
        /// 年度市场交易走势
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTradeLineByYear()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "2014年12月";
            da1.value = "60.18";
            da1.value2 = "399.39";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "2015年1月";
            da2.value = "49.04";
            da2.value2 = "279.73";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "2015年2月";
            da3.value = "36.58";
            da3.value2 = "196.81";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "2015年3月";
            da4.value = "52.72";
            da4.value2 = "292.06";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "2015年4月";
            da5.value = "54.89";
            da5.value2 = "305.57";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "2015年5月";
            da6.value = "52";
            da6.value2 = "303.63";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "2015年6月";
            da7.value = "76.03";
            da7.value2 = "447.63";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "2015年7月";
            da8.value = "77.05";
            da8.value2 = "461.68";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "2015年8月";
            da9.value = "74.79";
            da9.value2 = "426.5";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);

            return list;
        }

        /// <summary>
        /// 2014-2015二手车交易量月度统计表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTradeTotalByMonth()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "2014/02";
            da1.value = "32.95";
            da1.value2 = "15.25";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "2014/03";
            da2.value = "51.37";
            da2.value2 = "5.48";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "2014/04";
            da3.value = "50.15";
            da3.value2 = "8.46";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "2014/05";
            da4.value = "48.71";
            da4.value2 = "0";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "2014/06";
            da5.value = "50.43";
            da5.value2 = "24.15";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "2014/07";
            da6.value = "52.38";
            da6.value2 = "17.79";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "2014/08";
            da7.value = "48.98";
            da7.value2 = "10.74";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "2014/09";
            da8.value = "54.06";
            da8.value2 = "0";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "2014/10";
            da9.value = "53.28";
            da9.value2 = "0";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "2014/02";
            da10.value = "53.91";
            da10.value2 = "16.29";
            DataAnalysisModel da11 = new DataAnalysisModel();
            da11.key = "2014/03";
            da11.value = "60.18";
            da11.value2 = "12.76";
            DataAnalysisModel da12 = new DataAnalysisModel();
            da12.key = "2014/04";
            da12.value = "49.04";
            da12.value2 = "0";
            DataAnalysisModel da13 = new DataAnalysisModel();
            da13.key = "2014/05";
            da13.value = "36.58";
            da13.value2 = "11.02";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);
            list.Add(da11);
            list.Add(da12);
            list.Add(da13);

            return list;
        }

        #endregion

        #region 二手车属性

        /// <summary>
        /// 二手车使用年限分析
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsedCarYearAnalysis()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "3年内";
            da1.value = "18.32";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "3-6年";
            da2.value = "67.25";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "6-10年";
            da3.value = "5.38";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "10年以上";
            da4.value = "9.05";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);

            return list;
        }

        /// <summary>
        /// 可接受的二手车行驶里程
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsedCarAccept()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "<=1万公里";
            da1.value = "17.4";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "1-3万公里";
            da2.value = "28.5";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "3-5万公里";
            da3.value = "31.7";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "5-10万公里";
            da4.value = "17.4";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "10-15万公里";
            da5.value = "3.8";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = ">15万公里";
            da6.value = "17.4";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);

            return list;
        }

        /// <summary>
        /// 3-5万公里满意度
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSatisfaction3To5()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "丰田";
            da1.value = "37.1";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "日产";
            da2.value = "37";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "现代";
            da3.value = "36.2";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "马自达";
            da4.value = "36.1";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "福特";
            da5.value = "36";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);

            return list;
        }

        /// <summary>
        /// 3-5万公里不满意度
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUnSatisfaction3To5()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "路虎";
            da1.value = "29.7";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "奥迪";
            da2.value = "28.9";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "金杯";
            da3.value = "27.9";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "吉利";
            da4.value = "26.5";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "保时捷";
            da5.value = "25.5";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);

            return list;
        }

        /// <summary>
        /// 1-3万公里满意度
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSatisfaction1To3()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "长城";
            da1.value = "32.8";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "雪弗兰";
            da2.value = "32.1";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "大众";
            da3.value = "32.0";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "奥迪";
            da4.value = "31.5";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "保时捷";
            da5.value = "30";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);

            return list;
        }

        /// <summary>
        /// 1-3万公里不满意度
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUnSatisfaction1To3()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "比亚迪";
            da1.value = "27.2";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "现代";
            da2.value = "26.5";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "本田";
            da3.value = "25.7";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "日产";
            da4.value = "24.7";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "丰田";
            da5.value = "24.2";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);

            return list;
        }

        /// <summary>
        /// 供应量占比
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSupplyPer()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "黑";
            da1.value = "28";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "白";
            da2.value = "19";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "银";
            da3.value = "18";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "红";
            da4.value = "10";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "灰";
            da5.value = "9";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "蓝";
            da6.value = "8";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "金";
            da7.value = "3";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "绿";
            da8.value = "2";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "黄";
            da9.value = "2";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "橙";
            da10.value = "2";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);

            return list;
        }

        #endregion

        #region 品牌热度排行

        /// <summary>
        /// 月度品牌热搜前10
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetHotBrandTop10()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "大众";
            da1.value = "18";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "丰田";
            da2.value = "14";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "奥迪";
            da3.value = "12";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "宝马";
            da4.value = "12";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "本田";
            da5.value = "11";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "奔驰";
            da6.value = "8";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "现代";
            da7.value = "8";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "别克";
            da8.value = "7";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "日产";
            da9.value = "6";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "其他";
            da10.value = "4";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);

            return list;
        }

        /// <summary>
        /// 1-7月交易量占比
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTradePer1To7()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "五菱荣光";
            da1.value = "5.77";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "大众捷达";
            da2.value = "2.85";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "长安之星";
            da3.value = "2.43";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "雪弗兰赛欧三厢";
            da4.value = "2.30";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "一汽夏利";
            da5.value = "2.29";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "本田雅阁";
            da6.value = "1.69";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "别克君威";
            da7.value = "1.62";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "奥迪A6L";
            da8.value = "1.38";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "雪铁龙富康";
            da9.value = "1.34";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "奇瑞QQ";
            da10.value = "1.31";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);

            return list;
        }

        #endregion

        #region 交易量

        /// <summary>
        /// 二手车交易量全国占比排行前10省份
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsedCarTradeTop10()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "广东";
            da1.value = "19.56";
            da1.value2 = "10.08";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "江苏";
            da2.value = "0.65";
            da2.value2 = "8.22";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "四川";
            da3.value = "13.09";
            da3.value2 = "7.79";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "浙江";
            da4.value = "5.25";
            da4.value2 = "7.41";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "北京";
            da5.value = "9.24";
            da5.value2 = "7.36";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "山东";
            da6.value = "7.07";
            da6.value2 = "6.31";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "河北";
            da7.value = "2.07";
            da7.value2 = "5.47";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "上海";
            da8.value = "6.22";
            da8.value2 = "4.36";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "辽宁";
            da9.value = "1.39";
            da9.value2 = "4.24";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "河南";
            da10.value = "7.54";
            da10.value2 = "3.88";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);

            return list;
        }

        /// <summary>
        /// 二手车交易量全国占比排行倒数8省份
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsedCarTradeLaset8()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "新疆";
            da1.value = "0.32";
            da1.value2 = "1.39";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "天津";
            da2.value = "1.26";
            da2.value2 = "1.31";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "湖南";
            da3.value = "1.22";
            da3.value2 = "1.22";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "宁夏";
            da4.value = "0.41";
            da4.value2 = "0.81";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "甘肃";
            da5.value = "0.4";
            da5.value2 = "0.81";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "海南";
            da6.value = "0.4";
            da6.value2 = "0.4";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "青海";
            da7.value = "0.59";
            da7.value2 = "0.17";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "西藏";
            da8.value = "0.12";
            da8.value2 = "0.12";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);

            return list;
        }

        /// <summary>
        /// 二手车近几年交易量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsedCarTradeRecentYears()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "2000";
            da1.value = "25";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "2001";
            da2.value = "37";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "2002";
            da3.value = "71";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "2003";
            da4.value = "88";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "2004";
            da5.value = "134";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "2005";
            da6.value = "145";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "2006";
            da7.value = "191";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "2007";
            da8.value = "266";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "2008";
            da9.value = "274";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "2009";
            da10.value = "334";
            DataAnalysisModel da11 = new DataAnalysisModel();
            da11.key = "2010";
            da11.value = "385";
            DataAnalysisModel da12 = new DataAnalysisModel();
            da12.key = "2011";
            da12.value = "433";
            DataAnalysisModel da13 = new DataAnalysisModel();
            da13.key = "2012";
            da13.value = "479";
            DataAnalysisModel da14 = new DataAnalysisModel();
            da14.key = "2013";
            da14.value = "520";
            DataAnalysisModel da15 = new DataAnalysisModel();
            da15.key = "2014";
            da15.value = "605";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);
            list.Add(da11);
            list.Add(da12);
            list.Add(da13);
            list.Add(da14);
            list.Add(da15);

            return list;
        }

        #endregion

        #region 个人收入分析

        /// <summary>
        /// 个人季度收入
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPersonalIncomeByQuarter()
        {

            //获取当前年份
            int yearflg = DateTime.Now.Year;

            const string sql = @"select sum(ifnull(dealprice, 0)) as value4, 
                                 quarter(sold_time) as `key` from car_info
                                    where year(sold_time) = @yearflg   
                                    group by `key` 
                                    order by `key` desc;";
            try
            {
                return Helper.Query<DataAnalysisModel>(sql, new { yearflg });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 个人月收入
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPersonalIncomeByMonth()
        {

            //获取当前年份
            int yearflg = DateTime.Now.Year;

            const string sql = @"select sum(ifnull(dealprice, 0)) as value4, month(sold_time) as `key`, 
                                 quarter(sold_time) as value from car_info
                                    where year(sold_time) = @yearflg   
                                    group by `key`, value 
                                    order by `key` desc;";
            try
            {
                return Helper.Query<DataAnalysisModel>(sql, new { yearflg });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 个人收入分析
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPersonalIncome()
        {

            List<DataAnalysisModel> list = new List<DataAnalysisModel>();
            List<DataAnalysisModel> list1 = new List<DataAnalysisModel>();
            List<DataAnalysisModel> list2 = new List<DataAnalysisModel>();
            List<DataAnalysisModel> list3 = new List<DataAnalysisModel>();
            List<DataAnalysisModel> list4 = new List<DataAnalysisModel>();


            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "一季度";
            da1.value = "85859";

            DataAnalysisModel daM1 = new DataAnalysisModel();
            daM1.key = "1";
            daM1.value = "35655";
            list1.Add(daM1);
            DataAnalysisModel daM2 = new DataAnalysisModel();
            daM2.key = "2";
            daM2.value = "12268";
            list1.Add(daM2);
            DataAnalysisModel daM3 = new DataAnalysisModel();
            daM3.key = "3";
            daM3.value = "37936";
            list1.Add(daM3);
            da1.value3 = list1;

            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "二季度";
            da2.value = "122286";
            DataAnalysisModel daM4 = new DataAnalysisModel();
            daM4.key = "4";
            daM4.value = "45634";
            list2.Add(daM4);
            DataAnalysisModel daM5 = new DataAnalysisModel();
            daM5.key = "5";
            daM5.value = "36566";
            list2.Add(daM5);
            DataAnalysisModel daM6 = new DataAnalysisModel();
            daM6.key = "6";
            daM6.value = "40086";
            list2.Add(daM6);
            da2.value3 = list2;

            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "三季度";
            da3.value = "123989";
            DataAnalysisModel daM7 = new DataAnalysisModel();
            daM7.key = "7";
            daM7.value = "38919";
            list3.Add(daM7);
            DataAnalysisModel daM8 = new DataAnalysisModel();
            daM8.key = "8";
            daM8.value = "45052";
            list3.Add(daM8);
            DataAnalysisModel daM9 = new DataAnalysisModel();
            daM9.key = "9";
            daM9.value = "40018";
            list3.Add(daM9);
            da3.value3 = list3;

            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "四季度";
            da4.value = "101525";
            DataAnalysisModel daM10 = new DataAnalysisModel();
            daM10.key = "10";
            daM10.value = "37536";
            list4.Add(daM10);
            DataAnalysisModel daM11 = new DataAnalysisModel();
            daM11.key = "11";
            daM11.value = "32835";
            list4.Add(daM11);
            DataAnalysisModel daM12 = new DataAnalysisModel();
            daM12.key = "12";
            daM12.value = "31154";
            list4.Add(daM12);
            da4.value3 = list4;

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);

            return list;
        }
        #endregion

        #region 汽车保有量

        /// <summary>
        /// 汽车保有量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetCarRetainQuantity()
        {
            List<DataAnalysisModel> list = new List<DataAnalysisModel>();

            DataAnalysisModel da1 = new DataAnalysisModel();
            da1.key = "北京";
            da1.value = "537.1";
            DataAnalysisModel da2 = new DataAnalysisModel();
            da2.key = "重庆";
            da2.value = "399.8";
            DataAnalysisModel da3 = new DataAnalysisModel();
            da3.key = "成都";
            da3.value = "336.1";
            DataAnalysisModel da4 = new DataAnalysisModel();
            da4.key = "深圳";
            da4.value = "290.5";
            DataAnalysisModel da5 = new DataAnalysisModel();
            da5.key = "上海";
            da5.value = "272.3";
            DataAnalysisModel da6 = new DataAnalysisModel();
            da6.key = "广州";
            da6.value = "269.5";
            DataAnalysisModel da7 = new DataAnalysisModel();
            da7.key = "天津";
            da7.value = "258.9";
            DataAnalysisModel da8 = new DataAnalysisModel();
            da8.key = "杭州";
            da8.value = "251.7";
            DataAnalysisModel da9 = new DataAnalysisModel();
            da9.key = "苏州";
            da9.value = "245";
            DataAnalysisModel da10 = new DataAnalysisModel();
            da10.key = "郑州";
            da10.value = "230.8";

            list.Add(da1);
            list.Add(da2);
            list.Add(da3);
            list.Add(da4);
            list.Add(da5);
            list.Add(da6);
            list.Add(da7);
            list.Add(da8);
            list.Add(da9);
            list.Add(da10);

            return list;
        }

        #endregion

        #region  日数据统计

        /// <summary>
        /// 获取汇总数据（会员/粉丝/车辆）
        /// </summary>
        /// <returns></returns>
        public DataAnalysisModel GetTotal(string userid)
        {
            StringBuilder sql = new StringBuilder();
            var strCode = string.Empty;
            var strName = string.Empty;
            if (!string.IsNullOrWhiteSpace(userid))
            {
                strCode = "and cityid in (select GROUP_CONCAT(cityid) from sys_user_city where userid='" + userid + "')";
                strName = "and city in (select GROUP_CONCAT(s1.name) from sys_department as s1 left join sys_user_city as s2 on s2.cityid=s1.code where s2.userid='" + userid + "')";
            }
            //sql.AppendFormat(@"select * from (
            //            select count(1) as value from cust_info where 1=1 {0}) as t1
            //            ,(select count(1) as value2 from car_info where 1=1 {0}) as t2
            //            ,(select count(1) as value4 from wechat_friend where 1=1 {1}) as t3; ", strCode, strName);
            sql.AppendFormat(@"select * from (
                        select count(1) as value from cust_info where 1=1 {0} ) as t1
                        ,(select count(1) as value2 from car_info where 1=1 {0} ) as t2
                        ,(select count(1) as value4 from wechat_friend where 1=1 {1} ) as t3; ", strCode, strName);
            try
            {
                var custModel = Helper.Query<DataAnalysisModel>(sql.ToString()).FirstOrDefault();
                return custModel;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取会员/车辆增长量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataAnalysisModel> GetDayGrowth(DateTime startTime, DateTime endTime, string userid)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_startdatetime = startTime,
                    p_enddatetime = endTime,
                    p_userid = userid
                };

                var args = new DynamicParameters(obj);
                using (var result = conn.QueryMultiple("ccnsp_daygrowth", args, commandType: CommandType.StoredProcedure))
                {
                    var list= result.Read<DataAnalysisModel>(); 
                    //获取结果集
                    return list;
                }

            }
        }

        #endregion

        #region 商户车辆数据统计
        /// <summary>
        /// 获取近12个月车辆增长量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataAnalysisModel> GetRecent12Growth(string custid)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_custid = custid
                };

                var args = new DynamicParameters(obj);
                using (var result = conn.QueryMultiple("ccnsp_increaserecent1year", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    return result.Read<DataAnalysisModel>();
                }

            }
        }

        /// <summary>
        /// 获取近12个月车辆销售量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataAnalysisModel> GetRecent12Sell(string custid)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_custid = custid
                };

                var args = new DynamicParameters(obj);
                using (var result = conn.QueryMultiple("ccnsp_sellrecent1year", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    return result.Read<DataAnalysisModel>();
                }

            }
        }
        #endregion

        #region 不同地区数据统计
        /// <summary>
        /// 获取地区的数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetDataByBity()
        {
            const string sql = @"select t1.innerid as `key`,t1.cityname as value,ifnull(t2.count,0) as value2,ifnull(t3.count,0) as value4,ifnull(t4.count,0) as value6 from base_city as t1
                                left join(select count(1) as count,cityid from cust_info group by cityid) as t2 on t2.cityid=t1.innerid
                                left join (select count(1) as count,cityid from car_info group by cityid) as t3 on t3.cityid=t1.innerid
                                left join (select count(1) as count,city from cust_wechat group by city) as t4 on t4.city=t1.cityname
                                where t2.cityid is not null or t3.cityid is not null
                                order by t3.count desc";
            try
            {
                return Helper.Query<DataAnalysisModel>(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}
