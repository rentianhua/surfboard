using System.Security.Principal;

using HiiP.Framework.Common.Client;

using NCS.IConnect.Security;

namespace HiiP.Framework.Security.AccessControl.Authorization
{
    public class AuthorizationProxy : ServiceProxyBase<IBusinessAction>, IBusinessAction
    {

        public AuthorizationProxy(string endpointName)
            : base(endpointName)
        { }

        public AuthorizationProxy()
        {
            this.WrapObject(new AuthorizationProxy(Interface.Constants.EndpointName.BusinessActionWcfService));
        }

        #region IBusinessAction Members

        public bool ActionExists(string applicationName, string actionCode)
        {
            return this.Proxy.ActionExists(applicationName, actionCode);
        }

        public void AddActionToRole(string applicationName, string actionCode, string roleName, string addedBy)
        {
            this.Proxy.AddActionToRole(applicationName, actionCode, roleName, addedBy);
        }

        public void AddActionToRoles(string applicationName, string actionCode, string[] roleNames, string addedBy)
        {
            this.Proxy.AddActionToRoles(applicationName, actionCode, roleNames, addedBy);
        }

        public void AddActionsToRole(string applicationName, string[] actionCodes, string roleName, string addedBy)
        {
            this.Proxy.AddActionsToRole(applicationName, actionCodes, roleName, addedBy);
        }

        public void AddActionsToRoles(string applicationName, string[] actionCodes, string[] roleNames, string addedBy)
        {
            this.Proxy.AddActionsToRoles(applicationName, actionCodes, roleNames, addedBy);
        }

        public bool Authorize(string applicationName, string userName, string actionCode)
        {
            return this.Proxy.Authorize(applicationName, userName, actionCode);
        }

        public bool Authorize(string applicationName, IPrincipal principal, string actionCode)
        {
            return this.Proxy.Authorize(applicationName, principal, actionCode);
        }

        public BusinessActionCollection CreateActions(string applicationName, BusinessActionCollection actions, string createdBy)
        {
            return this.Proxy.CreateActions(applicationName, actions, createdBy);
        }

        public bool DeleteActions(string applicationName, BusinessActionCollection actions, string deletedBy)
        {
            return this.Proxy.DeleteActions(applicationName, actions, deletedBy);
        }

        public BusinessActionCollection FindActionsByCodeAsCollection(string applicationName, string actionCodeToMatch)
        {
            return this.Proxy.FindActionsByCodeAsCollection(applicationName, actionCodeToMatch);
        }

        public string[] FindActionsInRole(string applicationName, string roleName, string actionCodeToMatch)
        {
            return this.Proxy.FindActionsInRole(applicationName, roleName, actionCodeToMatch);
        }

        public BusinessActionCollection FindActionsInRoleAsCollection(string applicationName, string roleName, string actionCodeToMatch)
        {
            return this.Proxy.FindActionsInRoleAsCollection(applicationName, roleName, actionCodeToMatch);
        }

        public BusinessAction GetAction(string applicationName, string actionCode)
        {
            return this.Proxy.GetAction(applicationName, actionCode);
        }

        public string[] GetActionsForUser(string applicationName, string userName)
        {
            return this.Proxy.GetActionsForUser(applicationName, userName);
        }

        public BusinessActionCollection GetActionsForUserAsCollection(string applicationName, string userName)
        {
            return this.Proxy.GetActionsForUserAsCollection(applicationName, userName);
        }

        public string[] GetActionsInRole(string applicationName, string roleName)
        {
            return this.Proxy.GetActionsInRole(applicationName, roleName);
        }

        public BusinessActionCollection GetActionsInRoleAsCollection(string applicationName, string roleName)
        {
            return this.Proxy.GetActionsInRoleAsCollection(applicationName, roleName);
        }

        public string[] GetAllActions(string applicationName)
        {
            return this.Proxy.GetAllActions(applicationName);
        }

        public BusinessActionCollection GetAllActionsAsCollection(string applicationName)
        {
            return this.Proxy.GetAllActionsAsCollection(applicationName);
        }

        public string[] GetRolesForAction(string applicationName, string actionCode)
        {
            return this.Proxy.GetRolesForAction(applicationName, actionCode);
        }

        public ExtendedRoleCollection GetRolesForActionAsCollection(string applicationName, string actionCode)
        {
            return this.Proxy.GetRolesForActionAsCollection(applicationName, actionCode);
        }

        public bool IsActionInRole(string applicationName, string actionCode, string roleName)
        {
            return this.Proxy.IsActionInRole(applicationName, actionCode, roleName);
        }

        public void RemoveActionsFromRoles(string applicationName, string[] actionCodes, string[] roleNames, string removedBy)
        {
            this.Proxy.RemoveActionsFromRoles(applicationName, actionCodes, roleNames, removedBy);
        }

        public BusinessActionCollection UpdateActions(string applicationName, BusinessActionCollection actions, string updatedBy)
        {
            return this.Proxy.UpdateActions(applicationName, actions, updatedBy);
        }

        #endregion
    }
}
