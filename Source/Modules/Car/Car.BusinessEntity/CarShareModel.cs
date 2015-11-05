using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆分享相关数据model
    /// </summary>
    public class CarShareModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 分享次数
        /// </summary>
        public int ShareCount { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        public int SeeCount { get; set; }

        /// <summary>
        /// 点赞次数
        /// </summary>
        public int PraiseCount { get; set; }

        /// <summary>
        /// 评论次数
        /// </summary>
        public int CommentCount { get; set; }

    }
}
