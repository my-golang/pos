namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using LL.POS.Common;
    using LL.POS.Model;
    using LL.Common;
    using System.Configuration;
    using LL.Utility;

    //using Msmk.BLL;
    //using Msmk.Device;
    //
    public static class Gattr
    {
        //返回业务逻辑对象
        private static PosBll _bll = new PosBll();
        private static string _posSalePrcPoint = "N2"; //流水原价
        private static string _posSaleNumPoint = "N2"; //流水数量
        private static string _posSaleAmtPoint = "N2";//流水小计
        private static string _posSaleNumPointF = "N4";
        private static string _posSalePrcPointF = "N4";
        private static Dictionary<string, t_attr_function> _FunKeys = null;
        private static ServerBll _serverBll = new ServerBll(); //操作服务器类
        private static bool _isTotRollBack; //结算是否回滚操作
        private static string _operId; //当前操作人编码
        private static string _operFullName; //操作人名称
        private static string _operPwd; //当前用户操作密码
        private static string _checkStandId;//收银台ID
        private static string _oper_flag;//操作人标记
        private static string _net_status;//联网状态
        private static PosPrint _posPrinter = new PosPrint();//打印类
        private static int _numHeaderNL = 0; //打印换行
        private static string _myHelp = Application.StartupPath + @"\MyHelp.chm";
        private static string _textFile = Application.StartupPath + @"\notepad.txt";
        private static string _appTitle = ConfigurationManager.AppSettings["appName"]; //"美食美客（开阳店）"; //分店名称
        private static string _billFile = Application.StartupPath + @"\log\小票" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
        private static bool _isBillPrintLog = true;//是否写日志
        private static int _prtLen = ExtendUtility.Instance.ParseToInt32(ConfigurationManager.AppSettings["posWidth"]); //小票纸张宽度
        private static string _posModel = ConfigurationManager.AppSettings["posName"];// "POS58"; //POS机型号
        private static string _posPort = ""; //POS机端口
        //private static string _vipCardNo = "";//会员编号
        //private static bool _isVip = false;//是否是VIP
        //private static decimal _vipScore = 0M; //VIP积分
        private static string _webPosServicePath = "ws/PosServices.asmx";//"http://esalesapi.9zhou.com/soap/Server.php?WSDL";//Webservice文件名称 http://localhost/eStore/server.php?wsdl
        private static string _posId = ""; //POS机ID
        private static string _PortType = ConfigurationManager.AppSettings["portType"];//打印端口类型
        private static string _PortName = ConfigurationManager.AppSettings["portName"];//端口名称
        private static string _branchNo = "";//分支ID
        private static string _branchName = "";//分支名称
        private static string _httpAddress = "http://localhost";//webservice地址
        private static string _errMsg = "";
        private static bool _isConnectonServer;
        private static int _flowNo = 1; //流水ID
        private static string _flowNoFile;//流水文件路径
        private static bool _isWeigh; //是否是称重
        #region 双屏属性
        public static short DoubleColorBlue = 240;
        public static short DoubleColorGreen = 240;
        public static short DoubleColorRed = 240;
        public static int DoubleSetheight = 600;// 双屏高
        public static int DoubleSetwidth = 800; // 双屏宽
        public static bool IsDoubleSetwh = false;

        private static string _isDouble = ExtendUtility.Instance.ParseToString(ConfigurationManager.AppSettings["isDouble"]); //流水ID
        private static bool _isDoubleModule = false;//开启双屏标记
        private static string _adUrl = string.Empty;

        public static string LoggerName = "MsmkLogger";

        public static string adUrl
        {
            get
            {
                return _adUrl;
            }
            set
            {
                _adUrl = value;
            }
        }
        #endregion
        private static int _pendingOrderMaxNo = 10;//最大挂单数
        private static bool _isRecordPosAccount = true;//自动记录对帐信息


        public static String ITEM_DB_FILE = Application.StartupPath + @"\item.db";
        public static String SALE_DB_FILE = Application.StartupPath + @"\sale.db";

        public static string ITEM_DB_FILE_NAME = GobalStaticString.SQLITE_DB_STRING.Replace("%path%", Application.StartupPath + @"\item.db");
        public static string SALE_DB_ITEM_NAME = GobalStaticString.SQLITE_DB_STRING.Replace("%path%", Application.StartupPath + @"\sale.db");

        private static Dictionary<String, String> _usedDicKeyTags = new Dictionary<string, string>();

        public static string BranchName
        {
            get
            {
                return _branchName;
            }
            set
            {
                _branchName = value;
            }
        }
        private static string _server = "192.168.1.136";
        //private static string _server = "122.144.131.108";
        /// <summary>
        /// 服务器
        /// </summary>
        public static string Server
        {
            get { return _server; }
            set { _server = value; }
        }
        private static string _user = "root";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string User
        {
            get { return _user; }
            set { _user = value; }
        }
        //private static string _password = "SKDB@)!*2018shikang2017";
        private static string _password = "root";
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        //private static string _database = "esales";
        private static string _database = "yii3_shop";
        /// <summary>
        /// 数据库
        /// </summary>
        public static string Database
        {
            get { return _database; }
            set { _database = value; }
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string CONNECT_STRING
        {
            get { return GobalStaticString.MYSQL_DB_STRING.Replace("%server%", Server).Replace("%user%", User).Replace("%password%", Password).Replace("%database%", Database); }
        }
        /// <summary>
        /// 已使用键
        /// </summary>
        public static Dictionary<String, String> UsedDicKeyTag
        {
            get
            {
                return _usedDicKeyTags;
            }
            set
            {
                _usedDicKeyTags = value;
            }
        }

        private static Dictionary<String, PosOpType> _dicPosOpType = new Dictionary<string, PosOpType>();
        /// <summary>
        /// POS机操作tag和枚举对应
        /// </summary>
        public static Dictionary<String, PosOpType> DicPosOpType
        {
            get
            {

                return _dicPosOpType;
            }
            set
            {
                _dicPosOpType = value;
            }
        }
        private static string _applicationXml = "Application.xml";
        /// <summary>
        /// 应用程序XML配置
        /// </summary>
        public static string ApplicationXml
        {
            get
            {
                return _applicationXml;
            }
            set
            {
                _applicationXml = value;
            }
        }
        /// <summary>
        /// 联网状态显示
        /// </summary>
        public static String NetStatus
        {
            get
            {
                return _net_status;
            }
            set
            {
                _net_status = value;
            }
        }
        private static String _PayHtml;
        /// <summary>
        /// 二维码支付界面html
        /// </summary>
        public static String PayHtml
        {
            set
            {
                _PayHtml = value;
            }
            get
            {
                return _PayHtml;
            }
        }
        /// <summary>
        /// 会员服务接口地址
        /// </summary>
        private static String _memberServiceUri = ConfigurationManager.AppSettings["memberUrl"];
        /// <summary>
        /// 会员服务接口地址
        /// </summary>
        public static String MemberServiceUri
        {
            get
            {
                return _memberServiceUri;
            }
            set
            {
                _memberServiceUri = value;
            }
        }

        private static String _useToken = "de9b968de5ba6c10f683661a3a7fe18ac5946ed4";
        /// <summary>
        /// 会员服务秘钥key
        /// </summary>
        public static String UseToken
        {
            get
            {
                return _useToken;
            }
            set
            {
                _useToken = value;
            }
        }
        private static String _serviceWay = "POS";
        /// <summary>
        /// 会员服务访问标示
        /// </summary>
        public static String ServiceWay
        {
            get
            {
                return _serviceWay;
            }
            set
            {
                _serviceWay = value;
            }
        }
        /// <summary>
        /// 是否连接服务器
        /// </summary>
        public static bool IsConnectonServer
        {
            get { return Gattr._isConnectonServer; }
            set { Gattr._isConnectonServer = value; }
        }
        /// <summary>
        /// POS机所属分支
        /// </summary>
        public static string BranchNo
        {
            get { return Gattr._branchNo; }
            set { Gattr._branchNo = value; }
        }
        /// <summary>
        /// webService地址
        /// </summary>
        public static string HttpAddress
        {
            get { return Gattr._httpAddress; }
            set { Gattr._httpAddress = value; }
        }
        /// <summary>
        /// PosWebservice文件名称
        /// PosService.asmx
        /// </summary>
        public static string WebPosServicePath
        {
            get { return Gattr._webPosServicePath; }
            set { Gattr._webPosServicePath = value; }
        }
        /// <summary>
        /// POS机ID
        /// </summary>
        public static string PosId
        {
            get { return Gattr._posId; }
            set { Gattr._posId = value; }
        }
        /// <summary>
        /// 打印机端口名称
        /// </summary>
        public static string PortName
        {
            get { return Gattr._PortName; }
            set { Gattr._PortName = value; }
        }
        /// <summary>
        /// 打印机端口类型
        /// </summary>
        public static string PortType
        {
            get { return Gattr._PortType; }
            set { Gattr._PortType = value; }
        }
        /// <summary>
        /// POS机端口
        /// </summary>
        public static string PosPort
        {
            get { return Gattr._posPort; }
            set { Gattr._posPort = value; }
        }
        /// <summary>
        /// POS机型号
        /// </summary>
        public static string PosModel
        {
            get { return Gattr._posModel; }
            set { Gattr._posModel = value; }
        }
        /// <summary>
        /// 服务器操作类
        /// </summary>
        public static ServerBll ServerBll
        {
            get { return Gattr._serverBll; }
        }
        /// <summary>
        /// 小票纸张宽度
        /// </summary>
        public static int PrtLen
        {
            get { return Gattr._prtLen; }
            set { Gattr._prtLen = value; }
        }
        /// <summary>
        /// 结算是否回滚操作
        /// </summary>
        public static bool IsTotRollBack
        {
            get { return _isTotRollBack; }
            set { _isTotRollBack = value; }
        }

        /// <summary>
        /// 数据库操作实体
        /// </summary>
        public static PosBll Bll
        {
            get
            {
                return _bll;
            }
        }
        /// <summary>
        /// 商品数量两位小数
        /// </summary>
        public static string PosSaleNumPoint
        {
            get
            {
                return _posSaleNumPoint;
            }
            set
            {
                _posSaleNumPoint = value;
            }
        }
        /// <summary>
        /// 商品原价两位小数
        /// </summary>
        public static string PosSalePrcPoint
        {
            get
            {
                return _posSalePrcPoint;
            }
            set
            {
                _posSalePrcPoint = value;
            }
        }
        /// <summary>
        /// 流水小计
        /// </summary>
        public static string PosSaleAmtPoint
        {
            get
            {
                return _posSaleAmtPoint;
            }
            set
            {
                _posSaleAmtPoint = value;
            }
        }
        /// <summary>
        /// 快捷集合
        /// </summary>
        public static Dictionary<string, t_attr_function> FunKeys
        {
            get
            {
                return _FunKeys;
            }
            set
            {
                _FunKeys = value;
            }
        }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public static string OperId
        {
            get { return Gattr._operId; }
            set { Gattr._operId = value; }
        }
        /// <summary>
        /// 用户操作标记
        /// </summary>
        public static string Oper_flag
        {
            get { return Gattr._oper_flag; }
            set { Gattr._oper_flag = value; }
        }
        /// <summary>
        /// 操作人密码
        /// </summary>
        public static string OperPwd
        {
            get { return Gattr._operPwd; }
            set { Gattr._operPwd = value; }
        }
        private static string _operGrant;
        /// <summary>
        /// 操作人权限
        /// </summary>
        public static string OperGrant
        {
            get { return Gattr._operGrant; }
            set { Gattr._operGrant = value; }
        }
        private static Decimal _operDiscount;
        /// <summary>
        /// 操作人打折折扣率
        /// </summary>
        public static Decimal OperDiscount
        {
            get { return Gattr._operDiscount; }
            set { Gattr._operDiscount = value; }
        }
        /// <summary>
        /// 收银台编码
        /// </summary>
        public static string CheckStandId
        {
            get { return Gattr._checkStandId; }
            set { Gattr._checkStandId = value; }
        }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public static string OperFullName
        {
            get { return Gattr._operFullName; }
            set { Gattr._operFullName = value; }
        }
        /// <summary>
        /// POS打印对象
        /// </summary>
        public static PosPrint PosPrinter
        {
            get
            {
                return _posPrinter;
            }
        }
        /// <summary>
        /// 打印头部信息后换行数
        /// </summary>
        public static int NumHeaderNL
        {
            get { return Gattr._numHeaderNL; }
            set { Gattr._numHeaderNL = value; }
        }
        /// <summary>
        /// 分店名称
        /// </summary>
        public static string AppTitle
        {
            get { return Gattr._appTitle; }
            set { Gattr._appTitle = value; }
        }
        /// <summary>
        /// 记事本文件
        /// </summary>
        public static string TextFile
        {
            get { return Gattr._textFile; }
            set { Gattr._textFile = value; }
        }
        /// <summary>
        /// 操作手册
        /// </summary>
        public static string MyHelp
        {
            get { return Gattr._myHelp; }
            set { Gattr._myHelp = value; }
        }
        /// <summary>
        /// 账单LOG文件
        /// </summary>
        public static string BillFile
        {
            get { return Gattr._billFile; }
            set { Gattr._billFile = value; }
        }
        /// <summary>
        /// 是否写账单日志
        /// </summary>
        public static bool IsBillPrintLog
        {
            get { return Gattr._isBillPrintLog; }
            set { Gattr._isBillPrintLog = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public static string ErrMsg
        {
            get { return Gattr._errMsg; }
            set { Gattr._errMsg = value; }
        }

        private static string dateFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 时间格式
        /// </summary>
        public static string DateFormat
        {
            get { return Gattr.dateFormat; }
            set { Gattr.dateFormat = value; }
        }
        private static string stampFormat = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// 时间格式
        /// </summary>
        public static string StampFormat
        {
            get { return Gattr.stampFormat; }
            set { Gattr.stampFormat = value; }
        }
        public static string PosSalePrcPointF
        {
            get { return Gattr._posSalePrcPointF; }
            set { Gattr._posSalePrcPointF = value; }
        }
        public static string PosSaleNumPointF
        {
            get { return Gattr._posSaleNumPointF; }
            set { Gattr._posSaleNumPointF = value; }
        }
        /// <summary>
        /// 流水ID
        /// </summary>
        public static int FlowNo
        {
            get { return Gattr._flowNo; }
            set { Gattr._flowNo = value; }
        }
        /// <summary>
        /// 流水文件
        /// </summary>
        public static string FlowNoFile
        {
            get { return Gattr._flowNoFile; }
            set { Gattr._flowNoFile = value; }
        }
        /// <summary>
        /// 是否是称重
        /// </summary>
        public static bool IsWeigh
        {
            get { return Gattr._isWeigh; }
            set { Gattr._isWeigh = value; }
        }
        /// <summary>
        /// 最大挂单数
        /// </summary>
        public static int PendingOrderMaxNo
        {
            get
            {
                return _pendingOrderMaxNo;
            }
            set
            {
                _pendingOrderMaxNo = value;
            }
        }
        /// <summary>
        /// 是否启动双屏模块
        /// </summary>
        public static bool IsDoubleModule
        {
            get
            {
                if (_isDouble == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //set { Gattr._isDoubleModule = value; }
        }
        /// <summary>
        /// 自动记录对账信息
        /// </summary>
        public static bool IsRecordPosAccount
        {
            get { return Gattr._isRecordPosAccount; }
            set { Gattr._isRecordPosAccount = value; }
        }


        private static string _client_id = "POS";
        /// <summary>
        /// 访问接口服务的客户端类型
        /// </summary>
        public static string client_id
        {
            get { return _client_id; }
            set { _client_id = value; }
        }
        private static string _access_token = "c2e3c130b7040fbe18e7f9b319844b42558aeb34";
        /// <summary>
        /// 访问接口服务的秘钥TOKEN
        /// </summary>
        public static string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        private static string _serverUrl = ConfigurationManager.AppSettings["serviceUrl"]; //"http://esales1/manager/api/ws/api";
        /// <summary>
        /// 接口服务的URL
        /// </summary>
        public static string serverUrl
        {
            get { return _serverUrl; }
            set { _serverUrl = value; }
        }
        private static string _oper_pwd;
        /// <summary>
        /// 登陆密码 
        /// </summary>
        public static string Oper_Pwd
        {
            get { return _oper_pwd; }
            set { _oper_pwd = value; }
        }
        /// <summary>
        /// 数据文件路径
        /// </summary>
        public static string DB_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "DBSQLite/sql/";
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public static string LAST_UPDATE_TIME = System.DateTime.Now.Year + "-01-01";
        /// <summary>
        /// 系统配置文件
        /// </summary>
        public static string INI_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "EPOS.ini";
    }
}
