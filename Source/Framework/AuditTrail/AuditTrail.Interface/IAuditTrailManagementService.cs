using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framwork.AuditTrail.BusinessEntity;

namespace Cedar.Framwork.AuditTrail.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditTrailManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool InsertAuditLog(AuditLogModel entity);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //bool DeleteAuditLog(string key);

        //bool UpdateAuditLog(AuditLogModel entity);

        //dynamic GetAuditLog();

        //dynamic RetreiveAuditLogs();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<AuditLogModel> GetAllAuditLogs();
    }
}
