// ==============================================================================
// Copyright(C) 2005 NCS Pte Ltd
//
// SYSTEM NAME			: NCS HiiP System
// COMPONENT ID			: HiiP.Foundation.Workflow.ForwardDestinationPresenter
// COMPONENT DESC		: the presenter of forward the work item to another user
//
// CREATED DATE/BY	    :  04 Jun 2008 / Wang Xunnian
//
// REVISION HISTORY:
// DATE/BY                                  ISSUE#/SR#/CS/PM#/OTHERS	DESCRIPTION OF CHANGE
// ==============================================================================

using System;
using System.Linq;
using HiiP.Framework.Common.Constants;
using Microsoft.Practices.CompositeUI;
using System.Collections.Generic;
using Microsoft.Practices.CompositeUI.EventBroker;

using HiiP.Infrastructure.Interface;
using HiiP.Infrastructure.Interface.BusinessEntities;
using HiiP.Foundation.Workflow.Interface.Constants;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Common.Client;
using Infragistics.Win;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.ServiceProxies;
using HiiP.Framework.Common;
using HiiP.Framework.Security.UserManagement.Constants;
using System.Data;
using HiiP.Framework.Logging.Library;

namespace HiiP.Framework.Security.UserManagement
{

    public partial class ForwardDestinationPresenter : Presenter<IForwardDestination>
    {        
        const int RegionOptionSetKey = 1;
        const int HeadOfficeOptionSetKey = 0;
        const string RegionOfficeID = "059";
        private OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable _lookupOrganisationalUnitHierarchy;
       
        public DataRow[] GetRootOrganizationalUnits(bool isHeadOffice, out string valueField, out string textField)
        {
            valueField = _lookupOrganisationalUnitHierarchy.OrganisationalUnitTypeIDColumn.ColumnName;
            textField = _lookupOrganisationalUnitHierarchy.OrganisationalUnitNameColumn.ColumnName;
            string filterExpression ; 
            if (isHeadOffice)
            {
                filterExpression = string.Format("{0} = '{1}'", _lookupOrganisationalUnitHierarchy.OrganisationalUnitIDColumn.ColumnName, OrganisationalUnitID.DelegateHierarchyRoot);
                var roots = _lookupOrganisationalUnitHierarchy.Select(filterExpression, string.Empty, DataViewRowState.CurrentRows);
                if (roots.Length == 0)
                {
                    return new DataRow[0];
                }

                int headOfficeRootId = ((OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow)roots[0]).OrganisationalUnitTypeID;
                filterExpression = string.Format("{0} = {1} AND {2}<>'{3}'", 
                    _lookupOrganisationalUnitHierarchy.ParentIDColumn.ColumnName, headOfficeRootId,
                    _lookupOrganisationalUnitHierarchy.OrganisationalUnitIDColumn, RegionOfficeID);
            }
            else
            {
                filterExpression = string.Format("{0} = '{1}'", _lookupOrganisationalUnitHierarchy.OrganisationalUnitIDColumn.ColumnName, OrganisationalUnitID.RegionalHierarchyRoot);
                var roots = _lookupOrganisationalUnitHierarchy.Select(filterExpression, string.Empty, DataViewRowState.CurrentRows);
                if (roots.Length == 0)
                {
                    return new DataRow[0];
                }

                int regionRootId = ((OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow)roots[0]).OrganisationalUnitTypeID;
                filterExpression = string.Format("{0} = {1}", _lookupOrganisationalUnitHierarchy.ParentIDColumn.ColumnName, regionRootId);
            }
            return _lookupOrganisationalUnitHierarchy.Select(filterExpression, string.Empty, DataViewRowState.CurrentRows);
        }

