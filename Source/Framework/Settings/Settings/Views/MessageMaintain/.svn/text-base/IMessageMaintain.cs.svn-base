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
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using NCS.IConnect.Messaging;

namespace HiiP.Framework.Settings
{
    public interface IMessageMaintain
    {
        /// <summary>
        /// Bind severity data to Severity dropdownlist
        /// </summary>
        void BindSeverity(string[] severities);

        /// <summary>
        /// Bind severity data to Category dropdownlist
        /// </summary>
        void BindCategory(string[] categories);

        /// <summary>
        /// Bind data to UltraGrid
        /// </summary>
        void BindUltraGrid(DataTable messages, bool needMerge);
    }
}

