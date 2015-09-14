#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME    :  Housing Integrated Information Program
// COMPONENT ID   :  User management/Business entity
// COMPONENT DESC :   
//
// CREATED DATE/BY:  
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
    public class ETRoleEntity
    {
        public ETRoleEntity() { }

        public ETRoleEntity(int roleId,
            string roleName,
            string description,
            string etType,
            string userId
            )
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            EtType = etType;
            UserId = userId;
        }
        [DataMember]
        public int RoleId
        { get; set; }

        [DataMember]
        public string RoleName
        { get; set; }

        [DataMember]
        public string Description
        { get; set; }

        [DataMember]
        public string EtType
        { get; set; }

        [DataMember]
        public int VersionNo
        { get; set; }

        [DataMember]
        public string UserId
        { get; set; }
    }
}
