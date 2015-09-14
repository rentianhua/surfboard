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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace HiiP.Framework.Logging.Library
{
    [Assembler(typeof(MonitoringCallHandlerAssembler))]
    public class MonitoringCallHandlerData : LogCallHandlerData
    {
        #region Variable

        private const string ModuleIdPropertyName = "moduleId";
        private const string FunctionIdPropertyName = "functionId";
        private const string ComponentPropertyName = "component";
        private const string OrdinalPropertyName = "ordinal";

        #endregion
       

        #region Properties
        [ConfigurationProperty(ModuleIdPropertyName, DefaultValue = "", IsRequired = false)]
        public string ModuleId
        {
            get { return (string)base[ModuleIdPropertyName]; }
            set { base[ModuleIdPropertyName] = value; }
        }

        [ConfigurationProperty(FunctionIdPropertyName, DefaultValue = "", IsRequired = false)]
        public string FunctionId
        {
            get { return (string)base[FunctionIdPropertyName]; }
            set { base[FunctionIdPropertyName] = value; }
        }

        [ConfigurationProperty(ComponentPropertyName, IsRequired = true)]
        public ComponentType Component
        {
            get { return (ComponentType)base[ComponentPropertyName]; }
            set { base[ComponentPropertyName] = value; }
        }
        [ConfigurationProperty(OrdinalPropertyName, DefaultValue = 0, IsRequired = false)]
        int Ordinal {
            get { return (int)base[OrdinalPropertyName]; }
            set { base[OrdinalPropertyName] = value; }
        }

        #endregion

        public class MonitoringCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
        {
            public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration,
                                      IConfigurationSource configurationSource,
                                      ConfigurationReflectionCache reflectionCache)
            {
                MonitoringCallHandlerData handlerData = objectConfiguration as MonitoringCallHandlerData;
                if (handlerData==null)
                {
                    return new MonitoringCallHandler();
                }
                MonitoringCallHandler callHandler = new MonitoringCallHandler(configurationSource);
                //switch (handlerData.LogBehavior)
                //{
                //    case HandlerLogBehavior.Before:
                //        callHandler.LogBeforeCall = true;
                //        callHandler.LogAfterCall = false;
                //        break;

                //    case HandlerLogBehavior.After:
                //        callHandler.LogBeforeCall = false;
                //        callHandler.LogAfterCall = true;
                //        break;

                //    case HandlerLogBehavior.BeforeAndAfter:
                //        callHandler.LogBeforeCall = true;
                //        callHandler.LogAfterCall = true;
                //        break;
                //}

                //callHandler.BeforeMessage = handlerData.BeforeMessage;
                //callHandler.AfterMessage = handlerData.AfterMessage;
                //callHandler.IncludeCallStack = handlerData.IncludeCallStack;
                //callHandler.IncludeCallTime = handlerData.IncludeCallTime;
                //callHandler.IncludeParameters = handlerData.IncludeParameterValues;
                callHandler.ModuleId = handlerData.ModuleId;
                callHandler.FunctionId = handlerData.FunctionId;
                callHandler.Ordinal = handlerData.Ordinal;

                //callHandler.Categories.Clear();

                //handlerData.Categories.ForEach(
                //    delegate(LogCallHandlerCategoryEntry entry)
                //    {
                //        callHandler.Categories.Add(entry.Name);
                //    }
                //);

                return callHandler;
            }
        }
    }
}
