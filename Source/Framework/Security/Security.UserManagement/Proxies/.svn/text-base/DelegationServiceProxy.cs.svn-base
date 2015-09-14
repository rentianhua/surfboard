using System.Collections.Generic;
using HiiP.Framework.Common.Client;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface;
using HiiP.Framework.Security.UserManagement.Interface.Constants;

namespace HiiP.Framework.Security.UserManagement.ServiceProxies
{
    public class DelegationServiceProxy : ServiceProxyBase<IDelegationService>,
                                                  IDelegationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegationServiceProxy"/> class.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        protected DelegationServiceProxy(string endpointName)
            : base(endpointName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegationServiceProxy"/> class.
        /// </summary>
        public DelegationServiceProxy()
        {
            WrapObject(new DelegationServiceProxy(EndpointNames.DelegationServiceEndpoint));
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationFunctionEntity> GetAllDelegationFunctions()
        {
            return Proxy.GetAllDelegationFunctions();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> GetAllDelegationValueEntity()
        {
            return Proxy.GetAllDelegationValueEntity();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> FindDelegationValueEntity(DelegationSearchCriteria objDelegationSearchCriteria)
        {
            return Proxy.FindDelegationValueEntity(objDelegationSearchCriteria);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.CreateDelegationFunctionID)]
        public DelegationValueEntity SearchDelegationValueByID(int delegationValueID)
        {
            return Proxy.SearchDelegationValueByID(delegationValueID);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.UpdateDelegationFunctionID)]
        public void UpdateDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            Proxy.UpdateDelegationValue(objDelegationValueEntity);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.CreateDelegationFunctionID)]
        public void InsertDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            Proxy.InsertDelegationValue(objDelegationValueEntity);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public List<CodeTableEntity> GetAllAct()
        {
            return Proxy.GetAllAct();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllBranch()
        {
            return Proxy.GetAllBranch();
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllUnit(string organisationalUnitID)
        {
            return Proxy.GetAllUnit(organisationalUnitID);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllSubUnit(string organisationalUnitID)
        {
            return Proxy.GetAllSubUnit(organisationalUnitID);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public string GetDelegationValues(int delegationFunctionId, string userName, string act)
        {
            return Proxy.GetDelegationValues(delegationFunctionId, userName, act);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public string GetDelegationValuesByName(string delegationFunctionName, string userName, string act)
        {
            return Proxy.GetDelegationValuesByName(delegationFunctionName, userName, act);
        }

        [MonitoringCallHandler(ComponentType.ServiceProxy, Ordinal = 1, ModuleID = FunctionNames.DelegationsModuleID,
FunctionID = FunctionNames.DelegationsModuleID)]
        public List<string> GetUsersByFinancialAmount(int delegationFunctionId, string act, double financialAmount)
        {
            return Proxy.GetUsersByFinancialAmount(delegationFunctionId, act, financialAmount);
        }
    }
}
