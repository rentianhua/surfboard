using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framwork.AuditTrail.BusinessComponent;
using Cedar.Framwork.AuditTrail.BusinessEntity;
using Cedar.Framwork.AuditTrail.Interface;

namespace Cedar.Framwork.AuditTrail.BusinessService
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditTrailManagementService : ServiceBase<AuditTrailBC>, IAuditTrailManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        public AuditTrailManagementService(AuditTrailBC bc) : base(bc)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertAuditLog(AuditLogModel entity)
        {
            return BusinessComponent.InsertAuditLog(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AuditLogModel> GetAllAuditLogs()
        {
            return BusinessComponent.GetAllAuditLogs();
        }
    }
}
