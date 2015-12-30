using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;
using CCN.Modules.DataAnalysis.BusinessEntity;

namespace CCN.Modules.DataAnalysis.Interface
{
    /// <summary>
    /// 数据接口
    /// </summary>
    public interface IDataAnalysisManagementService
    {
        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <returns></returns>
        JResult GetLocalByMonthTop10();

        #region 买家分布

        /// <summary>
        /// 获取不同年龄段买家分布
        /// </summary>
        /// <returns></returns>
        JResult GetAgeArea();

        /// <summary>
        /// 获取买家性别比例
        /// </summary>
        /// <returns></returns>
        JResult GetGenterPer();

        /// <summary>
        /// 职业分布
        /// </summary>
        /// <returns></returns>
        JResult GetOccupationPer();

        #endregion

        #region  2015年交易额交易量折线图

        /// <summary>
        /// 年度市场交易走势
        /// </summary>
        /// <returns></returns>
        JResult GetTradeLineByYear();

        /// <summary>
        /// 2014-2015二手车交易量月度统计表
        /// </summary>
        /// <returns></returns>
        JResult GetTradeTotalByMonth();

        #endregion

        #region 二手车属性

        /// <summary>
        /// 二手车使用年限分析
        /// </summary>
        /// <returns></returns>
        JResult GetUsedCarYearAnalysis();

        /// <summary>
        /// 可接受的二手车行驶里程
        /// </summary>
        /// <returns></returns>
        JResult GetUsedCarAccept();

        /// <summary>
        /// 3-5万公里满意度
        /// </summary>
        /// <returns></returns>
        JResult GetSatisfaction3To5();

        /// <summary>
        /// 3-5万公里不满意度
        /// </summary>
        /// <returns></returns>
        JResult GetUnSatisfaction3To5();

        /// <summary>
        /// 1-3万公里满意度
        /// </summary>
        /// <returns></returns>
        JResult GetSatisfaction1To3();

        /// <summary>
        /// 1-3万公里不满意度
        /// </summary>
        /// <returns></returns>
        JResult GetUnSatisfaction1To3();

        /// <summary>
        /// 供应量占比
        /// </summary>
        /// <returns></returns>
        JResult GetSupplyPer();
        #endregion

        #region 品牌热度排行

        /// <summary>
        /// 月度品牌热搜前10
        /// </summary>
        /// <returns></returns>
        JResult GetHotBrandTop10();

        /// <summary>
        /// 1-7月交易量占比
        /// </summary>
        /// <returns></returns>
        JResult GetTradePer1To7();

        #endregion

        #region 交易量

        /// <summary>
        /// 二手车交易量全国占比排行前10省份
        /// </summary>
        /// <returns></returns>
        JResult GetUsedCarTradeTop10();

        /// <summary>
        /// 二手车交易量全国占比排行倒数8省份
        /// </summary>
        /// <returns></returns>
        JResult GetUsedCarTradeLaset8();

        /// <summary>
        /// 二手车近几年交易量
        /// </summary>
        /// <returns></returns>
        JResult GetUsedCarTradeRecentYears();

        #endregion

        #region

        /// <summary>
        /// 个人收入分析
        /// </summary>
        /// <returns></returns>
        JResult GetPersonalIncome();

        /// <summary>
        /// 个人季度收入
        /// </summary>
        /// <returns></returns>
        JResult GetPersonalIncomeByQuarter();

        /// <summary>
        /// 个人月收入
        /// </summary>
        /// <returns></returns>
        JResult GetPersonalIncomeByMonth();

        #endregion

        #region 汽车保有量

        /// <summary>
        /// 汽车保有量
        /// </summary>
        /// <returns></returns>
        JResult GetCarRetainQuantity();

        #endregion

        /// <summary>
        /// 获取日增长量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        JResult GetDayGrowth(DataQueryModel query);

        /// <summary>
        /// 获取汇总数据（会员/粉丝/车辆）
        /// </summary>
        /// <returns></returns>
        JResult GetTotal(string cityid);

        #region 商户车辆数据统计
        /// <summary>
        /// 获取近12个月车辆增长量
        /// </summary>
        /// <returns></returns>
        JResult GetRecent12Growth(string custid);

        /// <summary>
        /// 获取近12个月车辆销售量
        /// </summary>
        /// <returns></returns>
        JResult GetRecent12Sell(string custid);

        #endregion

        #region 不同地区数据统计
        /// <summary>
        /// 获取地区的数据
        /// </summary>
        /// <returns></returns>
        JResult GetDataByBity();
        #endregion

    }
}
