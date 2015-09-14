using System.Runtime.Serialization;
using System;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class DelegationValueEntity 
    {
        public DelegationValueEntity() { }


        public DelegationValueEntity(int delegationFunctionID,
                                    string branch,
                                    string unit,
                                    string subunit,
                                    string grade,
                                    string act,
                                    string delegationValue,
                                    string policyValue,
                                    string adminNotes,
                                    DateTime startDate,
                                    DateTime endDate)
        {
            DelegationFunctionId = delegationFunctionID;
            Branch = branch;
            Unit = unit;
            Subunit = subunit;
            Grade = grade;
            Act = act;
            DelegationValue = delegationValue;
            PolicyValue = policyValue;
            DelegationReference = adminNotes;
            StartDate = startDate;
            EndDate = endDate;
        }

        [DataMember]
        public int DelegationValueId
        {
            get;
            set;
        }


        [DataMember]
        public int DelegationFunctionId
        {
            get;
            set;
        }

        [DataMember]
        public string DelegationFunctionName
        {
            get;
            set;
        }

        [DataMember]
        public string Branch
        {
            get;
            set;
        }

        [DataMember]
        public string Unit
        {
            get;
            set;
        }

        [DataMember]
        public string Subunit
        {
            get;
            set;
        }


        [DataMember]
        public string Grade
        {
            get;
            set;
        }


        [DataMember]
        public string Act
        {
            get;
            set;
        }

        [DataMember]
        public string DelegationValue
        {
            get;
            set;
        }

        [DataMember]
        public string PolicyValue
        {
            get;
            set;
        }

        [DataMember]
        public string OperationalCondition
        {
            get;
            set;
        }


        [DataMember]
        public string DelegationReference
        {
            get;
            set;
        }


        [DataMember]
        public DateTime StartDate
        {
            get;
            set;
        }


        [DataMember]
        public DateTime EndDate
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