        public DataRow[] GetSubOrganizationalUnits(string parentUnitTypeId, out string valueField, out string textField)
        {
            valueField = _lookupOrganisationalUnitHierarchy.OrganisationalUnitTypeIDColumn.ColumnName;
            textField = _lookupOrganisationalUnitHierarchy.OrganisationalUnitNameColumn.ColumnName;
            string filterExpression = string.Format("{0} = {1}", _lookupOrganisationalUnitHierarchy.ParentIDColumn.ColumnName, parentUnitTypeId);
            return _lookupOrganisationalUnitHierarchy.Select(filterExpression, string.Empty, DataViewRowState.CurrentRows);
        }

        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        protected override void InitData()
        {
            using (UserMaintenanceServiceProxy proxy = new UserMaintenanceServiceProxy())
            {
                Guid id = HiiP.Framework.Logging.Library.Utility.SetContextValues();
                using (new MonitoringTracer(id, FunctionNames.ForwardDestinationName, FunctionNames.ForwardDestinationName, ComponentType.Screen))
                {
                    _lookupOrganisationalUnitHierarchy = proxy.GetOfficesAllHierarchy(string.Empty).LookupOrganisationalUnitHierarchy;
                }
            }
            this.View.BindRegions();
            this.View.BindHeadOffices();

            var currentUserReprotsTo = GetDefaultUser();
            var reportToUserInfo = new ReportToUserInfo(currentUserReprotsTo, _lookupOrganisationalUnitHierarchy);
            var isSuccessful = reportToUserInfo.Calculate();

            if (!isSuccessful)
            {
                //In UDG, optionSet must have one default option
                View.SetDefaultOption(RegionOptionSetKey);
                return;
            }

            if (reportToUserInfo.IsHeadOffice)
            {
                View.SetDefaultOption(HeadOfficeOptionSetKey);
                if (reportToUserInfo.Branch != 0) View.SetDefaultBranch(reportToUserInfo.Branch);
                if (reportToUserInfo.Unit != 0) View.SetDefaultUnit(reportToUserInfo.Unit);
                if (reportToUserInfo.SubUnit != 0) View.SetDefaultSubUnit(reportToUserInfo.SubUnit);
                View.SetDefaultHeadOfficeUser(reportToUserInfo.UserName);
            }
            else
            {
                View.SetDefaultOption(RegionOptionSetKey);
                if (reportToUserInfo.Region != 0) View.SetDefaultRegion(reportToUserInfo.Region);
                if (reportToUserInfo.Office != 0) View.SetDefaultOffice(reportToUserInfo.Office);
                View.SetDefaultRegionUser(reportToUserInfo.UserName);
            }
            base.InitData();
        }

        private string GetDefaultUser()
        {
            string defaultUser = this.Key;

            if (string.IsNullOrEmpty(defaultUser))
            {
                //if no input, use the ReportTo of current user
                using (var proxy = new UserMaintenanceServiceProxy())
                {
                    UserInfoEntity entity = proxy.GetUser(AppContext.Current.UserName);
                    defaultUser = (entity != null) ? entity.ReportsTo : "";
                }
            }
            return defaultUser;
        }

        public OfficesDataSet GetSubOffices(int officeUnitTypeID)
        {
            return OrganizationUtility.GetSubOffices(officeUnitTypeID);
        }

        public UserBasicInfoEntity[] GetUsersWithSpecifiedOffice(int officeUnitTypeID)
        {
            return OrganizationUtility.GetAllActiveUsersInOffice(officeUnitTypeID);
        }

        public override AppTitleData GetAppTitle()
        {
            CurrentModule = base.CurrentViewStatus;

            return new AppTitleData("Reports to", FunctionNames.ForwardDestinationScreenID);
        }

        /// <summary>
        /// if user click cancel button, will publish a event
        /// </summary>
        internal void CancelAssignment()
        {
            ViewParameter parameter = new ViewParameter();
            switch (CurrentModule)
            {
                case FunctionNames.ManualAssignmentID:
                    WorkItem.EventTopics[HiiP.Foundation.Workflow.Interface.Constants.EventTopicNames.CancelAssignmentEvent].Fire(this, new EventArgs<ViewParameter>(parameter), this.WorkItem, PublicationScope.Global);
                    break;
                case FunctionNames.SelectOfficeUser:
                    WorkItem.EventTopics[HiiP.Foundation.Workflow.Interface.Constants.EventTopicNames.CancelSelectOfficeUserEvent].Fire(this, new EventArgs<ViewParameter>(parameter), this.WorkItem, PublicationScope.Global);
                    break;
            }
        }

