using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;

namespace HiiP.Framework.Security.AccessControl.Interface
{
    /// <summary>
    /// The interface of Authorization service.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IViewAuthorizationService
    {
        /// <summary>
        /// Tries the get action code.
        /// </summary>
        /// <param name="viewType">The full name of view type.</param>
        /// <param name="viewStatus">The view status.</param>
        /// <param name="actionCode">The action code matching the combination of view type and view status.</param>
        /// <returns>A <see cref="Boolean"/> value indicating if the view exists in the mapping table.</returns>
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        [OperationContract]
        string GetActionCode(string viewType, string viewStatus);
    }
}
