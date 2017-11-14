

namespace LL.POS.Transfer
{
    using System;
    /// <summary>
    /// 全局设置内容
    /// </summary>
    public static class GlobalSet
    {
        private static String _appname = "POS数据传输";
        /// <summary>
        ///应用程序文件
        /// </summary>
        public static String appname
        {
            get
            {
                return _appname;
            }
            set
            {
                _appname = value;
            }
        }

        private static String _appinifile = "transfer.ini";
        /// <summary>
        /// appini文件名称
        /// </summary>
        public static String appinifile
        {
            get
            {
                return _appinifile;
            }
            set
            {
                _appinifile = value;
            }
        }

        private static String _posServiceUrl = "http://192.168.1.36/";
        /// <summary>
        /// posservice服务url
        /// </summary>
        public static String posserviceurl
        {
            get
            {
                return _posServiceUrl;
            }
            set
            {
                _posServiceUrl = value;
            }
        }

        private static String _posServiceName = "ws/PosServices.asmx";
        /// <summary>
        /// posservice服务名称
        /// </summary>
        public static String posservicename
        {
            get
            {
                return _posServiceName;
            }
            set
            {
                _posServiceName = value;
            }
        }

        private static String _dbsaleconnection;
        /// <summary>
        /// 销售数据库连接字符串
        /// </summary>
        public static String dbsaleconn
        {
            get
            {
                return _dbsaleconnection;
            }
            set
            {
                _dbsaleconnection = value;
            }
        }

        private static string _connect_string;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectString
        {
            get { return _connect_string; }
            set { _connect_string = value; }
        }

        private static string _client_id = "POS";
        /// <summary>
        /// 接口访问参数
        /// </summary>
        public static string client_id
        {
            get { return _client_id; }
            set { _client_id = value; }
        }
        private static string _access_token = "c2e3c130b7040fbe18e7f9b319844b42558aeb34";
        /// <summary>
        /// 接口访问秘钥
        /// </summary>
        public static string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        private static string _serverUrl = "";
        /// <summary>
        /// 接口服务URL
        /// </summary>
        public static string serverUrl
        {
            get { return _serverUrl; }
            set { _serverUrl = value; }
        }


        private static string _memberUrl;
        /// <summary>
        /// 会员接口服务地址
        /// </summary>
        public static string memberUrl
        {
            get
            {
                return _memberUrl;
            }
            set {
                _memberUrl = value;
            }
        }

        private static string _access_way;
        /// <summary>
        /// 访问方式
        /// </summary>
        public static string access_way
        {
            get
            {
                return _access_way;
            }
            set
            {
                _access_way = value;
            }
        }
        private static string _mem_access_token;
        /// <summary>
        /// 会员访问token
        /// </summary>
        public static string mem_access_token
        {
            get
            {
                return _mem_access_token;
            }
            set
            {
                _mem_access_token = value;
            }
        }
    }
}
