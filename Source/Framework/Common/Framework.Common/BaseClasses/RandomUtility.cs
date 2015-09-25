using System;

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
            var rad = new Random();//实例化随机数产生器rad；

            var init = 1;
            for (var i = 1; i < length; i++)
            {
                init = init * 10;
            }

            var value = rad.Next(init, init * 10);

            return value.ToString();
        }
    }
}
