using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Cedar.AuditTrail.Interception.Configuration;

namespace Cedar.AuditTrail.Interception
{
    [ConfigurationElementType(typeof(AuditTrailCallHandlerData))]
    public class AuditTrailCallHandler : ICallHandler
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        public string FunctionName { get; private set; }

        /// <summary>
        /// Order in which the handler will be executed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Create a new AuditTrailCallHandler
        /// </summary>
        /// <param name="functionName">The name of the function to audit.</param>
        /// <param name="order">The order in which the handler will be executed.</param>
        public AuditTrailCallHandler(string functionName, int order)
        {
            this.FunctionName = functionName;
            this.Order = order;
        }

        /// <summary>
        /// Create a new AuditTrailCallHandler
        /// </summary>
        /// <param name="functionName">The name of the function to audit.</param>
        public AuditTrailCallHandler(string functionName)
            : this(functionName, 0)
        {
        }

        /// <summary>
        /// Invoking the Audit Trail related operation.
        /// </summary>
        /// <param name="input">Method Invocation Message.</param>
        /// <param name="getNext">A GetNextHandlerDelegate object delegating the invocation to the next CallHandler or Target instance.</param>
        /// <returns>The return message of the method invocation.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result;
            try
            {
                IMethodReturn methodReturn = getNext()(input, getNext);
                if (methodReturn != null)
                {
                    //todo audit
                }
                result = methodReturn;
            }
            catch (Exception ex)
            {
                result = input.CreateExceptionMethodReturn(ex);
            }
            return result;
        }
    }
}
