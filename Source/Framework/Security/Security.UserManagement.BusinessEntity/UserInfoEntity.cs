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
using System.Xml;
using System.Xml.Linq;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class UserInfoEntity
    {
        public UserInfoEntity() { }

        public UserInfoEntity(
            string userName,
            string firstName,
            string initials,
            string lastName,
            string display,
            string alias,
            string gender,
            string title,
            DateTime dateOfBirth,
            string email,
            string telephoneNo,
            string faxNo,
            string mobileNo,
            string pageNo,
            //string organisation,
            string remarks,
            string status,
            DateTime createdOn,
            string isMaster,
            string office,
            string allOffices,
            string jobTitle,
            string branch,
            string unit,
            string subunit,
            string grade,
            string reportsTo
            )
        {
            UserName = userName;
            FirstName = firstName;
            Initials = initials;
            LastName = lastName;
            Display = display;
            Alias = alias;
            Gender = gender;
            Title = title;
            DateOfBirth = dateOfBirth;
            Email = email;
            TelephoneNo = telephoneNo;
            FaxNo = faxNo;
            MobileNo = mobileNo;
            PageNo = pageNo;
            //Organisation = organisation;
            Remarks = remarks;
            UserStatus = status;
            CreatedOn = createdOn;
            IsMaster = isMaster;
            Office = office;
            AllOffices = allOffices;
            JobTitle = jobTitle;
            Branch = branch;
            Unit = unit;
            SubUnit = subunit;
            Grade = grade;
            ReportsTo = reportsTo;
        }

        [DataMember]
        public string UserName
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
        public string Initials
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
        public string Display
        {
            get;
            set;
        }

        [DataMember]
        public string Alias
        {
            get;
            set;
        }

        [DataMember]
        public string Gender
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
        public DateTime DateOfBirth
        {
            get;
            set;
        }

        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [DataMember]
        public string TelephoneNo
        {
            get;
            set;
        }

        [DataMember]
        public string FaxNo
        {
            get;
            set;
        }

        [DataMember]
        public string MobileNo
        {
            get;
            set;
        }

        [DataMember]
        public string PageNo
        {
            get;
            set;
        }

        //[DataMember]
        //public string Organisation
        //{
        //    get;
        //    set;
        //}

        [DataMember]
        public string Remarks
        {
            get;
            set;
        }

        [DataMember]
        public string UserStatus
        {
            get;
            set;
        }

        [DataMember]
        public DateTime CreatedOn
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
        public string AllOffices
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

        //[DataMember]
        //public XElement ConfigurationInfo
        //{
        //    get;
        //    set;
        //}

        [DataMember]
        public int VersionNo
        {
            get;
            set;
        }

        [DataMember]
        public string JobTitle
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
        public int? EPRIN { get; set; }

        [DataMember]
        public bool IsInternal { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string SubUnit { get; set; }

        [DataMember]
        public string Grade { get; set; }


    }
}
