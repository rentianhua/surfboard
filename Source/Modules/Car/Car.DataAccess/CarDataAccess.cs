using System;
using System.Collections.Generic;
using System.Text;
using CCN.Modules.Car.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

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

        #region 车辆

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public BasePageList<CarInfoModel> GetCarPageList(CarQueryModel query)
        {
            const string spName = "sp_common_pager";
            const string tableName = @"car_info_bak";
            const string fields = " * ";
            var orderField = string.IsNullOrWhiteSpace(query.Order) ? "createdtime desc" : query.Order;
            //查询条件 
            var sqlWhere = "1=1";
            PagingModel model = new PagingModel(spName, tableName, fields, orderField, sqlWhere, query.PageSize, query.PageIndex);
            //var list2 = Helper.ExecutePaging<dynamic>(model, query.Echo);
            BasePageList<CarInfoModel> list = Helper.ExecutePaging<CarInfoModel>(model, query.Echo);
            return list;

            //string where = " 1=1";
            //if (!string.IsNullOrWhiteSpace(initial))
            //{
            //    where += $" and initial='{initial.ToUpper()}'";
            //}
            //var sql = $"select * from car_info where {where}";
            //var provList = Helper.Query<CarInfoModel>(sql);
            //return provList;
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int AddCar(CarInfoModel model)
        {
            const string sql = @"INSERT INTO `car_info`
                        (`innerid`,`carid`,`title`,`pic_url`,`provid`,`cityid`,`brand_id`,`series_id`,`model_id`,`colorid`,`literid`,`dischargeid`,`carshructid`,`gearid`,`mileage`,`register_date`,`buytime`,`buyprice`,`price`,`dealprice`,`isproblem`,`remark`,`sellreason`,`masterdesc`,`ckyear_date`,`tlci_date`,`audit_date`,`istain`,`status`,`createdtime`,`modifiedtime`,`seller_type`,`tel`,`contactor`,`reg_year`,`post_time`,`audit_time`,`sold_time`,`keep_time`,`eval_price`,`next_year_eval_price`,`vpr`,`mile_age`,`gear_type`,`color`,`liter`,`url`)
                        VALUES
                        (@innerid,@carid,@title,@pic_url,@provid,@cityid,@brand_id,@series_id,@model_id,@colorid,@literid,@dischargeid,@carshructid,@gearid,@mileage,@register_date,@buytime,@buyprice,@price,@dealprice,@isproblem,@remark,@sellreason,@masterdesc,@ckyear_date,@tlci_date,@audit_date,@istain,@status,@createdtime,@modifiedtime,@seller_type,@tel,@contactor,@reg_year,@post_time,@audit_time,@sold_time,@keep_time,@eval_price,@next_year_eval_price,@vpr,@mile_age,@gear_type,@color,@liter,@url);";
            var result = Helper.Execute(sql, model);
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
            sql.Append(Helper.CreateField(model));
            sql.Append(" where innerid = @innerid");
            var result = Helper.Execute(sql.ToString(), model);
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
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        public int UpdateCarStatus(string carid, int status)
        {
            try
            {
                const string sql = "update car_info set status=@status where innerid`=@innerid;";
                Helper.Execute(sql, new { innerid = carid });
            }
            catch (Exception)
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
                var resCount = Helper.Execute(sql, new {carid = id});
                if (resCount == 0)
                {
                    //表示没有子表数据
                    sql = "INSERT INTO `car_share`(`innerid`,`carid`,`sharecount`,`seecount`,`praisecount`,`commentcount`) VALUES(uuid(), @carid, 1, 0, 0, 0);";
                    Helper.Execute(sql, new { carid = id });
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }
        
        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public int UpSeeCount(string id)
        {
            const string sql = @"update car_share set seecount=seecount+1 where carid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid = id });
            }
            catch (Exception)
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
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        public int CommentCar(string id,string content)
        {
            const string sql = @"update car_share set commentcount=commentcount+1 where carid=@carid;";
            try
            {
                Helper.Execute(sql, new { carid = id });
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }
        
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

        /// <summary>
        /// 车辆归档
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int KeepCar(string id)
        {
            const string sql = @"UPDATE `car_info` SET `keep_time`=@keep_time WHERE `innerid`=@innerid;";
            try
            {
                var result = Helper.Execute(sql, new { keep_time = DateTime.Now, innerid = id });
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
    }
}
