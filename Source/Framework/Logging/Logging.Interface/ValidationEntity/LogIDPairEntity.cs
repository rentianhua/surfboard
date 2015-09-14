#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Interface
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HiiP.Framework.Logging.Interface.ValidationEntity
{
    [DataContract(Namespace="http://hiip.ooh.dhs.vic.gov.au/")]
    public class LogIDPairEntity
    {
        public LogIDPairEntity()
        {
            MinLogID = 0;
            MaxLogID = 0;
        }

        public LogIDPairEntity(Int64 minLogID, Int64 maxLogID)
        {
            MinLogID = minLogID;
            MaxLogID = maxLogID;
        }

        [DataMember]
        public Int64 MinLogID
        {
            get;
            set;
        }

        [DataMember]
        public Int64 MaxLogID
        {
            get;
            set;
        }
    }
}
