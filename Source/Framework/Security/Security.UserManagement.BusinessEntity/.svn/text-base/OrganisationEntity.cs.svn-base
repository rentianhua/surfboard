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

using System.Runtime.Serialization;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class OrganisationEntity
    {
        public OrganisationEntity()
        {
        }

        public OrganisationEntity(string organisationName, string organisationDescription)
        {
            OrganisationName = organisationName;
            OrganisationDescription = organisationDescription;
        }

        [DataMember]
        public string OrganisationName
        {
            get;
            set;
        }

        [DataMember]
        public string OrganisationDescription
        {
            get;
            set;
        }

        [DataMember]
        public int VersionNo
        {
            get;
            set;
        }
    }
}