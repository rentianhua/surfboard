#region

using CCN.Resource.Properties;
using Cedar.Framework.Common.Client.MVCExtention;

#endregion

namespace CCN.Resource.Main.Common
{
    public class DefaultController : ExtendedController
    {
        public DefaultController()
            : base(Resources.DefaultLocalServiceName)
        {
        }

        
    }
}