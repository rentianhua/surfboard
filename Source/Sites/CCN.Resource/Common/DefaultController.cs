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