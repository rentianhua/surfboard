using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Core.EntLib.Data;
using Cedar.Core.IoC;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.Framwork.AuditTrail.BusinessEntity;
using Newtonsoft.Json;

namespace Cedar.Framwork.AuditTrail.DataAccess
{
    public class AuditTrailDA : DataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertAuditLog(AuditLogModel entity)
        {
            var warpper = new RedisDatabaseWrapper();
            return warpper.StringSet(entity.ID, JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AuditLogModel> GetAllAuditLogs()
        {
            throw new NotImplementedException();
        }
    }
}
