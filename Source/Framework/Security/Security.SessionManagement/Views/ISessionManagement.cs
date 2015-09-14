using HiiP.Infrastructure.Interface.BusinessEntities;

namespace HiiP.Framework.Security.SessionManagement
{
    public interface ISessionManagementView
    {
        void ShowSessionList(SessionCriteria sessionCriteria);

        void KillSession();

        //void ShowFilterSession();

        void AccessControls(bool hasKillRight);
    }
}

