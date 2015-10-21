using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.DataAnalysis.BusinessComponent;
using CCN.Modules.DataAnalysis.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.Common.BaseClasses;
using Cedar.AuditTrail.Interception;


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
    }
}
