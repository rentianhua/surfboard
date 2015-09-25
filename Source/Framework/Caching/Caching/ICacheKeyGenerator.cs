using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framwork.Caching
{
    public interface ICacheKeyGenerator
    {
        /// <summary>
		/// Creates a cache key for the given method and set of input arguments.
		/// </summary>
		/// <param name="method">Method being called.</param>
		/// <param name="inputs">Input arguments.</param>
		/// <returns>A (hopefully) unique string to be used as a cache key.</returns>
		string CreateCacheKey(MethodBase method, object[] inputs);
    }
}
