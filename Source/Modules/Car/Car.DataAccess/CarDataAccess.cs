using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CCN.Modules.Car.BusinessEntity;
using Cedar.Core.Logging;
using Cedar.Framework.AuditTrail.Interception;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Dapper;

namespace CCN.Modules.Car.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class CarDataAccess : DataAccessBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public CarDataAccess()
        {
        }

        #region 车辆基本信息

        /// <summary>
        /// 全城搜车(官网页面)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListEx(CarGlobalExQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("a.status=1"); //在售车辆

            //省份
            if (query.provid != null)
            {
                sqlWhere.Append($" and a.provid={query.provid}");
            }

            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and a.cityid={query.cityid}");
            }

            //品牌
            if (query.brand_id != null && query.brand_id != 0)
            {
                sqlWhere.Append($" and a.brand_id={query.brand_id}");
            }

            //车系
            if (query.series_id != null && query.series_id != 0)
            {
                sqlWhere.Append($" and a.series_id={query.series_id}");
            }

            //车型
            if (query.model_id != null && query.model_id != 0)
            {
                sqlWhere.Append($" and a.model_id={query.model_id}");
            }

            //销售价大于..
            if (query.minprice.HasValue)
            {
                sqlWhere.Append($" and a.price>={query.minprice}");
            }

            //销售价小于..
            if (query.maxprice.HasValue)
            {
                sqlWhere.Append($" and a.price<={query.maxprice}");
            }

            //上牌时间 <
            if (query.minyear.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.minyear.Value).ToShortDateString();
                sqlWhere.Append($" and a.register_date<='{date}'");
            }

            //上牌时间 >
            if (query.maxyear.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.maxyear.Value).ToShortDateString();
                sqlWhere.Append($" and a.register_date>={date}");
            }

            //行驶里程 >
            if (query.minmileage.HasValue)
            {
                sqlWhere.Append($" and a.mileage>='{query.minmileage}'");
            }

            //行驶里程 <
            if (query.maxmileage.HasValue)
            {
                sqlWhere.Append($" and a.mileage<='{query.maxmileage}'");
            }

            //颜色
            if (query.colorid.HasValue)
            {
                sqlWhere.Append($" and a.colorid={query.colorid}");
            }

            //排量
            if (!string.IsNullOrWhiteSpace(query.liter))
            {
                sqlWhere.Append($" and c3.liter='{query.liter.Trim()}'");
            }

            //变速箱类型
            if (!string.IsNullOrWhiteSpace(query.gear))
            {
                sqlWhere.Append($" and c3.geartype='{query.gear.Trim()}'");
            }

            //关键字搜索
            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                sqlWhere.Append($" and (c1.brandname like '%{query.keyword}%' or c2.seriesname like '%{query.keyword}%')");
            }

            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 全城搜车列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageList(CarGlobalQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname," +
                                  $" (select count(1) from cust_relations where userid='{query.custid}' and friendsid=a.custid) as isfriend";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("a.status=1");

            //省份
            if (query.provid != null)
            {
                sqlWhere.Append($" and a.provid={query.provid}");
            }

            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and a.cityid={query.cityid}");
            }

            //品牌
            if (query.brand_id != null && query.brand_id != 0)
            {
                sqlWhere.Append($" and a.brand_id={query.brand_id}");
            }

            //车系
            if (query.series_id != null && query.series_id != 0)
            {
                sqlWhere.Append($" and a.series_id={query.series_id}");
            }

            //车型
            if (query.model_id != null && query.model_id != 0)
            {
                sqlWhere.Append($" and a.model_id={query.model_id}");
            }

            //销售价大于..
            if (query.minprice.HasValue)
            {
                sqlWhere.Append($" and a.price>={query.minprice}");
            }

            //销售价小于..
            if (query.maxprice.HasValue)
            {
                sqlWhere.Append($" and a.price<={query.maxprice}");
            }

            if (query.minyear.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.minyear.Value).ToShortDateString();
                sqlWhere.Append($" and a.register_date<='{date}'");
            }

            if (query.maxyear.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.maxyear.Value).ToShortDateString();
                sqlWhere.Append($" and a.register_date>={date}");
            }

            if (!string.IsNullOrWhiteSpace(query.keyword) && query.model_id == null)
            {
                //sqlWhere.Append($" and (c1.brandname like '%{query.keyword}%' or c2.seriesname like '%{query.keyword}%')");
                //车辆添加时会将【品牌/车系】放到该字段
                sqlWhere.Append($" and title like '%{query.keyword}%'");
            }

            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveSearchRecord(CarSearchRecordModel model)
        {
            const string sql = @"INSERT INTO `car_searchrecord`
                                (innerid, custid, createdtime, jsonobj)
                                VALUES
                                (@innerid, @custid, @createdtime, @jsonobj);";
            int result;
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
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarPageList(CarQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            const string fields = "a.innerid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("1=1");

            sqlWhere.Append(query.status != null
                ? $" and a.status={query.status}"
                : " and a.status<>0");

            if (!string.IsNullOrWhiteSpace(query.custid))
            {
                sqlWhere.Append($" and a.custid='{query.custid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.title))
            {
                sqlWhere.Append($" and a.title like '%{query.title}%'");
            }

            //省份
            if (query.provid != null)
            {
                sqlWhere.Append($" and a.provid={query.provid}");
            }

            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and a.cityid={query.cityid}");
            }

            //品牌
            if (query.brand_id != null && query.brand_id != 0)
            {
                sqlWhere.Append($" and a.brand_id={query.brand_id}");
            }

            //车系
            if (query.series_id != null && query.series_id != 0)
            {
                sqlWhere.Append($" and a.series_id={query.series_id}");
            }

            //车型
            if (query.model_id != null && query.model_id != 0)
            {
                sqlWhere.Append($" and a.model_id={query.model_id}");
            }

            //收购价大于..
            if (query.minbuyprice.HasValue)
            {
                sqlWhere.Append($" and a.buyprice>={query.minbuyprice}");
            }

            //收购价小于..
            if (query.maxbuyprice.HasValue)
            {
                sqlWhere.Append($" and a.buyprice<={query.maxbuyprice}");
            }

            if (!string.IsNullOrWhiteSpace(query.SearchField) && query.model_id == null)
            {
                //sqlWhere.Append($" and (c1.brandname like '%{query.SearchField}%' or c2.seriesname like '%{query.SearchField}%')");
                //车辆添加时会将【品牌/车系】放到该字段
                sqlWhere.Append($" and title like '%{query.SearchField}%'");
            }

            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carSets">车辆ID集合</param>
        /// <returns></returns>
        public IEnumerable<CarShareModel> GetShareList(string carSets)
        {
            var sql = $"select innerid, carid, sharecount, seecount, praisecount, commentcount from car_share where carid in ({carSets});";
            try
            {
                return Helper.Query<CarShareModel>(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        /// <summary>
        /// 获取车辆详细信息(info)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public CarInfoModel GetCarInfoById(string id)
        {
            const string sql =
                @"select 
                a.innerid,
                a.custid,
                a.carid,
                a.title,
                a.pic_url,
                a.provid,
                a.cityid,
                a.brand_id,
                a.series_id,
                a.model_id,
                a.colorid,
                a.mileage,
                a.register_date,
                a.buytime,
                a.buyprice,
                a.price,
                a.dealprice,
                a.isproblem,
                a.remark,
                a.ckyear_date,
                a.tlci_date,
                a.audit_date,
                a.istain,
                a.sellreason,
                a.masterdesc,
                a.dealdesc,
                a.deletedesc,
                a.estimateprice,
                a.status,
                a.createdtime,
                a.modifiedtime,
                a.seller_type,
                a.post_time,
                a.audit_time,
                a.sold_time,
                a.closecasetime,
                a.eval_price,
                a.next_year_eval_price,
                pr.provname,
                ct.cityname,
                cb.brandname as brand_name,
                cs.seriesname as series_name,
                cm.modelname as model_name,
                cm.liter,
                cm.geartype,
                cm.dischargestandard as dischargeName,
                bc1.codename as color
                from `car_info` as a 
                left join base_province as pr on a.provid=pr.innerid
                left join base_city as ct on a.cityid=ct.innerid
                left join base_carbrand as cb on a.brand_id=cb.innerid
                left join base_carseries as cs on a.series_id=cs.innerid
                left join base_carmodel as cm on a.model_id=cm.innerid
                left join base_code as bc1 on a.colorid=bc1.codevalue and bc1.typekey='car_color'
                where a.innerid=@innerid";
            var result = Helper.Query<CarInfoModel>(sql, new { innerid = id }).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 获取车辆详情(view)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public CarInfoModel GetCarViewById(string id)
        {
            const string sql =
                @"select a.innerid, a.custid, a.carid, a.title, pic_url, 
                a.mileage, a.register_date, a.buytime, a.buyprice, 
                a.price, a.dealprice, a.isproblem, 
                a.remark, a.ckyear_date, a.tlci_date, a.audit_date, a.istain, 
                sellreason, a.masterdesc, a.dealdesc, a.deletedesc, a.estimateprice, 
                a.`status`, a.createdtime, a.modifiedtime, 
                a.seller_type,
                a.post_time, a.audit_time, a.sold_time, a.closecasetime, 
                a.eval_price, a.next_year_eval_price, 
                pr.provname,
                ct.cityname,
                cb.brandname as brand_name,
                cs.seriesname as series_name,
                cm.modelname as model_name,
                cm.liter,
                cm.geartype,
                cm.dischargestandard as dischargeName,
                bc1.codename as color
                from `car_info` as a 
                left join base_province as pr on a.provid=pr.innerid
                left join base_city as ct on a.cityid=ct.innerid
                left join base_carbrand as cb on a.brand_id=cb.innerid
                left join base_carseries as cs on a.series_id=cs.innerid
                left join base_carmodel as cm on a.model_id=cm.innerid
                left join base_code as bc1 on a.colorid=bc1.codevalue and bc1.typekey='car_color'
                where a.innerid=@innerid";
            var result = Helper.Query<CarInfoModel>(sql, new { innerid = id }).FirstOrDefault();
            return result;
        }

        #region 感兴趣

        /// <summary>
        /// 获取感兴趣的车列表(根据车系)
        /// </summary>
        /// <param name="carid">当前查看的车辆id</param>
        /// <returns></returns>
        public IEnumerable<CarInfoListViewModel> GetInterestBySeriesList(string carid)
        {
            var sql = "SELECT * FROM car_info where innerid<>@carid and series_id=1 order by rand() limit 4;";
            try
            {
                return Helper.Query<CarInfoListViewModel>(sql,new
                {
                    carid                
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取感兴趣的车列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetInterestList(CarInterestQueryModel query)
        {

            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a
                                        left join base_carbrand as c1 on a.brand_id = c1.innerid
                                        left join base_carseries as c2 on a.series_id = c2.innerid
                                        left join base_carmodel as c3 on a.model_id = c3.innerid
                                        left join base_city as ct on a.cityid = ct.innerid
                                        inner join car_share as b on a.innerid = b.carid";
            const string fields =
                "a.innerid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "b.seecount" : query.Order;

            #region 查询条件

            var sqlWhere = new StringBuilder("a.status=1");

            sqlWhere.Append(
                $" and a.innerid <> '{query.carid}' and(a.series_id = {query.series_id} or(a.price > {query.price - 3 ?? 0} and a.price < {query.price + 5 ?? 0}) or(register_date > '{query.regdate?.AddMonths(-6) ?? DateTime.Now}' and register_date < '{query.regdate?.AddMonths(6) ?? DateTime.Now}'))");

            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        #endregion

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int AddCar(CarInfoModel model)
        {
            const string sql = @"INSERT INTO `car_info`
                                (`innerid`,`custid`,`carid`,`title`,`pic_url`,`provid`,`cityid`,`brand_id`,`series_id`,`model_id`,`colorid`,`mileage`,`register_date`,`buytime`,`buyprice`,`price`,`dealprice`,`isproblem`,`remark`,`ckyear_date`,`tlci_date`,`audit_date`,`istain`,`sellreason`,`masterdesc`,`dealdesc`,`deletedesc`,`estimateprice`,`status`,`createdtime`,`modifiedtime`,`seller_type`,`post_time`,`audit_time`,`sold_time`,`closecasetime`,`eval_price`,`next_year_eval_price`)
                                VALUES
                                (@innerid,@custid,@carid,@title,@pic_url,@provid,@cityid,@brand_id,@series_id,@model_id,@colorid,@mileage,@register_date,@buytime,@buyprice,@price,@dealprice,@isproblem,@remark,@ckyear_date,@tlci_date,@audit_date,@istain,@sellreason,@masterdesc,@dealdesc,@deletedesc,@estimateprice,@status,@createdtime,@modifiedtime,@seller_type,@post_time,@audit_time,@sold_time,@closecasetime,@eval_price,@next_year_eval_price);";
            int result;
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
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateCar(CarInfoModel model)
        {
            var sql = new StringBuilder("update `car_info` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));

            //非必填字段的修改
            if (!model.buytime.HasValue)
            {
                sql.Append(",buytime=null");
            }
            if (!model.buyprice.HasValue)
            {
                sql.Append(",buyprice=null");
            }
            if (!model.ckyear_date.HasValue)
            {
                sql.Append(",ckyear_date=null");
            }
            if (!model.tlci_date.HasValue)
            {
                sql.Append(",tlci_date=null");
            }
            if (!model.audit_date.HasValue)
            {
                sql.Append(",audit_date=null");
            }

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
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        public int DeleteCar(string id)
        {
            const string sql = @"delete from car_info where innerid`=@innerid;";
            try
            {
                Helper.Execute(sql, new { innerid = id });
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public int DeleteCar(CarInfoModel model)
        {
            try
            {
                const string sql = "update car_info set status=0,deletedesc=@deletedesc where `innerid`=@innerid;";
                Helper.Execute(sql, new { innerid = model.Innerid, model.deletedesc });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 车辆成交
        /// </summary>
        /// <param name="model">车辆成交model</param>
        /// <returns>1.操作成功</returns>
        public int DealCar(CarInfoModel model)
        {
            try
            {
                const string sql = "update car_info set status=2,dealprice=@dealprice,dealdesc=@dealdesc,sold_time=@sold_time,closecasetime=@closecasetime where `innerid`=@innerid;";
                Helper.Execute(sql, new { innerid = model.Innerid, model.dealprice, model.dealdesc });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 保存评估记录
        /// </summary>
        /// <param name="carInfo">车辆信息</param>
        /// <returns>1.操作成功</returns>
        public int SaveCarEvaluateInfo(CarEvaluateModel carInfo)
        {
            try
            {
                const string sql = @"insert into `car_evaluation` (`innerid`, `model_id`, `cityid`, `register_date`, `mileage`, `estimateprice`,`price`, `createdtime`,`openid`) 
                    values (uuid(),@modelid, @cityid, @registerdate, @mileage, @estimateprice,now());";
                Helper.Execute(sql, new { modelid = carInfo.model_id, cityid = carInfo.cityid, registerdate = carInfo.register_date, mileage = carInfo.mileage, estimateprice = carInfo.estimateprice, price = carInfo.price, openid = carInfo.openid });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 保存评估信息
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="evaluate"></param>
        /// <returns>1.操作成功</returns>
        public int SaveCarEvaluateInfo(string carid, string evaluate)
        {
            try
            {
                const string sql = "update car_info set estimateprice=@estimateprice where `innerid`=@innerid;";
                Helper.Execute(sql, new { innerid = carid, estimateprice = evaluate });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 获取本月本车型成交数量
        /// </summary>
        /// <param name="modelid">车型id</param>
        /// <returns></returns>
        public int GetCarSales(string modelid)
        {
            const string sql1 = "select count(1) from car_info where model_id=@modelid and `status`=2 and date_format(sold_time,'%Y-%m')=date_format(now(),'%Y-%m');";
            const string sql2 = "select count(1) from car_info_bak where model_id=@modelid and date_format(modifiedtime,'%Y-%m')=date_format(now(),'%Y-%m');";
            var num1 = Helper.ExecuteScalar<int>(sql1, new { modelid });
            var num2 = Helper.ExecuteScalar<int>(sql2, new { modelid });
            return num1 + num2;
        }

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        public int UpdateCarStatus(string carid, int status)
        {
            try
            {
                const string sql = "update car_info set status=@status where `innerid`=@innerid;";
                Helper.Execute(sql, new { innerid = carid, status });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int ShareCar(string id)
        {
            try
            {
                //累计分享次数
                var sql = "update car_share set sharecount=sharecount+1 where carid=@carid;";
                var resCount = Helper.Execute(sql, new { carid = id });
                //if (resCount == 0)
                //{
                //    //表示没有子表数据
                //    sql = "INSERT INTO `car_share`(`innerid`,`carid`,`sharecount`,`seecount`,`praisecount`,`commentcount`) VALUES(uuid(), @carid, 1, 0, 0, 0);";
                //    Helper.Execute(sql, new { carid = id });
                //}
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="count">新增次数</param>
        /// <returns>1.累计成功</returns>
        public int UpSeeCount(string id, int count = 1)
        {
            const string sql = @"update car_share set seecount=seecount+@count where carid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid = id, count = count });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public int UpPraiseCount(string id)
        {
            const string sql = @"update car_share set praisecount=praisecount+1 where carid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid = id });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 累计评论次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        public int CommentCar(string id, string content)
        {
            const string sql = @"update car_share set commentcount=commentcount+1 where carid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid = id });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 初始化车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public int AddShareInfo(string carid)
        {
            const string sql = "insert into car_share (innerid, carid, sharecount, seecount, praisecount, commentcount) values (uuid(), @carid, 0, 0, 0, 0);";
            var result = Helper.Execute(sql, new { carid });
            return result;
        }

        /// <summary>
        /// 获取车辆 分享/查看次数
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public CarShareModel GetCarShareInfo(string carid)
        {
            const string sql = @"select innerid, carid, sharecount, seecount, commentcount from car_share where carid=@carid;";
            var result = Helper.Query<CarShareModel>(sql, new { carid }).FirstOrDefault();
            return result;
        }
        
        #region 赞不用

        /// <summary>
        /// 审核车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="status">审核状态</param>
        /// <returns>1.操作成功</returns>
        public int AuditCar(string id, int status)
        {
            const string sql = @"UPDATE `car_info` SET status=@status,`audit_time`=@audit_time WHERE `innerid`=@innerid;";
            try
            {
                var result = Helper.Execute(sql, new
                {
                    status,
                    audit_time = DateTime.Now,
                    innerid = id
                });
                if (result == 0)
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 核销车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int CancelCar(string id)
        {
            const string sql = @"UPDATE `car_info` SET `sold_time`=@sold_time WHERE `innerid`=@innerid;";
            try
            {
                var result = Helper.Execute(sql, new { sold_time = DateTime.Now, innerid = id });
                if (result == 0)
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        #endregion

        #endregion

        #region 车辆图片

        /// <summary>
        /// 添加车辆图片
        /// </summary>
        /// <param name="model">车辆图片信息</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.AddCarPicture")]
        public int AddCarPicture(CarPictureModel model)
        {
            const string sql = @"INSERT INTO car_picture
                        (innerid, carid, typeid, path, sort, createdtime)
                        VALUES
                        (@innerid, @carid, @typeid, @path, @sort, @createdtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 设置车辆封面图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="imgKey">封面图片key</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.SetCarCover")]
        public int SetCarCover(string carid, string imgKey)
        {
            const string sql = @"update car_info set pic_url=@pic_url where innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new { innerid = carid, pic_url = imgKey });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除车辆图片
        /// </summary>
        /// <param name="innerid">车辆图片id</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.DeleteCarPicture")]
        public int DeleteCarPicture(string innerid)
        {
            using (var conn = Helper.GetConnection())
            {
                //参数
                var obj = new
                {
                    p_pictureid = innerid
                };

                var args = new DynamicParameters(obj);
                args.Add("p_values", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var result = conn.QueryMultiple("ccnsp_deletepicture", args, commandType: CommandType.StoredProcedure))
                {
                    //获取结果集
                    //var data = result.Read<T>();
                }

                return args.Get<int>("p_values");
            }


            //const string sql = @"delete from car_picture where innerid=@innerid;";

            //try
            //{
            //    Helper.Execute(sql, new {innerid});
            //    return 1;
            //}
            //catch (Exception ex)
            //{
            //    return 0;
            //}
        }

        /// <summary>
        /// 图片调换位置
        /// </summary>
        /// <param name="listPicture">车辆图片列表</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.ExchangePictureSort")]
        public int ExchangePictureSort(List<CarPictureModel> listPicture)
        {
            const string sql = @"update car_picture set sort=@sort where innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { innerid = listPicture[0].Innerid, sort = listPicture[0].Sort }, tran);
                    conn.Execute(sql, new { innerid = listPicture[1].Innerid, sort = listPicture[1].Sort }, tran);

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
        /// 获取车辆已有图片的最大排序
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.GetCarPictureMaxSort")]
        public int GetCarPictureMaxSort(string carid)
        {
            const string sql = @"select max(sort) from car_picture where carid=@carid;";

            try
            {
                var maxsort = Helper.ExecuteScalar<int>(sql, new { carid });
                return maxsort;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        [AuditTrailCallHandler("CarDataAccess.GetCarPictureByCarid")]
        public IEnumerable<CarPictureModel> GetCarPictureByCarid(string carid)
        {
            const string sql = @"select innerid, carid, typeid, path, sort, createdtime from car_picture where carid=@carid order by sort asc;";

            try
            {
                var list = Helper.Query<CarPictureModel>(sql, new { carid });
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 车辆收藏

        /// <summary>
        /// 检查重复收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarCollectionModel CheckCollection(CarCollectionModel model)
        {
            const string sql =
                @"select innerid, custid, carid, remark, createdtime from car_collection where custid=@custid and carid=@carid;";
            return
                Helper.Query<CarCollectionModel>(sql, new {custid = model.Custid, carid = model.Carid}).FirstOrDefault();

        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCollection(CarCollectionModel model)
        {
            const string sql = @"INSERT INTO car_collection
                        (innerid, custid, carid, remark, createdtime)
                        VALUES
                        (@innerid, @custid, @carid, @remark, @createdtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加收藏异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除收藏 by innerid
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteCollection(string innerid)
        {
            const string sql = "delete from car_collection where innerid=@innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除收藏异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除收藏 by carid
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public int DeleteCollectionByCarid(string carid)
        {
            const string sql = "delete from car_collection where carid=@carid;";

            try
            {
                Helper.Execute(sql, new { carid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除收藏异常[by carid]：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取收藏的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarCollectionViewListModel> GetCollectionList(CarCollectionQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_collection as b 
                                    inner join car_info as a on a.innerid=b.carid
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid";
            var fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date," +
                         "c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname,b.remark,b.createdtime as Collectiontime," +
                         $" (select count(1) from cust_relations where userid='{query.Custid}' and friendsid=a.custid) as isfriend";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "b.createdtime desc" : query.Order;

            #region 查询条件

            var sqlWhere = $"b.custid='{query.Custid}' and a.status<>0";

            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarCollectionViewListModel>(model, query.Echo);
            return list;
        }

        #endregion
    }
}
