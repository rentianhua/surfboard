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
            const string tableName = @"auction_carinfo as a 
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            const string fields = "a.innerid,a.pic_url,a.mileage,a.register_date,a.validtime,a.lowestprice,a.createdtime,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder(" status=2 ");
            
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
                                    left join base_carbrand as c1 on a.brand_id=c1.innerid 
                                    left join base_carseries as c2 on a.series_id=c2.innerid 
                                    left join base_carmodel as c3 on a.model_id=c3.innerid 
                                    left join base_city as ct on a.cityid=ct.innerid ";
            const string fields = "a.innerid,a.mobile,a.pic_url,a.status,a.mileage,a.register_date,a.validtime,a.lowestprice,a.createdtime,c1.brandname as brand_name,c2.seriesname as series_name,c3.modelname as model_name,ct.cityname";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.mobile))
            {
                sqlWhere.Append($" and a.mobile like '%{query.mobile}%'");
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
            const string sql = "select * from auction_carinfo where innerid=@id";
            var model = Helper.Query<AuctionCarInfoModel>(sql, new {id}).FirstOrDefault();
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
                @"select a.innerid, a.mobile, a.title, pic_url, 
                a.mileage, a.register_date, a.buytime, a.buyprice, 
                a.lowestprice, a.isproblem, a.istain, a.istransferfee,
                a.ckyear_date, a.tlci_date, a.audit_date, a.remark, 
                a.sellreason, a.masterdesc, a.estimateprice, 
                a.dealrewards,a.transferrisk,a.remind,a.tips,
                a.`status`, a.createrid,a.createdtime, 
                a.modifierid,a.modifiedtime, 
                a.deleterid,a.deletedtime,a.deletedesc,
                a.publisherid,a.publishedtime,
                a.dealerid, a.dealedtime, a.dealedprice, a.dealdesc, a.dealmobile,
                a.validtime, 
                pr.provname,
                ct.cityname,
                cb.brandname as brand_name,
                cs.seriesname as series_name,
                cm.modelname as model_name,
                cm.liter,
                cm.geartype,
                cm.dischargestandard as dischargeName,
                bc1.codename as color
                from auction_carinfo as a 
                left join base_province as pr on a.provid=pr.innerid
                left join base_city as ct on a.cityid=ct.innerid
                left join base_carbrand as cb on a.brand_id=cb.innerid
                left join base_carseries as cs on a.series_id=cs.innerid
                left join base_carmodel as cm on a.model_id=cm.innerid
                left join base_code as bc1 on a.colorid=bc1.codevalue and bc1.typekey='car_color'
                where a.innerid=@innerid";
            var model = Helper.Query<AuctionCarInfoViewModel>(sql, new {innerid = id}).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAuctionCar(AuctionCarInfoModel model)
        {
            const string sql = @"INSERT INTO `auction_carinfo`
                                (innerid, mobile, title, pic_url, provid, cityid, brand_id, series_id, model_id, colorid, mileage, register_date, buytime, buyprice, lowestprice, isproblem, istain, istransferfee, ckyear_date, tlci_date, audit_date, remark, sellreason, masterdesc, estimateprice, dealrewards, transferrisk, remind, tips, status, createrid, createdtime, modifierid, modifiedtime, deleterid, deletedtime, deletedesc, publisherid, publishedtime, dealerid, dealedtime, dealedprice, dealdesc, dealmobile, validtime)
                                VALUES
                                (@innerid, @mobile, @title, @pic_url, @provid, @cityid, @brand_id, @series_id, @model_id, @colorid, @mileage, @register_date, @buytime, @buyprice, @lowestprice, @isproblem, @istain, @istransferfee, @ckyear_date, @tlci_date, @audit_date, @remark, @sellreason, @masterdesc, @estimateprice, @dealrewards, @transferrisk, @remind, @tips, @status, @createrid, @createdtime, @modifierid, @modifiedtime, @deleterid, @deletedtime, @deletedesc, @publisherid, @publishedtime, @dealerid, @dealedtime, @dealedprice, @dealdesc, @dealmobile, @validtime);";
            
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
        /// 修改拍卖车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateAuctionCar(AuctionCarInfoModel model)
        {
            var sql = new StringBuilder("update `auction_carinfo` set ");
            sql.Append(Helper.CreateField(model).Trim().TrimEnd(','));

            //非必填字段的修改
            if (!model.lowestprice.HasValue)
            {
                sql.Append(",lowestprice=null");
            }
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
                LoggerFactories.CreateLogger().Write("修改拍卖车辆异常：", TraceEventType.Information, ex);
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
                Helper.Execute(sql, new {
                    innerid = model.Innerid,
                    model.deleterid,
                    model.deletedtime,
                    model.deletedesc });
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
            var list = Helper.Query<AuctionCarPictureModel>(sql, new {carid});
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
            const string tableName = @"auction_participant as a";
            const string fields = "a.innerid,a.auctionid,a.mobile,a.amount,a.isbid,a.createrid,a.createdtime";
            var oldField = string.IsNullOrWhiteSpace(query.Order) ? " a.createdtime asc " : query.Order;

            var sqlWhere = new StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(query.Auctionid))
            {
                sqlWhere.Append($" and a.auctionid='{query.Auctionid}'");
            }

            if (!string.IsNullOrWhiteSpace(query.Mobile))
            {
                sqlWhere.Append($" and a.mobile='{query.Mobile}'");
            }
            
            var model = new PagingModel(spName, tableName, fields, oldField, sqlWhere.ToString(), query.PageSize, query.PageIndex);
            var list = Helper.ExecutePaging<AuctionCarParticipantViewModel>(model, query.Echo);
            return list;
        }
        
        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddParticipant(AuctionCarParticipantModel model)
        {
            const string sql = @"INSERT INTO `auction_participant`
                                (innerid, auctionid, mobile, amount, isbid, remark, createrid, createdtime, modifierid, modifiedtime)
                                VALUES
                                (@innerid, @auctionid, @mobile, @amount, @isbid, @remark, @createrid, @createdtime, @modifierid, @modifiedtime);";

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
            return Helper.ExecuteScalar<int>(sql, new {auctionid});            
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
                result = Helper.Execute(sql, new {innerid});
            }
            catch (Exception ex)
            {
                result = 0;
                LoggerFactories.CreateLogger().Write("删除押金异常：", TraceEventType.Information, ex);
            }
            return result;
        }

        #endregion
    }
}