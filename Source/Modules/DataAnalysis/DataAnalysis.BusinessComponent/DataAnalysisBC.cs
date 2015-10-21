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
    }
}
