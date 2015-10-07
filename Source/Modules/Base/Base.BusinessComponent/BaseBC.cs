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

        #region Code

        /// <summary>
        /// 获取代码值列表
        /// </summary>
        /// <param name="typekey">代码类型key</param>
        /// <returns></returns>
        public JResult GetCodeByTypeKey(string typekey)
        {
            var list = DataAccess.GetCodeByTypeKey(typekey);
            if (list.Any())
            {
                return new JResult
                {
                    errcode = 0,
                    errmsg = list
                };
            }
            return new JResult
            {
                errcode = 400,
                errmsg = "没有数据"
            };
        }

        #endregion

        #region 验证码

        /// <summary>
        /// 会员注册获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult SendVerification(BaseVerification model)
        {
            var jResult = new JResult();
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

            if (model.Result == 0)
            {
                //发送失败
                jResult.errcode = 400;
                jResult.errmsg = "发送验证码失败";
                //return jResult;  //uu
            }

            var saveRes = DataAccess.SaveVerification(model);
            if (saveRes == 0)
            {
                //保存失败
                jResult.errcode = 401;
                jResult.errmsg = "保存验证码失败";
                return jResult;
            }
            jResult.errcode = 0;
            jResult.errmsg = "发送验证码成功";
            return jResult;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vcode">验证码</param>
        /// <returns>返回结果。1.正确，0不正确,-1.验证码过期</returns>
        public JResult CheckVerification(string target,string vcode)
        {
            var jResult = new JResult();
            var m = DataAccess.GetVerification(target);
            //验证码不正确
            if (m == null || !m.Vcode.Equals(vcode))
            {
                jResult.errcode = 400;
                jResult.errmsg = "验证码错误";
                return jResult;
            }
            //
            if (m.Createdtime.AddSeconds(m.Valid) < DateTime.Now)
            {
                jResult.errcode = 401;
                jResult.errmsg = "验证码过期";
                return jResult;
            }
            jResult.errcode = 0;
            jResult.errmsg = "验证码正确";
            return jResult;
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
            //QiniuUtility qinniu = new QiniuUtility();
            //var sss = qinniu.PutFile("D:\\favicon.ico");
            //var sssw = qinniu.ResumablePutFile("D:\\favicon.ico");

            return DataAccess.GetProvList(initial);
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="provId">省份id</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCity> GetCityList(int provId, string initial)
        {
            return DataAccess.GetCityList(provId,initial);
        }

        /// <summary>
        /// 获取省份（扩展方法，根据首字母分类）
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public JResult GetProvListEx(string initial)
        {
            var list = DataAccess.GetProvList(initial).ToList();
            if (!list.Any())
            {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "没有数据"
                };
            }
            
            var listProv = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var listResult = new List<JsonGroupByModel>();
            foreach (var item in listProv)
            {
                var il = list.Where(x => x.Initial == item).ToList();
                if (!il.Any())
                {
                    continue;
                }
                var m = new JsonGroupByModel
                {
                    Initial = item,
                    ProvList = il
                };
                listResult.Add(m);
            }

            return new JResult
            {
                errcode = 0,
                errmsg = listResult
            };
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
