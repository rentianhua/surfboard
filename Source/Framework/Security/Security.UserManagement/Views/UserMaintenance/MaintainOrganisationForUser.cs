#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME      :  Housing Integrated Information Program
// COMPONENT ID     :  User management
// COMPONENT DESC   :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using HiiP.Framework.Common.Client;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.UserManagement.Constants;

using Infragistics.Win.UltraWinListView;
using Infragistics.Win.UltraWinTree;

namespace HiiP.Framework.Security.UserManagement
{
    public sealed class MaintainOrganisationForUser:IDisposable
    {
        private OfficesHierarchyDataSet.OrganisationUserDataTable _currentUserOffices;
        private string _defaultOfficekey, _changedDefaultOfficeKey = string.Empty;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_currentUserOffices!=null) _currentUserOffices.Dispose();
            }
        }
        private void ResortOrganisationalUnitTree(UltraTree organisationalUnitTree)
        {
            DevTreeNode delegationHierarchyRoot = null;
            foreach (DevTreeNode node in organisationalUnitTree.Nodes)
            {
                OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow row = node.Tag as OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow;

                if (row==null)
                {
                    continue;
                }
                if (row.OrganisationalUnitID == OrganisationalUnitID.DelegateHierarchyRoot)
                {
                    delegationHierarchyRoot = node;
                }
            }

            organisationalUnitTree.Nodes.Clear();
            if (delegationHierarchyRoot!=null)
            {
                foreach (var delegationNode in delegationHierarchyRoot.Nodes)
                {
                    organisationalUnitTree.Nodes.Add(delegationNode);
                }
            }
        }

        public void InitOrganisationTree(UltraTree organisationTree, OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable hierarchy)
        {
            organisationTree.Nodes.Clear();
            var rootRows = hierarchy.Select(string.Format("{0} IS NULL", hierarchy.ParentIDColumn.ColumnName), string.Empty, DataViewRowState.CurrentRows);
            foreach (OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyRow rootRow in rootRows)
            {
                DevTreeNode childNode = new DevTreeNode();
                childNode.Key = rootRow.OrganisationalUnitHierarchyID.ToString();
                childNode.PrimaryKey = rootRow.OrganisationalUnitHierarchyID.ToString();
                childNode.Text = rootRow.OrganisationalUnitName;
                childNode.Tag = rootRow;
                BuildOrganizationalTree(childNode, rootRow.MapPath, rootRow.Depth, hierarchy);
                organisationTree.Nodes.Add(childNode);
            }
            ResortOrganisationalUnitTree(organisationTree);
            organisationTree.ExpandAll();
        }

        private void BuildOrganizationalTree(DevTreeNode parentNode, string parentMapPath, int parentDepth, OfficesHierarchyDataSet.LookupOrganisationalUnitHierarchyDataTable hierarchy)
        {
            var childRows = from child in hierarchy
                            where child.MapPath.StartsWith(parentMapPath) && child.Depth == parentDepth + 1
                            select child;
            foreach (var row in childRows)
            {
                DevTreeNode childNode = new DevTreeNode();
                childNode.Key = row.OrganisationalUnitHierarchyID.ToString();
                childNode.PrimaryKey = row.OrganisationalUnitHierarchyID.ToString();
                childNode.Text = row.OrganisationalUnitName;
                childNode.Tag = row;
                parentNode.Nodes.Add(childNode);
                BuildOrganizationalTree(childNode, row.MapPath, row.Depth, hierarchy);
            }
        }

        public static UltraListViewItem CreateOfficeOfUser(string key, string text, string tag)
        {
            var item = new UltraListViewItem(text, null) {Key = key, Tag = tag};
            return item;
        }

        public void InitUserOrganisationTree(UltraListView userOrganisationTree,
            string defaultOfficeKey, string viewStatus, OfficesHierarchyDataSet.OrganisationUserDataTable officesOfUser)
        {
            _currentUserOffices = officesOfUser;
            if (_currentUserOffices == null || _currentUserOffices.Rows.Count == 0) return;

            string officeName ;
            //Get the default office organisation
            //string hierarchyId = GetOfficeHierarchyId(defaultOfficeKey, officeHierarchy, out officeName);
            this._defaultOfficekey = defaultOfficeKey;
            var offices = this._currentUserOffices.Distinct<OfficesHierarchyDataSet.OrganisationUserRow>(new Comparint());

            foreach (OfficesHierarchyDataSet.OrganisationUserRow office in offices)
            {
                string key = office.UnitID;

                var officeDetail = OrganizationUtility.GetAllOffices().CM_LookupOrganisationalUnit.FindByOrganisationalUnitID(key);
                officeName = (officeDetail == null) ? string.Empty : officeDetail.OrganisationalUnitName;

                var tag = office.NodeID.ToString();
                userOrganisationTree.Items.Add(CreateOfficeOfUser(key, officeName, tag));
                if (string.IsNullOrEmpty(_defaultOfficekey)) continue;
                if (!_defaultOfficekey.Equals(key)) continue;

                userOrganisationTree.Items[key].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                _defaultOfficekey = key;
            }

            switch (viewStatus)
            {
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Add:
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.Update:
                    break;
                case HiiP.Framework.Security.UserManagement.Interface.Constants.ViewStatus.CopyNewUser:
                    //Reset it so that SaveSelectedOffices() will insert records into database
                    _currentUserOffices = new OfficesHierarchyDataSet.OrganisationUserDataTable();
                    break;
            }
        }

        //Highlighted the default office node
        public void SetDefaultOffice(UltraListView selectedOffice)
        {
            if (selectedOffice.ActiveItem == null) return;

            if ((!string.IsNullOrEmpty(_changedDefaultOfficeKey))
                && (selectedOffice.Items.IndexOf(_changedDefaultOfficeKey) > -1))
            {
                //Reset the previous node to non-highlight
                selectedOffice.Items[_changedDefaultOfficeKey].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            }
            if ((!string.IsNullOrEmpty(this._defaultOfficekey))
                && (selectedOffice.Items.IndexOf(_defaultOfficekey) > -1))
            {
                //Reset the original node to non-highlight
                selectedOffice.Items[_defaultOfficekey].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            }

            _changedDefaultOfficeKey = selectedOffice.ActiveItem.Key;

            //Reset it to blank so that 'SaveSelectedOffices' can know whether there was change.
            if (_changedDefaultOfficeKey.Equals(_defaultOfficekey)) _changedDefaultOfficeKey = string.Empty;

            selectedOffice.ActiveItem.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
        }

        public void SaveSelectedOffices(UltraListView selectedOffice, string userName, out OfficesHierarchyDataSet.OrganisationUserDataTable removedOfficeList, out OfficesHierarchyDataSet.OrganisationUserDataTable newOfficeList)
        {
            //selectedOfficeKeys is current assigned office in the right pane.
            var selectedOfficeKeys = selectedOffice.Items.Cast<UltraListViewItem>().ToDictionary(item => item.Key, item => item.Tag.ToString());

            //Get to delete orgasation nodes.
            removedOfficeList = null;
            //List<OrganizationNode> newOrganizationNodes = new List<OrganizationNode>();
            if (_currentUserOffices != null && _currentUserOffices.Rows.Count > 0)
            {
                removedOfficeList = _currentUserOffices.Copy() as OfficesHierarchyDataSet.OrganisationUserDataTable;
                if (removedOfficeList!=null)
                {
                    for (int i = removedOfficeList.Rows.Count - 1; i >= 0; i--)
                    {
                        OfficesHierarchyDataSet.OrganisationUserRow officeOfUser = removedOfficeList[i];

                        string key = officeOfUser.UnitID;
                        //Find the node in right pane
                        if (!selectedOfficeKeys.ContainsKey(key))
                        {
                            //Unable to find out it in right pane, so remove it from current office nodes of the user.
                            officeOfUser.Delete();
                        }
                        else
                        {
                            //Exclude these non-changed nodes so that just left the new nodes at last
                            selectedOfficeKeys.Remove(key);
                        }
                    }

                    removedOfficeList = removedOfficeList.GetChanges(DataRowState.Deleted) as OfficesHierarchyDataSet.OrganisationUserDataTable;
                }

            }

            //Gets the new orgasation nodes.
            newOfficeList = new OfficesHierarchyDataSet.OrganisationUserDataTable();

            foreach (var selectedOfficeKey in selectedOfficeKeys)
            {
                var unitId = selectedOfficeKey.Key;
                string unitIDColumnName = newOfficeList.UnitIDColumn.ColumnName;
                if (removedOfficeList != null && removedOfficeList.AsEnumerable().Any(row => row[unitIDColumnName, DataRowVersion.Original].ToString() == unitId))
                    continue;
                //Including original nodes + added nodes
                OfficesHierarchyDataSet.OrganisationUserRow newRow = newOfficeList.NewOrganisationUserRow();
                newRow.UserID = userName;
                newRow.NodeID = selectedOfficeKey.Value;
                newRow.UnitID = unitId;

                newRow.VersionNO = 1;
                newRow.TransactionID = Guid.NewGuid().ToString();
                newRow.CreatedBY = AppContext.Current.UserName;
                newRow.CreatedTime = DateTime.Now;
                newRow.LastUpdatedBy = AppContext.Current.UserName;
                newRow.LastUpdatedTime = DateTime.Now;

                newOfficeList.Rows.Add(newRow);

            }
        }
    }

    internal class DevTreeNode : UltraTreeNode
    {

        public string PrimaryKey
        {
            get;
            set;

        }

    }


    public class Comparint : IEqualityComparer<OfficesHierarchyDataSet.OrganisationUserRow>
    {

        #region IEqualityComparer<OrganisationUserRow> Members

        public bool Equals(OfficesHierarchyDataSet.OrganisationUserRow x, OfficesHierarchyDataSet.OrganisationUserRow y)
        {
            if (x==null || y==null)
            {
                return false;
            }

            return x.UnitID == y.UnitID;
        }

        public int GetHashCode(OfficesHierarchyDataSet.OrganisationUserRow obj)
        {
            return obj.UnitID.GetHashCode();
        }

        #endregion
    }

}
