using System.Collections.Generic;
using System.Text;
using System.Data;
using NCS.IConnect.Helpers.Data;
using System.Data.Common;
using System.Collections;
using System.Web.Profile;
using System.Web.Security;
using NCS.IConnect.Security;

using NCS.IConnect.CodeTable;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using System.Linq;
using HiiP.Framework.Common;
using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Infrastructure.Interface.Constants;
using System;

namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class DelegationServiceDA : HiiPDataAccessBase
    {
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationFunctionEntity> GetAllDelegationFunctions()
        {
            List<DelegationFunctionEntity> lstDelegationFunctionEntity = new List<DelegationFunctionEntity>();
            DataTable dtDelegationFunction = new DataTable();

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationFunction_S");
            helper.AssignParameterValues(command);
            Helper.Fill(dtDelegationFunction, command);
            
            foreach (DataRow dr in dtDelegationFunction.Rows)
            {
                lstDelegationFunctionEntity.Add(new DelegationFunctionEntity(Convert.ToInt32(dr["DelegationFunctionID"].ToString()), dr["Name"].ToString(), dr["Description"].ToString()));
            }
            

            return lstDelegationFunctionEntity;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> GetAllDelegationValueEntity()
        {
            List<DelegationValueEntity> lstDelegationValueEntity = new List<DelegationValueEntity>();
            DataTable dtDelegationValueEntity = new DataTable();


            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationValue_S");
            helper.AssignParameterValues(command);
            Helper.Fill(dtDelegationValueEntity, command);


            foreach (DataRow dr in dtDelegationValueEntity.Rows)
            {
                DelegationValueEntity objDelegationValueEntity = new DelegationValueEntity();
                objDelegationValueEntity.DelegationValueId = Convert.ToInt32(dr["DelegationValueID"].ToString());
                objDelegationValueEntity.DelegationFunctionId = Convert.ToInt32(dr["DelegationFunctionID"].ToString());
                objDelegationValueEntity.DelegationFunctionName = dr["DelegationName"].ToString();
                objDelegationValueEntity.Branch = dr["Branch"].ToString();
                objDelegationValueEntity.Unit = dr["Unit"].ToString();
                objDelegationValueEntity.Subunit = dr["Subunit"].ToString();
                objDelegationValueEntity.Grade = dr["Grade"].ToString();
                objDelegationValueEntity.Act = dr["Act"].ToString();
                objDelegationValueEntity.DelegationValue = dr["DelegationValue"].ToString();
                objDelegationValueEntity.PolicyValue = dr["PolicyValue"].ToString();
                objDelegationValueEntity.DelegationReference = dr["DelegationReference"].ToString();
                objDelegationValueEntity.StartDate = Convert.ToDateTime(dr["EffectiveStartDate"].ToString());
                objDelegationValueEntity.EndDate = Convert.ToDateTime(dr["EffectiveEndDate"].ToString());
                
                lstDelegationValueEntity.Add(objDelegationValueEntity);
            }


            return lstDelegationValueEntity;
        }


        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainDelegationFunctionID)]
        public List<DelegationValueEntity> FindDelegationValueEntity(DelegationSearchCriteria objDelegationSearchCriteria)
        {
            int showActive = objDelegationSearchCriteria.IsShowActive ?1:0;

            List<DelegationValueEntity> lstDelegationValueEntity = new List<DelegationValueEntity>();

            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationValue_SEARCH");
            helper.AssignParameterValues(
                command,
                objDelegationSearchCriteria.DelegationFunctionId,
                objDelegationSearchCriteria.AdminNotes,
                showActive
                );
            Helper.Fill(dt, command);

            
            foreach (DataRow dr in dt.Rows)
            {
                DelegationValueEntity objDelegationValueEntity = new DelegationValueEntity();
                objDelegationValueEntity.DelegationValueId = Convert.ToInt32(dr["DelegationValueID"].ToString());
                objDelegationValueEntity.DelegationFunctionId = Convert.ToInt32(dr["DelegationFunctionID"].ToString());
                objDelegationValueEntity.DelegationFunctionName = dr["DelegationName"].ToString();
                objDelegationValueEntity.Branch = dr["Branch"].ToString();
                objDelegationValueEntity.Unit = dr["Unit"].ToString();
                objDelegationValueEntity.Subunit = dr["Subunit"].ToString();
                objDelegationValueEntity.Grade = dr["Grade"].ToString();
                objDelegationValueEntity.Act = dr["Act"].ToString();
                objDelegationValueEntity.DelegationValue = dr["DelegationValue"].ToString();
                objDelegationValueEntity.OperationalCondition = dr["OperationalConditions"].ToString();
                objDelegationValueEntity.PolicyValue = dr["PolicyValue"].ToString();
                objDelegationValueEntity.DelegationReference = dr["DelegationReference"].ToString();
                objDelegationValueEntity.StartDate = Convert.ToDateTime(dr["EffectiveStartDate"].ToString());
                objDelegationValueEntity.EndDate = Convert.ToDateTime(dr["EffectiveEndDate"].ToString());

                lstDelegationValueEntity.Add(objDelegationValueEntity);
            }

            return lstDelegationValueEntity;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CreateDelegationFunctionID)]
        public DelegationValueEntity SearchDelegationValueByID(int DelegationValueID)
        {
            DelegationValueEntity objDelegationValueEntity = new DelegationValueEntity();

            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationValue_S_ByDelegationValueID");
            helper.AssignParameterValues(
                command,
                DelegationValueID
                );
            Helper.Fill(dt, command);

            foreach (DataRow dr in dt.Rows)            
            {               
                objDelegationValueEntity.DelegationValueId = Convert.ToInt32(dr["DelegationValueID"].ToString());
                objDelegationValueEntity.DelegationFunctionId = Convert.ToInt32(dr["DelegationFunctionID"].ToString());
                objDelegationValueEntity.DelegationFunctionName = dr["DelegationName"].ToString();
                objDelegationValueEntity.Branch = dr["Branch"].ToString();
                objDelegationValueEntity.Unit = dr["Unit"].ToString();
                objDelegationValueEntity.Subunit = dr["Subunit"].ToString();
                objDelegationValueEntity.Grade = dr["Grade"].ToString();
                objDelegationValueEntity.Act = dr["Act"].ToString();
                objDelegationValueEntity.DelegationValue = dr["DelegationValue"].ToString();
                objDelegationValueEntity.PolicyValue = dr["PolicyValue"].ToString();
                objDelegationValueEntity.DelegationReference = dr["DelegationReference"].ToString();
                objDelegationValueEntity.OperationalCondition = dr["OperationalConditions"].ToString();
                objDelegationValueEntity.StartDate = Convert.ToDateTime(dr["EffectiveStartDate"].ToString());
                objDelegationValueEntity.EndDate = Convert.ToDateTime(dr["EffectiveEndDate"].ToString());
                objDelegationValueEntity.VersionNo = Convert.ToInt32(dr["VersionNo"].ToString());
            }

            
            return objDelegationValueEntity;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateDelegationFunctionID)]
        public void UpdateDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationValue_U");
            helper.AssignParameterValues(
                command,
                objDelegationValueEntity.DelegationValueId,
                objDelegationValueEntity.DelegationFunctionId,
                objDelegationValueEntity.Branch,
                objDelegationValueEntity.Unit,
                objDelegationValueEntity.Subunit,
                objDelegationValueEntity.Grade,
                objDelegationValueEntity.Act,
                objDelegationValueEntity.DelegationValue,
                objDelegationValueEntity.PolicyValue,
                objDelegationValueEntity.OperationalCondition,
                objDelegationValueEntity.DelegationReference,
                objDelegationValueEntity.StartDate,
                objDelegationValueEntity.EndDate,
                objDelegationValueEntity.VersionNo,
                NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId,
                AppContext.Current.UserName
               );
            Helper.ExecuteNonQuery(command);
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.CreateDelegationFunctionID)]
        public void InsertDelegationValue(DelegationValueEntity objDelegationValueEntity)
        {
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_DelegationValue_I");
            helper.AssignParameterValues(
                command,
                objDelegationValueEntity.DelegationFunctionId,
                objDelegationValueEntity.Branch,
                objDelegationValueEntity.Unit,
                objDelegationValueEntity.Subunit,
                objDelegationValueEntity.Grade,
                objDelegationValueEntity.Act,
                objDelegationValueEntity.DelegationValue,
                objDelegationValueEntity.PolicyValue,
                objDelegationValueEntity.OperationalCondition,
                objDelegationValueEntity.DelegationReference,
                objDelegationValueEntity.StartDate,
                objDelegationValueEntity.EndDate,
                NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId,
                AppContext.Current.UserName
               );
            Helper.ExecuteNonQuery(command);
        }

