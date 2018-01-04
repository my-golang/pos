

namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using LL.POS.Common;
    using LL.POS.Model;
    using LL.Utility;
    using System.Configuration;
    using System.IO.Ports;

    /// <summary>
    /// 公共方法类
    /// </summary>
    static class Gfunc
    {

        private static List<t_pos_printer_out> _listWinPrtStr = new List<t_pos_printer_out>();

        /// <summary>
        /// 写启动日志
        /// </summary>
        /// <param name="mess"></param>
        public static void WriteStartLog(string mess)
        {
            try
            {
                string path = Application.StartupPath + @"\log";
                string str2 = string.Format("StartLog{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileStream stream = new FileStream(path + @"\" + str2, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(mess);
                writer.Close();
                stream.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 处理销售商品最后的名称
        /// </summary>
        /// <param name="itemNo"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static string ItemNameFormat(string itemNo, string itemName)
        {
            return (itemNo.Trim() + "\n" + itemName.Trim());
        }
        /// <summary>
        /// 解析快捷键代码
        /// </summary>
        /// <param name="bShift">检测是否按SHIFT键</param>
        /// <param name="keyCode">当前键盘按键</param>
        /// <returns></returns>
        public static string ParseKeyCode(bool bShift, Keys keyCode)
        {
            string str = "";
            if ((keyCode >= Keys.A) && (keyCode <= Keys.Z))
            {
                str = keyCode.ToString();
            }
            else if ((keyCode >= Keys.D0) && (keyCode <= Keys.D9))
            {
                if (bShift)
                {
                    switch (keyCode)
                    {
                        case Keys.D0:
                            str = ")";
                            break;

                        case Keys.D1:
                            str = "!";
                            break;

                        case Keys.D2:
                            str = "@";
                            break;

                        case Keys.D3:
                            str = "#";
                            break;

                        case Keys.D4:
                            str = "$";
                            break;

                        case Keys.D5:
                            str = "%";
                            break;

                        case Keys.D6:
                            str = "^";
                            break;

                        case Keys.D7:
                            str = "&";
                            break;

                        case Keys.D8:
                            str = "*";
                            break;

                        case Keys.D9:
                            str = "(";
                            break;
                    }
                }
                else
                {
                    str = keyCode.ToString().Substring(1, 1);
                }
            }
            else if ((keyCode >= Keys.NumPad0) && (keyCode <= Keys.NumPad9))
            {
                str = keyCode.ToString().Substring(6, 1);
            }
            else
            {
                switch (keyCode)
                {
                    case Keys.Space:
                        str = " ";
                        break;

                    case Keys.End:
                    case Keys.Home:
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Right:
                    case Keys.Down:
                    case Keys.Insert:
                    case Keys.Delete:
                    case Keys.Tab:
                        str = keyCode.ToString();
                        break;

                    case Keys.Multiply:
                        str = "*";
                        break;

                    case Keys.Add:
                        str = "+";
                        break;

                    case Keys.Subtract:
                        str = "-";
                        break;

                    case Keys.Decimal:
                        str = ".";
                        break;

                    case Keys.Divide:
                        str = "/";
                        break;

                    case Keys.Oem1:
                        if (!bShift)
                        {
                            str = ";";
                            break;
                        }
                        str = ":";
                        break;

                    case Keys.Oemplus:
                        if (!bShift)
                        {
                            str = "=";
                            break;
                        }
                        str = "+";
                        break;

                    case Keys.Oemcomma:
                        if (!bShift)
                        {
                            str = ",";
                            break;
                        }
                        str = "<";
                        break;

                    case Keys.OemMinus:
                        if (!bShift)
                        {
                            str = "-";
                            break;
                        }
                        str = "_";
                        break;

                    case Keys.OemPeriod:
                        if (!bShift)
                        {
                            str = ".";
                            break;
                        }
                        str = ">";
                        break;

                    case Keys.Oem2:
                        if (!bShift)
                        {
                            str = "/";
                            break;
                        }
                        str = "?";
                        break;

                    case Keys.Oem4:
                        if (!bShift)
                        {
                            str = "[";
                            break;
                        }
                        str = "{";
                        break;

                    case Keys.Oem5:
                        if (!bShift)
                        {
                            str = @"\";
                            break;
                        }
                        str = "|";
                        break;

                    case Keys.Oem6:
                        if (!bShift)
                        {
                            str = "]";
                            break;
                        }
                        str = "}";
                        break;

                    case Keys.Oem7:
                        if (!bShift)
                        {
                            str = "'";
                            break;
                        }
                        str = "\"";
                        break;
                }
            }
            if ((bShift || (GetKeyState(20) != 0)) && (!bShift || (GetKeyState(20) != 1)))
            {
                return str;
            }
            return str.ToLower();
        }
        [DllImport("user32.dll")]
        public static extern int GetKeyState(int nVirtKey);

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="digitValue"></param>
        /// <returns></returns>
        public static bool IsDigit(string digitValue)
        {
            try
            {
                Convert.ToDecimal(digitValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 数字字符串转换成Decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal TypeToDecimal(string str, decimal def)
        {
            decimal num;
            if (decimal.TryParse(str, out num))
            {
                return num;
            }
            return def;
        }
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            //long i = 1;
            //foreach (byte b in Guid.NewGuid().ToByteArray())
            //{
            //    i *= ((int)b + 1);
            //}
            //return string.Format("{0:x}", i - DateTime.Now.Ticks);
            return string.Format("{0}{1}{2}{3}", Gattr.BranchNo, Gattr.OperId, Gattr.PosId, System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
        /// <summary>
        /// 格式化数字字符串并保留两位小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal strToDecimal(string str)
        {
            return Math.Round(Convert.ToDecimal(String.Format("{0:F}", System.Decimal.Parse(str.Trim()))), 2);
        }
        /// <summary>
        /// 打印小票头部信息
        /// </summary>
        public static void PrintHeader()
        {
            List<PrintString> listPrtStr = new List<PrintString>();
            for (int i = 0; i < Gattr.NumHeaderNL; i++)
            {
                listPrtStr.Add(new PrintString("\r\n"));
            }
            PrintString item = new PrintString
            {
                IsBigSize = true,
                PrtSrt = PrintStrAlignForBigSize(Gattr.AppTitle, Gattr.PrtLen, TextAlign.Center, false)
            };
            listPrtStr.Add(item);
            PrintOut(listPrtStr);
        }
        /// <summary>
        /// 设置打印布局
        /// </summary>
        /// <param name="str"></param>
        /// <param name="intLen"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static string PrintStrAlignForBigSize(string str, int intLen, TextAlign align, bool isBigSize)
        {
            int num = 0;
            int byteCount = Encoding.Default.GetByteCount(str);
            int length = str.Length;
            if (intLen == 0)
            {
                str = "";
                return str;
            }
            if (isBigSize)
            {
                intLen = ExtendUtility.Instance.ParseToInt32((intLen / 1.25).ToString());
            }
            if (length < intLen)
            {
                switch (align)
                {
                    case TextAlign.Center:
                        if ((byteCount + 1) < intLen)
                        {
                            num = ((intLen - byteCount) / 2) + length;
                            str = str.PadLeft((num < 0) ? 0 : num, ' ');
                        }
                        return str;

                    case TextAlign.Right:
                        if (str.Length < intLen)
                        {
                            num = (intLen - byteCount) + length;
                            str = str.PadLeft((num < 0) ? 0 : num, ' ');
                        }
                        return str;
                }
                if (str.Length < intLen)
                {
                    num = (intLen - byteCount) + length;
                    str = str.PadRight((num < 0) ? 0 : num, ' ');
                }
            }
            return str;
        }
        /// <summary>
        /// 设置打印布局
        /// </summary>
        /// <param name="str"></param>
        /// <param name="intLen"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static string PrintStrAlign(string str, int intLen, TextAlign align)
        {
            int num = 0;
            int byteCount = Encoding.Default.GetByteCount(str);
            int length = str.Length;
            if (intLen == 0)
            {
                str = "";
                return str;
            }

            if (length < intLen)
            {
                switch (align)
                {
                    case TextAlign.Center:
                        if ((byteCount + 1) < intLen)
                        {
                            num = ((intLen - byteCount) / 2) + length;
                            str = str.PadLeft((num < 0) ? 0 : num, ' ');
                        }
                        return str;

                    case TextAlign.Right:
                        if (str.Length < intLen)
                        {
                            num = (intLen - byteCount) + length;
                            str = str.PadLeft((num < 0) ? 0 : num, ' ');
                        }
                        return str;
                }
                if (str.Length < intLen)
                {
                    num = (intLen - byteCount) + length;
                    str = str.PadRight((num < 0) ? 0 : num, ' ');
                }
            }
            return str;
        }
        /// <summary>
        /// 打印操作
        /// </summary>
        /// <param name="listPrtStr"></param>
        public static void PrintOut(List<PrintString> listPrtStr)
        {
            if (Gattr.IsBillPrintLog)
            {
                string path = Gattr.BillFile.Substring(0, Gattr.BillFile.LastIndexOf(@"\"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileStream stream = new FileStream(Gattr.BillFile, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                foreach (PrintString str2 in listPrtStr)
                {
                    writer.WriteLine(str2.PrtSrt.ToString());
                }
                writer.Close();
                stream.Close();
            }

            if (Gattr.PosPrinter.IsWindowPrinter)
            {
                foreach (PrintString str in listPrtStr)
                {
                    t_pos_printer_out item = new t_pos_printer_out
                    {
                        BigFont = str.IsBigSize,
                        Text = str.PrtSrt
                    };
                    _listWinPrtStr.Add(item);
                }
            }
        }
        /// <summary>
        /// 发送打印信息
        /// </summary>
        public static void SendToPrint()
        {
            if (Gattr.PosPrinter.IsWindowPrinter)
            {
                Gattr.PosPrinter.PrintOut(_listWinPrtStr);
                _listWinPrtStr.Clear();
            }
            else
            {
                //RetailGlobal.PosCashBox.OpenCashBox();
            }
        }
        public static void SendToAPIPrint()
        {
            bool IsOpen = false;
            switch (Gattr.PortType)
            {
                case "并口":
                    SendToLTP();
                    break;
                case "串口":
                    System.IO.Ports.SerialPort sPort = new System.IO.Ports.SerialPort();
                    sPort.PortName = Gattr.PortName;
                    sPort.BaudRate = 9600;
                    sPort.DataBits = 8;
                    sPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "1");
                    sPort.Parity = (Parity)Enum.Parse(typeof(Parity), "1");
                    sPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), "1");
                    IsOpen = POSUtility.Instance.OpenComPort(ref sPort);
                    sPort.Dispose();
                    break;
                case "USB":
                    IsOpen = POSUtility.Instance.OpenUSBPort(Gattr.PortName);
                    break;
                case "驱动":
                    Gfunc.SendToPrint();
                    //IsOpen = POSUtility.Instance.OpenPrinter(Gattr.PortName);
                    break;
                case "网口":
                    IsOpen = POSUtility.Instance.OpenNetPort(Gattr.PortName);
                    break;
            }
            if (IsOpen)
            {
                if (((_listWinPrtStr != null) && (_listWinPrtStr.Count != 0)))
                {
                    try
                    {
                        //开钱箱
                        IntPtr res = POSUtility.POS_KickOutDrawer(0x00, 100, 100);
                        if ((uint)res != POSUtility.Instance.POS_SUCCESS)
                        {
                            MessageBox.Show("开钱箱失败!指令调用返回值：" + res.ToString(), "系统提示");
                            LoggerHelper.Log("MsmkLogger", "开钱箱失败!指令调用返回值：" + res.ToString(), LogEnum.SysLog);
                        }
                        POSUtility.POS_StartDoc();
                        POSUtility.POS_SetMode(0x00);
                        POSUtility.POS_SetRightSpacing(0);
                        POSUtility.POS_SetLineSpacing(80);
                        foreach (t_pos_printer_out _out in _listWinPrtStr)
                        {
                            POSUtility.POS_FeedLine();
                            POSUtility.POS_S_TextOut(_out.Text, 0, 1, 1, POSUtility.Instance.POS_FONT_TYPE_STANDARD, POSUtility.Instance.POS_FONT_STYLE_NORMAL);
                        }
                        POSUtility.POS_FeedLines(4);
                        POSUtility.POS_Reset();
                        POSUtility.POS_EndDoc();
                        _listWinPrtStr.Clear();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "系统提示");
                        LoggerHelper.Log("MsmkLogger", "API打印报错：" + exception.Message, LogEnum.SysLog);
                    }
                }
                //关闭端口
                POSUtility.Instance.ClosePrinterPort();
            }
            else
            {
                if (Gattr.PortType != "驱动" && Gattr.PortType != "并口")
                {
                    MessageBox.Show(Gattr.PortType + "端口打印机无效！", Gattr.AppTitle);
                    LoggerHelper.Log("MsmkLogger", Gattr.PortType + "端口打印机无效！", LogEnum.SysLog);
                }
            }
        }

        /// <summary>
        /// LTP并口打印
        /// </summary>
        public static void SendToLTP()
        {
            PrintLPTUtility PrintLPT = new PrintLPTUtility();
            if (PrintLPT.Open(Gattr.PortName))
            {
                if (((_listWinPrtStr != null) && (_listWinPrtStr.Count != 0)))
                {
                    try
                    {
                        //开钱箱
                        bool res = PrintLPT.OpenMbox();
                        if (!res)
                        {
                            MessageBox.Show("LTP开钱箱失败!指令调用返回值：" + res.ToString(), "系统提示");
                            LoggerHelper.Log("MsmkLogger", "LTP开钱箱失败!指令调用返回值：" + res.ToString(), LogEnum.SysLog);
                        }
                        //PrintLPT.SetLeft(1);
                        //PrintLPT.SetFontSizeB();
                        foreach (t_pos_printer_out _out in _listWinPrtStr)
                        {
                            PrintLPT.PrintText(_out.Text.TrimEnd());
                        }
                        _listWinPrtStr.Clear();
                        //进纸
                        PrintLPT.Feed(3);
                    }
                    catch (Exception exception)
                    {
                        PrintLPT.Close();
                        MessageBox.Show(exception.Message, "系统提示");
                        LoggerHelper.Log("MsmkLogger", "LTP打印报错：" + exception.Message, LogEnum.SysLog);
                    }
                }
                //关闭端口
                PrintLPT.Close();
            }
            else
            {
                MessageBox.Show(Gattr.PortType + "端口打印机无效！LPT" + Gattr.PortName, Gattr.AppTitle);
                LoggerHelper.Log("MsmkLogger", Gattr.PortType + "端口打印机无效！LTP", LogEnum.SysLog);
            }
        }

        /// <summary>
        /// 初始化系统配置
        /// </summary>
        /// <returns></returns>
        public static long InitSystem()
        {
            //校验服务时间和本地时间
            //实现数据下载
            //初始化系统环境应用文件
            long num = InitEnvironment();
            Gattr.IsConnectonServer = Gattr.ServerBll.TestConnection();

            return num;
        }
        /// <summary>
        /// 初始话所有参数
        /// </summary>
        public static void InitSysVar()
        {

        }
        /// <summary>
        /// 初始化门店调用服务信息、门店、POS信息
        /// </summary>
        /// <returns></returns>
        public static long InitEnvironment()
        {
            try
            {
                //门店POS配置文件
                String xmlpath = Application.StartupPath + @"\" + Gattr.ApplicationXml;
                //流水号文件
                Gattr.FlowNoFile = Application.StartupPath + @"\flowno.isf";
                //是否启动双屏模块
                //Gattr.IsDoubleModule = false;
                CheckSysInfo();
                InitSysIni();
                string[] nodePaths = new string[1];
                nodePaths[0] = "//applicationConfig/BranchNo";
                Dictionary<String, String> dic = XMLUtility.Instance.GetNodesText(xmlpath, nodePaths);
                LoggerHelper.Log("MsmkLogger", "开始检测是否需要更新配置信息", LogEnum.InitLog);
                if (dic == null || SIString.IsNullOrEmptyOrDBNull(dic[nodePaths[0]]))
                {
                    LoggerHelper.Log("MsmkLogger", "更新配置信息开始Start", LogEnum.InitLog);
                    FrmInitData frmInitData = new FrmInitData();
                    if (frmInitData.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        LoggerHelper.Log("MsmkLogger", "取消配置信息Cancel", LogEnum.InitLog);
                        return -1;
                    }//if (frmInitData.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    LoggerHelper.Log("MsmkLogger", "更新配置信息完成End", LogEnum.InitLog);
                }//if (SIString.IsNullOrEmptyOrDBNull(dic[nodePaths[0]]))
                LoggerHelper.Log("MsmkLogger", "读取配置信息开始Start", LogEnum.InitLog);
                nodePaths = new string[3];
                nodePaths[0] = "//applicationConfig/ServerUrl";
                nodePaths[1] = "//applicationConfig/BranchNo";
                nodePaths[2] = "//applicationConfig/PosId";
                dic = XMLUtility.Instance.GetNodesText(xmlpath, nodePaths);
                //Gattr.HttpAddress = dic[nodePaths[0]];
                Gattr.serverUrl = dic[nodePaths[0]];
                Gattr.BranchNo = dic[nodePaths[1]];
                Gattr.PosId = dic[nodePaths[2]];
                string err = string.Empty;
                //获取二维码支付html
                Gattr.PayHtml = FileUtility.Instance.GetFileContent(Application.StartupPath + @"\pay.htm", ref err);
                LoggerHelper.Log("MsmkLogger", "读取配置信息完成End", LogEnum.InitLog);
                string errorMessage1 = string.Empty;
                if (!PosServericeBll.Instance.ValidatePos(Gattr.BranchNo, Gattr.PosId, ref errorMessage1))
                {
                    MessageBox.Show(errorMessage1, Gattr.AppTitle);
                    return -3;
                }
                //if (!ValidatePos(Gattr.BranchNo, Gattr.PosId))
                //{
                //    return -3;
                //}
                bool isok = true;
                string errorMessage = string.Empty;
                Dictionary<string, object> _dicParameters = GetServiceParameter();
                _dicParameters.Add("branch_no", Gattr.BranchNo);
                _dicParameters.Add("posid", Gattr.PosId);
                string result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Getadurl", _dicParameters, ref isok, ref errorMessage);
                if (isok)
                {
                    if (result != "-10" && result != "-20" && result != "404")
                    {
                        Gattr.adUrl = result;
                    }
                }
                else
                {
                    LoggerHelper.Log("MsmkLogger", "读取门店广告信息错误:" + errorMessage, LogEnum.InitLog);
                }
                //Gattr.adUrl
            }
            catch (Exception)
            {
                return -2;
            }
            return 1;
        }
        /// <summary>
        /// 初始化数据下载
        /// </summary>
        private static void InitSysIni()
        {
            string iniFile = Gattr.INI_FILE_PATH;
            string errorMessage = string.Empty;
            if (!File.Exists(iniFile))
            {
                if (FileUtility.Instance.CreateFile(iniFile, false, ref errorMessage))
                {
                    //第一次初始化成最初的一个值
                    WindowsAPIUtility.WritePrivateProfileString("download", "t_product_food", Gattr.LAST_UPDATE_TIME, iniFile);
                    WindowsAPIUtility.WritePrivateProfileString("download", "t_pos_branch_stock", Gattr.LAST_UPDATE_TIME, iniFile);
                    WindowsAPIUtility.WritePrivateProfileString("download", "t_pos_branch_price", Gattr.LAST_UPDATE_TIME, iniFile);
                    WindowsAPIUtility.WritePrivateProfileString("download", "barcode", Gattr.LAST_UPDATE_TIME, iniFile);
                }
                else
                {
                    LoggerHelper.Log("MsmkLogger", "初始化系统配置文件失败:" + errorMessage, LogEnum.InitLog);
                }
            }
        }
        /// <summary>
        /// 验证当前选择的POS的信息
        /// </summary>
        /// <param name="branch_no"></param>
        /// <param name="posid"></param>
        /// <returns></returns>
        private static bool ValidatePos(string branch_no, string posid)
        {
            bool isok = false;
            try
            {
                string mac = ComputerUtility.Instance.GetMacAddress();
                string disk = ComputerUtility.Instance.GetDiskVolumeSerialNumber();
                if (disk == LL.Common.GobalStaticString.UN_KNOWN_DISK)
                {
                    MessageBox.Show("获取机器信息失败", Gattr.AppTitle);
                    return false;
                }
                string hostname = System.Environment.MachineName;
                Dictionary<string, object> _dic = Gfunc.GetServiceParameter();
                bool isok1 = true;
                string errorMessage = string.Empty;
                string result1 = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Testconn", _dic, ref isok1, ref errorMessage);
                if (isok1 == true && result1 == "1")
                {
                    _dic.Add("hostname", hostname);
                    _dic.Add("hostmac", mac);
                    _dic.Add("hostdisk", disk);
                    _dic.Add("branch_no", branch_no);
                    _dic.Add("posid", posid);
                    string result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Validpos", _dic, ref isok1, ref errorMessage);
                    if (isok1)
                    {
                        if (result != "-10" && result != "-20")
                        {
                            switch (result)
                            {
                                case "0":
                                    isok = true;
                                    break;
                                case "1":
                                    //MessageBox.Show("注册成功", Gattr.AppTitle);
                                    isok = true;
                                    break;
                                case "2":
                                    MessageBox.Show("该机器已注册其他POS机", Gattr.AppTitle);
                                    break;
                                case "3":
                                    MessageBox.Show("该POS机已被其他机器注册", Gattr.AppTitle);
                                    break;
                                case "-2":
                                case "-1":
                                default:
                                    MessageBox.Show("注册/验证POS机失败", Gattr.AppTitle);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    isok = true;
                    // MessageBox.Show("注册/验证POS机失败", Gattr.AppTitle);
                    // LoggerHelper.Log("MsmkLogger", "LL.POS->Init->FrmInitData->ValidatePos->Exception:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("注册/验证POS机失败", Gattr.AppTitle);
                LoggerHelper.Log("MsmkLogger", "LL.POS->Init->FrmInitData->ValidatePos->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isok;
        }
        /// <summary>
        /// 初始化POS机操作
        /// </summary>
        public static void InitPosKeyTag()
        {
            Gattr.DicPosOpType.Clear();
            Gattr.UsedDicKeyTag.Clear();
            String[] _dicTag = new String[] { 
                    "plu", "num", "can", "prc", "dct", 
                    "gdj",  "vip", "dou", "tot", "oth", 
                    "exit", "rmb","cdt", "csc", "gwq", 
                    "payother", "ads", "aca", "payesc","ret",
            "lok","enter","opd","yezf","azs","agz","zfb","wx"};
            PosOpType[] types = new PosOpType[] { 
                    PosOpType.PLU, PosOpType.数量, PosOpType.删除, PosOpType.单价, PosOpType.折扣, 
                    PosOpType.挂单, PosOpType.会员, PosOpType.营业员, PosOpType.结算, PosOpType.其他, 
                    PosOpType.退出, PosOpType.现金, PosOpType.银行卡, PosOpType.储值卡, PosOpType.购物券,      
                    PosOpType.其他方式, PosOpType.整单折扣, PosOpType.整单取消, PosOpType.支付退出, PosOpType.退货,
            PosOpType.交易查询,PosOpType.现金,PosOpType.开钱箱,PosOpType.余额支付,PosOpType.整单赠送,PosOpType.挂账,PosOpType.支付宝,PosOpType.微信};
            for (int index = 0; index < _dicTag.Length; index++)
            {
                Gattr.DicPosOpType.Add(_dicTag[index], types[index]);
            }
            String[] _value = new String[] { "rmb", "cdt", "csc", "yezf", "payother", "ads", "aca","azs","agz" };
            String[] _key = new String[] { "-", "i", "c", "=", ",", "j", "e" ,"s","g"};
            for (int index = 0; index < _key.Length; index++)
            {
                Gattr.UsedDicKeyTag.Add(_key[index], _value[index]);
            }
        }
        /// <summary>
        /// 设置流水号
        /// </summary>
        /// <returns></returns>
        public static bool SetFlowNo()
        {
            try
            {
                int num = Gattr.FlowNo + 1;
                if (num > 0x1869f)
                {
                    num = 1;
                }
                FileStream stream = new FileStream(Gattr.FlowNoFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                string str = reader.ReadLine();
                if (str != null)
                {
                    DateTime time = Convert.ToDateTime(str.Substring(0, 10));
                    if (DateTime.Today.Subtract(time).Days != 0)
                    {
                        string flowNo = Gattr.BranchNo + Gattr.PosId + DateTime.Today.ToString("yyyyMMdd").Substring(2, 6).Trim();
                        num = Gattr.Bll.GetIntFlow(flowNo) + 1;
                    }
                }
                reader.Close();
                stream.Close();
                Gattr.FlowNo = num;
                FileStream stream2 = new FileStream(Gattr.FlowNoFile, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream2);
                writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd") + "  " + num.ToString());
                writer.Close();
                stream2.Close();
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("警告：获得流水号失败！原因：\n{0}\n请向系统管理员联系解决问题。", exception.Message), Gattr.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return false;
        }
        /// <summary>
        /// 设置流水号
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool SetFlowNo(int num)
        {
            try
            {
                FileStream stream = new FileStream(Gattr.FlowNoFile, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd") + "  " + num.ToString());
                writer.Close();
                stream.Close();
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("警告：获得流水号失败！原因：\n{0}\n请向系统管理员联系解决问题。", exception.Message), Gattr.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return false;
        }
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        public static string GetFlowNo()
        {
            bool flag = false;
            string str4 = "";
            int num = 1;
            try
            {
                FileStream stream = new FileStream(Gattr.FlowNoFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                string str = reader.ReadLine();
                if (str != null)
                {
                    string str3 = str.Substring(0, 10);
                    num = TypeToInt(str.Substring(12), 0);
                    if ((num > 0) && (num <= 0x270f))
                    {
                        if (str3 != DateTime.Today.Date.ToString("yyyy-MM-dd"))
                        {
                            flag = true;
                        }
                        if (num != Gattr.FlowNo)
                        {
                            str4 = string.Format("获得流水号。从文件得到的流水号不匹配系统FlowNo变量！\n 文件值：{0}\n系统值：{1}", num.ToString("0000"), Gattr.FlowNo.ToString("0000"));
                        }
                    }
                    else
                    {
                        str4 = "从文件得到的流水号不在[0-9999]！";
                    }
                }
                else
                {
                    str4 = "获取流水号失败，没有读取到数据。";
                }
                reader.Close();
                stream.Close();
            }
            catch (Exception exception)
            {
                str4 = "打开流水号文件失败。" + exception.Message;
            }
            if (str4.Length > 0)
            {
                MessageBox.Show(string.Format("警告：获得流水号失败！原因：\n{0}\n请向系统管理员联系解决问题。", str4), Gattr.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (flag)
            {
                SetFlowNo();
            }
            string str6 = Gattr.BranchNo + Gattr.PosId + DateTime.Today.ToString("yyyyMMdd").Substring(2, 6).Trim() + Gattr.FlowNo.ToString("00000");
            if (str6.Length > 20)
            {
                throw new ApplicationException("获取交易流水号时发生错误！");
            }
            return str6;
        }
        public static int TypeToInt(string str, int def)
        {
            int num;
            if (int.TryParse(str, out num))
            {
                return num;
            }
            return def;
        }
        private static long CheckSysInfo()//bool isConnectServer)
        {
            string str4;
            string str2 = "";
            try
            {
                FileStream stream = new FileStream(Gattr.FlowNoFile, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string str = reader.ReadLine();
                if (str != null)
                {
                    string str3 = str.Substring(0, 10);
                    DateTime time = Convert.ToDateTime(str3);
                    TimeSpan span = DateTime.Today.Subtract(time);
                    str4 = string.Format("当前系统日期可能有错误：\n上次使用时间为：{0} \n本机当前时间为：{1}\n请调整本机时间后再继续！\n是否继续？", str3, DateTime.Today.ToString("yyyy-MM-dd"));
                    if (span.Days < 0)
                    {
                        MessageBox.Show(string.Format("当前系统日期可能有错误：\n上次使用时间为：{0} \n本机当前时间为：{1}\n请调整本机时间后再继续！", str3, DateTime.Today.ToString("yyyy-MM-dd")), Gattr.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Question);
                        reader.Close();
                        stream.Close();
                        return 0;
                    }
                    if ((span.Days > 10) && (MessageBox.Show(str4, Gattr.AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                    {
                        reader.Close();
                        stream.Close();
                        return 0;
                    }
                    str = str.Substring(12);
                    Gattr.FlowNo = TypeToInt(str, 0);
                    if ((Gattr.FlowNo <= 0) || (Gattr.FlowNo >= 0x270f))
                    {
                        str2 = "初始化全局变量FileRead()读取的流水号不在范围[0-9999]！读取值：" + str;
                    }
                }
                else
                {
                    str2 = "读取流水号文件信息失败!";
                }
                reader.Close();
                stream.Close();
            }
            catch (FileNotFoundException exception)
            {
                Gattr.FlowNo = 0;
                str2 = "系统流水号文件不存在！\r\n" + exception.Message;
            }
            catch (Exception exception2)
            {
                str2 = "系统初始化打开流水号文件失败！\n" + exception2.Message;
            }
            if ((str2.Length > 0) && !SetFlowNo())
            {
                return 0;
            }
            return 0;
        }

        public static void OpenCash()
        {
            Gattr.PosPrinter.OpenPrinter(Gattr.PosModel, Gattr.PosPort);
            string cashierNo = Gattr.PosId;
            if (Gattr.PosPrinter.IsWindowPrinter)// && RetailGlobal.IsWinPrtCashBox)
            {
                List<t_pos_printer_out> listOut = new List<t_pos_printer_out>();
                t_pos_printer_out item = new t_pos_printer_out
                {
                    BigFont = false,
                    //  Text = "."
                };
                listOut.Add(item);
                Gattr.PosPrinter.PrintOut(listOut);
            }
            else
            {
                //Gattr.PosCashBox.OpenCashBox();
            }
            //RetailGlobal.Bll.OpenCashBoxNumberAdd(RetailGlobal.CashierNo);
            //RetailGlobal.PosPrinter.ClosePrinter();
        }

        /// <summary>
        /// 获取系统设置
        /// </summary>
        public static void InitPosSysSet()
        {
            List<t_sys_pos_set> posset = Gattr.Bll.GetPosSets();
            if (posset != null && posset.Count > 0)
            {
                foreach (t_sys_pos_set set in posset)
                {
                    switch (set.sys_var_id)
                    {
                        case "member_path":
                            Gattr.MemberServiceUri = set.sys_var_value;
                            break;
                        case "member_token":
                            Gattr.UseToken = set.sys_var_value;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 初始化本地数据库：断网情况下禁用
        /// </summary>
        /// <returns></returns>
        public static bool InitDB()
        {
            bool isok = true;
            bool isup1 = true;
            Dictionary<String, String[]> _dicPath = null;
            try
            {
                _dicPath = new Dictionary<string, string[]>();
                if (!File.Exists(Gattr.ITEM_DB_FILE))
                {
                    String pathItem = AppDomain.CurrentDomain.BaseDirectory + @"DBSQLite\sql\item";
                    String[] path1 = System.IO.Directory.GetFiles(pathItem);
                    _dicPath.Add(Gattr.ITEM_DB_FILE, path1);
                    delUpdate(@"DBSQLite\sql\itemUpdate");
                }
                else
                {
                    isup1 = UpdateDb(Gattr.ITEM_DB_FILE, @"DBSQLite\sql\itemUpdate");
                }
                if (!File.Exists(Gattr.SALE_DB_FILE))
                {
                    String pathSale = AppDomain.CurrentDomain.BaseDirectory + @"DBSQLite\sql\sale";
                    String[] path2 = System.IO.Directory.GetFiles(pathSale);
                    _dicPath.Add(Gattr.SALE_DB_FILE, path2);
                    delUpdate(@"DBSQLite\sql\saleUpdate");

                }
                else
                {
                    isup1 = UpdateDb(Gattr.SALE_DB_FILE, @"DBSQLite\sql\saleUpdate");
                }
                isok = DBCreater.Instance.CreateDB(_dicPath);
                if (!isok || !isup1) isok = false;
            }
            catch
            {
                isok = false;
            }
            return isok;
        }
        /// <summary>
        /// 获取访问服务所需的参数
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> GetServiceParameter()
        {
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("username", Gattr.OperId);
            _dic.Add("password", Gattr.Oper_Pwd);
            _dic.Add("client_id", Gattr.client_id);
            _dic.Add("access_token", Gattr.access_token);
            return _dic;
        }
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="dbName">数据库</param>
        /// <param name="upPath">更新文件夹</param>
        /// <returns></returns>
        private static bool UpdateDb(string dbName, string upPath)    
        {
            bool isok = true;
            //使用itemUpdate文件夹内文件更新数据库
            String pathSaleUp = AppDomain.CurrentDomain.BaseDirectory + upPath;
            if (Directory.Exists(pathSaleUp))//判断目录是否存在
            {
                String[] path3 = System.IO.Directory.GetFiles(pathSaleUp);
                if (path3.Length > 0)
                {
                    //执行SQL语句
                    if (DBCreater.Instance.UpdateSQL(dbName, path3))
                    {
                        //执行成功则删除文件
                        foreach (string p in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + upPath))
                        {
                            File.Delete(p);
                        }
                    }
                    else
                    {
                        isok = false;
                    }
                }
            }
            return isok;
        }
        /// <summary>
        /// 删除更新文件
        /// </summary>
        /// <param name="upPath">更新文件夹</param>
        private static void delUpdate(string upPath)
        {
            //使用itemUpdate文件夹内文件更新数据库
            String pathSaleUp = AppDomain.CurrentDomain.BaseDirectory + upPath;
            if (Directory.Exists(pathSaleUp))//判断目录是否存在
            {
                String[] path3 = System.IO.Directory.GetFiles(pathSaleUp);
                if (path3.Length > 0)
                {
                    //执行成功则删除文件
                    foreach (string p in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + upPath))
                    {
                        File.Delete(p);
                    }
                }
            }
        }


    }
}
