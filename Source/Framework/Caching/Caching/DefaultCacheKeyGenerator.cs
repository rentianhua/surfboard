using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cedar.Framwork.Caching
{

    public class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        private readonly Guid KeyGuid = new Guid("ECFD1B0F-0CBA-4AA1-89A0-179B636381CA");

        /// <summary>
        /// Create a cache key for the given method and set of input arguments.
        /// </summary>
        /// <param name="method">Method being called.</param>
        /// <param name="inputs">Input arguments.</param>
        /// <returns>A (hopefully) unique string to be used as a cache key.</returns>
        public string CreateCacheKey(MethodBase method, params object[] inputs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}:", this.KeyGuid);
            if (method.DeclaringType != null)
            {
                stringBuilder.Append(method.DeclaringType.FullName);
            }
            stringBuilder.Append(':');
            stringBuilder.Append(method.Name);
            if (inputs != null)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    object obj = inputs[i];
                    stringBuilder.Append(':');
                    if (obj != null)
                    {
                        stringBuilder.Append(obj.GetHashCode().ToString());
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
