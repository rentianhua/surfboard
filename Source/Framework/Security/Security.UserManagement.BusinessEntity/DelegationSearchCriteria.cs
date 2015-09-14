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
    public class DelegationSearchCriteria
    {




        [DataMember]
        public int DelegationFunctionId
        {
            get;
            set;
        }

        [DataMember]
        public string AdminNotes
        {
            get;
            set;
        }


        [DataMember]
        public bool IsShowActive
        {
            get;
            set;
        }


    }
}
