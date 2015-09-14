using HiiP.Infrastructure.Interface.Services;

namespace HiiP.Framework.Security.AccessControl.Authentication
{
    public class UserSelectorService : IUserSelectorService
    {
        #region IUserSelectorService Members

        public string SelectUser()
        {
            using (LoginForm loginForm = new LoginForm())
            {
                return loginForm.SectionUserName();
            }
        }

        #endregion
    }
}
