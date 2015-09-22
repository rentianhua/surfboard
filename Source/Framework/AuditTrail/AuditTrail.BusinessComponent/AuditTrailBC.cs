using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framwork.AuditTrail.BusinessEntity;
using Cedar.Framwork.AuditTrail.DataAccess;

namespace Cedar.Framwork.AuditTrail.BusinessComponent
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditTrailBC : BusinessComponentBase<AuditTrailDA>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        public AuditTrailBC(AuditTrailDA da) : base(da)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertAuditLog(AuditLogModel entity)
        {
            return DataAccess.InsertAuditLog(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AuditLogModel> GetAllAuditLogs()
        {
            return DataAccess.GetAllAuditLogs();
        }
    }
}
