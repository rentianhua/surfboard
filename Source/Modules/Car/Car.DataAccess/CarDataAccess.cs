﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// 全城搜车(官网页面)（置顶车辆补齐）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListTopFill(CarTopFillQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid 
                                    inner join cust_info as cc on cc.innerid=a.custid ";
            const string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname,cc.type";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.price desc,a.refreshtime desc" : query.Order;  //排序以后调整为 支付时间
            var sqlWhere = new StringBuilder("a.status=1 and a.istop=1");

            if (!string.IsNullOrWhiteSpace(query.where))
            {
                query.where = "a.status=1 and a.istop=1 " + query.where;
                query.where =
                    query.where
                        .Replace("a.", "ia.")
                        .Replace("c1.", "ic1.")
                        .Replace("c2.", "ic2.")
                        .Replace("c3.", "ic3.");

                sqlWhere.Append(
                    $@" and a.innerid not in (select ia.innerid from car_info as ia 
                                    left join base_carbrand as ic1 on ia.brand_id = ic1.innerid
                                    left join base_carseries as ic2 on ia.series_id = ic2.innerid
                                    left join base_carmodel as ic3 on ia.model_id = ic3.innerid where {
                        query.where})");
            }
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 全城搜车(官网页面)（查询到置顶车辆）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> SearchCarPageListTop(CarGlobalExQueryModel query, out string strwhere)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid 
                                    inner join cust_info as cc on cc.innerid=a.custid ";
            const string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname,cc.type";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.price desc,a.refreshtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("a.status=1 and a.istop=1");

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
                sqlWhere.Append($" and a.register_date>='{date}'");
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

            strwhere = sqlWhere.ToString().Replace("a.status=1 and a.istop=1", "").Trim();   //只需要手动设置的条件
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

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
                                    left join base_city as ct on a.cityid=ct.innerid 
                                    inner join cust_info as cc on cc.innerid=a.custid ";
            string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname,cc.type";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.refreshtime desc" : query.Order;

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
                sqlWhere.Append($" and a.register_date>='{date}'");
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
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.refreshtime desc" : query.Order;

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
                sqlWhere.Append($" and a.register_date>='{date}'");
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
            const string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,a.istop,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
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
            //用户ID
            if (!string.IsNullOrWhiteSpace(query.userid))
            {
                sqlWhere.Append($" and cityid in (select cityid from sys_user_city where userid='{query.userid}')");
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
                return Helper.Query<CarInfoListViewModel>(sql, new
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
                "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
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
                                (`innerid`,`custid`,`carid`,`title`,`pic_url`,`provid`,`cityid`,`brand_id`,`series_id`,`model_id`,`colorid`,`mileage`,`register_date`,`buytime`,`buyprice`,`price`,`dealprice`,`isproblem`,`remark`,`ckyear_date`,`tlci_date`,`audit_date`,`istain`,`sellreason`,`masterdesc`,`dealdesc`,`deletedesc`,`estimateprice`,`status`,`createdtime`,`modifiedtime`,`seller_type`,`post_time`,`audit_time`,`sold_time`,`closecasetime`,`eval_price`,`next_year_eval_price`,`refreshtime`, `istop`, `istransferfee`)
                                VALUES
                                (@innerid,@custid,@carid,@title,@pic_url,@provid,@cityid,@brand_id,@series_id,@model_id,@colorid,@mileage,@register_date,@buytime,@buyprice,@price,@dealprice,@isproblem,@remark,@ckyear_date,@tlci_date,@audit_date,@istain,@sellreason,@masterdesc,@dealdesc,@deletedesc,@estimateprice,@status,@createdtime,@modifiedtime,@seller_type,@post_time,@audit_time,@sold_time,@closecasetime,@eval_price,@next_year_eval_price,@refreshtime, @istop, @istransferfee);";

            const string sqlSelectCust = "select count(1) as count from cust_info where innerid=@custid;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var num = conn.Query<int>(sqlSelectCust, new {model.custid}).FirstOrDefault();
                    if (0 == num)
                    {
                        return -1;
                    }

                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加车辆异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
            
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

        /// <summary>
        /// 刷新车辆
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int RefreshCar(string carid)
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string sql = $"update car_info set refreshtime={(long)ts.TotalSeconds} where innerid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="istop">1.置顶 0取消置顶</param>
        /// <returns>1.操作成功</returns>
        public int DoTopCar(string carid, int istop)
        {
            const string sql = "update car_info set istop=@istop where innerid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid, istop });
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
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
        /// 单次添加图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCarPictureEx(CarPictureModel model)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from car_picture where carid=@carid order by sort desc;";//查询车辆图片
            const string sqlIPic = @"insert into car_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @carid, @typeid, @path, @sort, @createdtime);";
            const string sqlUCover = @"update car_info set pic_url=@pic_url where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<CarPictureModel>(sqlSCarPic, new { carid = model.Carid }).ToList();
                var number = picedList.Count + 1;
                if (number > 9)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var tran = conn.BeginTransaction();
                try
                {
                    if (picedList.Count == 0)
                    {
                        model.Sort = 1;
                    }
                    else
                    {
                        model.Sort = picedList[0].Sort + 1;
                    }
                    
                    conn.Execute(sqlIPic, model, tran); //插入图片

                    //表示添加张图片
                    if (picedList.Count == 0)
                    {
                        conn.Execute(sqlUCover, new {carid = model.Carid, pic_url = model.Path}, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("单次添加图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
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
            const string sql = @"select ifnull(max(sort),0) as maxsort from car_picture where carid=@carid;";

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

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="innerid">车辆id</param>
        /// <returns></returns>
        public CarPictureModel GetCarPictureByid(string innerid)
        {
            const string sql = @"select innerid, carid, typeid, path, sort, createdtime from car_picture where innerid=@innerid;";

            try
            {
                var model = Helper.Query<CarPictureModel>(sql, new { innerid }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取需要删除的图片列表
        /// </summary>
        /// <param name="idList">车辆ids</param>
        /// <returns></returns>
        public IEnumerable<CarPictureModel> GetCarPictureByIds(List<string> idList)
        {
            var ids = idList.Aggregate("", (current, it) => current + $"'{it}',").TrimEnd(',');
            var sql = $"select innerid, carid, typeid, path, sort, createdtime from car_picture where innerid in ({ids});";
            try
            {
                var list = Helper.Query<CarPictureModel>(sql);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 批量保存图片(添加)
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="carid"></param>
        /// <returns></returns>
        public int AddCarPictureList(List<string> pathList, string carid)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from car_picture where carid=@carid;";                              //查询车辆所有图片的最大排序
            const string sqlIPic = @"insert into car_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @carid, @typeid, @path, @sort, @createdtime);";
            const string sqlUCover = @"update car_info set pic_url=(select path from car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<CarPictureModel>(sqlSCarPic, new { carid }).ToList();
                var number = picedList.Count + pathList.Count;
                if (number > 9)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var maxsort = conn.ExecuteScalar<int>(sqlSMaxSort, new { carid });
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var path in pathList)
                    {
                        conn.Execute(sqlIPic, new CarPictureModel
                        {
                            Carid = carid,
                            Createdtime = DateTime.Now,
                            Path = path,
                            Innerid = Guid.NewGuid().ToString(),
                            Sort = ++maxsort
                        }, tran); //插入图片
                    }

                    //表示添加首批图片
                    if (maxsort == pathList.Count)
                    {
                        conn.Execute(sqlUCover, new { carid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量添加图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 批量保存图片(删除)
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="carid"></param>
        /// <returns></returns>
        public int DelCarPictureList(List<string> idList, string carid)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlDPic = @"delete from car_picture where innerid=@innerid;";
            const string sqlUCover = @"update car_info set pic_url=(select path from car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<CarPictureModel>(sqlSCarPic, new { carid }).ToList();
                var number = picedList.Count - idList.Count;
                if (number < 3)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var tran = conn.BeginTransaction();
                try
                {
                    //标示是否修改封面
                    var isUCover = false;
                    //获取封面图片
                    var coverid = picedList.First().Innerid;
                    foreach (var id in idList)
                    {
                        if (id.Equals(coverid))
                        {
                            isUCover = true;
                        }
                        conn.Execute(sqlDPic, new { innerid = id }, tran);
                    }

                    if (isUCover)
                    {
                        conn.Execute(sqlUCover, new { carid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量删除图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
            }
        }


        /// <summary>
        /// 批量保存图片(添加+删除)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveCarPicture(BatchPictureListModel model)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from car_picture where carid=@carid;";//查询车辆所有图片的最大排序
            const string sqlIPic = @"insert into car_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @carid, @typeid, @path, @sort, @createdtime);";
            const string sqlDPic = @"delete from car_picture where innerid=@innerid;";
            const string sqlUCover = @"update car_info set pic_url=(select path from car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picList = conn.Query<CarPictureModel>(sqlSCarPic, new { carid = model.Carid }).ToList();
                var number = picList.Count + model.AddPaths.Count - model.DelIds.Count;
                if (number < 3 || number > 9)
                {
                    //图片数量控制在>=3 and <=9
                    return 402;
                }

                var maxsort = conn.ExecuteScalar<int>(sqlSMaxSort, new { carid = model.Carid });
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var path in model.AddPaths)
                    {
                        conn.Execute(sqlIPic, new CarPictureModel
                        {
                            Carid = model.Carid,
                            Createdtime = DateTime.Now,
                            Path = path,
                            Innerid = Guid.NewGuid().ToString(),
                            Sort = ++maxsort
                        }, tran); //插入图片
                    }

                    //标示是否修改封面
                    var isUCover = false;
                    //获取封面图片
                    var coverid = picList.First().Innerid;
                    foreach (var id in model.DelIds)
                    {
                        if (id.Equals(coverid))
                        {
                            isUCover = true;
                        }
                        conn.Execute(sqlDPic, new { innerid = id }, tran);
                    }

                    if (isUCover)
                    {
                        conn.Execute(sqlUCover, new { carid = model.Carid }, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("批量保存图片异常：" + ex.Message, TraceEventType.Warning);
                    return 0;
                }
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
                Helper.Query<CarCollectionModel>(sql, new { custid = model.Custid, carid = model.Carid }).FirstOrDefault();

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

        #region 精品车商


        #region 精品店基本信息
        
        /// <summary>
        /// 获取精品车商列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarBoutiqueListModel> GetBoutiquePageList(CarBoutiqueQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"boutique_info as a 
                                    inner join cust_info as b on b.innerid=a.custid ";
            const string fields =
                "a.innerid,a.enterprisename,a.logo,a.telephone,a.mobile,a.address,a.tempid, a.createdtime, a.createrid, a.modifiedtime, a.modifierid";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.sort desc, a.createdtime desc" : query.Order;
            var sqlWhere = new StringBuilder(" b.type=3 ");

            //省份
            if (query.Provid != null)
            {
                sqlWhere.Append($" and b.provid={query.Provid}");
            }

            //城市
            if (query.Cityid != null)
            {
                sqlWhere.Append($" and b.cityid={query.Cityid}");
            }

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CarBoutiqueListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加精品店基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBoutique(CarBoutiqueModel model)
        {
            const string sql = @"INSERT INTO boutique_info
                        (innerid, custid, enterprisename, logo, introduces, telephone, mobile, address, tempid, sort, expand, createdtime, createrid, modifiedtime, modifierid)
                        VALUES
                        (@innerid, @custid, @enterprisename, @logo, @introduces, @telephone, @mobile, @address, @tempid, @sort, @expand, @createdtime, @createrid, @modifiedtime, @modifierid);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加精品精品店基本信息异常：", TraceEventType.Error, ex);
                return 0;
            }
        }
        
        /// <summary>
        /// 修改精品店基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBoutique(CarBoutiqueModel model)
        {
            var sql = new StringBuilder("update boutique_info set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            try
            {
                Helper.Execute(sql.ToString(), model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改精品精品店基本信息异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取精品店基本信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CarBoutiqueModel GetBoutiqueById(string innerid)
        {
            const string sql = @"select * from boutique_info where innerid = @innerid;";

            try
            {
                return Helper.Query<CarBoutiqueModel>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取精品精品店基本信息异常：", TraceEventType.Error, ex);
                return null;
            }
        }

        #endregion

        #region 精品店模板信息

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarBoutiqueTempListModel> GetBoutiqueTempPageList(CarBoutiqueTempQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"boutique_template";
            const string fields =
                "innerid, tempname, introduces, pageurl, previewurl, createdtime, createrid, modifiedtime, modifierid";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            var sqlWhere = new StringBuilder("1=1");
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize,
                query.PageIndex);
            var list = Helper.ExecutePaging<CarBoutiqueTempListModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加模板基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBoutiqueTemp(CarBoutiqueTempModel model)
        {
            const string sql = @"INSERT INTO boutique_template
                        (innerid, tempname, introduces, pageurl, previewurl, createdtime, createrid, modifiedtime, modifierid)
                        VALUES
                        (@innerid, @tempname, @introduces, @pageurl, @previewurl, @createdtime, @createrid, @modifiedtime, @modifierid);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加模板基本信息异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改模板基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBoutiqueTemp(CarBoutiqueTempModel model)
        {
            var sql = new StringBuilder("update boutique_template set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            try
            {
                Helper.Execute(sql.ToString(), model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改模板基本信息异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取模板基本信息
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public CarBoutiqueTempModel GetBoutiqueTempById(string innerid)
        {
            const string sql = @"select * from boutique_template where innerid = @innerid;";

            try
            {
                return Helper.Query<CarBoutiqueTempModel>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取模板基本信息异常：", TraceEventType.Error, ex);
                return null;
            }
        }

        #endregion





        #region 公司简介
        /// <summary>
        /// 添加精品车商的公司简介
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddIntroduces(dynamic model)
        {
            const string sql = @"INSERT INTO cust_site_introduces
                        (innerid, custid, enterprisename, logo, introduces, telephone, mobile, address, website, wechatnumber, wechatqrcode, spare1)
                        VALUES
                        (@innerid, @custid, @enterprisename, @logo, @introduces, @telephone, @mobile, @address, @website, @wechatnumber, @wechatqrcode, @spare1);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加精品车商的公司简介异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改精品车商的公司简介
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateIntroduces(dynamic model)
        {
            var sql = new StringBuilder("update cust_site_introduces set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            try
            {
                Helper.Execute(sql.ToString(), model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改精品车商的公司简介异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除精品车商的公司简介
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DelIntroduces(string innerid)
        {
            const string sql = @"delete from cust_site_introduces where innerid = @innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除精品车商的公司简介异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取精品车商的公司简介
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public dynamic GetIntroducesById(string innerid)
        {
            const string sql = @"select * from cust_site_introduces where innerid = @innerid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取精品车商的公司简介异常：", TraceEventType.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取精品车商的公司简介列表
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public dynamic GetIntroducesList(string custid)
        {
            const string sql = @"select * from cust_site_introduces where custid = @custid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { custid });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取精品车商的公司简介列表异常：", TraceEventType.Error, ex);
                return null;
            }
        }
        #endregion

        #region 推荐品牌

        /// <summary>
        /// 添加推荐品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRecommendBrand(dynamic model)
        {
            const string sql = @"INSERT INTO cust_site_recommendbrand
                        (innerid, custid, brandid, createdtime, modifiedtime)
                        VALUES (@innerid, @custid, @brandid, @createdtime, @modifiedtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加推荐品牌异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改推荐品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateRecommendBrand(dynamic model)
        {
            var sql = new StringBuilder("update cust_site_recommendbrand set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            try
            {
                Helper.Execute(sql.ToString(), model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改推荐品牌异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除推荐品牌
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DelRecommendBrand(string innerid)
        {
            const string sql = @"delete from cust_site_recommendbrand where innerid = @innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除推荐品牌异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取推荐品牌
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public dynamic GetRecommendBrandById(string innerid)
        {
            const string sql = @"select * from cust_site_recommendbrand where innerid = @innerid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取推荐品牌异常：", TraceEventType.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取推荐品牌列表
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public dynamic GetRecommendBrandList(string custid)
        {
            const string sql = @"select * from cust_site_recommendbrand where custid = @custid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { custid });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取推荐品牌列表异常：", TraceEventType.Error, ex);
                return null;
            }
        }
        #endregion

        #region 推荐车源

        /// <summary>
        /// 添加推荐车源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRecommendCar(dynamic model)
        {
            const string sql = @"INSERT INTO cust_site_recommendcar
                        (innerid, custid, carid, createdtime, modifiedtime)
                        VALUES (@innerid, @custid, @carid, @createdtime, @modifiedtime);";

            try
            {
                Helper.Execute(sql, model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("添加推荐车源异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改推荐车源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateRecommendCar(dynamic model)
        {
            var sql = new StringBuilder("update cust_site_recommendcar set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));
            sql.Append(" where innerid = @innerid");
            try
            {
                Helper.Execute(sql.ToString(), model);
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改推荐车源异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除推荐车源
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DelRecommendCar(string innerid)
        {
            const string sql = @"delete from cust_site_recommendcar where innerid = @innerid;";

            try
            {
                Helper.Execute(sql, new { innerid });
                return 1;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("删除推荐车源异常：", TraceEventType.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取推荐车源
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public dynamic GetRecommendCarById(string innerid)
        {
            const string sql = @"select * from cust_site_recommendcar where innerid = @innerid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { innerid }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取推荐车源异常：", TraceEventType.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取推荐车源列表
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public dynamic GetRecommendCarList(string custid)
        {
            const string sql = @"select * from cust_site_recommendcar where custid = @custid;";

            try
            {
                return Helper.Query<dynamic>(sql, new { custid });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取推荐车源列表异常：", TraceEventType.Error, ex);
                return null;
            }
        }
        #endregion

        #endregion

        #region 车辆悬赏

        /// <summary>
        /// 车辆悬赏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarRewardViewModel> CarRewardPageList(CarRewardQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_reward as a  ";
            const string fields = "a.* ";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;  
            var sqlWhere = new StringBuilder(" 1=1 ");
            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarRewardViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加车辆悬赏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCarReward(CarReward model)
        {
            const string sql = @"INSERT INTO `car_reward`
                                (`innerid`, `brand_id`, `series_id`, `mileage`, `coty`, `price`, `provid`, `cityid`, `status`, `username`, `usermobile`, `createdid`, `createdtime`, `modifiedtime`)
                                VALUES
                                (@innerid, @brand_id, @series_id, @mileage, @coty, @price, @provid, @cityid, @status, @username, @usermobile, @createdid, @createdtime, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加车辆悬赏信息异常：", TraceEventType.Information, ex);
                    result = 0;
                }
                return result;
            }
        }

        /// <summary>
        /// 车辆悬赏推荐
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<CarInfoListViewModel> GetCarRewardPageList(CarQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            const string fields = "a.innerid,a.custid,a.pic_url,a.price,a.buyprice,a.dealprice,a.buytime,a.status,a.mileage,a.register_date,a.istop,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "a.createdtime desc" : query.Order;

            #region 查询条件
            var sqlWhere = new StringBuilder("a.status=1");

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

            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" or a.cityid={query.cityid}");
            }

            //销售价大于..
            if (query.minprice.HasValue)
            {
                sqlWhere.Append($" or ( a.price>={query.minprice}");
            }

            //销售价小于..
            if (query.maxprice.HasValue)
            {
                sqlWhere.Append($" and  a.price<={query.maxprice} )");
            }

            //车龄大于
            if (query.mincoty.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.mincoty.Value).ToShortDateString();
                sqlWhere.Append($" or ( a.register_date<='{date}'");
            }
            //车龄小于
            if (query.maxcoty.HasValue)
            {
                var date = DateTime.Now.AddYears(-query.maxcoty.Value).ToShortDateString();
                sqlWhere.Append($" and a.register_date>='{date}' )");
            }

            //里程大于
            if (query.mincoty.HasValue)
            {
                sqlWhere.Append($" or ( a.mileage<='{query.mincoty}'");
            }
            //里程小于
            if (query.maxcoty.HasValue)
            {
                sqlWhere.Append($" and a.mileage<='{query.maxcoty}' )");
            }
            #endregion

            var model = new PagingModel(spName, tableName, fields, orderField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<CarInfoListViewModel>(model, query.Echo);
            return list;
        }

        #endregion
    }
}
