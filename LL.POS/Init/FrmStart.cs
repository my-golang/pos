
namespace LL.POS
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using LL.POS.Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text.RegularExpressions;
    using LL.Utility;

    /// <summary>
    /// 程序启动主窗体
    /// </summary>
    public partial class FrmStart : Form
    {
        /// <summary>
        /// 设置控件是否显示代理声名
        /// </summary>
        /// <param name="isSet">是否显示</param>
        /// <returns></returns>
        public delegate void SetControlVisibleDelegate(bool isSet);
        /// <summary>
        /// 
        /// </summary>
        public bool IsLogined = false;
        #region 窗体构造
        public FrmStart()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(106, 195, 240);
            this.Load += new System.EventHandler(FrmStart_Load);
        }
        #endregion
        #region 窗体事件处理逻辑
        /// <summary>
        /// 窗体加载事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FrmStart_Load(object sender, System.EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Action));
            thread.Start();
        }
        #endregion
        #region 窗体私有方法
        /// <summary>
        /// 加载所有信息
        /// </summary>
        void Action()
        {
            //首先判断门店信息,如果没有则提示初始化门店信息
            //然后根据门店信息和POS机信息，初始化POS机信息
            System.Threading.Thread.Sleep(500);
            //bool isok = false;
            try
            {
                SetControlVisible(false);
                LoggerHelper.Log("MsmkLogger", "信息初始化开始Starting...", LogEnum.InitLog);
                long result = Gfunc.InitEnvironment();
                switch (result)
                {
                    case -2:
                        LoggerHelper.Log("MsmkLogger", "基本配置信息初始化失败Error", LogEnum.InitLog);
                        break;
                    case -1:
                        LoggerHelper.Log("MsmkLogger", "取消基本配置信息初始化Cancel", LogEnum.InitLog);
                        break;
                    case -3:
                        System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + Gattr.ApplicationXml);
                        //Application.Exit();
                        Application.Restart();
                        return;
                    //FrmInitData frmData = new FrmInitData();
                    //frmData.ShowDialog();
                    //this.Close();
                    //break;
                }
                SetControlVisible(true);
                if (result < 0)
                {
                    if (result == -1)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("基本配置信息初始化失败", Gattr.AppTitle);
                    }
                    return;
                }
                else
                {
                    LoggerHelper.Log("MsmkLogger", "基本配置信息加载完成", LogEnum.InitLog);
                }

                Thread.Sleep(500);
                SetControlVisible(false);
                LoggerHelper.Log("MsmkLogger", "基本数据信息初始化开始Start", LogEnum.InitLog);
                Dictionary<string, object> _dic = Gfunc.GetServiceParameter();
                string errorMessage = string.Empty;
                bool isok = true;
                //通过 ping 判断丢包率
                decimal d = ExtendUtility.Instance.ParseToDecimal(RunCmd(Gattr.serverUrl));
                string res = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Testconn", _dic, ref isok, ref errorMessage);
                if (isok && res == "1"&&d<10)
                {
                    Gattr.NetStatus = "【联网】";
                    FrmSyncPosData frmData = new FrmSyncPosData(true);
                    if (frmData.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //TODO:数据初始化完成 显示主登录窗体
                        goto TOLOGIN;
                    }
                    else
                    {
                        LoggerHelper.Log("MsmkLogger", "取消基本数据信息初始化Cancel", LogEnum.InitLog);
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("服务器连接失败，将以断网方式进行销售。");
                    Gattr.NetStatus = "【断网】";
                    goto TOLOGIN;
                }

            TOLOGIN:
                Gattr.BranchName = Gattr.Bll.GetBranchNameByNo(Gattr.BranchNo);
                Gattr.AppTitle = Gattr.BranchName == string.Empty ? ConfigurationManager.AppSettings["appName"] : Gattr.BranchName;
                Gattr.adUrl = (Gattr.adUrl == string.Empty) ? AppDomain.CurrentDomain.BaseDirectory + "AD.htm" : Gattr.adUrl;
                //TODO:数据初始化完成 显示主登录窗体
                LoggerHelper.Log("MsmkLogger", "基本数据信息初始化完成Complete", LogEnum.InitLog);
                Process[] processesByName = Process.GetProcessesByName("LL.POS.Transfer");
                Process[] array3 = processesByName;
                for (int i = 0; i < array3.Length; i++)
                {
                    Process process = array3[i];
                    process.Kill();
                    process.WaitForExit();
                    process.Close();
                    process.Dispose();
                }
                String path = AppDomain.CurrentDomain.BaseDirectory + "LL.POS.Transfer.exe";
                //String dbpath = AppDomain.CurrentDomain.BaseDirectory + "sale.db";
                String dbpath = "sale.db";
                Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    Arguments =
                        dbpath + " " +
                        "transfer.ini" + " " +
                        Gattr.AppTitle + " " +
                        Gattr.client_id + " " +
                        Gattr.access_token + " " +
                        Gattr.serverUrl + " " +
                        Gattr.MemberServiceUri + " " +
                        Gattr.ServiceWay + " " +
                        Gattr.UseToken
                });
                FrmLogin frmLogin = new FrmLogin(this);
                frmLogin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        /// <summary>
        /// 设置控件隐藏和显示
        /// </summary>
        /// <param name="isVisible"></param>
        void SetControlVisible(bool isVisible)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new SetControlVisibleDelegate(SetControlVisible), isVisible);
            }
            else
            {
                base.Visible = isVisible;
            }
        }
        /// <summary>
        /// ping 检测丢包率
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private string RunCmd(string url)
        {
            Process process = new Process();//实例
            process.StartInfo.CreateNoWindow = true;//设定不显示窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "cmd.exe"; //设定程序名  
            process.StartInfo.RedirectStandardInput = true;   //重定向标准输入
            process.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
            process.StartInfo.RedirectStandardError = true;//重定向错误输出
            process.Start();
            Uri u = new Uri(url);
            string command = "ping " + u.Host;
            process.StandardInput.WriteLine(command);//执行的命令
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            StreamReader myStreamReader = process.StandardOutput;
            process.StandardOutput.Read();
            string output = myStreamReader.ReadToEnd();
            process.Close();
            Regex reg = new Regex(@"(?<dd>\d+)%\s+丢失\)，");
            Match match = reg.Match(output);
            output = "0";
            if (match.Success)
            {
                output = match.Groups["dd"].Value;
            }
            return output;

        }
        #endregion
    }
}
