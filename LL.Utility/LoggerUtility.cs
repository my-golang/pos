namespace LL.Utility
{
    using System;
    using log4net;
    /// <summary>
    /// 日志记录工具类
    /// </summary>
    public class LoggerUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static LoggerUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private LoggerUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static LoggerUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new LoggerUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 变量声明
        static ILog logger = null;
        #endregion
        #region 私有方法
        void GetLogger(String loggerName)
        {
            if (logger == null)
            {
                logger = log4net.LogManager.GetLogger(loggerName);
            }
        }
        #endregion
        #region 对外公开方法
        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="String">使用日志记录类型名称</param>
        /// <param name="ex">异常对象</param>
        /// <param name="exType">异常类型</param>
        /// <param name="mark">记录类型：1致命错误2错误3警告4信息5调试</param>
        public void WriteLog(String loggerName, Exception ex, String exType, Int32 mark)
        {
            GetLogger(loggerName);
            switch (mark)
            {
                case 1:
                    logger.Fatal(exType, ex);
                    break;
                case 2:
                    logger.Error(exType, ex);
                    break;
                case 3:
                    logger.Warn(exType, ex);
                    break;
                case 5:
                    logger.Debug(exType, ex);
                    break;
                default:
                    logger.Info(exType, ex);
                    break;
            }
        }
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="String">使用日志记录类型名称</param>
        /// <param name="msg">需记录信息</param>
        /// <param name="mark">记录类型：1致命错误2错误3警告4信息5调试</param>
        public void WriteLog(String loggerName, string msg, Int32 mark)
        {
            GetLogger(loggerName);
            switch (mark)
            {
                case 1:
                    logger.Fatal(msg);
                    break;
                case 2:
                    logger.Error(msg);
                    break;
                case 3:
                    logger.Warn(msg);
                    break;
                case 5:
                    logger.Debug(msg);
                    break;
                default:
                    logger.Info(msg);
                    break;
            }
        }
        #endregion
    }
}
