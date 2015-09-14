using Microsoft.Practices.Unity.Utility;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Cedar.Core.EntLib.IoC
{
    public class FixedAutoInterceptorPolicy : AutoInterceptorPolicy
    {
        private IInstanceInterceptor interceptor;

        /// <summary>
        /// Gets the <see cref="T:Microsoft.Practices.Unity.InterceptionExtension.IInstanceInterceptor" />.
        /// </summary>
        public override IInstanceInterceptor Interceptor
        {
            get
            {
                return this.interceptor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interceptor"></param>
        public FixedAutoInterceptorPolicy(IInstanceInterceptor interceptor)
        {
            Guard.ArgumentNotNull(interceptor, "interceptor");
            this.interceptor = interceptor;
        }
    }
}
