using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Base.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framework.Common.BaseClasses;
using Dapper;

namespace CCN.Modules.Base.DataAccess
{
    /// <summary>
    /// 基础模块
    /// </summary>
    public class BaseDA : DataAccessBase
    {
        //private readonly Database Helper;

        /// <summary>
        /// 
        /// </summary>
        public BaseDA()
        {
            //Helper = new DatabaseWrapperFactory().GetDatabase("mysqldb");
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
            const string spName = "sp_common_pager";
            const string tableName = @"base_code_type ";
            const string fields = "innerid,typekey,typename,isenabled";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " innerid asc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(query.Typename))
            {
                sqlWhere.Append($" and typename like '%{query.Typename}%'");
            }
            if (!string.IsNullOrWhiteSpace(query.Typekey))
            {
                sqlWhere.Append($" and typekey = '{query.Typekey}'");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseCodeTypeListModel>(model, query.Echo);
            return list;
        }
        /// <summary>
        /// 更新基础数据代码类型状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateCodeTypeStatus(string id, int status)
        {
            const string sql = "update base_code_type set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = id, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 删除基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCodeType(string innerid)
        {
            const string sql = @"delete from base_code_type where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获取基础数据代码类型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCodeTypeModel GetCodeTypeById(string innerid)
        {
            const string sql = @"select innerid,typekey,typename,isenabled from base_code_type where innerid=@innerid";
            try
            {
                var codetypemodel = Helper.Query<BaseCodeTypeModel>(sql, new { innerid }).FirstOrDefault();
                return codetypemodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 添加基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCodeType(BaseCodeTypeModel model)
        {
            const string sql = @"INSERT INTO `base_code_type`
                                (`innerid`,`typekey`,`typename`,`isenabled`)
                                VALUES
                                (@innerid,@typekey,@typename,@isenabled);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 更新基础数据代码类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCodeType(BaseCodeTypeModel model)
        {
            var sql = new StringBuilder("update `base_code_type` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        ///  获取基础数据代码类型key
        /// </summary>
        /// <param name="typekey"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCodeTypeModel GetCodeTypeByTypeKey(string typekey, string innerid)
        {
            //：：更新验证：/ 启用中的，根据typekey，查询语句，进行判断
            string where = " isenabled=1 and typekey='" + typekey + "'";
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid !='{innerid}'";
            }
            var sql = $"select  typekey,innerid from base_code_type where {where} ";
            try
            {
                var codeModel = Helper.Query<BaseCodeTypeModel>(sql).FirstOrDefault();
                return codeModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 验证CodeType下是否还有Code
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public string VerifyCodeType(string innerid)
        {
            string result;
            const string sql = @"select b.codename from base_code_type as a left join base_code as b on a.typekey=b.typekey where a.innerid=@innerid;";
            result = Helper.ExecuteScalar<string>(sql, new { innerid });
            return result;
        }
        #endregion
        #region 基础数据代码值
        /// <summary>
        /// 获取基础数据代码值列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseCodeSelectModel> GetCodeList(BaseCodeQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"base_code as a left join base_code_type as b on a.typekey=b.typekey ";
            const string fields = "a.innerid,codevalue,codename,sort,a.typekey,a.isenabled,ifnull(remark,'') remark,b.typename";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " sort asc,b.typename asc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(query.Typekey))
            {
                sqlWhere.Append($" and a.typekey = '{query.Typekey}'");
            }
            if (!string.IsNullOrWhiteSpace(query.CodeName))
            {
                sqlWhere.Append($" and codename like '%{query.CodeName}%'");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseCodeSelectModel>(model, query.Echo);
            return list;
        }
        /// <summary>
        /// 获取基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public IEnumerable<BaseCodeTypeModel> GetCodeType(string innerid)
        {
            var where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid='{innerid}'";
            }
            var sql = $"select innerid,typekey,typename from base_code_type where {where} order by innerid asc , typename asc";
            var CodeTypeList = Helper.Query<BaseCodeTypeModel>(sql);
            return CodeTypeList;
        }
        /// <summary>
        /// 更新基础数据代码值状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateCodeStatus(string id, int status)
        {
            const string sql = "update base_code set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = id, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 删除基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCode(string innerid)
        {
            const string sql = @"delete from base_code where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获得基础数据代码值
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCodeSelectModel GetCodeById(string innerid)
        {
            const string sql = @"select a.innerid,codevalue,codename,sort,a.typekey,a.isenabled,remark,b.typename,b.innerid as typeid from base_code as a left join base_code_type as b on a.typekey=b.typekey where a.innerid=@innerid";
            try
            {
                var codemodel = Helper.Query<BaseCodeSelectModel>(sql, new { innerid }).FirstOrDefault();
                return codemodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 添加基础数据代码值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCode(BaseCodeModel model)
        {
            const string sql = @"INSERT INTO `base_code`
                                (`innerid`,`codevalue`,`codename`,`sort`,`typekey`,`remark`,`isenabled`)
                                VALUES
                                (@innerid,@codevalue,@codename,@sort,@typekey,@remark,@isenabled);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 更新基础数据代码值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCode(BaseCodeModel model)
        {
            var sql = new StringBuilder("update `base_code` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 获取基础数据代码值key
        /// </summary>
        /// <param name="typekey"></param>
        /// <param name="codename"></param>
        /// <param name="codevalue"></param>
        /// <returns></returns>
        public BaseCodeModel GetCodeByTypeKey(string typekey, string codename, string codevalue)
        {

            string where = " isenabled=1 and typekey='" + typekey + "'";
            if (!string.IsNullOrWhiteSpace(codename))
            {
                where += $" and codename ='{codename}'";
            }
            if (!string.IsNullOrWhiteSpace(codevalue))
            {
                where += $" and codevalue !='{codevalue}'";
            }
            var sql = $"select typekey,codename,codevalue from base_code  where {where} ";
            try
            {
                var codeModel = Helper.Query<BaseCodeModel>(sql).FirstOrDefault();
                return codeModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取代码值列表
        /// </summary>
        /// <param name="typekey">代码类型key</param>
        /// <returns></returns>
        public IEnumerable<BaseCodeSelectModel> GetCodeByTypeKey(string typekey)
        {
            const string sql = "select codevalue, codename from base_code where isenabled=1 and typekey=@typekey order by sort asc;";
            var list = Helper.Query<BaseCodeSelectModel>(sql, new { typekey });
            return list;
        }
        #endregion

        #region 验证码

        /// <summary>
        /// 验证码保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveVerification(BaseVerification model)
        {
            int result;
            const string sql = "insert into base_verification (innerid, target, vcode, valid, createdtime, ttype,utype,content, result) values (uuid(), @target, @vcode, @valid, @createdtime, @ttype, @utype,@content, @result);";
            try
            {
                result = Helper.Execute(sql, model);
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="utype">用处类型[1注册,2登录,3,其他]</param>
        /// <returns></returns>
        public BaseVerification GetVerification(string target, int utype)
        {
            const string sql = "select innerid, target, vcode, valid, createdtime, ttype, utype,content, result from base_verification where target=@target and utype=@utype order by createdtime desc limit 1;";
            var m = Helper.Query<BaseVerification>(sql, new { target, utype }).FirstOrDefault();
            return m;
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
            string where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, provname, initial, isenabled, remark from base_province where {where} order by initial asc,provname asc";
            var provList = Helper.Query<BaseProvince>(sql);
            return provList;
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provId">省份ID</param>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<BaseCity> GetCityList(int provId, string initial)
        {
            string where = " isenabled=1 and provid=" + provId;
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, cityname, initial, provid, isenabled, remark from base_city where {where} order by initial asc,cityname asc";
            var cityList = Helper.Query<BaseCity>(sql);
            return cityList;
        }

        /// <summary>
        /// 根据省份获取区县
        /// </summary>
        /// <param name="cityId"> 城市ID</param>
        /// <returns></returns>
        public IEnumerable<BaseCounty> GetCountyList(int cityId)
        {
            var sql =
                $"select innerid, areacode, countyname, cityid, typename from base_county where cityid={cityId} order by innerid asc";
            var countyList = Helper.Query<BaseCounty>(sql);
            return countyList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BaseProvinceAll> GetTotalAreaList()
        {
            var sqlProvince = $"select innerid as provid, provname, initial from base_province where isenabled=1";
            var sqlCity = $"select innerid as cityid, cityname, initial, provid from base_city where isenabled=1";
            var sqlCounty = $"select innerid as countyid, countyname, cityid from base_county";
            var provinceList = Helper.Query<BaseProvinceAll>(sqlProvince).ToList();
            var cityList = Helper.Query<BaseCityAll>(sqlCity).ToList();
            var countyList = Helper.Query<BaseCountyAll>(sqlCounty).ToList();
            
            foreach (var pitem in provinceList)
            {
                pitem.citylist = cityList.Where(x => x.provid == pitem.provid).ToList();
                foreach (var citem in pitem.citylist)
                {
                    citem.countylist = countyList.Where(x => x.cityid == citem.cityid).ToList();
                }
            }

            return provinceList;
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
            var where = " isenabled=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select innerid, brandname, initial, isenabled, remark, logurl, hot from base_carbrand where {where} order by initial asc,brandname asc";
            var brandList = Helper.Query<BaseCarBrandModel>(sql);
            return brandList;
        }

        /// <summary>
        /// 获取品牌热度Top n
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseCarBrandModel> GetCarBrandHotTop(int top)
        {
            var sql = "select innerid, brandname, initial, isenabled, remark, logurl, hot from base_carbrand where isenabled=1 order by hot desc,brandname asc limit " + top;
            var brandList = Helper.Query<BaseCarBrandModel>(sql);
            return brandList;
        }

        /// <summary>
        /// 根据品牌id获取车系
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarSeriesModel> GetCarSeries(int brandId)
        {
            var sql = $"select innerid, seriesname, seriesgroupname, brandid, isenabled, remark, hot from base_carseries where isenabled=1 and brandid={brandId}";
            var seriesList = Helper.Query<BaseCarSeriesModel>(sql);
            return seriesList;
        }

        /// <summary>
        /// 获取热门车系Top n
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseCarSeriesModel> GetCarSeriesHotTop(int top)
        {
            var sql = "select innerid, seriesname, seriesgroupname, brandid, isenabled, remark, hot from base_carseries where isenabled=1 order by hot desc,seriesname asc limit " + top;
            var seriesList = Helper.Query<BaseCarSeriesModel>(sql);
            return seriesList;
        }

        /// <summary>
        /// 根据车系ID获取车型
        /// </summary>
        /// <param name="seriesId">车系id</param>
        /// <returns></returns>
        public IEnumerable<BaseCarModelModel> GetCarModel(int seriesId)
        {
            var sql = $"select innerid, modelname, modelprice, modelyear, minregyear, maxregyear, liter, geartype, dischargestandard, seriesid, isenabled, remark from base_carmodel where isenabled=1 and seriesid={seriesId}";
            var modelList = Helper.Query<BaseCarModelModel>(sql);
            return modelList;
        }

        /// <summary>
        /// 根据ID获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public BaseCarModelModel GetCarModelById(int innerid)
        {
            var sql = $"select innerid, modelname, modelprice, modelyear, minregyear, maxregyear, liter, geartype, dischargestandard, seriesid, isenabled, remark from base_carmodel where innerid={innerid}";
            var model = Helper.Query<BaseCarModelModel>(sql).FirstOrDefault();
            return model;
        }

        #endregion

        #region 品牌信息
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<BaseCarBrandListViewModel> GetCarBrandList(BaseCarBrandQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"base_carbrand ";
            const string fields = " innerid,brandname,initial,isenabled,ifnull(remark,'') remark,ifnull(logurl,'') logurl,hot ";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " initial asc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(query.BrandName))
            {
                sqlWhere.Append($" and brandname like '%{query.BrandName}%'");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseCarBrandListViewModel>(model, query.Echo);
            return list;
        }
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCarBrandModel GetCarBrandById(string innerid)
        {
            const string sql = @"select * from base_carbrand where innerid=@innerid;";
            try
            {
                var carbrandModel = Helper.Query<BaseCarBrandModel>(sql, new { innerid }).FirstOrDefault();
                return carbrandModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateBrandStatus(string carid, int status)
        {
            const string sql = "update base_carbrand set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = carid, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCarBrand(BaseCarBrandModel model)
        {
            const string sql = @"INSERT INTO `base_carbrand`
                                (`innerid`,`brandname`,`initial`,`isenabled`,`remark`,`logurl`,`hot`)
                                VALUES
                                (@innerid,@brandname,@initial,@isenabled,@remark,@logurl,@hot);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCarBrand(string innerid)
        {
            const string sql = @"delete from base_carbrand where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        ///更新品牌信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCarBrand(BaseCarBrandModel model)
        {
            var sql = new StringBuilder("update `base_carbrand` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 获取ID最大值
        /// </summary>
        /// <returns></returns>
        public int GetCarBrandMaxId()
        {
            int result;
            const string sql = @"select max(base_carbrand.innerid) MaxId from base_carbrand;";
            result = Helper.Execute<int>(sql);
            return result;
        }
        /// <summary>
        /// 验证品牌信息是否同名
        /// </summary>
        /// <param name="brandname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCarBrandModel GetCarBrandName(string brandname, string innerid)
        {
            string where = " isenabled=1 and brandname='" + brandname + "'";
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid!='{innerid}'";
            }
            var sql = $"select brandname,innerid from base_carbrand where {where} ";
            try
            {
                var carbrandModel = Helper.Query<BaseCarBrandModel>(sql).FirstOrDefault();
                return carbrandModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 验证品牌下是否还有车系
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public string VerifyCarBrand(string innerid)
        {
            string result;
            const string sql = @"select b.seriesname from base_carbrand as a left join base_carseries as b on a.innerid=b.brandid where a.innerid=@innerid;";
            result = Helper.Execute<string>(sql, new { innerid });
            return result;
        }
        #endregion
        #region 車系信息
        /// <summary>
        /// 获取车系列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<BaseCarSeriesListViewModel> GetCarSeriesList(BaseCarSeriesQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"base_carseries as a left join base_carbrand as b on a.brandid=b.innerid ";
            const string fields = "a.innerid,seriesname,seriesgroupname,brandid,a.isenabled,ifnull(a.remark,'') remark,b.brandname ";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " brandid asc" : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(query.SeriesName))
            {
                sqlWhere.Append($" and seriesname like '%{query.SeriesName}%'");
            }
            if (!string.IsNullOrWhiteSpace(query.BrandId))
            {
                sqlWhere.Append($" and b.innerid = '{query.BrandId}'");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseCarSeriesListViewModel>(model, query.Echo);
            return list;
        }
        /// <summary>
        /// 获取车系信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public BaseCarSeriesModel GetCarSeriesById(string innerid)
        {
            const string sql = @"select a.*,b.brandname from base_carseries as a left join base_carbrand as b on a.brandid=b.innerid where a.innerid=@innerid;";
            try
            {
                var carseriesModel = Helper.Query<BaseCarSeriesModel>(sql, new { innerid }).FirstOrDefault();
                return carseriesModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 更改车系状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateSeriesStatus(string carid, int status)
        {
            const string sql = "update base_carseries set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = carid, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 添加车系信息AddCarSeries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCarSeries(BaseCarSeriesModel model)
        {
            const string sql = @"INSERT INTO `base_carseries`
                                (`innerid`,`seriesname`,`seriesgroupname`,`brandid`,`isenabled`,`remark`,hot)
                                VALUES
                                (@innerid,@seriesname,@seriesgroupname,@brandid,@isenabled,@remark,@hot);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 删除车系信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCarSeries(string innerid)
        {
            const string sql = @"delete from base_carseries where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        ///更新车系信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCarSeries(BaseCarSeriesModel model)
        {
            var sql = new StringBuilder("update `base_carseries` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 获取ID最大值
        /// </summary>
        /// <returns></returns>
        public int GetCarSeriesMaxId()
        {
            int result;
            const string sql = @"select max(innerid) MaxId from base_carseries;";
            result = Helper.Execute<int>(sql);
            return result;
        }

        /// <summary>
        ///  获取车系名称
        /// </summary>
        /// <param name="seriesname"></param>
        /// <param name="innerid"></param>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public BaseCarSeriesModel GetCarSeriesName(string seriesname, string innerid, string brandid)
        {
            string where = " isenabled=1 and seriesname='" + seriesname + "' and brandid ='" + brandid + "'";
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid!='{innerid}'";
            }
            var sql = $"select seriesname,innerid from base_carseries where {where} ";
            try
            {
                var carseriesModel = Helper.Query<BaseCarSeriesModel>(sql).FirstOrDefault();
                return carseriesModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 验证车系下面还有车型
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public string VerifyCarSeries(string innerid)
        {
            string result;
            const string sql = @"select b.modelname from base_carseries as a left join base_carmodel as b on a.innerid=b.seriesid where a.innerid=@innerid;";
            result = Helper.Execute<string>(sql, new { innerid });
            return result;
        }
        #endregion
        #region 车型信息
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<BaseCarModelListViewModel> GetCarModelList(BaseCarModelQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"base_carmodel as a left join base_carseries as b on a.seriesid=b.innerid left join base_carbrand c on b.brandid=c.innerid";
            const string fields = " a.innerid,modelname,modelprice,modelyear,minregyear,maxregyear,liter,geartype,dischargestandard,seriesid,a.isenabled,ifnull(a.remark,'') remark,b.seriesname,c.brandname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " innerid asc" : query.Order;
            //查詢條件
            var sqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(query.Modelname))
            {
                sqlWhere.Append($" and modelname like '%{query.Modelname}%'");
            }
            if (!string.IsNullOrWhiteSpace(query.BrandId))
            {
                sqlWhere.Append($" and c.innerid = '{query.BrandId}'");
            }
            if (!string.IsNullOrWhiteSpace(query.SeriesId))
            {
                sqlWhere.Append($" and b.innerid = '{query.SeriesId}'");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseCarModelListViewModel>(model, query.Echo);
            return list;
        }
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="innerid">id</param>
        /// <returns></returns>
        public BaseCarModelModel GetBaseCarModelById(string innerid)
        {
            const string sql = @"select a.*,b.seriesname,c.brandname from base_carmodel as a left join base_carseries as b on a.seriesid=b.innerid left join base_carbrand c on b.brandid=c.innerid where a.innerid=@innerid ;";
            try
            {
                var carmodelModel = Helper.Query<BaseCarModelModel>(sql, new { innerid }).FirstOrDefault();
                return carmodelModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 更改车型状态
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateModelStatus(string carid, int status)
        {
            const string sql = "update base_carmodel set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = carid, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 添加车型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCarModel(BaseCarModelModel model)
        {
            const string sql = @"INSERT INTO `base_carmodel`
                                (`innerid`,`modelname`,`modelprice`,`modelyear`,`minregyear`,`maxregyear`,`liter`,`geartype`,`dischargestandard`,`seriesid`,`isenabled`,`remark`)
                                VALUES
                                (@innerid,@modelname,@modelprice,@modelyear,@minregyear,@maxregyear,@liter,@geartype,@dischargestandard,@seriesid,@isenabled,@remark);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 删除车型信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCarModel(string innerid)
        {
            const string sql = @"delete from base_carmodel where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        ///更新车型信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCarModel(BaseCarModelModel model)
        {
            var sql = new StringBuilder("update `base_carmodel` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 获取车型ID最大值
        /// </summary>
        /// <returns></returns>
        public int GetCarModelMaxId()
        {
            int result;
            const string sql = @"select max(base_carmodel.innerid) MaxId from base_carmodel;";
            result = Helper.Execute<int>(sql);
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCarModelModel GetCarModelName(string modelname, string innerid)
        {
            string where = " isenabled=1 and modelname=" + modelname;
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid!='{innerid}'";
            }
            var sql = $"select modelname from base_carmodel where {where} ";
            try
            {
                var carseriesModel = Helper.Query<BaseCarModelModel>(sql).FirstOrDefault();
                return carseriesModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取系统后台基础信息

        #region 用户管理
        /// <summary>
        /// 获取登录人信息
        /// </summary>
        /// <param name="loginname">登录账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public BaseUserModel GetUserInfo(string loginname, string password)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from sys_user where loginname=@loginname and password=@password", loginname, password);
            try
            {
                var codemodel = Helper.Query<BaseUserModel>(sql.ToString(), new { loginname = loginname, password = password }).FirstOrDefault();
                return codemodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseUserModel> GetUserList(BaseUserQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"sys_user ";
            const string fields = "innerid, username, loginname, password, mobile, telephone, email, status, createdtime, modifiedtime";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " modifiedtime desc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            if (query.status != null)
            {
                sqlWhere.Append($" and status = %{query.status}%");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseUserModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加用户信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUser(BaseUserModel model)
        {
            const string sql = @"INSERT INTO `sys_user`
                                (`innerid`, `username`, `loginname`, `password`, `mobile`, `telephone`, `email`, `status`, `createdtime`, `modifiedtime`,depid,`level`)
                                VALUES
                                (uuid(), @username, @loginname, @password, @mobile, @telephone, @email, @status, now(), now(),@depid,@level);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 编辑用户信息  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateUser(BaseUserModel model)
        {
            var sql = new StringBuilder("update `sys_user` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateUserStatus(string innerid, int status)
        {
            const string sql = @"update `sys_user` set status=@status where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid, status = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseUserModel GetUserInfoByID(string innerid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from sys_user where innerid=@innerid; ", innerid);
            try
            {
                var usermodel = Helper.Query<BaseUserModel>(sql.ToString(), new { innerid = innerid }).FirstOrDefault();
                return usermodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户对应权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<BaseRoleUserModel> GetRoleByUerid(string userid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from sys_role_user where userid='{0}';", userid);
            var menuList = Helper.Query<BaseRoleUserModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 获取用户对应菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<BaseMenuModel> GetMenuByUerid(string userid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select t4.* from (select * from sys_user where innerid='{0}') as t1
                                inner join sys_role_user as t2 on t2.userid = t1.innerid
                                inner join sys_role_menu as t3 on t3.roleid = t2.roleid
                                inner join sys_menu as t4 on t4.innerid = t3.menuid; ", userid);
            var menuList = Helper.Query<BaseMenuModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<BaseUserModel> GetUserInfo(BaseUserModel model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select * from sys_user where 1=1 ");
            var sqlWhere = new StringBuilder();
            //用户名
            if (!string.IsNullOrWhiteSpace(model.loginname))
            {
                sqlWhere.AppendFormat(" and loginname ='{0}' ", model.loginname);
            }
            //部门
            if (!string.IsNullOrWhiteSpace(model.depid))
            {
                sqlWhere.AppendFormat(" and depid ='{0}' ", model.depid);
            }
            //等级
            if (model.level.HasValue)
            {
                sqlWhere.AppendFormat(" and level ={0} ", model.level);
            }
            sql.Append(sqlWhere.ToString());
            var menuList = Helper.Query<BaseUserModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 根据登入名获取用户信息
        /// </summary>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public BaseUserModel GetUserInfoByLoginName(string loginname)
        {
            var userinfo = GetUserInfo(new BaseUserModel() { loginname = loginname }).FirstOrDefault();
            return userinfo;
        }

        /// <summary>
        /// 根据登入名，id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public BaseUserModel GetUserInfoByidname(string id, string loginname)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select * from sys_user where 1=1 and loginname='{0}' and innerid !='{1}';", loginname, id);
            var userinfo = Helper.Query<BaseUserModel>(sql.ToString()).FirstOrDefault();
            return userinfo;
        }
        #endregion

        #region 角色管理

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseRoleViewModel> GetRoleList(BaseRoleQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"sys_role ";
            const string fields = "innerid, name, remark, isenabled";
            //var oldField = string.IsNullOrWhiteSpace(query.Order) ? " modifiedtime desc " : query.Order;
            var oldField = "";
            var sqlWhere = new StringBuilder("1=1");
            if (query.isenabled != null)
            {
                sqlWhere.Append($" and isenabled = %{query.isenabled}%");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseRoleViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseRoleModel> GetAllRole(BaseRoleModel model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select * from sys_role where isenabled=1 ;");
            var menuList = Helper.Query<BaseRoleModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 添加角色信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRole(BaseRoleModel model)
        {
            const string sql = @"INSERT INTO `sys_role`
                                (`innerid`, `name`, `remark`, `isenabled`)
                                VALUES
                                (uuid(), @name, @remark, 1);";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 编辑角色信息  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateRole(BaseRoleModel model)
        {
            var sql = new StringBuilder("update `sys_role` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="innerid"></param>
        /// <param name="isenabled"></param>
        /// <returns></returns>
        public int UpdateRoleStatus(string innerid, int isenabled)
        {
            const string sql = @"update `sys_role` set isenabled=@isenabled where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = innerid, isenabled = isenabled }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseRoleModel GetRoleInfoByID(string innerid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from sys_role where innerid=@innerid; ", innerid);
            try
            {
                var usermodel = Helper.Query<BaseRoleModel>(sql.ToString(), new { innerid = innerid }).FirstOrDefault();
                return usermodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<MenuViewMode> GetMenuList(MenuQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"sys_menu ";
            const string fields = "innerid, ifnull(code,'') as code,ifnull(name,'') as name, ifnull(url,'') as url, sort, parentid, level, openmode, isenabled, ifnull(remark,'') as remark, createdtime, modifiedtime, icon, submenu";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " level asc " : query.Order;
            //  var groupField = string.IsNullOrWhiteSpace(query.Group) ? " parentid " : query.Group; ;
            var sqlWhere = new StringBuilder("1=1");
            //菜单名称
            if (!string.IsNullOrWhiteSpace(query.name))
            {
                sqlWhere.AppendFormat(" and name like '%{0}%' ", query.name);
            }
            //层级
            if (query.level != null)
            {
                sqlWhere.AppendFormat(" and level = {0} ", query.level);
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<MenuViewMode>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuViewMode> GetAllMenu(BaseMenuModel model)
        {
            StringBuilder sqlwhere = new StringBuilder();
            //层级
            if (model.level != null)
            {
                sqlwhere.AppendFormat(" and level ={0} ", model.level);
            }
            //innerid
            if (!string.IsNullOrWhiteSpace(model.innerid))
            {
                sqlwhere.AppendFormat(" and innerid ='{0}' ", model.innerid);
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select innerid, ifnull(code,'') as code,ifnull(name,'') as name, ifnull(url,'') as url, sort, parentid, level, openmode, isenabled, ifnull(remark,'') as remark, createdtime, modifiedtime, 
                            icon, submenu from sys_menu where isenabled=1 {0} order by modifiedtime desc ;", sqlwhere.ToString());


            var menuList = Helper.Query<MenuViewMode>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddMenu(BaseMenuModel model)
        {
            const string sql = @"INSERT INTO `sys_menu`
                                (`innerid`, `code`, `name`, `url`, `sort`, `parentid`, `level`, `openmode`, `isenabled`, `remark`, `createdtime`, `modifiedtime`, `icon`, `submenu`)
                                VALUES
                                (uuid(), @code, @name, @url, @sort, @parentid, @level, @openmode, 1, @remark, now(), now(), @icon, @submenu);";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMenu(BaseMenuModel model)
        {
            var sql = new StringBuilder("update `sys_menu` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 删除菜单（物理删除）
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteMenu(string innerid)
        {
            const string sql = @"delete from sys_menu where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, new { innerid = innerid });
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseMenuModel GetMenuInfo(string innerid)
        {
            var menuIfo = GetAllMenu(new BaseMenuModel() { innerid = innerid }).FirstOrDefault();
            return menuIfo;
        }

        /// <summary>
        /// 给角色赋相应的权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRoleMenu(BaseRoleMenuModel model)
        {
            //删除角色对应数据
            const string sql = @"delete from sys_role_menu where roleid=@roleid;";
            var menuids = model.menuid.TrimEnd(',').Split(',');
            string sqlAdd = string.Empty;
            foreach (var menuid in menuids)
            {
                sqlAdd = sqlAdd + "INSERT INTO `sys_role_menu` (`innerid`,`roleid`,`menuid`) VALUES (uuid(),'" + model.roleid + "','" + menuid + "');";
            }
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { roleid = model.roleid }, tran);
                    conn.Execute(sqlAdd, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 保存职员对应角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUserRole(BaseRoleUserModel model)
        {
            //删除角色对应数据
            const string sql = @"delete from sys_role_user where userid=@userid;";
            var roleids = model.roleid.TrimEnd(',').Split(',');
            string sqlAdd = string.Empty;
            foreach (var roleid in roleids)
            {
                sqlAdd = sqlAdd + "INSERT INTO `sys_role_user` (`innerid`,`roleid`,`userid`) VALUES (uuid(),'" + roleid + "','" + model.userid + "');";
            }
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { userid = model.userid }, tran);
                    conn.Execute(sqlAdd, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取角色对应所有的菜单
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public IEnumerable<BaseRoleMenuModel> GetRoleToMenu(string roleid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select `innerid`,`roleid`,`menuid` from sys_role_menu where roleid= '{0}';", roleid);

            var menuList = Helper.Query<BaseRoleMenuModel>(sql.ToString());
            return menuList;
        }


        #endregion

        #region 部门管理
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseDepartmentViewModel> GetManageCityList(BaseDepartmentQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"sys_department ";
            const string fields = "`id`, `name`, `areaid`, `tel`, `email`, `sort`, `remark`, `code`";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? " sort asc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            //菜单名称
            if (!string.IsNullOrWhiteSpace(query.name))
            {
                sqlWhere.AppendFormat(" and name like '%{0}%' ", query.name);
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseDepartmentViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseDepartmentViewModel> GetAllDepartment(BaseDepartmentModel model)
        {
            StringBuilder sqlwhere = new StringBuilder();
            //innerid
            if (!string.IsNullOrWhiteSpace(model.id))
            {
                sqlwhere.AppendFormat(" and id ='{0}' ", model.id);
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select `id`, `name`, `areaid`, `tel`, `email`, `sort`, `remark`, `code`
                                from sys_department where 1=1 {0} order by sort asc ;", sqlwhere.ToString());

            var menuList = Helper.Query<BaseDepartmentViewModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDepartment(BaseDepartmentModel model)
        {
            const string sql = @"INSERT INTO `sys_department`
                                (`id`, `name`, `areaid`, `tel`, `email`, `sort`, `remark`, `code`)
                                VALUES
                                (uuid(), @name, @areaid, @tel, @email, @sort, @remark, @code);";
            using (var conn = Helper.GetConnection())
            {
                try
                {
                    conn.Execute(sql, model);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDepartment(BaseDepartmentModel model)
        {
            var sql = new StringBuilder("update `sys_department` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where id = @id");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 获取职员对应所有的部门
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<BaseUserDepartmentModel> GetUserToDepartment(string userid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select `innerid`,`userid`,`cityid` from sys_user_city where userid= '{0}';", userid);

            var menuList = Helper.Query<BaseUserDepartmentModel>(sql.ToString());
            return menuList;
        }

        /// <summary>
        /// 保存职员对应部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUserDepartment(BaseUserDepartmentAddModel model)
        {
            //删除用户对应数据
            const string sql = @"delete from sys_user_city where userid=@userid;";
            //var ids = model.ids.TrimEnd(',').Split(',');
            string sqlAdd = string.Empty;
            foreach (var cityid in model.ids)
            {
                sqlAdd = sqlAdd + "INSERT INTO `sys_user_city` (`innerid`,`cityid`,`userid`) VALUES (uuid()," + cityid + ",'" + model.userid + "');";
            }
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { userid = model.userid }, tran);
                    conn.Execute(sqlAdd, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseDepartmentModel GetDepartmentByID(string id)
        {
            var depinfo = GetAllDepartment(new BaseDepartmentModel() { id = id }).FirstOrDefault();
            return depinfo;
        }

        #endregion  

        #endregion

        #region 广告管理

        /// <summary>
        /// 获取广告列表--分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<BaseBannerPageListModel> GetBannerPageList(BaseBannerQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"base_banner ";
            const string fields = "innerid, title, picurl, linkurl, autoenabletime, autodisabletime, sort, isenabled, createrid, createdtime, modifierid, modifiedtime";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " sort asc " : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                sqlWhere.Append($" and title like '%{query.Title}%'");
            }

            if (query.Isenabled != null)
            {
                sqlWhere.Append($" and isenabled = '{query.Isenabled}'");
            }
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<BaseBannerPageListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseBannerListModel> GetBannerList()
        {
            const string sql = "select innerid, title, picurl, linkurl from base_banner where isenabled=1 and autoenabletime<=now() and autodisabletime>now() order by sort asc;";
            var list = Helper.Query<BaseBannerListModel>(sql);
            return list;
        }

        /// <summary>
        /// 更新广告状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateBannerStatus(string id, int status)
        {
            const string sql = "update base_banner set isenabled=@isenabled where innerid=@innerid";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = id, isenabled = status }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteBannerById(string innerid)
        {
            const string sql = @"delete from base_banner where innerid=@innerid;";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new {innerid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取广告详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseBannerModel GetBannerById(string innerid)
        {
            const string sql = @"select innerid, title, picurl, linkurl, autoenabletime, autodisabletime, sort, remark, isenabled, createrid, createdtime, modifierid, modifiedtime from base_banner where innerid=@innerid";
            try
            {
                var model = Helper.Query<BaseBannerModel>(sql, new { innerid }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBanner(BaseBannerModel model)
        {
            const string sql = @"INSERT INTO `base_banner`
                                (innerid, title, picurl, linkurl, autoenabletime, autodisabletime, sort, remark, isenabled, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @title, @picurl, @linkurl, @autoenabletime, @autodisabletime, @sort, @remark, @isenabled, @createrid, @createdtime, @modifierid, @modifiedtime);";
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBanner(BaseBannerModel model)
        {
            var sql = new StringBuilder("update `base_banner` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            int result;
            try
            {
                result = Helper.Execute(sql.ToString(), model);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        
        #endregion
    }
}
