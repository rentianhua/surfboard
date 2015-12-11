#region

using CCN.Resource.Properties;
using Cedar.Framework.Common.Client.MVCExtention;
using CCN.Modules.Base.BusinessEntity;

#endregion

namespace CCN.Resource.Main.Common
{
    public class DefaultController : ExtendedController
    {
        public DefaultController()
            : base(Resources.DefaultLocalServiceName)
        {
        }

        public const string ADMIN = "d0017f1b-8f4b-11e5-87ae-000c2977415d";

        /// <summary>
        /// 初始化用户登录信息
        /// </summary>
        public BaseUserModel UserInfo {
            get
            {
                if (Session["UserInfo"] != null)
                {
                    return (BaseUserModel)Session["UserInfo"];
                }
                return null;
            }
            set
            {
                Session["UserInfo"] = value;
            }
        }
    }
}