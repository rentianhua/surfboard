#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// SYSTEM NAME      : Housing Integrated Information Program
// COMPONENT ID     : Settings/MessageMaintainPresenter
// COMPONENT DESC   :  
// CREATED DATE/BY  : 05/09/2008/Mei Bo
// REVISION HISTORY : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using NCS.IConnect.Messaging;

namespace HiiP.Framework.Settings
{
    public interface IMessageDetailView
    {
        /// <summary>
        /// Bind severity data to Severity dropdownlist
        /// </summary>
        void BindSeverity(string[] severities);

        /// <summary>
        /// Bind category data to Category dropdownlist
        /// </summary>
        void BindCategory(List<string> categories);

        /// <summary>
        /// Bind update data on UI
        /// </summary>
        /// <param name="message"></param>
        void BindUpdateData(DataRow message);
        
        void Authorize(bool canRead, bool canWrite);
    }
}

