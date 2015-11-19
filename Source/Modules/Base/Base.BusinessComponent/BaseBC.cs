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
        #region 基础数据代码类型
        /// <summary>
        /// 获取基础数据代码类型列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCodeTypeListModel> GetCodeTypeList(BaseCodeTypeQueryModel query)
        {
            return DataAccess.GetCodeTypeList(query);
        }
        /// <summary>
        /// 更新基础数据代码类型状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCodeTypeStatus(string id, int status)
        {
            if (string.IsNullOrWhiteSpace(id) || (status != 0 && status != 1))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateCodeTypeStatus(id, status);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 删除基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCodeType(string innerid)
        {
            if (string.IsNullOrWhiteSpace(innerid))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.DeleteCodeType(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCodeTypeById(string innerid)
        {
            var model = DataAccess.GetCodeTypeById(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCodeType(BaseCodeTypeModel model)
        {
            var test=DataAccess.GetCodeTypeByTypeKey(model.Typekey);
            if (test==model.Typekey) {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "代码类型key重复！"
                };
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.Isenabled = 1;
            var result = DataAccess.AddCodeType(model);
            return new JResult {
                errcode=0,
                errmsg=result
            };
        }
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCodeType(BaseCodeTypeModel model)
        {
            var test = DataAccess.GetCodeTypeByTypeKey(model.Typekey);
            if (test== model.Typekey) {
                return new JResult
                {
                    errcode = 400,
                    errmsg = "代码类型key重复！"
                };
            }
            //添加信息
            var result = DataAccess.UpdateCodeType(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }
        #endregion

        /// <summary>
        /// 获取基础数据代码值列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCodeSelectModel> GetCodeList(BaseCodeQueryModel query)
        {
            return DataAccess.GetCodeList(query);
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public IEnumerable<BaseCodeTypeModel> GetCodeType(string innerid)
        {

            return DataAccess.GetCodeType(innerid);
        }
        /// <summary>
        /// 获取基础数据代码值状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateCodeStatus(string id, int status)
        {
            if (string.IsNullOrWhiteSpace(id) || (status != 0 && status != 1))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateCodeStatus(id, status);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 删除基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCode(string innerid)
        {
            if (string.IsNullOrWhiteSpace(innerid))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.DeleteCode(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCodeById(string innerid)
        {
            var model = DataAccess.GetCodeById(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCode(BaseCodeModel model)
        {
              
            var test = DataAccess.GetCodeByTypeKey(model.Typekey,model.CodeName,model.CodeValue);
            if (test.Typekey == model.Typekey)
            {
                if (test.CodeName==model.CodeName) {
                    return new JResult
                    {
                        errcode = 400,
                        errmsg = "代码名称已存在！"
                    };
                }
            }
            model.Innerid = Guid.NewGuid().ToString();
            model.IsEnabled = 1;
            model.Sort = model.CodeValue;
            var result = DataAccess.AddCode(model);
            return new JResult
            {
                errcode = 0,
                errmsg = result
            };
        }
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCode(BaseCodeModel model)
        {
            var test = DataAccess.GetCodeByTypeKey(model.Typekey, model.CodeName, model.CodeValue);
            if (test.Typekey == model.Typekey)
            {
                if (test.CodeName != model.CodeName&&test.CodeValue!=model.CodeValue)
                {
                    return new JResult
                    {
                        errcode = 400,
                        errmsg = "更新信息失败！"
                    };
                }
            }
            var result = DataAccess.UpdateCode(model);
            return new JResult
            {
                errcode = 0,
                errmsg = result
            };
        }



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
                errmsg = ""
            };
        }
       
        #endregion

        #region 验证码

        /// <summary>
        /// 组织验证码内容
        /// </summary>
        /// <param name="utype"></param>
        /// <param name="vcode"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        private static string GetVerifiByType(int utype ,string vcode,int valid)
        {
            //有效期从秒转换成分
            valid = valid / 60;
            string content;
            switch (utype)
            {
                case 1:
                    content = $"{vcode}（平台注册验证码，{valid}分钟内有效）";
                    break;
                case 2:
                    content = $"{vcode}（平台登录验证码，{valid}分钟内有效）";
                    break;
                case 3:
                    content = $"{vcode}（平台找回密码的验证码，{valid}分钟内有效）";
                    break;
                case 0:
                    content = $"{vcode}（平台验证码，{valid}分钟内有效）";
                    break;
                default:
                    content = $"{vcode}（平台验证码，{valid}分钟内有效）";
                    break;
            }

            return content;
        }

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
            model.Content = GetVerifiByType(model.UType, model.Vcode, model.Valid);

            var saveRes = DataAccess.SaveVerification(model);
            if (saveRes == 0)
            {
                //保存失败
                jResult.errcode = 401;
                jResult.errmsg = "发送验证码失败";
                return jResult;
            }

            #region 发送验证码
            Task.Run(() =>
            {
                switch (model.TType)
                {
                    case 1:
                        //发送手机
                        var sms = new SMSMSG();
                        var result = sms.PostSms(model.Target, model.Content);
                        if (result.errcode != "0")
                        {
                            model.Result = 0;
                        }
                        break;
                    case 2:
                        //发送邮件
                        break;
                }
            });
            #endregion

            jResult.errcode = 0;
            jResult.errmsg = "发送验证码成功";
            return jResult;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vcode">验证码</param>
        /// <param name="utype">用处类型[1注册,2登录,3,其他]</param>
        /// <returns>返回结果。1.正确，0不正确,-1.验证码过期</returns>
        public JResult CheckVerification(string target,string vcode, int utype)
        {
            var jResult = new JResult();
            var m = DataAccess.GetVerification(target,utype);
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
                    errmsg = ""
                };
            }
            
            var listProv = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var listResult = (from item in listProv
                let il = list.Where(x => x.Initial.ToUpper().Trim() == item).ToList()
                where il.Any()
                select new JsonGroupByModel
                {
                    Initial = item, ProvList = il
                }).ToList();

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
        /// 获取品牌热度Top n
        /// </summary>
        /// <returns></returns>
        public JResult GetCarBrandHotTop(int top)
        {
            var list = DataAccess.GetCarBrandHotTop(top);
            return list.Any() ? JResult._jResult(0,list) : JResult._jResult(400, "");
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

        /// <summary>
        /// 根据ID获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCarModelById(int innerid)
        {
            var model = DataAccess.GetCarModelById(innerid);
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

        #endregion

        #region 品牌信息
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCarBrandListViewModel> GetCarBrandList(BaseCarBrandQueryModel query)
        {
            return DataAccess.GetCarBrandList(query);
        }
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarBrandById(string innerid)
        {
            var model = DataAccess.GetCarBrandById(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateBrandStatus(string carid, int status)
        {
            if (string.IsNullOrWhiteSpace(carid) || (status != 0 && status != 1))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateBrandStatus(carid, status);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarBrand(BaseCarBrandModel model)
        {
            model.IsEnabled = 1;
            var result = DataAccess.AddCarBrand(model);
            return JResult._jResult(result);
        }
        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarBrand(string innerid)
        {
            if (string.IsNullOrWhiteSpace(innerid))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.DeleteCarBrand(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarBrand(BaseCarBrandModel model)
        {
            if (model.Remark == null)
            {
                model.Remark = "  ";
            }
            //添加信息
            var result = DataAccess.UpdateCarBrand(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }
        /// <summary>
        /// ID最大值
        /// </summary>
        /// <returns></returns>
        public JResult GetCarBrandMaxId()
        {
            var model = DataAccess.GetCarBrandMaxId();
            return JResult._jResult(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarBrandName(string brandname, string innerid)
        {
            var model = DataAccess.GetCarBrandName(brandname, innerid);
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
        #endregion
        #region 车型信息
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCarModelListViewModel> GetCarModelList(BaseCarModelQueryModel query)
        {
            return DataAccess.GetCarModelList(query);
        }
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetBaseCarModelById(string innerid)
        {
            var model = DataAccess.GetBaseCarModelById(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新车型状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateModelStatus(string carid, int status)
        {
            if (string.IsNullOrWhiteSpace(carid) || (status != 0 && status != 1))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateModelStatus(carid, status);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 添加车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarModel(BaseCarModelModel model)
        {
            //model.innerid = Guid.NewGuid();设置值
            model.IsEnabled = 1;
            var result = DataAccess.AddCarModel(model);
            return JResult._jResult(result);
        }
        /// <summary>
        /// 删除车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarModel(string innerid)
        {
            if (string.IsNullOrWhiteSpace(innerid))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.DeleteCarModel(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarModel(BaseCarModelModel model)
        {
            if (model.Remark == null)
            {
                model.Remark = "  ";
            }
            //添加信息
            var result = DataAccess.UpdateCarModel(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }
        /// <summary>
        /// 获取ID最大值
        /// </summary>
        /// <returns></returns>
        public JResult GetCarModelMaxId()
        {
            var model = DataAccess.GetCarModelMaxId();
            return JResult._jResult(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarModelName(string modelname,string innerid)
        {
            var model = DataAccess.GetCarModelName(modelname,innerid);
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
        #endregion
        #region 车系信息
        /// <summary>
        /// 获取车系信息列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<BaseCarSeriesListViewModel> GetCarSeriesList(BaseCarSeriesQueryModel query)
        {
            return DataAccess.GetCarSeriesList(query);
        }
        /// <summary>
        /// 获取车系信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public JResult GetCarSeriesById(string innerid)
        {
            var model = DataAccess.GetCarSeriesById(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新车系状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JResult UpdateSeriesStatus(string carid, int status)
        {
            if (string.IsNullOrWhiteSpace(carid) || (status != 0 && status != 1))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.UpdateSeriesStatus(carid, status);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 添加车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult AddCarSeries(BaseCarSeriesModel model)
        {
            //model.innerid = Guid.NewGuid();设置值
            //model.Innerid = 2400;
            model.IsEnabled = 1;
            var result = DataAccess.AddCarSeries(model);
            return JResult._jResult(result);
        }
        /// <summary>
        /// 删除车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult DeleteCarSeries(string innerid)
        {
            if (string.IsNullOrWhiteSpace(innerid))
            {
                return JResult._jResult(402, "参数不完整");
            }
            var model = DataAccess.DeleteCarSeries(innerid);
            return JResult._jResult(model);
        }
        /// <summary>
        /// 更新车系信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JResult UpdateCarSeries(BaseCarSeriesModel model)
        {
            if (model.Remark == null)
            {
                model.Remark = "  ";
            }
            //添加信息
            var result = DataAccess.UpdateCarSeries(model);
            return new JResult
            {
                errcode = result > 0 ? 0 : 400,
                errmsg = ""
            };
        }
        /// <summary>
        /// 获取ID最大值
        /// </summary>
        /// <returns></returns>
        public JResult GetCarSeriesMaxId()
        {
            var model = DataAccess.GetCarSeriesMaxId();
            return JResult._jResult(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seriesname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public JResult GetCarSeriesName(string seriesname,string innerid)
        {
            var model = DataAccess.GetCarSeriesName(seriesname, innerid);
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
        #endregion
    }
}
