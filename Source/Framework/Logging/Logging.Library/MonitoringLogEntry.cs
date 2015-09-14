#region Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.

// ==================================================================================================
// Copyright(C) 2008 NCS Pte. Ltd. All rights reserved.
//
// SYSTEM NAME         :  Housing Integrated Information Program
// COMPONENT ID      :  Logging/Library
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
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Logging;

namespace HiiP.Framework.Logging.Library
{
    [Serializable]
    public class MonitoringLogEntry : TraceLogEntry
    {
        #region Variable
        private string extendedActivityId;

        private Nullable<long> tracingStartTicks;
        private Nullable<long> tracingEndTicks;
        private Nullable<decimal> secondsElapsed;

        private string userID;
        private string userName;
        private string ipAddress;
        private string userRoles;
        private string userGraphicArea;
        private string moduleId;
        private string functionId;
        private string office;
        private int flag;

        private ComponentType component;

        private string parameterValues;

        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        public MonitoringLogEntry()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="title"></param>
        /// <param name="properties"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="extendedActivityId"></param>
        /// <param name="tracingStartTicks"></param>
        /// <param name="tracingEndTicks"></param>
        /// <param name="secondsElapsed"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        /// <param name="userRoles"></param>
        /// <param name="userGraphicArea"></param>
        /// <param name="moduleId"></param>
        /// <param name="functionId"></param>
        /// <param name="component"></param>
        /// <param name="office"></param>
        public MonitoringLogEntry(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties, string typeName, string methodName, string extendedActivityId, Nullable<long> tracingStartTicks, Nullable<long> tracingEndTicks, Nullable<decimal> secondsElapsed, string userName, string ipAddress, string userRoles, string userGraphicArea, string moduleId, string functionId, ComponentType component, string office)
            : base(message, category, priority, eventId, severity, title, properties, typeName, methodName)
        {
            this.extendedActivityId = extendedActivityId;
            this.tracingStartTicks = tracingStartTicks;
            this.tracingEndTicks = tracingEndTicks;
            this.secondsElapsed = secondsElapsed;
            this.userName = userName;
            this.ipAddress = ipAddress;
            this.userRoles = userRoles;
            this.userGraphicArea = userGraphicArea;
            //this.organization = organization;
            this.moduleId = moduleId;
            this.functionId = functionId;
            this.component = component;
            this.office = office;
        }

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public Nullable<long> TracingStartTicks
        {
            get { return tracingStartTicks; }
            set { tracingStartTicks = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<long> TracingEndTicks
        {
            get { return tracingEndTicks; }
            set { tracingEndTicks = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> SecondsElapsed
        {
            get { return secondsElapsed; }
            set { secondsElapsed = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserRoles
        {
            get { return userRoles; }
            set { userRoles = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserGraphicArea
        {
            get { return userGraphicArea; }
            set { userGraphicArea = value; }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //public string Organization
        //{
        //    get { return organization; }
        //    set { organization = value; }
        //}

        /// <summary>
        /// 
        /// </summary>
        public string Office
        {
            get { return office; }
            set { office = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FunctionId
        {
            get { return functionId; }
            set { functionId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExtendedActivityId
        {
            get { return extendedActivityId; }
            set { extendedActivityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentType Component
        {
            get { return component; }
            set { component = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterValues
        {
            get { return parameterValues; }
            set { parameterValues = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        //Added field for exception log
        public string InstanceID
        {
            get;
            set;
        }

        #endregion
    }
}
