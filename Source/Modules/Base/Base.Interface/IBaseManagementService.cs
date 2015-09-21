using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;

namespace CCN.Modules.Base.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseManagementService
    {
        #region Area

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseProvince> GetProvList(string initial);

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provId">省份ID</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        IEnumerable<BaseCity> GetCityList(int provId, string initial);

        #endregion
    }
}
