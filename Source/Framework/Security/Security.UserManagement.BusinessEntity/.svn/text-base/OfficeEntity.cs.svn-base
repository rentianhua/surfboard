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
    public class OfficeEntity
    {
        public OfficeEntity()
        { 
        }

        public OfficeEntity(string officeID, string userName, string isMaster, string office, DateTime effectiveDate, string officeRecordStatus)
        {
            OfficeID = officeID;
            UserName = userName;
            IsMaster = isMaster;
            Office = office;
            EffectiveDate = effectiveDate;
            OfficeRecordStatus = officeRecordStatus;
        }

        [DataMember]
        public string OfficeID
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
        public string IsMaster
        {
            get;
            set;
        }

        [DataMember]
        public string Office
        {
            get;
            set;
        }

        [DataMember]
        public DateTime EffectiveDate
        {
            get;
            set;
        }

        [DataMember]
        public string OfficeRecordStatus
        {
            get;
            set;
        }

        
    }
}
