#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Yang Jian Hua
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using HiiP.Framework.Logging.BusinessEntity;
namespace HiiP.Framework.Logging
{
    public interface IUsageView
    {
        LoggingUsageDataSet.T_IC_LOGGING_USAGERow CurrentRow
        {
            get;
            set;
        }

        LoggingUsageDataSet LoggingUsageData
        {
            get;
            set;
        }
    }
}

