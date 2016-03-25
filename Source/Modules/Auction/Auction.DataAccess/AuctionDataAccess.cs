using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CCN.Modules.Auction.BusinessEntity;
using Cedar.Core.Logging;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;
using Dapper;

namespace CCN.Modules.Auction.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDataAccess : DataAccessBase
    {
        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctioningList(AuctionCarInfoQueryModel query)
        {
            const string spName = "sp_common_pager";
            string tableName = @"auction_carinfo as a 
                                    left join car_info as b on b.innerid=a.carid
                                    left join base_carbrand as c1 on b.brand_id=c1.innerid 
                                    left join base_carseries as c2 on b.series_id=c2.innerid 
                                    left join base_carmodel as c3 on b.model_id=c3.innerid 
                                    left join base_city as ct on b.cityid=ct.innerid
                                    left join base_province as pr on b.provid=pr.innerid 
                                    left join (select count(1) as count,auctionid from auction_participant group by auctionid) d on d.auctionid=a.innerid ";

            string fields = @"a.innerid,a.mobile,a.lowestprice,a.status as auditstatus,b.pic_url,b.status,b.price,b.mileage,b.register_date,a.validtime,b.createdtime
                ,c1.brandname as brand_name,a.no,c2.seriesname as series_name,c3.modelname as model_name,c3.modelprice,ct.cityname,pr.provname,d.count";

            if (!string.IsNullOrWhiteSpace(query.userid))
            {
                tableName += " left join (select count(1) as follow, auctionid from auction_follow where isdelete=0 and userid = '" + query.userid + "' group by auctionid ) e on e.auctionid = a.innerid";
                fields += " ,ifnull(e.follow,0) as follow ";
            }

            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder(" a.status=6 ");

            //省份
            if (query.provid != null)
            {
                sqlWhere.Append($" and b.provid={query.provid}");
            }

            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and b.cityid={query.cityid}");
            }

            //品牌
            if (query.brand_id != null && query.brand_id != 0)
            {
                sqlWhere.Append($" and b.brand_id={query.brand_id}");
            }

            //车系
            if (query.series_id != null && query.series_id != 0)
            {
                sqlWhere.Append($" and b.series_id={query.series_id}");
            }

            //车型
            if (query.model_id != null && query.model_id != 0)
            {
                sqlWhere.Append($" and b.model_id={query.model_id}");
            }

            //开始时间
            if (query.publishedtime.HasValue)
            {
                sqlWhere.Append($" and a.publishedtime='{query.publishedtime}'");
            }

            //结束时间
            if (query.validtime.HasValue)
            {
                sqlWhere.Append($" and a.validtime='{query.validtime}'");
            }
            else
            {
                sqlWhere.Append($" and a.validtime>='{DateTime.Now}'");
            }

            //里程数
            if (query.minmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage>={query.minmileage}");
            }
            //里程数
            if (query.maxmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage<{query.maxmileage}");
            }
            //上牌时间
            if (query.register_date.HasValue)
            {
                sqlWhere.Append($" and YEAR(b.register_date)=YEAR('{query.register_date}')");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionCarInfoViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetAuctionList(AuctionCarInfoQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"auction_carinfo as a 
                                    left join car_info as b on b.innerid=a.carid
                                    left join base_carbrand as c1 on b.brand_id=c1.innerid 
                                    left join base_carseries as c2 on b.series_id=c2.innerid 
                                    left join base_carmodel as c3 on b.model_id=c3.innerid 
                                    left join base_city as ct on b.cityid=ct.innerid 
                                    left join base_province as pr on b.provid=pr.innerid 
                                    left join sys_user as su on su.innerid=a.operatedid";
            const string fields = "a.innerid,a.no,a.carid,a.mobile,a.lowestprice,a.status as auditstatus,b.pic_url,a.status,b.price,b.mileage,b.register_date,a.publishedtime,a.validtime,b.createdtime,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,c3.modelprice,ct.cityname,pr.provname,su.username";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("1=1");

            //车商手机号
            if (!string.IsNullOrWhiteSpace(query.mobile))
            {
                sqlWhere.Append($" and a.mobile like '%{query.mobile}%'");
            }

            //省份
            if (query.provid != null)
            {
                sqlWhere.Append($" and b.provid={query.provid}");
            }

            //城市（车源地）
            if (query.cityid != null)
            {
                sqlWhere.Append($" and b.cityid={query.cityid}");
            }

            //品牌
            if (query.brand_id != null && query.brand_id != 0)
            {
                sqlWhere.Append($" and b.brand_id={query.brand_id}");
            }

            //车系
            if (query.series_id != null && query.series_id != 0)
            {
                sqlWhere.Append($" and b.series_id={query.series_id}");
            }

            //车型
            if (query.model_id != null && query.model_id != 0)
            {
                sqlWhere.Append($" and b.model_id={query.model_id}");
            }
            //里程数
            if (query.minmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage>={query.minmileage}");
            }
            //里程数
            if (query.maxmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage<{query.maxmileage}");
            }
            //上牌时间
            if (query.register_date.HasValue)
            {
                sqlWhere.Append($" and b.register_date='{query.register_date}'");
            }
            //状态
            if (query.status.HasValue)
            {
                sqlWhere.Append($" and a.status='{query.status}'");
            }
            //员工编号
            if (query.userno != null)
            {
                sqlWhere.Append($" and su.no='{query.userno}'");
            }
            //拍品编号
            if (query.auctionno != null)
            {
                sqlWhere.Append($" and a.no='{query.auctionno}'");
            }
            //业务员Id operatedid
            if (query.operatedid != null)
            {
                sqlWhere.Append($" and a.operatedid='{query.operatedid}'");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionCarInfoViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 获取拍卖车辆详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuctionCarInfoModel GetAuctionInfoById(string id)
        {
            const string sql = "select * from auction_carinfo where carid=@id";
            var model = Helper.Query<AuctionCarInfoModel>(sql, new { id }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 获取拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuctionCarInfoViewModel GetAuctionViewById(string id)
        {
            const string sql =
                @"select a.innerid, a.mobile,a.carid,a.no, ci.title, ci.pic_url, a.address,
                ci.mileage, ci.register_date, ci.buytime, ci.buyprice, 
                a.lowestprice, ci.isproblem, ci.istain, ci.istransferfee,
                ci.ckyear_date, ci.tlci_date, ci.audit_date, ci.remark, 
                ci.sellreason,ci.masterdesc, ci.estimateprice, 
                a.dealrewards,a.transferrisk,a.remind,a.tips,
                a.`status`, a.createrid,a.createdtime, 
                a.modifierid,a.modifiedtime, 
                a.deleterid,a.deletedtime,a.deletedesc,
                a.publisherid,a.publishedtime,
                a.dealerid, a.dealedtime, a.dealedprice, a.dealdesc, a.dealmobile,
                a.validtime, a.havepurchasetax,
                a.isoperation, a.certificatesdeliver, a.isnewcar, a.vin, a.enginenum, a.transfer, a.violationdes, a.configuredes, a.supplementdes, a.picturedes,a.evaluationtest,a.introduction,a.sellername,a.sellermobile,
                pr.provname,
                ct.cityname,
                cb.brandname as brand_name,
                cs.seriesname as series_name,
                cm.modelname as model_name,
                cm.liter,
                cm.geartype,
                cm.dischargestandard as dischargeName,
                bc1.codename as color,
                cm.modelprice,ci.price,
                now() as currenttime
                from auction_carinfo as a 
                left join car_info as ci on ci.innerid=a.carid
                left join base_province as pr on ci.provid=pr.innerid
                left join base_city as ct on ci.cityid=ct.innerid
                left join base_carbrand as cb on ci.brand_id=cb.innerid
                left join base_carseries as cs on ci.series_id=cs.innerid
                left join base_carmodel as cm on ci.model_id=cm.innerid
                left join base_code as bc1 on ci.colorid=bc1.codevalue and bc1.typekey='car_color'
                where a.innerid=@innerid";
            var model = Helper.Query<AuctionCarInfoViewModel>(sql, new { innerid = id }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAuctionCar(AuctionCarInfoModel model)
        {
            //添加拍卖车辆
            const string sql = @"INSERT INTO `auction_carinfo`
                                (innerid, carid, carno,no, mobile, dealrewards, transferrisk, remind, tips, status, lowestprice, isoperation, certificatesdeliver, isnewcar, vin, enginenum, transfer, violationdes, configuredes, supplementdes, picturedes, havepurchasetax, evaluationtest, introduction, address, evaluationpics,createrid, createdtime, modifierid, modifiedtime, deleterid, deletedtime, deletedesc, publisherid, publishedtime, dealerid, dealedtime, dealedprice, dealdesc, dealmobile, validtime,sellername,sellermobile,operatedid)
                                VALUES
                                (@Innerid, @carid, @carno,@no, @mobile, @dealrewards, @transferrisk, @remind, @tips, @status, @lowestprice, @isoperation, @certificatesdeliver, @isnewcar, @vin, @enginenum, @transfer, @violationdes, @configuredes, @supplementdes, @picturedes, @havepurchasetax, @evaluationtest, @introduction, @address,@evaluationpics, @createrid, @createdtime, @modifierid, @modifiedtime, @deleterid, @deletedtime, @deletedesc, @publisherid, @publishedtime, @dealerid, @dealedtime, @dealedprice, @dealdesc, @dealmobile, @validtime,@sellername,@sellermobile,@operatedid);";
 
            using (var conn = Helper.GetConnection())
            {
                int result;
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, model, tran);
                    tran.Commit();
                    result = 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("添加拍卖车辆异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateAuctionCar(AuctionCarInfoModel model)
        {
            var sql = new StringBuilder("update `auction_carinfo` set ");
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
                LoggerFactories.CreateLogger().Write("修改拍卖车辆异常：", TraceEventType.Information, ex);
            }
            return result;
        }

        /// <summary>
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateAuctionCarStatus(AuctionCarInfoModel model)
        {
            //更新拍卖信息状态
            var sql = new StringBuilder("update `auction_carinfo` set `status`=@status where innerid = @innerid;");
            //更车辆状态
            var sqlcar = new StringBuilder("update `car_info`  ");
            if (model.status == 7)
            {
                sqlcar.Append(" set status=2 where innerid =@carid; ");
            }
            else if (model.status == 8)
            {
                sqlcar.Append(" set status=1 where innerid =@carid; ");
            }
            else if (model.status==6)//上线
            {
                sqlcar.Append(" set status=3 where innerid =@carid; ");
            }
            int result;
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql.ToString(), model, tran);
                    if (model.status == 6 || model.status == 7 || model.status == 8)
                    {
                        conn.Execute(sqlcar.ToString(), model, tran);
                    }

                    tran.Commit();
                    result = 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result = 0;
                    LoggerFactories.CreateLogger().Write("修改拍卖车辆异常：", TraceEventType.Information, ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 删除拍卖车辆 
        /// </summary>
        /// <param name="model">删除成交model</param>
        /// <returns>1.操作成功</returns>
        public int DeleteAuctionCar(AuctionCarInfoModel model)
        {
            try
            {
                const string sql = "update auction_carinfo set status=0,deleterid=@deleterid,deletedtime=@deletedtime,deletedesc=@deletedesc where `innerid`=@innerid;";
                Helper.Execute(sql, new
                {
                    innerid = model.Innerid,
                    model.deleterid,
                    model.deletedtime,
                    model.deletedesc
                });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("修改拍卖车辆异常：", TraceEventType.Information, ex);
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 发布拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public int PublishAuctionCar(AuctionCarInfoModel model)
        {
            try
            {
                model.status = model.status == 1 ? 2 : 1;
                const string sql = "update auction_carinfo set status=@status,publisherid=@publisherid,publishedtime=@publishedtime where `innerid`=@innerid;";
                Helper.Execute(sql, new
                {
                    innerid = model.Innerid,
                    model.publisherid,
                    model.publishedtime,
                    model.status
                });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("发布拍卖车辆异常：", TraceEventType.Information, ex);
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 成交拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1.操作成功</returns>
        public int DealAuctionCar(AuctionCarInfoModel model)
        {
            try
            {
                const string sql = "update auction_carinfo set status=3,dealerid=@dealerid,dealedtime=@dealedtime,dealedprice=@dealedprice,dealdesc=@dealdesc,dealmobile=@dealmobile where `innerid`=@innerid;";
                Helper.Execute(sql, new
                {
                    innerid = model.Innerid,
                    model.dealerid,
                    model.dealedtime,
                    model.dealedprice,
                    model.dealdesc,
                    model.dealmobile
                });
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("成交拍卖车辆异常：", TraceEventType.Information, ex);
                return 0;
            }
            return 1;
        }

        #endregion

        #region 图片处理

        /// <summary>
        /// 获取车辆已有图片
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <returns></returns>
        public IEnumerable<AuctionCarPictureModel> GetCarPictureByCarid(string carid)
        {
            const string sql =
                @"select innerid, carid, typeid, path, sort, createdtime from auction_car_picture where carid=@carid order by sort asc;";
            var list = Helper.Query<AuctionCarPictureModel>(sql, new { carid });
            return list;
        }

        /// <summary>
        /// 获取需要删除的图片列表
        /// </summary>
        /// <param name="idList">车辆ids</param>
        /// <returns></returns>
        public IEnumerable<AuctionCarPictureModel> GetCarPictureByIds(List<string> idList)
        {
            var ids = idList.Aggregate("", (current, it) => current + $"'{it}',").TrimEnd(',');
            var sql = $"select innerid, carid, typeid, path, sort, createdtime from auction_car_picture where innerid in ({ids});";
            try
            {
                var list = Helper.Query<AuctionCarPictureModel>(sql);
                return list;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取需要删除的图片列表：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 批量保存图片(添加)
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="carid"></param>
        /// <returns></returns>
        public int AddAuctionPictureList(List<string> pathList, string carid)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from auction_car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from auction_car_picture where carid=@carid;";                              //查询车辆所有图片的最大排序
            const string sqlIPic = @"insert into auction_car_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @carid, @typeid, @path, @sort, @createdtime);";
            const string sqlUCover = @"update auction_carinfo set pic_url=(select path from auction_car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<AuctionCarPictureModel>(sqlSCarPic, new { carid }).ToList();
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
                        conn.Execute(sqlIPic, new AuctionCarPictureModel
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
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from auction_car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlDPic = @"delete from auction_car_picture where innerid=@innerid;";
            const string sqlUCover = @"update auction_carinfo set pic_url=(select path from auction_car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picedList = conn.Query<AuctionCarPictureModel>(sqlSCarPic, new { carid }).ToList();
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
        public int SaveCarPicture(AuctionPictureListModel model)
        {
            const string sqlSCarPic = "select innerid, carid, typeid, path, sort, createdtime from auction_car_picture where carid=@carid order by sort;";//查询车辆图片
            const string sqlSMaxSort = "select ifnull(max(sort),0) as maxsort from auction_car_picture where carid=@carid;";//查询车辆所有图片的最大排序
            const string sqlIPic = @"insert into auction_car_picture (innerid, carid, typeid, path, sort, createdtime) values (@innerid, @carid, @typeid, @path, @sort, @createdtime);";
            const string sqlDPic = @"delete from auction_car_picture where innerid=@innerid;";
            const string sqlUCover = @"update auction_carinfo set pic_url=(select path from auction_car_picture where carid=@carid order by sort limit 1) where innerid=@carid;";

            using (var conn = Helper.GetConnection())
            {
                //获取车辆图片
                var picList = conn.Query<AuctionCarPictureModel>(sqlSCarPic, new { carid = model.Carid }).ToList();
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
                        conn.Execute(sqlIPic, new AuctionCarPictureModel
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

        #region 竞拍

        /// <summary>
        /// 获取竞拍人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarParticipantViewModel> GetAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"auction_participant as a
                                       left join auction_carinfo as b on b.innerid=a.auctionid
                                       left join car_info as c on c.innerid=b.carid
                                       left join base_carmodel as c1 on c.model_id=c1.innerid
                                       left join sys_user as su on su.innerid=b.operatedid
                                       left join base_city as ct on c.cityid=ct.innerid
                                       left join base_province as pr on c.provid=pr.innerid
                                       left join (select count(1) as pricecount,auctionid from auction_participant group by auctionid) as e on e.auctionid=a.auctionid";
            const string fields = @"a.innerid,a.auctionid,a.mobile,a.amount,a.status,a.createrid,a.createdtime,a.username,
                                    a.orderno,b.no as auctionno,b.lowestprice,b.validtime,b.status as auditstatus, now() as currenttime,c.register_date,c.mileage,c.pic_url,
                                    c1.modelprice as price,c1.modelname as model_name,
                                    ct.cityname,pr.provname,e.pricecount";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("1=1");

            //拍卖ID
            if (!string.IsNullOrWhiteSpace(query.Auctionid))
            {
                sqlWhere.Append($" and a.auctionid='{query.Auctionid}'");
            }
            //手机号
            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and a.mobile='{query.Mobile}'");
            }
            //用户ID
            if (!string.IsNullOrWhiteSpace(query.userid))
            {
                sqlWhere.Append($" and a.userid='{query.userid}'");
            }
            //员工编号
            if (!string.IsNullOrWhiteSpace(query.userno))
            {
                sqlWhere.Append($" and su.no='{query.userno}'");
            }
            //拍卖编号
            if (!string.IsNullOrWhiteSpace(query.auctionno))
            {
                sqlWhere.Append($" and b.no='{query.auctionno}'");
            }
            //状态
            if (query.status.HasValue)
            {
                sqlWhere.Append($" and a.status={query.status}");
            }
            //业务员
            if (!string.IsNullOrWhiteSpace(query.operatedid))
            {
                sqlWhere.Append($" and b.operatedid='{query.operatedid}'");
            }
            //里程数
            if (query.minmileage.HasValue)
            {
                sqlWhere.Append($" and c.mileage>={query.minmileage}");
            }
            //里程数
            if (query.maxmileage.HasValue)
            {
                sqlWhere.Append($" and c.mileage<{query.maxmileage}");
            }
            //上牌时间
            if (query.register_date.HasValue)
            {
                sqlWhere.Append($" and YEAR(c.register_date)=YEAR('{query.register_date}')");
            }
            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and c.cityid={query.cityid}");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionCarParticipantViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        ///  获取所有竞拍记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<AuctionCarParticipantViewModel> GetAllAuctionParticipantList(AuctionCarParticipantQueryModel query)
        {
            var sql = new StringBuilder("select * from auction_participant as a ");

            var sqlWhere = new StringBuilder(" where 1=1");
            if (query != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Auctionid))
                {
                    sqlWhere.Append($" and a.auctionid='{query.Auctionid}'");
                }

                if (!string.IsNullOrWhiteSpace(query.Mobile))
                {
                    sqlWhere.Append($" and a.mobile='{query.Mobile}'");
                }
            }
            sql.Append(sqlWhere.ToString());
            sql.Append(" order by createdtime desc; ");
            try
            {
                var list = Helper.Query<AuctionCarParticipantViewModel>(sql.ToString());
                return list;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取竞拍记录：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取出价详情
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public AuctionCarParticipantViewModel GetAuctionParticipantByID(string innerid)
        {
            var sql = new StringBuilder(@"select a.innerid,a.auctionid,a.orderno,a.mobile,a.qrcode,a.amount,a.status,a.createrid,a.createdtime,a.recordlist,a.remark,
                        a.username,b.no as auctionno,b.lowestprice,c1.modelname as model_name from auction_participant as a
                                       left join auction_carinfo as b on b.innerid = a.auctionid
                                       left join car_info as c on c.innerid = b.carid
                                       left join base_carmodel as c1 on c.model_id = c1.innerid where a.innerid=@innerid;");

            try
            {
                var model = Helper.Query<AuctionCarParticipantViewModel>(sql.ToString(), new { innerid }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取竞拍记录：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 根据订单号获取出价详情
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public AuctionCarParticipantModel GetAuctionParticipantByOrderNo(string orderno)
        {
            var sql = new StringBuilder(@"select a.* from auction_participant as a where a.orderno=@orderno;");

            try
            {
                var model = Helper.Query<AuctionCarParticipantModel>(sql.ToString(), new { orderno }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("根据订单号获取竞拍记录：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 根据拍卖ID 获取竞拍记录
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public int GetPriceCount(string auctionid)
        {
            var list = GetAllAuctionParticipantList(new AuctionCarParticipantQueryModel() { Auctionid = auctionid });
            if (list != null)
            {
                return list.Count();
            }
            return 0;
        }

        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddParticipant(AuctionCarParticipantModel model)
        {
            const string sql = @"INSERT INTO `auction_participant`
                                (innerid, auctionid, mobile, amount,username,userid, status, remark, createrid, createdtime, modifierid, modifiedtime,orderno)
                                VALUES
                                (uuid(), @auctionid, @mobile, @amount,@username,@userid, @status, @remark, @createrid, @createdtime, @modifierid, @modifiedtime,@orderno);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加拍卖竞拍人员异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 更新竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateParticipant(AuctionCarParticipantModel model)
        {
            var sql = new StringBuilder("update `auction_participant` set ");
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
                LoggerFactories.CreateLogger().Write("更新竞价信息异常：", TraceEventType.Information, ex);
            }
            return result;
        }

        /// <summary>
        /// 若已成交更改其他竞价信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOtherParticipant(AuctionCarParticipantModel model)
        {
            //更新竞价列表
            var sql = new StringBuilder("update `auction_participant` set  status=7 ");
            sql.Append(" where auctionid=@auctionid and innerid != @innerid");

            var carmodel = GetAuctionViewById(model.Auctionid);
            var carid = string.Empty;
            if (carmodel != null)
            {
                carid = carmodel.carid;
            }
            //更新车辆信息
            var sqlcar = new StringBuilder("update car_info set `status`=2 where innerid=@carid;");
            int result;
            using (var conn = Helper.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql.ToString(), model, tran);
                    conn.Execute(sqlcar.ToString(), new { carid }, tran);
                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result = 0;
                    LoggerFactories.CreateLogger().Write("更新其他竞价信息异常：", TraceEventType.Information, ex);
                }
            }
            return result;
        }


        /// <summary>
        /// 中标拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int BidParticipant(AuctionCarParticipantModel model)
        {
            const string sql = @"update auction_participant set isbid=@isbid,modifierid=@modifierid,modifiedtime=@modifiedtime where innerid=@innerid;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("中标拍卖竞拍人员异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 检查是否有人中标
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public int CheckBidParticipant(string auctionid)
        {
            const string sql = @"select count(1) as count from auction_participant where auctionid=@auctionid and isbid=1;";
            return Helper.ExecuteScalar<int>(sql, new { auctionid });
        }

        #endregion

        #region 押金

        /// <summary>
        /// 获取押金列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionDepositViewModel> GetAuctionDepositList(AuctionDepositQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"auction_deposit as a";
            const string fields = "a.innerid,a.auctionid,a.mobile,a.contacts,a.type,a.payer,a.dpsamount,a.dpsserialnum,a.dpsaccount,a.createrid,a.createdtime";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("1=1");

            if (query.Type.HasValue)
            {
                sqlWhere.Append($" and a.type={query.Type.Value}");
            }

            if (query.Payer.HasValue)
            {
                sqlWhere.Append($" and a.payer={query.Payer.Value}");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionDepositViewModel>(model, query.Echo);
            return list;
        }

        /// <summary>
        /// 添加押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDeposit(AuctionDepositModel model)
        {
            const string sql = @"INSERT INTO `auction_deposit`
                                (innerid, auctionid, type, payer, mobile, contacts, dpsamount, dpsserialnum, dpsaccount, pictures, remark, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @auctionid, @type, @payer, @mobile, @contacts, @dpsamount, @dpsserialnum, @dpsaccount, @pictures, @remark, @createrid, @createdtime, @modifierid, @modifiedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加押金异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 修改押金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDeposit(AuctionDepositModel model)
        {
            var sql = new StringBuilder("update `auction_deposit` set ");
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
                LoggerFactories.CreateLogger().Write("修改押金异常：", TraceEventType.Information, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取押金详情 info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuctionDepositModel GetDepositInfoById(string id)
        {
            const string sql = "select * from auction_deposit where innerid=@id";
            var model = Helper.Query<AuctionDepositModel>(sql, new { id }).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 删除押金
        /// </summary>
        /// <param name="innerid"></param>
        /// <returns></returns>
        public int DeleteDeposit(string innerid)
        {
            const string sql = @"delete from auction_deposit where innerid = @innerid;";
            int result;
            try
            {
                result = Helper.Execute(sql, new { innerid });
            }
            catch (Exception ex)
            {
                result = 0;
                LoggerFactories.CreateLogger().Write("删除押金异常：", TraceEventType.Information, ex);
            }
            return result;
        }

        #endregion

        #region 拍卖时间区间

        /// <summary>
        /// 获取拍卖时间列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuctionTimeViewModel> GetAuctionTimeList()
        {
            var currentTime = DateTime.Now.Hour;
            //var now = DateTime.Now;
            const string sql =
                @"select innerid, no, type, beginhour, endhour, beginmin, endmin, modifiedtime, createdtime,now() as currenttime from auction_timerange 
                         where beginhour<=@currentTime and endhour>@currentTime or (`no`=
                         (select `no`+1 from auction_timerange where beginhour<=@currentTime and endhour>@currentTime)) order by no asc;";
            var list = Helper.Query<AuctionTimeViewModel>(sql, new { currentTime });
            return list;
        }

        #endregion

        #region 认证报告

        /// <summary>
        /// 获取认证项
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuctionAllCarInspection> AuctionCarInspectionItem()
        {
            var sqlItem = new StringBuilder();
            sqlItem.Append("select * from auction_carinspectionitem where isenabled=1 order by sort");
            try
            {
                var listItem = Helper.Query<AuctionAllCarInspection>(sqlItem.ToString());
                if (listItem != null && listItem.Count() > 0)
                {
                    foreach (var item in listItem)
                    {
                        var sqlDetail = new StringBuilder();
                        sqlDetail.AppendFormat(@"select * from auction_carinspectiondetail 
                                                where isenabled=1 and inspectionid='{0}' 
                                                order by sort", item.innerid);
                        var listDetail = Helper.Query<AuctionCarInspectionDetailShowModel>(sqlDetail.ToString());
                        if (listDetail != null && listDetail.Count() > 0)
                        {
                            item.auctioncarinspectiondetail = listDetail;
                        }
                    }
                }
                return listItem;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取认证项：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取车辆认证报告
        /// </summary>
        /// <param name="auctionid"></param>
        /// <returns></returns>
        public IEnumerable<AuctionCarInspectionModel> GetAuctionCarInspectionResult(string auctionid)
        {
            var sql = new StringBuilder();
            sql.Append("select * from auction_carinspectionfindings where auctionid=@auctionid");
            try
            {
                var list = Helper.Query<AuctionCarInspectionModel>(sql.ToString(), new { auctionid });
                return list;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取车辆认证报告：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 获取认证项明细信息
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public AuctionCarInspectionModel GetAuctionInspectionResult(string auctionid, string itemid)
        {
            var sql = new StringBuilder();
            sql.Append("select * from auction_carinspectionfindings where auctionid=@auctionid and inspectiondetailid=@itemid");
            try
            {
                var list = Helper.Query<AuctionCarInspectionModel>(sql.ToString(), new { auctionid, itemid }).FirstOrDefault(); ;
                return list;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取车辆认证报告：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAuctionInspection(AuctionCarInspectionModel model)
        {
            const string sql = @"INSERT INTO `auction_carinspectionfindings`
                                (innerid, carid,auctionid, inspectiondetailid, intactcount, result, createdid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (uuid(), @carid,@auctionid, @inspectiondetailid, @intactcount, @result, @createdid, now(), @modifierid, now());";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加拍卖车辆异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<AuctionAllCarInspection> GetInspectionResultForHtml(string id)
        {
            var sqlItem = new StringBuilder();
            sqlItem.Append("select * from auction_carinspectionitem where isenabled=1 order by sort");
            try
            {
                var listItem = Helper.Query<AuctionAllCarInspection>(sqlItem.ToString());
                if (listItem != null && listItem.Count() > 0)
                {
                    foreach (var item in listItem)
                    {
                        var sqlDetail = new StringBuilder();
                        sqlDetail.AppendFormat(@"select * from auction_carinspectiondetail 
                                                where isenabled=1 and inspectionid='{0}' 
                                                order by sort", item.innerid);
                        var listDetail = Helper.Query<AuctionCarInspectionDetailShowModel>(sqlDetail.ToString());
                        if (listDetail != null && listDetail.Count() > 0)
                        {
                            item.auctioncarinspectiondetail = listDetail;
                        }
                    }
                }
                return listItem;
            }
            catch (Exception ex)
            {
                LoggerFactories.CreateLogger().Write("获取认证项：", TraceEventType.Information, ex);
                return null;
            }
        }

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddCarInspection(List<AuctionSaveCarInspectionModel> list)
        {
            var sql = new StringBuilder();
            foreach (var item in list)
            {
                sql.AppendFormat(@"INSERT INTO `auction_carinspectionfindings`
                                (innerid, carid, auctionid, inspectiondetailid, intactcount, result, createdid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (uuid(), '{0}', '{1}', '{2}', {3}, '{4}', '{5}', now(), '{6}', now());",
                                item.carid, item.auctionid, item.inspectiondetailid, item.intactcount, item.result, item.createdid, item.modifierid);
            }
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql.ToString());
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加认证报告信息异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 添加认证报告信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int EditCarInspection(List<AuctionSaveCarInspectionModel> list)
        {
            var sql = new StringBuilder();
            foreach (var item in list)
            {
                sql.AppendFormat(@"update `auction_carinspectionfindings` set intactcount={1},result='{2}',modifiedtime=now(),modifierid='{3}' where auctionid='{4}' and inspectiondetailid='{5}';"
                                , item.innerid, item.intactcount, item.result, item.modifierid, item.auctionid, item.inspectiondetailid);
            }
            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql.ToString());
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加认证报告信息异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        #endregion

        #region 关注

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddFollow(AuctionFollowModel model)
        {
            const string sql = @"INSERT INTO `auction_follow`
                                (innerid, auctionid, userid, isdelete, createdtime, deletedtime)
                                VALUES
                                (@innerid, @auctionid, @userid, 0, @createdtime, @deletedtime);";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var recChk =
                        conn.Query<int>(
                            "select count(1) from auction_follow where auctionid=@auctionid and userid=@userid and isdelete=0;",
                            new { model.Auctionid, model.Userid }).FirstOrDefault();

                    if (recChk > 0)
                    {
                        return -1;
                    }

                    result = conn.Execute(sql, model);
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("添加关注异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 删除关注
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DelFollow(string auctionid, string userid)
        {
            const string sql = @"update auction_follow set isdelete=1 where auctionid=@auctionid and userid=@userid;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    result = conn.Execute(sql, new { auctionid, userid });
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("删除关注异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 判断用户是否关注了该拍卖车辆
        /// </summary>
        /// <param name="auctionid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int IsFollow(string auctionid, string userid)
        {
            const string sql = @"select * from auction_follow where auctionid=@auctionid and userid=@userid and isdelete=0;";

            using (var conn = Helper.GetConnection())
            {
                int result;
                try
                {
                    var list = Helper.Query<AuctionFollowModel>(sql.ToString(), new { auctionid, userid });
                    if (list != null)
                    {
                        result = list.Count();
                    }
                    else
                    {
                        result = 0;
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactories.CreateLogger().Write("判断是否关注异常：", TraceEventType.Information, ex);
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// 获取关注的拍卖车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasePageList<AuctionCarInfoViewModel> GetFollowPageList(AuctionFollowQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @" auction_follow as f 
                                    inner join auction_carinfo as a on f.auctionid=a.innerid
                                    left join car_info as b on b.innerid=a.carid
                                    left join base_carbrand as c1 on b.brand_id=c1.innerid 
                                    left join base_carseries as c2 on b.series_id=c2.innerid 
                                    left join base_carmodel as c3 on b.model_id=c3.innerid 
                                    left join base_city as ct on b.cityid=ct.innerid
                                    left join base_province as pr on b.provid=pr.innerid 
                                    left join (select count(1) as pricecount,auctionid from auction_participant group by auctionid) as e on e.auctionid=a.innerid";
            const string fields = @"a.innerid,a.mobile,a.lowestprice,a.status as auditstatus,a.no,
                                b.pic_url,b.status,b.price,b.mileage,b.register_date,a.validtime,b.createdtime,now() as currenttime,
                                c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,c3.modelprice,ct.cityname,pr.provname,e.pricecount";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " f.createdtime desc " : query.Order;

            var sqlWhere = new StringBuilder(" f.isdelete=0 and a.status>=2");

            //用户id
            if (query.Userid != null)
            {
                sqlWhere.Append($" and f.userid='{query.Userid}'");
            }
            //里程数
            if (query.minmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage>={query.minmileage}");
            }
            //里程数
            if (query.maxmileage.HasValue)
            {
                sqlWhere.Append($" and b.mileage<{query.maxmileage}");
            }
            //上牌时间
            if (query.register_date.HasValue)
            {
                sqlWhere.Append($" and YEAR(b.register_date)=YEAR('{query.register_date}')");
            }
            //城市
            if (query.cityid != null)
            {
                sqlWhere.Append($" and b.cityid={query.cityid}");
            }

            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionCarInfoViewModel>(model, query.Echo);
            return list;
        }



        #endregion

        #region 支付相关

        /// <summary>
        /// 添加定金拍卖定金支付记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddPaymentRecord(AuctionPaymentRecordModel model)
        {
            const string sql = @"INSERT INTO `auction_paymentrecord`
                                (innerid, appid, attach, bank_type, cash_fee, fee_type, is_subscribe, mch_id, nonce_str, openid, out_trade_no, result_code, return_code, sign, time_end, total_fee, trade_type, transaction_id, createdtime)
                                VALUES
                                (uuid(), @appid, @attach, @bank_type, @cash_fee, @fee_type, @is_subscribe, @mch_id, @nonce_str, @openid, @out_trade_no, @result_code, @return_code, @sign, @time_end, @total_fee, @trade_type, @transaction_id, now());";

            using (var conn = Helper.GetConnection())
            {
                int result;
                var tran = conn.BeginTransaction();
                try
                {
                    result = conn.Execute(sql, model, tran);

                    //更新竞拍人处理状态
                    var u = "update auction_participant set `status`=3,remark=CONCAT(remark,@remark) where orderno=@orderno;";
                    conn.Execute(u, new {orderno = model.out_trade_no, remark = "\n@通知支付完成"}, tran);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LoggerFactories.CreateLogger().Write("添加定金拍卖定金支付记录异常：", TraceEventType.Error, ex);
                    result = 0;
                }

                return result;
            }
        }

        #endregion
    }
}