#region

using System.Collections.Generic;
using CCN.Modules.CustRelations.BusinessComponent;
using CCN.Modules.CustRelations.BusinessEntity;
using CCN.Modules.CustRelations.Interface;
using Cedar.Framework.Common.Server.BaseClasses;
using Cedar.AuditTrail.Interception;
using Cedar.Framework.Common.BaseClasses;

#endregion

namespace CCN.Modules.CustRelations.BusinessService
{
    /// <summary>
    /// </summary>
    public class CustRelationsManagementService : ServiceBase<CustRelationsBC>, ICustRelationsManagementService
    {
        /// <summary>
        /// </summary>
        public CustRelationsManagementService(CustRelationsBC bc)
            : base(bc)
        {
        }

    }
}