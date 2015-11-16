using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Cedar.Framework.AuditTrail.Interception.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;

namespace Cedar.Framework.AuditTrail.Interception
{
    /// <summary>
    /// 
    /// </summary>
    [ConfigurationElementType(typeof(AuditTrailCallHandlerData))]
    public class AuditTrailCallHandler : ICallHandler
    {
        /// <summary>
        ///     Create a new AuditTrailCallHandler
        /// </summary>
        /// <param name="functionName">The name of the function to audit.</param>
        /// <param name="order">The order in which the handler will be executed.</param>
        public AuditTrailCallHandler(string functionName, int order)
        {
            FunctionName = functionName;
            Order = order;
        }

        /// <summary>
        ///     Create a new AuditTrailCallHandler
        /// </summary>
        /// <param name="functionName">The name of the function to audit.</param>
        public AuditTrailCallHandler(string functionName)
            : this(functionName, 0)
        {
            //auditservice = ServiceLocatorFactory.GetServiceLocator().GetService<IAuditTrailManagementService>();
        }

        //private IAuditTrailManagementService auditservice;

        /// <summary>
        ///     Gets or sets the name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        public string FunctionName { get; }

        /// <summary>
        ///     Order in which the handler will be executed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Invoking the Audit Trail related operation.
        /// </summary>
        /// <param name="input">Method Invocation Message.</param>
        /// <param name="getNext">
        ///     A GetNextHandlerDelegate object delegating the invocation to the next CallHandler or Target
        ///     instance.
        /// </param>
        /// <returns>The return message of the method invocation.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result;
            try
            {
                var methodReturn = getNext()(input, getNext);
                if (methodReturn != null)
                {
                    if (methodReturn.Exception == null)
                    {
                        //Task.Run(() => { Log(input, methodReturn); });
                    }
                }
                result = methodReturn;
            }
            catch (Exception ex)
            {
                result = input.CreateExceptionMethodReturn(ex);
            }
            return result;
        }

        private void Log(IMethodInvocation input, IMethodReturn methodReturn)
        {
            var auditLogger = AuditLogger.CreateAuditLogger(FunctionName);
            auditLogger.Write(input.MethodBase.Name, string.Empty,
               JsonConvert.SerializeObject(input.Arguments),
               JsonConvert.SerializeObject(methodReturn.ReturnValue));
            auditLogger.Flush();
        }
    }
}