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
    public class FunctionAndDataFilterEntity
    {
        public FunctionAndDataFilterEntity()
        {
        }

        public FunctionAndDataFilterEntity(
            string actionCode,            
            string dataFilter            )
        {
            ActionCode = actionCode;           
            DataFilter = dataFilter;
        }

        [DataMember]
        public string ActionCode
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
    }
}
