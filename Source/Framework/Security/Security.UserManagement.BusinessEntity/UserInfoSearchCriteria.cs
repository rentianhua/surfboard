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
using System.Globalization;
using System.Threading;
using HiiP.Framework.Common.Constants;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class UserInfoSearchCriteria
    {
        private string _UserName = String.Empty;
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _UserStatus = String.Empty;
        [DataMember]
        public string UserStatus
        {
            get { return _UserStatus; }
            set { _UserStatus = value; }
        }

        private DateTime _CreatedFrom = MinMaxValues.MinDate;
        [DataMember]
        public DateTime CreatedFrom
        {
            get { return _CreatedFrom; }
            set { _CreatedFrom = value; }
        }

        private DateTime _CreatedTo = MinMaxValues.MaxDate;
        [DataMember]
        public DateTime CreatedTo
        {
            get { return _CreatedTo; }
            set { _CreatedTo = value; }
        }

        private string _Display = String.Empty;
        [DataMember]
        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }

        private string _Email = String.Empty;
        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private bool? _userType ;
        [DataMember]
        public bool? UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        private string _Office = String.Empty;
        [DataMember]
        public string Office
        {
            get { return _Office; }
            set { _Office = value; }
        }
    }
}
