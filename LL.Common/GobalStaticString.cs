namespace LL.Common
{
    using System;
    /// <summary>
    /// 全局静态字符定义
    /// </summary>
    public class GobalStaticString
    {
        /// <summary>
        /// 默认检测失败的DISK的值
        /// </summary>
        public readonly static String UN_KNOWN_DISK = "UNKOWN_DISK";

        /// <summary>
        /// 默认检测部到MAC地址的值
        /// </summary>
        public readonly static String UN_KNOWN_MAC = "UNKOWN_MAC";
        /// <summary>
        /// 默认负的数字字符串
        /// </summary>
        public readonly static String DEFAULT_SUB_NUMBER = "-1";
        /// <summary>
        /// 默认数字1的字符串标示
        /// </summary>
        public readonly static String DEFAULT_NUMBER_ONE = "1";
        /// <summary>
        /// SQLLITEDB连接字符串
        /// </summary>
        public readonly static String SQLITE_DB_STRING = "Data Source=%path%;Emulator=True;Encoding=UTF8;";
        /// <summary>
        /// 日期时间格式格式化串
        /// </summary>
        public readonly static String DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 日期格式格式化串
        /// </summary>
        public readonly static String DATE_FORMAT = "yyyy-MM-dd";
        /// <summary>
        /// MYSQL连接字符串//Allow Zero Datetime=True用于解决Unable to convert MySQL date/time value to System.DateTime
        /// </summary>
        public readonly static string MYSQL_DB_STRING = "server=%server%;uid=%user%;password=%password%;database=%database%;Allow Zero Datetime=True";//
    }
}
