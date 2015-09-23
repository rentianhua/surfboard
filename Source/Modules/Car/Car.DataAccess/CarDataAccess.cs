using System;
using System.Collections.Generic;
using CCN.Modules.Car.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Car.DataAccess
{
    public class CarDataAccess : DataAccessBase
    {
        private readonly Database _factoy;

        /// <summary>
        /// 初始化
        /// </summary>
        public CarDataAccess()
        {
            _factoy = new DatabaseWrapperFactory().GetDatabase("mysqldb");
        }
        
        #region 车辆

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<CarInfoModel> GetProvList(string initial)
        {
            string where = " 1=1";
            if (!string.IsNullOrWhiteSpace(initial))
            {
                where += $" and initial='{initial.ToUpper()}'";
            }
            var sql = $"select * from car_info where {where}";
            var provList = _factoy.Query<CarInfoModel>(sql);
            return provList;
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int AddCar(CarInfoModel model)
        {
            const string sql = @"INSERT INTO `car_info`
                        (`innerid`,custid,`carid`,`title`,`pic_url`,`provid`,`cityid`,`brand_id`,`series_id`,`model_id`,`price`,`mileageid`,`register_date`,`reg_year`,`gearid`,`carageid`,`literid`,`colorid`,`carshructid`,`dischargeid`,`tel`,`contactor`,`dealer_id`,`seller_type`,`status`,`remark`,`createdtime`,`modifiedtime`,`post_time`,`audit_time`,`sold_time`,`keep_time`,`eval_price`,`next_year_eval_price`,`vpr`,`tlci_date`,`audit_date`,`mile_age`,`gear_type`,`color`,`liter`,`url`)
                        VALUES
                        (@innerid,@custid,@carid,@title,@pic_url,@provid,@cityid,@brand_id,@series_id,@model_id,@price,@mileageid,@register_date,@reg_year,@gearid,@carageid,@literid,@colorid,@carshructid,@dischargeid,@tel,@contactor,@dealer_id,@seller_type,@status,@remark,@createdtime,@modifiedtime,@post_time,@audit_time,@sold_time,@keep_time,@eval_price,@next_year_eval_price,@vpr,@tlci_date,@audit_date,@mile_age,@gear_type,@color,@liter,@url);";
            var result = _factoy.Execute(sql, model);
            return result;
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateCar(CarInfoModel model)
        {
            const string sql = @"UPDATE `car_info` SET
                                `title` = @title,
                                `pic_url` = @pic_url,
                                `provid` = @provid,
                                `cityid` = @cityid,
                                `brand_id` = @brand_id,
                                `series_id` = @series_id,
                                `model_id` = @model_id,
                                `price` = @price,
                                `mileageid` = @mileageid,
                                `register_date` = @register_date,
                                `reg_year` = @reg_year,
                                `gearid` = @gearid,
                                `carageid` = @carageid,
                                `literid` = @literid,
                                `colorid` = @colorid,
                                `carshructid` = @carshructid,
                                `dischargeid` = @dischargeid,
                                `tel` = @tel,
                                `contactor` = @contactor,
                                `dealer_id` = @dealer_id,
                                `seller_type` = @seller_type,
                                `remark` = @remark,
                                `modifiedtime` = @modifiedtime,
                                `eval_price` = @eval_price,
                                `next_year_eval_price` = @next_year_eval_price,
                                `vpr` = @vpr,
                                `tlci_date` = @tlci_date,
                                `audit_date` = @audit_date,
                                WHERE `innerid` = @innerid;";
            
            var result = _factoy.Execute(sql, model);
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
                _factoy.Execute(sql, new { innerid = id });
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
                _factoy.Execute(sql, new { innerid = carid });
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
                var resCount = _factoy.Execute(sql, new {carid = id});
                if (resCount == 0)
                {
                    //表示没有子表数据
                    sql = "INSERT INTO `car_share`(`innerid`,`carid`,`sharecount`,`seecount`,`praisecount`,`commentcount`) VALUES(uuid(), @carid, 1, 0, 0, 0);";
                    _factoy.Execute(sql, new { carid = id });
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
                _factoy.Execute(sql, new { carid = id });
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
                _factoy.Execute(sql, new { carid = id });
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
                _factoy.Execute(sql, new { carid = id });
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
                var result = _factoy.Execute(sql, new
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
                var result = _factoy.Execute(sql, new { sold_time = DateTime.Now, innerid = id });
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
                var result = _factoy.Execute(sql, new { keep_time = DateTime.Now, innerid = id });
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
