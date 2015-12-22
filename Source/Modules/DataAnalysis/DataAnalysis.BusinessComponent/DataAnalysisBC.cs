using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using CCN.Modules.DataAnalysis.DataAccess;

namespace CCN.Modules.DataAnalysis.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAnalysisBC : BusinessComponentBase<DataAnalysisDA>
    {
        /// <summary>
        /// </summary>
        /// <param name="da"></param>
        public DataAnalysisBC(DataAnalysisDA da)
            : base(da)
        {

        }

        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <returns></returns>
        public JResult GetLocalByMonthTop10()
        {
            var list = DataAccess.GetLocalByMonthTop10();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #region 买家分布

        /// <summary>
        /// 获取不同年龄段买家分布
        /// </summary>
        /// <returns></returns>
        public JResult GetAgeArea()
        {
            var list = DataAccess.GetAgeArea();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取买家性别比例
        /// </summary>
        /// <returns></returns>
        public JResult GetGenterPer()
        {
            var list = DataAccess.GetGenterPer();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 职业分布
        /// </summary>
        /// <returns></returns>
        public JResult GetOccupationPer()
        {
            var list = DataAccess.GetOccupationPer();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 2015年交易额交易量折线图

        /// <summary>
        /// 年度市场交易走势
        /// </summary>
        /// <returns></returns>
        public JResult GetTradeLineByYear()
        {
            var list = DataAccess.GetTradeLineByYear();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 2014-2015二手车交易量月度统计表
        /// </summary>
        /// <returns></returns>
        public JResult GetTradeTotalByMonth()
        {
            var list = DataAccess.GetTradeTotalByMonth();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 二手车属性

        /// <summary>
        /// 二手车使用年限分析
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarYearAnalysis()
        {
            var list = DataAccess.GetUsedCarYearAnalysis();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 可接受的二手车行驶里程
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarAccept()
        {
            var list = DataAccess.GetUsedCarAccept();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 3-5万公里满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetSatisfaction3To5()
        {
            var list = DataAccess.GetSatisfaction3To5();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 3-5万公里不满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetUnSatisfaction3To5()
        {
            var list = DataAccess.GetUnSatisfaction3To5();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 1-3万公里满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetSatisfaction1To3()
        {
            var list = DataAccess.GetSatisfaction1To3();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 1-3万公里不满意度
        /// </summary>
        /// <returns></returns>
        public JResult GetUnSatisfaction1To3()
        {
            var list = DataAccess.GetUnSatisfaction1To3();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 供应量占比
        /// </summary>
        /// <returns></returns>
        public JResult GetSupplyPer()
        {
            var list = DataAccess.GetSupplyPer();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 品牌热度排行

        /// <summary>
        /// 月度品牌热搜前10
        /// </summary>
        /// <returns></returns>
        public JResult GetHotBrandTop10()
        {
            var list = DataAccess.GetHotBrandTop10();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 1-7月交易量占比
        /// </summary>
        /// <returns></returns>
        public JResult GetTradePer1To7()
        {
            var list = DataAccess.GetTradePer1To7();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 交易量

        /// <summary>
        /// 二手车交易量全国占比排行前10省份
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeTop10()
        {
            var list = DataAccess.GetUsedCarTradeTop10();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 二手车交易量全国占比排行倒数8省份
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeLaset8()
        {
            var list = DataAccess.GetUsedCarTradeLaset8();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 二手车近几年交易量
        /// </summary>
        /// <returns></returns>
        public JResult GetUsedCarTradeRecentYears()
        {
            var list = DataAccess.GetUsedCarTradeRecentYears();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 个人收入分析

        /// <summary>
        /// 季度数字装汉字
        /// </summary>
        /// <returns></returns>
        private string NumToChanrQuarter(string quarterNum)
        {
            string strReturn = string.Empty;
            switch (quarterNum)
            {
                case "1":
                    strReturn = "一季度";
                    break;
                case "2":
                    strReturn = "二季度";
                    break;
                case "3":
                    strReturn = "三季度";
                    break;
                case "4":
                    strReturn = "四季度";
                    break;
            }

            return strReturn;
        }

        /// <summary>
        /// 个人收入分析
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncome()
        {
            var listQuarter = DataAccess.GetPersonalIncomeByQuarter();
            var listMonth = DataAccess.GetPersonalIncomeByMonth();
            if (listQuarter != null && listMonth != null)
            {
                foreach (var model in listQuarter)
                {
                    model.value5 = listMonth.Where(s => s.value == model.key).ToList();
                    model.key = NumToChanrQuarter(model.key);
                }
                return new JResult
                {
                    errcode = 0,
                    errmsg = listQuarter
                };
            }
            else
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
        }

        /// <summary>
        /// 个人季度收入
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncomeByQuarter()
        {
            var listQuarter = DataAccess.GetPersonalIncomeByQuarter();

            if (listQuarter == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = listQuarter
            };
        }

        /// <summary>
        /// 个人月收入
        /// </summary>
        /// <returns></returns>
        public JResult GetPersonalIncomeByMonth()
        {
            var listMonth = DataAccess.GetPersonalIncomeByMonth();

            if (listMonth == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = listMonth
            };
        }
        #endregion

        #region 汽车保有量

        /// <summary>
        /// 汽车保有量
        /// </summary>
        /// <returns></returns>
        public JResult GetCarRetainQuantity()
        {
            var listMonth = DataAccess.GetCarRetainQuantity();

            if (listMonth == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = listMonth
            };
        }

        #endregion

        /// <summary>
        /// 日增长量
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public JResult GetDayGrowth(DateTime startTime, DateTime endTime, string cityid)
        {
            var list = DataAccess.GetDayGrowth(startTime, endTime, cityid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取汇总数据（会员/粉丝/车辆）
        /// </summary>
        /// <returns></returns>
        public JResult GetTotal(string cityid)
        {
            var model = DataAccess.GetTotal(cityid);
            if (model == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = model
            };
        }

        #region 商户车辆数据统计
        /// <summary>
        /// 获取近12个月车辆增长量
        /// </summary>
        /// <returns></returns>
        public JResult GetRecent12Growth(string custid)
        {
            var list = DataAccess.GetRecent12Growth(custid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        /// <summary>
        /// 获取近12个月车辆销售量
        /// </summary>
        /// <returns></returns>
        public JResult GetRecent12Sell(string custid)
        {
            var list = DataAccess.GetRecent12Growth(custid);
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }

        #endregion

        #region 不同地区数据统计
        /// <summary>
        /// 获取地区的数据
        /// </summary>
        /// <returns></returns>
        public JResult GetDataByBity()
        {
            var list = DataAccess.GetDataByBity();
            if (list == null)
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = ""
                };
            }
            return new JResult
            {
                errcode = 0,
                errmsg = list
            };
        }
        #endregion

    }
}
