

namespace LL.POS.Common
{
    using System;
    using LL.Utility;
    /// <summary>
    /// 日志记录公用工具
    /// </summary>
    public class LoggerHelper
    {
        /// <summary>
        /// 根据日志类型记录到不同的日志文件中
        /// </summary>
        /// <param name="loggerName">日志记录使用对象名称</param>
        /// <param name="message">要记录日志信息</param>
        /// <param name="logEnum">日志类型</param>
        public static void Log(String loggerName, String message, LogEnum logEnum)
        {
            LoggerUtility.Instance.WriteLog(loggerName, message, (int)logEnum);
        }
    }
}