        internal string CurrentModule
        {
            get;
            private set ;
        }

        internal class ReportToUserInfo
        {
            public ReportToUserInfo(string currentUserReprotsTo, OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable hierarchy)
            {
                //if reportsTo is not exist, use current login user's reportsTo
                UserName = currentUserReprotsTo;
                _allHierarchy = hierarchy;
            }

            public bool Calculate()
            {
                if (!GetUserInfo()) return false;

                if (IsHeadOffice)
                {
                    CalculateHeadOffice();
                }
                else
                {
                    CalculateRegionOffice();
                }
                return true;
            }

            public string UserName { get; private set; }

            public int Branch { get; private set; }

            public int Unit { get; private set; }

            public int SubUnit { get; private set; }

            public int Region { get; private set; }

            public int Office { get; private set; }

            public bool IsHeadOffice { get; private set; }

            #region private

            private readonly OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable _allHierarchy;

            private string _unitName;
            private int _typeId;
            private string _codeId;
            private int _depth;
            private int _parentId;

            private bool GetUserInfo()
            {
                var defaultOfficeDetailForUser = OrganizationUtility.GetDefaultOfficeDetailForUser(UserName);
                if (defaultOfficeDetailForUser == null )
                {
                    //Retrun false, means the user had no default office, and it will set a default report to.
                    return false;
                }

                var defaultOffices = from unit in defaultOfficeDetailForUser.CM_LookupOrganisationalUnitByType
                                    join hierarchy in _allHierarchy.AsEnumerable() on unit.OrganisationalUnitTypeID equals hierarchy.OrganisationalUnitTypeID
                                    select new DataRow[]{unit,hierarchy};
                var defaultOffice = defaultOffices.FirstOrDefault<DataRow[]>();

                if (defaultOffice == null)
                {
                    return false;
                }

                OfficesDataSet.CM_LookupOrganisationalUnitByTypeRow unitRow = defaultOffice[0] as OfficesDataSet.CM_LookupOrganisationalUnitByTypeRow;
                OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow hierarchyRow = defaultOffice[1] as OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow;

                if (unitRow==null
                    || hierarchyRow==null)
                {
                    return false;
                }
                _typeId = unitRow.OrganisationalUnitTypeID;
                _codeId = unitRow.CodeID;
                _unitName = unitRow.OrganisationalUnitName;

                _depth = hierarchyRow.Depth;
                _parentId = hierarchyRow.ParentID;
                IsHeadOffice = IsHead();
                return true;
            }

            private void CalculateHeadOffice()
            {
                switch (_depth)
                {
                    case 1:
                        Branch = _typeId;
                        break;
                    case 2:
                        Branch = _parentId;
                        Unit = _typeId;
                        break;
                    case 3:
                        var hierarchyRow = _allHierarchy.AsEnumerable().FirstOrDefault(row => row.OrganisationalUnitTypeID == _parentId);

                        if (hierarchyRow == null)
                        {
                            throw new DataNotFoundException(string.Format("Unable to find out the housing region for '{0}'. Or '{0}' should not be one regional housing office.", _unitName));
                        }

                        Branch = hierarchyRow.ParentID;
                        Unit = _parentId;
                        SubUnit = _typeId;
                        break;
                    default:
                        break;
                }
            }

            private void CalculateRegionOffice()
            {
                switch (_depth)
                {
                    case 1:
                        Region = _typeId;
                        break;
                    case 2:
                        Region = _parentId;
                        Office = _typeId;
                        break;
                    default:
                        break;
                }
            }

            private bool IsHead()
            {
                switch (_codeId)
                {
                    case OfficeTypes.HousingRegionCodeID:
                    case OfficeTypes.RegionHousingOfficeCodeID:
                        return false;
                    case OfficeTypes.BranchCodeID:
                    case OfficeTypes.UnitCodeID:
                    case OfficeTypes.SubUnitCodeID:
                        return true;
                    default:
                        throw new NotSupportedException(string.Format("Do not support '{0}'", _unitName));
                }
            }
            #endregion
        }
    }
}

