#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Data access
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
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

namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class OrganisationMaintenanceServiceDA : HiiPDataAccessBase
    {

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
             FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewOrganisationFunctionID)]
        public OfficeDetailEntity GetOfficeDetails(string OrganisationalUnitID)
        {
            OfficeDetailEntity rOfficeDetailEntity = new OfficeDetailEntity();

            DataTable dt = new DataTable();
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_CM_OrganisationalUnitDetail_S");
            helper.AssignParameterValues(command, OrganisationalUnitID);
            Helper.Fill(dt, command);
            if (dt.Rows.Count > 0)
            {
                rOfficeDetailEntity.OrganisationalUnitDetailsID = int.Parse(dt.Rows[0]["OrganisationalUnitDetailID"].ToString());
                rOfficeDetailEntity.OrganisationalUnitID = dt.Rows[0]["OrganisationalUnitID"].ToString();
                rOfficeDetailEntity.OrganisationalUnitName = dt.Rows[0]["OrganisationalUnitName"].ToString();
                rOfficeDetailEntity.Address1 = dt.Rows[0]["Address1"].ToString();
                rOfficeDetailEntity.Address2 = dt.Rows[0]["Address2"].ToString();
                rOfficeDetailEntity.Address3 = dt.Rows[0]["Address3"].ToString();
                rOfficeDetailEntity.Postcode = dt.Rows[0]["Postcode"].ToString();
                rOfficeDetailEntity.Suburb = dt.Rows[0]["Suburb"].ToString();
                rOfficeDetailEntity.State = dt.Rows[0]["State"].ToString();
                rOfficeDetailEntity.CountryNameCode = dt.Rows[0]["CountryNameCode"].ToString();
                rOfficeDetailEntity.PostalDeliveryNumber = dt.Rows[0]["PostalDeliveryNumber"].ToString();
                rOfficeDetailEntity.StreetNumber1 = dt.Rows[0]["StreetNumber1"].ToString();
                rOfficeDetailEntity.StreetNumber2 = dt.Rows[0]["StreetNumber2"].ToString();
                rOfficeDetailEntity.StreetName = dt.Rows[0]["StreetName"].ToString();
                rOfficeDetailEntity.LevelNumber = dt.Rows[0]["LevelNumber"].ToString();
                rOfficeDetailEntity.UnitNumber = dt.Rows[0]["UnitNumber"].ToString();
                rOfficeDetailEntity.PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString();
                rOfficeDetailEntity.FaxNumber = dt.Rows[0]["FaxNumber"].ToString();
                rOfficeDetailEntity.EmailAddress = dt.Rows[0]["EmailAddress"].ToString();
                rOfficeDetailEntity.PhoneNumberTollFree = dt.Rows[0]["PhoneNumberTollFree"].ToString();

            }
            return rOfficeDetailEntity;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
        FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.MaintainOrganisationFunctionID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookup()
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable LookupOrganisationalUnitDataTable = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnit_S");
            helper.AssignParameterValues(command);
            Helper.Fill(LookupOrganisationalUnitDataTable, command);

            return LookupOrganisationalUnitDataTable;
        }


        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
        FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.ViewOrganisationFunctionID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationLookupByOrgID(string OrganisationalUnitID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable LookupOrganisationalUnitDataTable = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnitByOrgID_S");
            helper.AssignParameterValues(command, OrganisationalUnitID);
            Helper.Fill(LookupOrganisationalUnitDataTable, command);


            #region Get organisation info and address details
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtOrgDetail = GetOrganisationalUnitDetailByOrgID(OrganisationalUnitID);

            if (dtOrgDetail.Rows.Count > 0)
            {
                LookupOrganisationalUnitDataTable[0].UnitNumber = dtOrgDetail[0].UnitNumber;
                LookupOrganisationalUnitDataTable[0].StreetNumber = dtOrgDetail[0].StreetNumber;
                LookupOrganisationalUnitDataTable[0].StreetNumber = dtOrgDetail[0].StreetNumber;
                LookupOrganisationalUnitDataTable[0].Street = dtOrgDetail[0].Street;
                LookupOrganisationalUnitDataTable[0].Type = dtOrgDetail[0].Type;
                LookupOrganisationalUnitDataTable[0].Suffix = dtOrgDetail[0].Suffix;
                LookupOrganisationalUnitDataTable[0].Suburb = dtOrgDetail[0].Suburb;
                LookupOrganisationalUnitDataTable[0].PostCode = dtOrgDetail[0].PostCode;
                LookupOrganisationalUnitDataTable[0].State = dtOrgDetail[0].State;

                LookupOrganisationalUnitDataTable[0].Mobile = dtOrgDetail[0].Mobile;
                LookupOrganisationalUnitDataTable[0].Phone = dtOrgDetail[0].Phone;
                LookupOrganisationalUnitDataTable[0].TollFreePhone = dtOrgDetail[0].TollFreePhone;
                LookupOrganisationalUnitDataTable[0].Fax = dtOrgDetail[0].Fax;
                LookupOrganisationalUnitDataTable[0].Email = dtOrgDetail[0].Email;
            }
            #endregion

            return LookupOrganisationalUnitDataTable;
        }

        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID)]
        public LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetOrganisationalUnitDetailByOrgID(string OrganisationalUnitID)
        {
            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable LookupOrganisationalUnitDataTable = new LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable();

            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_OrganisationalUnitDetailByOrgID_S");
            helper.AssignParameterValues(command, OrganisationalUnitID);
            Helper.Fill(LookupOrganisationalUnitDataTable, command);

            return LookupOrganisationalUnitDataTable;
        }

[MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
      FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateOrganisationFunctionID)]
        public void SaveOrganisationLookup(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable)
        {            

            string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnit_U");

            helper.AssignParameterValues(command, 
                Membership.ApplicationName, 
                dtLookupOrganisationalUnitDataTable[0].OrganisationalUnitID, 
                dtLookupOrganisationalUnitDataTable[0].OrganisationalUnitName, 
                dtLookupOrganisationalUnitDataTable[0].VersionNo, 
                transactionid, 
                AppContext.Current.UserName.ToString(), 
                DateTime.Now);


            Helper.ExecuteNonQuery(command);

            LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtOrgDetail = GetOrganisationalUnitDetailByOrgID(dtLookupOrganisationalUnitDataTable[0].OrganisationalUnitID);


            int? addressId = SaveAddress(dtLookupOrganisationalUnitDataTable,dtOrgDetail);
        
        
            SaveOrganisationLookupDetail(dtLookupOrganisationalUnitDataTable,dtOrgDetail,addressId);

        }

        private void SaveOrganisationLookupDetail(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable,  LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtOrgDetail, int? AddressID)
        {
            string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            DbHelper helper = new DbHelper();
            DbCommand command  = null;


            if (dtOrgDetail.Rows.Count > 0)            
                command = helper.BuildDbCommand("P_SS_OrganisationalUnitDetail_U");            
            else
                command = helper.BuildDbCommand("P_SS_OrganisationalUnitDetail_I");

            helper.AssignParameterValues(command,
                Membership.ApplicationName,
                dtLookupOrganisationalUnitDataTable[0].OrganisationalUnitID,
                AddressID,
                dtLookupOrganisationalUnitDataTable[0].Phone,
                dtLookupOrganisationalUnitDataTable[0].Fax,
                dtLookupOrganisationalUnitDataTable[0].Mobile,
                dtLookupOrganisationalUnitDataTable[0].TollFreePhone,
                dtLookupOrganisationalUnitDataTable[0].Email,
                1,
                transactionid,
                AppContext.Current.UserName.ToString(),
                DateTime.Now);

            Helper.ExecuteNonQuery(command);
        }
        [MonitoringCallHandler(ComponentType.DataAccess, ModuleID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.OrganisationModuleID,
      FunctionID = HiiP.Framework.Security.UserManagement.Interface.Constants.FunctionNames.UpdateOrganisationFunctionID)]
        public void SaveOrganisationLookup(string OrganisationalUnitID, string OrganisationName, int VersionNumber)
        {

            string transactionid = NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
            DbHelper helper = new DbHelper();
            DbCommand command = helper.BuildDbCommand("P_SS_LookupOrganisationalUnit_U");

            helper.AssignParameterValues(command, Membership.ApplicationName, OrganisationalUnitID, OrganisationName, VersionNumber, transactionid, AppContext.Current.UserName.ToString(), DateTime.Now  );
            Helper.ExecuteNonQuery(command);
        }

        private int? SaveAddress(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable, LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtOrgDetail)
        {
            int? retaddressID = null;
            int versionAddressId = 1;

            var row = dtLookupOrganisationalUnitDataTable[0];
            if (string.IsNullOrEmpty(row.State)
                && string.IsNullOrEmpty(row.UnitNumber)
                && string.IsNullOrEmpty(row.StreetNumber)
                && string.IsNullOrEmpty(row.Street)
                && string.IsNullOrEmpty(row.Type)
                && string.IsNullOrEmpty(row.Suffix)
                && string.IsNullOrEmpty(row.Suburb)
                && string.IsNullOrEmpty(row.PostCode))
            {
                return retaddressID;
            }

            //If need to add address, State cannot be empty
            if (string.IsNullOrEmpty(row.State))
            {
                throw new ArgumentException("State is mandatory when to add address.");
            }

            string address1 = string.Concat(row.UnitNumber,
                                            string.IsNullOrEmpty(row.UnitNumber) &&
                                            string.IsNullOrEmpty(row.StreetNumber)
                                                ? string.Empty
                                                : "/", row.StreetNumber, " ",
                                            row.Street, " ",
                                            row.Type).Trim();

            if(string.IsNullOrEmpty(address1) == false)
            {
                if(address1.EndsWith("/"))
                {
                    address1 = address1.Substring(0, address1.Length - 1);
                }

                if(address1.StartsWith("/"))
                {
                    address1 = address1.Substring(1, address1.Length - 1);
                }

            }

            if (dtOrgDetail.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dtOrgDetail[0].AddressID))
                {
                    retaddressID = IsAddressExist(dtLookupOrganisationalUnitDataTable, address1, out versionAddressId);
                }
                else
                {
                    versionAddressId = GetAddressVersion(Convert.ToInt32(dtOrgDetail[0].AddressID));
                    retaddressID = Convert.ToInt32(dtOrgDetail[0].AddressID.ToString());
                }
            }
            else
                retaddressID = IsAddressExist(dtLookupOrganisationalUnitDataTable, address1, out versionAddressId);

            if (retaddressID == null)
            {
                string transactionid =
                    NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_CM_Address_I");


                helper.AssignParameterValues(command,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.UnitNumber)?null:row.UnitNumber,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.StreetNumber) ? null : row.StreetNumber,
                                             null,
                                             string.IsNullOrEmpty(row.Street) ? null : row.Street,
                                             string.IsNullOrEmpty(row.Type) ? null : row.Type,
                                             string.IsNullOrEmpty(row.Suffix) ? null : row.Suffix,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.Suburb) ? null : row.Suburb,
                                             string.IsNullOrEmpty(row.State) ? null : row.State,
                                             string.IsNullOrEmpty(row.PostCode) ? null : row.PostCode,
                                             string.IsNullOrEmpty(address1.Trim()) ? null : address1.Trim(),
                                             null,
                                             null,
                                             null,
                                             null,
                                             null,
                                             0,
                                             null,
                                             null,
                                             null,
                                             null,
                                             1101,
                                             null,
                                             0,
                                             versionAddressId,
                                             transactionid,
                                             AppContext.Current.UserName.ToString(),
                                             DateTime.Now,
                                             AppContext.Current.UserName.ToString(),
                                             DateTime.Now,
                                             null);



                object objAddID = null;
                
                Helper.ExecuteNonQuery(command);

                objAddID = command.Parameters["@addressid"].Value;
                
                return (int) objAddID;

                
            }

            else
            {
                string transactionid =
                    NCS.IConnect.ApplicationContexts.ApplicationContextFactory.GetApplicationContext().TransactionId;
                DbHelper helper = new DbHelper();
                DbCommand command = helper.BuildDbCommand("P_CM_Address_U");


                helper.AssignParameterValues(command,
                                             retaddressID,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.UnitNumber) ? null : row.UnitNumber,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.StreetNumber) ? null : row.StreetNumber,
                                             null,
                                             string.IsNullOrEmpty(row.Street) ? null : row.Street,
                                             string.IsNullOrEmpty(row.Type) ? null : row.Type,
                                             string.IsNullOrEmpty(row.Suffix) ? null : row.Suffix,
                                             null,
                                             null,
                                             string.IsNullOrEmpty(row.Suburb) ? null : row.Suburb,
                                             string.IsNullOrEmpty(row.State) ? null : row.State,
                                             string.IsNullOrEmpty(row.PostCode) ? null : row.PostCode,
                                             string.IsNullOrEmpty(address1) ? null : address1,
                                             null,
                                             null,
                                             null,
                                             null,
                                             null,
                                             0,
                                             null,
                                             null,
                                             null,
                                             null,
                                             1101,
                                             null,
                                             0,
                                             versionAddressId,
                                             transactionid,
                                             AppContext.Current.UserName.ToString(),
                                             DateTime.Now,
                                             AppContext.Current.UserName.ToString(),
                                             DateTime.Now,
                                             null);


                Helper.ExecuteNonQuery(command);
            }

            return retaddressID;
        }

        private int? IsAddressExist(LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable dtLookupOrganisationalUnitDataTable, string Address1,out int versionAddressId)
        {
            DataTable dtAddress = new DataTable();
            versionAddressId = 0;
            var row = dtLookupOrganisationalUnitDataTable[0];

            this.Helper.Fill(dtAddress, "P_CM_Address_S",
                              null,
                              row.Suburb,
                              row.State,
                              row.PostCode,
                              Address1,
                              null,
                              null
                              );

            if (dtAddress.Rows.Count > 0)
            {
                versionAddressId = Convert.ToInt32(dtAddress.Rows[0]["VersionNo"].ToString());
                return Convert.ToInt32(dtAddress.Rows[0]["AddressID"].ToString());
            }


            return null;

        }

        private int GetAddressVersion(int addressID)
        {
            DataTable dtAddress = new DataTable();
           
            this.Helper.Fill(dtAddress, "P_CM_AddressByID_S",addressID);

            if (dtAddress.Rows.Count > 0)
            {
               return Convert.ToInt32(dtAddress.Rows[0]["VersionNo"].ToString());
            }


            return 0;

        }





    }
}
