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
    public class BaseDA  : DataAccessBase
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
            if (!string.IsNullOrWhiteSpace(query.Typekey)) {
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
        /// 获取基础数据代码类型key
        /// </summary>
        /// <param name="typekey"></param>
        /// <returns></returns>
        public string GetCodeTypeByTypeKey(string typekey)
        {
            string  result;
            const string sql = "select typekey from base_code_type where isenabled=1 and typekey=@typekey;";
            result = Helper.ExecuteScalar<string>(sql,new { typekey});
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
            const string fields = "a.innerid,codevalue,codename,sort,a.typekey,a.isenabled,remark,b.typename";
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
        public BaseCodeModel GetCodeByTypeKey(string typekey,string codename,string codevalue)
        {

            string where = " isenabled=1 and typekey='" +typekey+"'";
            if (!string.IsNullOrWhiteSpace(codename))
            {
                where += $" or codename ='{codename}'";
            }
            if (!string.IsNullOrWhiteSpace(codevalue))
            {
                where += $" or codevalue ='{codevalue}'";
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
        public BaseVerification GetVerification(string target,int utype)
        {
            const string sql = "select innerid, target, vcode, valid, createdtime, ttype, utype,content, result from base_verification where target=@target and utype=@utype order by createdtime desc limit 1;";
            var m = Helper.Query<BaseVerification>(sql, new {target, utype }).FirstOrDefault();
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
        public IEnumerable<BaseCity> GetCityList(int provId,string initial)
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
            var sql = $"select innerid, brandname, initial, isenabled, remark, logurl,hot from base_carbrand where {where} order by initial asc,brandname asc";
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
            var sql = $"select innerid, seriesname, seriesgroupname, brandid, isenabled, remark from base_carseries where isenabled=1 and brandid={brandId}";
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
        public BaseCarModelModel GetCarBrandName(string brandname, string innerid)
        {
            string where = " isenabled=1 and brandname='" + brandname + "'";
            if (!string.IsNullOrWhiteSpace(innerid))
            {
                where += $" and innerid!='{innerid}'";
            }
            var sql = $"select brandname,innerid from base_carbrand where {where} ";
            try
            {
                var carbrandModel = Helper.Query<BaseCarModelModel>(sql).FirstOrDefault();
                return carbrandModel;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                                (`innerid`,`seriesname`,`seriesgroupname`,`brandid`,`isenabled`,`remark`)
                                VALUES
                                (@innerid,@seriesname,@seriesgroupname,@brandid,@isenabled,@remark);";
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
        public int  GetCarSeriesMaxId()
        {
            int result;
            const string sql = @"select max(base_carseries.innerid) MaxId from base_carseries;";
            result = Helper.Execute<int>(sql);
            return result;
        }
        /// <summary>
        ///  获取车系名称
        /// </summary>
        /// <param name="seriesname"></param>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public BaseCarSeriesModel GetCarSeriesName(string seriesname, string innerid)
        {
            string where = " isenabled=1 and seriesname='" + seriesname+"'";
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
    }
}
