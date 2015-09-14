#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
// SYSTEM NAME      : Housing Integrated Information Program
// COMPONENT ID     : Settings/CodeTableMaintainPresenter
// COMPONENT DESC   :  
// CREATED DATE/BY  : 25/08/2008/Mei Bo
// REVISION HISTORY : DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================
#endregion

using System.Data;
using System.Collections.Generic;
using NCS.IConnect.CodeTable;
using HiiP.Framework.Settings.BusinessEntity;

namespace HiiP.Framework.Settings
{
    public interface ICodeTableMaintain
    {
        /// <summary>
        /// Bind data to grid after search
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="needMerge"></param>
        void BindCodeTable(CodeTableDataSet.T_IC_CODEDataTable codes, bool needMerge);

        /// <summary>
        /// Bind category data to category dropdownlist
        /// </summary>
        /// <param name="listCategory"></param>
        void BindCategory(List<string> listCategory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        void SetSelectedCategory(string category);
    }
}

