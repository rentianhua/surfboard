#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Business entity
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
using System.Runtime.Serialization;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class DataFilterEntity
    {
        public DataFilterEntity()
        {
        }

        public DataFilterEntity(
            string userDataFilterValueID, 
            string userName, 
            string roleID, 
            string roleName, 
            string dataFilterID, 
            string dataFilter, 
            string dataFilterValueID, 
            string dataFilterValue, 
            string dataFilterRecordStatus
            )
        {
            UserDataFilterValueID = userDataFilterValueID;
            UserName = userName;
            RoleID = roleID;
            RoleName = roleName;
            DataFilterID = dataFilterID;
            DataFilter = dataFilter;
            DataFilterValueID = dataFilterValueID;
            DataFilterValue = dataFilterValue;
            RecordStatus = dataFilterRecordStatus;
        }

        [DataMember]
        public string UserDataFilterValueID
        {
            get;
            set;
        }

        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        [DataMember]
        public string RoleID
        {
            get;
            set;
        }

        [DataMember]
        public string RoleName
        {
            get;
            set;
        }

        [DataMember]
        public string DataFilterID
        {
            get;
            set;
        }

        [DataMember]
        public string DataFilter
        {
            get;
            set;
        }

        [DataMember]
        public string DataFilterValueID
        {
            get;
            set;
        }

        [DataMember]
        public string DataFilterValue
        {
            get;
            set;
        }

        [DataMember]
        public string RecordStatus
        {
            get;
            set;
        }
    }
}
