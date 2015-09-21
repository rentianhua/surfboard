using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessComponent;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.Interface;
using Cedar.AuditTrail.Interception;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Base.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseManagementService: ServiceBase<BaseBC>, IBaseManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        public BaseManagementService(BaseBC bc) :base(bc)
        {

        }

        #region Area
        
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [AuditTrailCallHandler("GetProvList")]
        public IEnumerable<BaseProvince> GetProvList(string initial)
        {
            return BusinessComponent.GetProvList(initial);
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="provId">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        [AuditTrailCallHandler("GetCityList")]
        public IEnumerable<BaseCity> GetCityList(int provId, string initial)
        {
            return BusinessComponent.GetCityList(provId, initial);
        }

        #endregion

    }
}
