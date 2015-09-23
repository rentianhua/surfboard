using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.Car.BusinessEntity;
using Cedar.Core.Data;
using Cedar.Core.EntLib.Data;
using Cedar.Framework.Common.BaseClasses;
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

            //`post_time` = @post_time,
            //`audit_time` = @audit_time,
            //`sold_time` = @sold_time,
            //`keep_time` = @keep_time,
            
            var result = _factoy.Execute(sql, model);
            return result;
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns></returns>
        public int ShareCar(string id)
        {
            const string sql = @"UPDATE `car_info` SET `post_time`=@post_time WHERE `innerid` = @innerid;";
            try
            {
                var result = _factoy.Execute(sql, new { post_time = DateTime.Now, innerid = id });
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
