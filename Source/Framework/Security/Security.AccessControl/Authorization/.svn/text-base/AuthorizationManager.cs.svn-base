using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Security;
using HiiP.Framework.Messaging;
using HiiP.Framework.Common;
using System;

namespace HiiP.Framework.Security.AccessControl.Authorization
{
    /// <summary>
    /// Authorization facade class.
    /// </summary>
    public static class AuthorizationManager
    {
        /// <summary>
        /// Authorize the current user aginst the given operation name.
        /// </summary>
        /// <param name="operationName">Operation name (Business action name)</param>
        /// <returns>A boolean value indicating whether to successfully authorize.</returns>
        public static bool Authorize(string operationName)
        {
            IAuthorizationProvider provider = AuthorizationFactory.GetAuthorizationProvider();
            return provider.Authorize(Thread.CurrentPrincipal, operationName);
        }

        ///// <summary>
        ///// Tries the get action code.
        ///// </summary>
        ///// <param name="viewType">The full name of view type.</param>
        ///// <param name="viewStatus">The view status.</param>
        ///// <param name="actionCode">The action code matching the combination of view type and view status.</param>
        ///// <returns>A <see cref="Boolean"/> value indicating if the view exists in the mapping table.</returns>
        //public static bool TryGetActionCode(string viewType, string viewStatus, out string actionCode)
        //{
        //    if (string.IsNullOrEmpty(viewType))
        //    {
        //        throw new ArgumentNullException("viewType");
        //    }

        //    if (string.IsNullOrEmpty(viewStatus))
        //    {
        //        throw new ArgumentNullException("viewStatus");
        //    }
        //    using (ViewAuthorizationProxy proxy = new ViewAuthorizationProxy())
        //    {
        //        actionCode = proxy.GetActionCode(viewType, viewStatus);
        //    }

        //    return !string.IsNullOrEmpty(actionCode);
        //}
    }
}
