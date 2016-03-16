using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCN.Modules.Car.BusinessEntity
{
    /// <summary>
    /// 车辆图片
    /// </summary>
    public class CarPictureModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Innerid { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 类型id code
        /// </summary>
        public int Typeid { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否封面 1.是，0不是
        /// </summary>
        public int IsCover { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? Createdtime { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CarPictureModel()
        {
            Createdtime = DateTime.Now;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WechatPictureModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 图片链接id
        /// </summary>
        public List<string> MediaIdList { get; set; }
    }

    /// <summary>
    /// 批量添加图片model
    /// </summary>
    public class PictureListModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 七牛key列表
        /// </summary>
        public List<string> KeyList { get; set; }
    }

    /// <summary>
    /// 批量删除图片model
    /// </summary>
    public class PictureDelListModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 需要删除的图片id集合
        /// </summary>
        public List<string> IdList { get; set; }
    }

    /// <summary>
    /// 批量处理图片(添加+删除)(微信端使用)
    /// </summary>
    public class BatchPictureListWeichatModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 需要删除的图片id集合
        /// </summary>
        public List<string> IdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WechatPictureModel WechatPicture { get; set; }
    }

    /// <summary>
    /// 批量处理图片(添加+删除)
    /// </summary>
    public class BatchPictureListModel
    {
        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 需要删除的图片id集合
        /// </summary>
        public List<string> DelIds { get; set; }

        /// <summary>
        /// 添加的图片
        /// </summary>
        public List<string> AddPaths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BatchPictureListModel()
        {
            DelIds = new List<string>();
            AddPaths = new List<string>();
        }
    }

    /// <summary>
    /// 微信webapp使用
    /// </summary>
    public class WechatPictureExModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 车辆id
        /// </summary>
        public string Carid { get; set; }

        /// <summary>
        /// 需要删除的图片id集合
        /// </summary>
        public List<string> DelIds { get; set; }

        /// <summary>
        /// 添加的图片链接id
        /// </summary>
        public List<string> MediaIdList { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WechatPictureExModel()
        {
            DelIds = new List<string>();
            MediaIdList = new List<string>();
        }
    }
}
