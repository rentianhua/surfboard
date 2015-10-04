#region

using System;
using System.Collections.Generic;
using CCN.Modules.CustRelations.BusinessEntity;
using CCN.Modules.CustRelations.DataAccess;
using Cedar.Core.IoC;
using Cedar.Framework.Common.BaseClasses;
using Cedar.Framework.Common.Server.BaseClasses;

#endregion

namespace CCN.Modules.CustRelations.BusinessComponent
{
    /// <summary>
    /// </summary>
    public class CustRelationsBC : BusinessComponentBase<CustRelationsDA>
    {
        public CustRelationsBC(CustRelationsDA daObject) : base(daObject)
        {
        }
    }
}