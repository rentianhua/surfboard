using System.Collections.Generic;
using CCN.Modules.Car.BusinessComponent;
using CCN.Modules.Car.BusinessEntity;
using CCN.Modules.Car.Interface;
using Cedar.Framework.Common.Server.BaseClasses;

namespace CCN.Modules.Car.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class CarManagementService : ServiceBase<CarBC>, ICarManagementService
    {
        /// <summary>
        /// </summary>
        public CarManagementService(CarBC bc)
            : base(bc)
        {
        }

        #region 车辆

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="initial">首字母</param>
        /// <returns></returns>
        public IEnumerable<CarInfoModel> GetProvList(string initial)
        {
            return BusinessComponent.GetProvList(initial);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int AddCar(CarInfoModel model)
        {
            return BusinessComponent.AddCar(model);
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public int UpdateCar(CarInfoModel model)
        {
            return BusinessComponent.UpdateCar(model);
        }

        /// <summary>
        /// 删除车辆(物理删除，暂不用)
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.删除成功</returns>
        public int DeleteCar(string id)
        {
            return BusinessComponent.DeleteCar(id);
        }

        /// <summary>
        /// 车辆状态更新
        /// </summary>
        /// <param name="carid">车辆id</param>
        /// <param name="status"></param>
        /// <returns>1.操作成功</returns>
        public int UpdateCarStatus(string carid, int status)
        {
            return BusinessComponent.UpdateCarStatus(carid, status);
        }

        /// <summary>
        /// 车辆分享
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int ShareCar(string id)
        {
            return BusinessComponent.ShareCar(id);
        }

        /// <summary>
        /// 累计车辆查看次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public int UpSeeCount(string id)
        {

            return BusinessComponent.UpSeeCount(id);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.累计成功</returns>
        public int UpPraiseCount(string id)
        {
            return BusinessComponent.UpPraiseCount(id);
        }

        /// <summary>
        /// 累计点赞次数
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="content">评论内容</param>
        /// <returns>1.累计成功</returns>
        public int CommentCar(string id, string content)
        {
            return BusinessComponent.CommentCar(id, content);
        }

        /// <summary>
        /// 审核车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <param name="status">审核状态</param>
        /// <returns>1.操作成功</returns>
        public int AuditCar(string id, int status)
        {
            return BusinessComponent.AuditCar(id, status);
        }

        /// <summary>
        /// 核销车辆
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int CancelCar(string id)
        {
            return BusinessComponent.CancelCar(id);
        }

        /// <summary>
        /// 车辆归档
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <returns>1.操作成功</returns>
        public int KeepCar(string id)
        {
            return BusinessComponent.KeepCar(id);
        }

        #endregion
    }
}
