#region

using CCN.WebAPI.Properties;
using Cedar.Framework.Common.Client.MVCExtention;

#endregion

namespace CCN.WebAPI.Common
{
    public class DefaultController : ExtendedController
    {
        public DefaultController()
            : base(Resources.DefaultLocalServiceName)
        {
        }
    }
}