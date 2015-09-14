#region

using Cedar.Framework.Common.Client.MVCExtention;
using Smartac.SR.Main.Properties;

#endregion

namespace Smartac.SR.Main.Common
{
    public class DefaultController : ExtendedController
    {
        public DefaultController()
            : base(Resources.DefaultLocalServiceName)
        {
        }
    }
}