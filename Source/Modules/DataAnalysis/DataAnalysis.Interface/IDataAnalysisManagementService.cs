using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

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
    }
}
