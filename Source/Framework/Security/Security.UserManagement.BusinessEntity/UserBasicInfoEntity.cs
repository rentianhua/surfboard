#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME          :  Housing Integrated Information Program
// COMPONENT ID         :  User management/Business entity
// COMPONENT DESC       :   
//
// CREATED DATE/BY  : 30/04/2009 Anton Fernando
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class UserBasicInfoEntity
    {

        public UserBasicInfoEntity() { }


        public UserBasicInfoEntity(
            string userName,
            string display,
            string title,
            string firstName,
            string lastName,
            string office,
            string nameTitle)
        {
            UserName = userName;
            Display = display;
            Title = title;
            Office = office;
            FirstName = firstName;
            LastName = lastName;
            NameTitle = nameTitle;

        }


        public UserBasicInfoEntity(
            string userName,
            string display,
            string title)
        {
            UserName = userName;
            Display = display;
            Title = title;

        }


        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        [DataMember]
        public string Display
        {
            get;
            set;
        }

        [DataMember]
        public string Title
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
        public string FirstName
        {
            get;
            set;
        }

        [DataMember]
        public string LastName
        {
            get;
            set;
        }

        [DataMember]
        public string NameTitle
        {
            get;
            set;
        }
        [DataMember]
        public string Status
        { get; set; }


        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [DataMember]
        public string ReportsTo
        {
            get;
            set;
        }
        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string SubUnit { get; set; }
    }
}
