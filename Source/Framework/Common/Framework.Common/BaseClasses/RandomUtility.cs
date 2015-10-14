using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    /// 生成随机数
    /// </summary>
    public class RandomUtility
    {
        /// <summary>
        /// 生成纯数字的随机码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandom(int length)
        {
            var sequence = new int[length];
            var output = new int[length];

            for (var i = 0; i < 10; i++)
            {
                sequence[i] = i;
            }

            //用GUID的hashcode不会出现重复数
            var ticks = Guid.NewGuid().GetHashCode();
            //var ticks = DateTime.Now.Ticks;

            var random = new Random(ticks);

            var end = length - 1;

            for (var i = 0; i < length; i++)
            {
                var num = random.Next(0, end + 1);
                output[i] = sequence[num];
                sequence[num] = sequence[end];
                end--;
            }

            return output.JoinStrings("");
        }
    }
}
