using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.DataAnalysis.BusinessComponent;
using CCN.Modules.DataAnalysis.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.AuditTrail.Interception;

namespace CCN.Modules.DataAnalysis.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAnalysisManagementService : ServiceBase<DataAnalysisBC>, IDataAnalysisManagementService
    {
        /// <summary>
        /// </summary>
        public DataAnalysisManagementService(DataAnalysisBC bc)
            : base(bc)
        {
        }

        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <returns></returns>
        [AuditTrailCallHandler("GetLocalByMonthTop10")]
        public JResult GetLocalByMonthTop10()
        {
            return BusinessComponent.GetLocalByMonthTop10();
        }

        #region 买家分布

        /// <summary>
        /// 获取不同年龄段买家分布
        /// </summary>
        /// <returns></returns>
        public JResult GetAgeArea()
        {
            return BusinessComponent.GetAgeArea();
        }

        /// <summary>
        /// 获取买家性别比例
        /// </summary>
        /// <returns></returns>
        public JResult GetGenterPer()
        {
            return BusinessComponent.GetGenterPer();
        }

        /// <summary>
        /// 职业分布
        /// </summary>
        /// <returns></returns>
        public JResult GetOccupationPer()
        {
            return BusinessComponent.GetOccupationPer();
        }

        #endregion

        #region  2015年交易额交易量折线图

        /// <summary>
        /// 年度市场交易走势
        /// </summary>
        /// <returns></returns>
        public JResult GetTradeLineByYear()
        {
            return BusinessComponent.GetTradeLineByYear();
        }

        /// <summary>
        /// 2014-2015二手车交易量月度统计表
        /// </summary>
        /// <returns></returns>
        public JResult GetTradeTotalByMonth()
        {
            return BusinessComponent.GetTradeTotalByMonth();
        }
        
        #endregion

        #region 二手车属性

        /// <summary>
        /// 年度市场交易走势
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarYearAnalysis()
        {
            return BusinessComponent.GetUsedCarYearAnalysis();
        }

        /// <summary>
        /// 可接受的二手车行驶里程
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarAccept()
        {
            return BusinessComponent.GetUsedCarAccept();
        }

        /// <summary>
        /// 3-5万公里满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetSatisfaction3To5()
        {
            return BusinessComponent.GetSatisfaction3To5();
        }

        /// <summary>
        /// 3-5万公里满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetUnSatisfaction3To5()
        {
            return BusinessComponent.GetUnSatisfaction3To5();
        }

        /// <summary>
        /// 1-3万公里满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetSatisfaction1To3()
        {
            return BusinessComponent.GetSatisfaction3To5();
        }

        /// <summary>
        /// 1-3万公里不满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetUnSatisfaction1To3()
        {
            return BusinessComponent.GetUnSatisfaction3To5();
        }

        /// <summary>
        /// 供应量占比
        /// </summary>
        /// <returns></returns>
        public JResult GetSupplyPer()
        {
            return BusinessComponent.GetSupplyPer();
        }

        #endregion

        #region 品牌热度排行

        /// <summary>
        /// 月度品牌热搜前10
        /// </summary>
        /// <returns></returns>
        public JResult GetHotBrandTop10()
        {
            return BusinessComponent.GetHotBrandTop10();
        }

        /// <summary>
        /// 1-7月交易量占比
        /// </summary>
        /// <returns></returns>
        public JResult GetTradePer1To7()
        {
            return BusinessComponent.GetTradePer1To7();
        }
        #endregion

        #region 交易量

        /// <summary>
        /// 二手车交易量全国占比排行前10省份
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeTop10()
        {
            return BusinessComponent.GetUsedCarTradeTop10();
        }

        /// <summary>
        /// 二手车交易量全国占比排行倒数8省份
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeLaset8()
        {
            return BusinessComponent.GetUsedCarTradeLaset8();
        }

        /// <summary>
        /// 二手车近几年交易量
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeRecentYears()
        {
            return BusinessComponent.GetUsedCarTradeRecentYears();
        }

        #endregion

        #region 个人收入分析 

        /// <summary>
        /// 个人收入分析
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncome()
        {
            return BusinessComponent.GetPersonalIncome();
        }

        /// <summary>
        /// 个人季度收入
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncomeByQuarter()
        {
            return BusinessComponent.GetPersonalIncomeByQuarter();
        }

        /// <summary>
        /// 个人月收入
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncomeByMonth()
        {
            return BusinessComponent.GetPersonalIncomeByMonth();
        }

        #endregion

        #region 汽车保有量

        /// <summary>
        /// 汽车保有量
        /// </summary>
        /// <returns></returns>
        public JResult GetCarRetainQuantity()
        {
            return BusinessComponent.GetCarRetainQuantity();
        }

        #endregion
    }
}
