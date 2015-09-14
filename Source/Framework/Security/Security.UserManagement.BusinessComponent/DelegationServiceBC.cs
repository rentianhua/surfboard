using System;
using System.Collections.Generic;
using System.Text;

using HiiP.Framework.Common;
using HiiP.Framework.Common.InstanceBuilders;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.DataAccess;

using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.Interface.Constants;
using System.Data;
using HiiP.Framework.Common.ApplicationContexts;

namespace HiiP.Framework.Security.UserManagement.BusinessComponent
{
    public class DelegationServiceBC : HiiPBusinessComponentBase
    {
        private DelegationServiceDA _delegationServiceDA =
           InstanceBuilder.Wrap<DelegationServiceDA>(new DelegationServiceDA());

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
 FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationFunctionEntity> GetAllDelegationFunctions()
        {
            return _delegationServiceDA.GetAllDelegationFunctions(); 
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> GetAllDelegationValueEntity()
        {

            return _delegationServiceDA.GetAllDelegationValueEntity(); 
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> FindDelegationValueEntity(DelegationSearchCriteria objDelegationSearchCriteria)
        {
            return _delegationServiceDA.FindDelegationValueEntity(objDelegationSearchCriteria);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CreateDelegationFunctionID)]
        public DelegationValueEntity SearchDelegationValueByID(int delegationValueID)
        {
            return _delegationServiceDA.SearchDelegationValueByID(delegationValueID);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateDelegationFunctionID)]
        public void UpdateDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            _delegationServiceDA.UpdateDelegationValue(objDelegationValueEntity);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CreateDelegationFunctionID)]
        public void InsertDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            _delegationServiceDA.InsertDelegationValue(objDelegationValueEntity);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public List<CodeTableEntity> GetAllAct()
        {
            return _delegationServiceDA.GetAllAct();
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllBranch()
        {
            return _delegationServiceDA.GetOrganisationalUnitByCodeID("FE195147-2D87-4CE6-8FC8-7E744BD3FD6D");
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllUnit(string organisationalUnitID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable DSUnitByBranch = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            int organisationalUnitTypeID = GetOrganisationalUnitTypeID("FE195147-2D87-4CE6-8FC8-7E744BD3FD6D",organisationalUnitID);
            if (organisationalUnitTypeID > 0)
            {
                List<int> lstOrganisationalUnitTypeID = GetOrganisationalUnitHierarchyByParentID(organisationalUnitTypeID);

                if (lstOrganisationalUnitTypeID.Count > 0)
                {
                    LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable DSAllUnits = _delegationServiceDA.GetOrganisationalUnitByCodeID("B2F34325-84EB-4972-B72E-EB35BB13A3E2");

                    foreach (LookupOrganisationalUnitDataSet.LookupOrganisationalUnitRow rw in DSAllUnits)
                    {
                        if (lstOrganisationalUnitTypeID.Contains(Convert.ToInt32(rw.OrganisationalUnitTypeID)))
                        {
                            DSUnitByBranch.Rows.Add(rw.ItemArray);
                        }
                    }
                }
            }
            return DSUnitByBranch;
            
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllSubUnit(string organisationalUnitID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable DSSubUnitByUnit = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            int organisationalUnitTypeID = GetOrganisationalUnitTypeID("B2F34325-84EB-4972-B72E-EB35BB13A3E2", organisationalUnitID);

            if (organisationalUnitTypeID > 0)
            {
                List<int> lstOrganisationalUnitTypeID = GetOrganisationalUnitHierarchyByParentID(organisationalUnitTypeID);

                if (lstOrganisationalUnitTypeID.Count > 0)
                {
                    LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable DSAllSubUnits = _delegationServiceDA.GetOrganisationalUnitByCodeID("80414AC7-728B-4916-A6C5-C4A212BDFE7D");

                    foreach (LookupOrganisationalUnitDataSet.LookupOrganisationalUnitRow rw in DSAllSubUnits)
                    {
                        if (lstOrganisationalUnitTypeID.Contains(Convert.ToInt32(rw.OrganisationalUnitTypeID)))
                        {
                            DSSubUnitByUnit.Rows.Add(rw.ItemArray);
                        }
                    }
                }
            }
            return DSSubUnitByUnit;
        }

        private List<int> GetOrganisationalUnitHierarchyByParentID(int organisationalUnitTypeID)
        {
            List<int> lstOrganisationalUnitTypeID = new List<int>();
            OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable dtOrgUnitHierarchy = _delegationServiceDA.GetOrganisationalUnitHierarchyByParentID(organisationalUnitTypeID);

            foreach (OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow rw in dtOrgUnitHierarchy)
            {
                if (organisationalUnitTypeID == rw.ParentID)
                {
                    lstOrganisationalUnitTypeID.Add(rw.OrganisationalUnitTypeID);
                }
            }

            return lstOrganisationalUnitTypeID;
        }

        private int GetOrganisationalUnitTypeID(string codeID, string organisationalUnitID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dt = _delegationServiceDA.GetOrganisationalUnitByCodeID(codeID);
            dt.PrimaryKey = new [] { dt.Columns["OrganisationalUnitID"] };
              
            DataRow OrganisationalUnitTypeID = dt.Rows.Find(organisationalUnitID);
            
            if (OrganisationalUnitTypeID == null) return 0;
            
            return Convert.ToInt32(OrganisationalUnitTypeID["OrganisationalUnitTypeID"].ToString());
        }


        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public string GetDelegationValues(int delegationFunctionId, string userName, string act)
        {
            return _delegationServiceDA.GetDelegationValues(delegationFunctionId, userName, act);
        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public string GetDelegationValuesByName(string delegationFunctionName, string userName, string act)
        {
            List<DelegationFunctionEntity> lstDelFunction = _delegationServiceDA.GetAllDelegationFunctions();
            
            DelegationFunctionEntity objDelFunction = lstDelFunction.Find(delegate(DelegationFunctionEntity d) { return d.Name.ToLower() == delegationFunctionName.ToLower(); });
            
            if (objDelFunction == null) return null;

            return _delegationServiceDA.GetDelegationValues(objDelFunction.DelegationFunctionId, userName, act);


        }

        [MonitoringCallHandler(ComponentType.BusinessComponent, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public List<string> GetUsersByFinancialAmount(int delegationFunctionId, string act, double financialAmount)
        {
            return _delegationServiceDA.GetUsersByFinancialAmount(delegationFunctionId, act, financialAmount);
        }
    }
}
