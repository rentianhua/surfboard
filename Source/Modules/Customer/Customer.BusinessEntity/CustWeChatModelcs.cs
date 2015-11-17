using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.BaseClasses;

namespace CCN.Modules.Customer.BusinessEntity
{
       /// <summary>
        /// cust_wechat
        /// </summary>
        public class CustWeChatModel
        {
        /// <summary>
        /// 
        /// </summary>
            public string innerid { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string accountid { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string photo { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string openid { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string remarkname { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string area { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public int? sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public int? isdel { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string subscribe_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public int? subscribe { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string province { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string city { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public DateTime? createdtime { get; set; }
        }
        /// <summary>
        /// 列表显示信息
        /// </summary>
        public class CustWeChatViewModel
        {
        /// <summary>
        /// 
        /// </summary>
            public string accountid { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string openid { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public string remarkname { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public int? sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
            public DateTime? createdtime { get; set; }
        }
        /// <summary>
        /// 查询信息条件
        /// </summary>
        public class CustWeChatQueryModel : QueryModel {
        /// <summary>
        /// 
        /// </summary>
            public string accountid { get; set; }
    }
    }