//        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
//FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
//        public List<ActionEntity> GetAllActionsForDelegations()
//        {
//            ActionEntity AE;
//            List<ActionEntity> LAE = new List<ActionEntity>();
//            DataTable Actions = new DataTable();
//            Helper.Fill(Actions, "P_IC_ACTIONS_GET_ALL", Membership.ApplicationName);
//            foreach (DataRow drow in Actions.Rows)
//            {
//                AE = new ActionEntity();
//                AE.ActionID = drow["ACTION_ID"].ToString();
//                AE.ActionCode = drow["ACTION_CODE"].ToString();
//                AE.ActionDesc = drow["ACTION_DESC"].ToString();
//                LAE.Add(AE);
//            }
//            return LAE;
//        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable GetOrganisationalUnitHierarchyByParentID(int ParentId)
        {
            OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable LookupOrganisationalUnitHierarchyDataTable = new OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnitHierarchy_S_ByParentID");
            helper.AssignParameterValues(command, ParentId);
            Helper.Fill(LookupOrganisationalUnitHierarchyDataTable, command);
            return LookupOrganisationalUnitHierarchyDataTable;
        }
    
        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public List<CodeTableEntity> GetAllAct()
        {
            return T_IC_CODEByCodeCategory("ActOfDelegation");
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        private List<CodeTableEntity> T_IC_CODEByCodeCategory(string CodeCategory)
        {
            CodeTableDataSet.T_IC_CODEDataTable dt = CodeManager.Find(CodeCategory, String.Format("%{0}%", String.Empty), String.Format("%{0}%", String.Empty));


            List<CodeTableEntity> objCodeTbl = new List<CodeTableEntity>();

            if (dt.Rows.Count > 0)
            {
                foreach (CodeTableDataSet.T_IC_CODERow row in dt.Rows)
                {
                    objCodeTbl.Add(
                        new CodeTableEntity(
                            row.CODE_ID,
                            row.CODE
                            )
                        );
                }
            }

            return objCodeTbl;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationalUnitByCodeID(string codeID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable LookupOrganisationalUnitDataTable = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnitByCodeID_S");
            helper.AssignParameterValues(command, codeID);
            Helper.Fill(LookupOrganisationalUnitDataTable, command);

            return LookupOrganisationalUnitDataTable;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public string GetDelegationValues(int delegationFunctionId, string userName, string act)
        {
            string DelegationValues;

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_GetDelegationValues_S");
            helper.AssignParameterValues(
                command,
                delegationFunctionId.ToString(),
                userName,
                act
                );
            
            DelegationValues = Helper.ExecuteScalar(command).ToString();
            
            return DelegationValues;
            
        }

        [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.DelegationsModuleID)]
        public List<string> GetUsersByFinancialAmount(int delegationFunctionId, string act, double financialAmount)
        {
            return null;
        }
    }
}
