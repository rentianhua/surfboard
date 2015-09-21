using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.DataAccess;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Base.BusinessComponent
{
    /// <summary>
    /// 基础模块
    /// </summary>
    public class BaseBC : BusinessComponentBase<BaseDA>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        public BaseBC(BaseDA da):base(da)
        {

        }

        #region Area

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseProvince> GetProvList(string initial)
        {
            return DataAccess.GetProvList(initial);
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="provId">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCity> GetCityList(int provId, string initial)
        {
            return DataAccess.GetCityList(provId,initial);
        }

        #endregion
    }
}
