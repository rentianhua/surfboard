using System.Web.Http;
using CCN.Modules.Auction.BusinessEntity;
using CCN.Modules.Auction.Interface;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.WebAPI.ApiControllers
{
    /// <summary>
    /// 基础模块
    /// </summary>
    [RoutePrefix("api/Auction")]
    public class AuctionController : ApiController
    {
        private readonly IAuctionManagementService _auctionservice;

        public AuctionController()
        {
            _auctionservice = ServiceLocatorFactory.GetServiceLocator().GetService<IAuctionManagementService>();
        }

        #region 拍卖车辆基本信息

        /// <summary>
        /// 获取正在拍卖的车辆列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuctioningList")]
        public BasePageList<AuctionCarInfoViewModel> GetAuctioningList([FromBody] AuctionCarInfoQueryModel query)
        {
            return _auctionservice.GetAuctioningList(query);
        }
        
        /// <summary>
        /// 获取正在拍卖车辆详情 view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctioningViewById")]
        public JResult GetAuctioningViewById(string id)
        {
            return _auctionservice.GetAuctioningViewById(id);
        }

        /// <summary>
        /// 添加拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAuctionCar")]
        public JResult AddAuctionCar([FromBody] AuctionCarInfoModel model)
        {
            return _auctionservice.AddAuctionCar(model);
        }

        /// <summary>
        /// 更新拍卖车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAuctionCar")]
        public JResult UpdateAuctionCar([FromBody] AuctionCarInfoModel model)
        {
            return _auctionservice.UpdateAuctionCar(model);
        }

        /// <summary>
        /// 获取拍卖车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionInfoById")]
        public JResult GetAuctionInfoById(string id)
        {
            return _auctionservice.GetAuctionInfoById(id);
        }

        #endregion

        #region 竞拍

        /// <summary>
        /// 添加拍卖竞拍人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddParticipant")]
        public JResult AddParticipant([FromBody]AuctionCarParticipantModel model)
        {
            return _auctionservice.AddParticipant(model);
        }

        #endregion

        #region 拍卖时间 

        /// <summary>
        /// 获取拍卖时间列表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuctionTimeList")]
        public JResult GetAuctionTimeList()
        {
            return _auctionservice.GetAuctionTimeList();
        }

        #endregion
    }
}
