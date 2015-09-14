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
    public class RoleEntity
    {
        public RoleEntity() { }

        public RoleEntity(
            string roleName,
            string description,
            string status
            )
        {
            RoleName = roleName;
            Description = description;
            Status = status;
        }
        [DataMember]
        public string RoleName
        { get; set; }

        [DataMember]
        public string Description
        { get; set; }

        [DataMember]
        public string Status
        { get; set; }

        [DataMember]
        public int VersionNo
        { get; set; }
    }
}
