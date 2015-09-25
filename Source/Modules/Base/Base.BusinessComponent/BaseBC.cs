using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;
using CCN.Modules.Base.DataAccess;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Core.IoC;
using Cedar.Foundation.SMS.Common;

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

        #region 验证码

        /// <summary>
        /// 会员注册获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SendVerification(BaseVerification model)
        {
            model.Createdtime = DateTime.Now;
            model.Vcode = RandomUtility.GetRandom(model.Length);

            #region 发送验证码

            if (model.TType == 1)
            {
                //发送手机
                SMSMSG sms = new SMSMSG();
                var result = sms.SendSms(model.Target, model.Vcode);
                if (result.errcode != "0")
                {
                    model.Result = 0;
                }
            }
            else if (model.TType == 2)
            {
                //发送邮件
            }
            
            #endregion

            return DataAccess.SaveVerification(model);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vcode">验证码</param>
        /// <returns>返回结果。1.正确，0不正确,-1.验证码过期</returns>
        public int CheckVerification(string target,string vcode)
        {
            var m = DataAccess.GetVerification(target);
            //验证码不正确
            if (m == null || !m.Vcode.Equals(vcode))
            {
                return 0;
            }
            //
            if (m.Createdtime.AddMinutes(m.Valid) < DateTime.Now)
            {
                return -1;
            }
            return 1;
        }

        #endregion

        #region 区域

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

        #region 品牌/车系/车型

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCarBrandModel> GetCarBrand(string initial)
        {
            return DataAccess.GetCarBrand(initial);
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId)
        {
            return DataAccess.GetCarSeries(brandId);
        }

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarModelModel> GetCarModel(int seriesId)
        {
            return DataAccess.GetCarModel(seriesId);
        }

        #endregion
    }
}
