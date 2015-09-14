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
using System.Text;

namespace HiiP.Framework.Logging.Interface.Constants
{
    public static class ContractNames
    {
        public const string ContractNamespaceConsts = "http://HiiP.NET/WCF";

        #region Nested type: DataContractNames

        public class DataContractNames
        {
            public const string CommonData = "CommonData";

            public const string LoggingData = "LoggingData";
        }

        #endregion

        #region Nested type: ServiceContractNames

        public class ServiceContractNames
        {
            public const string LoggingViewService = "ILoggingViewService";
        }

        #endregion
    }
}
