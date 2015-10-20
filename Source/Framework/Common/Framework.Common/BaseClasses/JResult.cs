namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    ///     接口返回格式
    /// </summary>
    public class JResult
    {
        /// <summary>
        ///     操作状态码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        ///     结果类型
        /// </summary>
        public object errmsg { get; set; }
    }
}