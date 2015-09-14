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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using NCS.IConnect.PolicyInjection.CallHandlers;
using System.Diagnostics;
//using HiiP.Framework.Logging.Interface.Constants;

namespace HiiP.Framework.Logging.Library
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class MonitoringCallHandlerAttribute : HandlerAttributeBase
    {
        #region Variable
        //private bool logBeforeCall = true;
        //private bool logAfterCall = true;
        //private string beforeMessage = string.Empty;
        //private string afterMessage = string.Empty;
        //private bool includeParameters = true;
        //private bool includeCallStack ;
        //private bool includeCallTime = true;

        //private string[] categories = new string[0];

        private string moduleId;
        private string functionId;
        private ComponentType component;

        #endregion

        #region Construction
        public MonitoringCallHandlerAttribute(ComponentType component)
        {
            this.component = component;
        }
        #endregion

        #region Properties
        ///// <summary>
        ///// Gets or sets the collection of categories to place the log entries into.
        ///// </summary>
        ///// <remarks>The category strings can include replacement tokens. See
        ///// the <see cref="Logging.CategoryFormatter"/> class for the list of tokens.</remarks>
        ///// <value>The list of category strings.</value>
        //public string[] Categories
        //{
        //    get { return categories; }
        //    set { categories = value; }
        //}


        ///// <summary>
        ///// Should there be a log entry before calling the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool LogBeforeCall
        //{
        //    get { return logBeforeCall; }
        //    set { logBeforeCall = value; }
        //}

        ///// <summary>
        ///// Should there be a log entry after calling the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool LogAfterCall
        //{
        //    get { return logAfterCall; }
        //    set { logAfterCall = value; }
        //}

        ///// <summary>
        ///// Message to include in a pre-call log entry.
        ///// </summary>
        ///// <value>The message</value>
        //public string BeforeMessage
        //{
        //    get { return beforeMessage; }
        //    set { beforeMessage = value; }
        //}

        ///// <summary>
        ///// Message to include in a post-call log entry.
        ///// </summary>
        ///// <value>the message.</value>
        //public string AfterMessage
        //{
        //    get { return afterMessage; }
        //    set { afterMessage = value; }
        //}

        ///// <summary>
        ///// Should the log entry include the parameters to the call?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeParameters
        //{
        //    get { return includeParameters; }
        //    set { includeParameters = value; }
        //}

        ///// <summary>
        ///// Should the log entry include the call stack?
        ///// </summary>
        ///// <remarks>Logging the call stack requires full trust code access security permissions.</remarks>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeCallStack
        //{
        //    get { return includeCallStack; }
        //    set { includeCallStack = value; }
        //}

        ///// <summary>
        ///// Should the log entry include the time to execute the target?
        ///// </summary>
        ///// <value>true = yes, false = no</value>
        //public bool IncludeCallTime
        //{
        //    get { return includeCallTime; }
        //    set { includeCallTime = value; }
        //}



        public string ModuleID
        {
            get { return moduleId; }
            set { moduleId = value; }
        }


        public string FunctionID
        {
            get { return functionId; }
            set { functionId = value; }
        }

        public ComponentType Component
        {
            get { return component; }
            set { component = value; }
        }

        #endregion

        #region ICallHandler method
        public override ICallHandler CreateHandler()
        {
            MonitoringCallHandler handler = new MonitoringCallHandler();
            //SetCategories(handler);
            //handler.LogAfterCall = logAfterCall;
            //handler.LogBeforeCall = logBeforeCall;
            //handler.BeforeMessage = beforeMessage;
            //handler.AfterMessage = afterMessage;
            //handler.IncludeParameters = includeParameters;
            //handler.IncludeCallStack = includeCallStack;
            //handler.IncludeCallTime = includeCallTime;
            handler.ModuleId = moduleId;
            handler.FunctionId = functionId;
            handler.Component = component;

            return handler;
        }
        #endregion

        #region Private method
        //private void SetCategories(MonitoringCallHandler handler)
        //{
        //    handler.Categories.Clear();
        //    //When the user haven't set the categories, we will use the default one
        //    //If you set the categories, will overwrite the default one
        //    if (Categories.Length == 0)
        //    {
        //        //modified by xiaofeng. To use const instead of hard code.
        //        handler.Categories.AddRange(new string[] { LoggingCategories.Monitoring });
        //    }
        //    else
        //    {
        //        handler.Categories.AddRange(Categories);
        //    }
        //}
        #endregion
    }
}
