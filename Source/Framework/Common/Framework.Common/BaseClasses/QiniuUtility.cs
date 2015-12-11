using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Cedar.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// </summary>
    public class QiniuUtility
    {
        private static readonly string BUCKET = ConfigurationManager.AppSettings["BUCKET"];

        /// <summary>
        /// </summary>
        public QiniuUtility()
        {
            //Config.ACCESS_KEY = "_Xw0SjdG8tbQuA_2kcVo0emRxk5GiFuSrG-TjWGs";
            //Config.SECRET_KEY = "d2BpCvutzDgHzu9ah92LMwDYRnR1sARGXbN1JMz_";
            Config.Init();
        }


        /// <summary>
        ///     普通上传
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>key</returns>
        public string Put(Stream stream, string bucket = "", string key = "")
        {
            if (bucket == "")
            {
                bucket = BUCKET;
            }
            var policy = new PutPolicy(bucket, 3600);
            var upToken = policy.Token();
            var extra = new PutExtra();
            var client = new IOClient();
            var ret = client.Put(upToken, key, stream, extra);
            if (ret != null)
            {
                LoggerFactories.CreateLogger().Write("上传图片:" + JsonConvert.SerializeObject(ret), TraceEventType.Warning);
                return ret.key;
            }
            return "";
        }


        /// <summary>
        ///     普通上传
        /// </summary>
        /// <param name="fname">文件本地路径</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>key</returns>
        public string PutFile(string fname, string bucket = "", string key = "")
        {
            if (bucket == "")
            {
                bucket = BUCKET;
            }
            var policy = new PutPolicy(bucket, 3600);
            var upToken = policy.Token();
            var extra = new PutExtra();
            var client = new IOClient();
            var ret = client.PutFile(upToken, key, fname, extra);
            if (ret != null)
            {
                return ret.key;
            }
            return "";
        }

        /// <summary>
        ///     获取token
        /// </summary>
        /// <returns>key</returns>
        public static string GetToken()
        {
            var policy = new PutPolicy(BUCKET);
            var upToken = policy.Token();
            return upToken;
        }

        /// <summary>
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="bucket"></param>
        public string ResumablePutFile(string fname, string bucket = "")
        {
            if (bucket == "")
            {
                bucket = BUCKET;
            }
            //Console.WriteLine("\n===> ResumablePutFile {0}:{1} fname:{2}", bucket, key, fname);
            var policy = new PutPolicy(bucket, 3600);
            var upToken = policy.Token();
            var setting = new Settings();
            var extra = new ResumablePutExtra();
            var client = new ResumablePut(setting, extra);
            var ret = client.PutFile(upToken, fname, Guid.NewGuid().ToString());
            if (ret != null)
            {
                return ret.Response;
            }
            return "";
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public static string GetFileName(Picture picture)
        {
            var filename = string.Concat(picture, "_", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".jpg");
            return filename;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFilePath(string filename)
        {
            var filepath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TempFile\\", filename);
            return filepath;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Picture
    {
        /// <summary>
        /// 
        /// </summary>
        car_picture,
        /// <summary>
        /// 
        /// </summary>
        cust_qrcode,
        /// <summary>
        /// 
        /// </summary>
        card_logo,
        /// <summary>
        /// 
        /// </summary>
        card_qrcode
    }
}