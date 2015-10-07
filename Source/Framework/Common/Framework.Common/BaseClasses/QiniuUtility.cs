using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Conf;
using Qiniu.RS;
using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RPC;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class QiniuUtility
    {
        private static string BUCKET = System.Configuration.ConfigurationManager.AppSettings["BUCKET"].ToString();

        /// <summary>
        /// 
        /// </summary>
        public QiniuUtility() {
            //Config.ACCESS_KEY = "_Xw0SjdG8tbQuA_2kcVo0emRxk5GiFuSrG-TjWGs";
            //Config.SECRET_KEY = "d2BpCvutzDgHzu9ah92LMwDYRnR1sARGXbN1JMz_";
            Config.Init();
        }

        /// <summary>
        /// 普通上传
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
            var policy = new PutPolicy(bucket,3600);
            string upToken = policy.Token();
            PutExtra extra = new PutExtra();
            IOClient client = new IOClient();
            PutRet ret = client.PutFile(upToken, key, fname, extra);
            if (ret != null)
            {
                return ret.key;
            }
            return "";
        }

        /// <summary>
        /// 
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
            PutPolicy policy = new PutPolicy(bucket, 3600);
            string upToken = policy.Token();
            Settings setting = new Settings();
            ResumablePutExtra extra = new ResumablePutExtra();
            ResumablePut client = new ResumablePut(setting, extra);
            CallRet ret = client.PutFile(upToken, fname, Guid.NewGuid().ToString());
            if (ret != null)
            {
                return ret.Response;
            }
            return "";
        }
    }
}
