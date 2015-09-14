#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  User management/Data access
// COMPONENT DESC :   
//
// CREATED DATE/BY  : 22/09/2008/Lu Ya Ming
//
// REVISION HISTORY: DATE/BY                    SR#/CS/PM#/OTHERS          DESCRIPTION OF CHANGE
// ==================================================================================================

#endregion

using HiiP.Framework.Common.Server;
using HiiP.Framework.Logging.Library;
using HiiP.Framework.Security.UserManagement.BusinessEntity;
using HiiP.Framework.Security.UserManagement.Interface.Constants;



namespace HiiP.Framework.Security.UserManagement.DataAccess
{
    public class UserProfileServiceDA : HiiPDataAccessBase
    {
         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
 FunctionID = FunctionNames.ViewGroupCalendarFunctionID)]
         public AppointmentGroupCalendar GetGroupCalendarsList(string userID)
         {
             AppointmentGroupCalendar ds = new AppointmentGroupCalendar();

             Helper.Fill(ds.SS_AppointmentGroupCalendar, "P_SS_AppointmentGroupCalendar_S_ByUserID", userID);

             return ds;
         }

         [MonitoringCallHandler(ComponentType.DataAccess, Ordinal = 1, ModuleID = FunctionNames.UserModuleID,
FunctionID = FunctionNames.SaveGroupCalendarFunctionID)]
         public void SaveGroupCalendarsList(AppointmentGroupCalendar dsGroupCalendar)
         {
             Helper.Update(dsGroupCalendar.SS_AppointmentGroupCalendar);
         }


    }
}
