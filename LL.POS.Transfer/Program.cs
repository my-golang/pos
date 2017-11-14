namespace LL.POS.Transfer
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using LL.Common;
    using LL.POS.Common;
    /// <summary>
    /// 数据传输服务窗体逻辑
    /// </summary>
    static class Program
    {
        private static Mutex appMutex;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            try
            {
                bool flag;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                #region 测试
                //GlobalSet.dbsaleconn = GobalStaticString.SQLITE_DB_STRING.Replace("%path%", AppDomain.CurrentDomain.BaseDirectory + "sale.db");
                //GlobalSet.appinifile = "transfer.ini";
                //GlobalSet.appname = "数据传输";
                //GlobalSet.client_id = "POS";
                //GlobalSet.access_token = "c2e3c130b7040fbe18e7f9b319844b42558aeb34";
                //GlobalSet.serverUrl = "http://jilan.cnlilan.com/manager/api/ws/API";//"http://shop.jilan.cc/manager/api/ws/API"
                //GlobalSet.memberUrl = "http://lilanclubapi.cnlilan.com/http/index.php";
                //GlobalSet.mem_access_token = "de9b968de5ba6c10f683661a3a7fe18ac5946ed4";
                //GlobalSet.access_way = "POS";
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new FormMain());
                #endregion
                #region 正式环境部署
                Program.appMutex = new Mutex(true, "LL.POS.Transfer", out flag);
                if (!flag)
                {
                    Program.appMutex.Close();
                    Program.appMutex = null;
                }
                else
                {

                }
                if (args != null && args.Length > 0)
                {
                    GlobalSet.dbsaleconn = GobalStaticString.SQLITE_DB_STRING.Replace("%path%", AppDomain.CurrentDomain.BaseDirectory + args[0].ToString());
                    GlobalSet.appinifile = args[1].ToString();
                    GlobalSet.appname = args[2].ToString() + "数据传输";
                    GlobalSet.client_id = args[3].ToString();
                    GlobalSet.access_token = args[4].ToString();
                    GlobalSet.serverUrl = args[5].ToString();
                    GlobalSet.memberUrl = args[6].ToString();
                    GlobalSet.access_way = args[7].ToString();
                    GlobalSet.mem_access_token = args[8].ToString();
                    //LoggerHelper.Log("MsmkLogger", args[0].ToString() + "----" + args[1].ToString() + "----" + args[2].ToString() + "----" + args[3].ToString() + "----" + args[4].ToString() + "---" + args[5].ToString(), LogEnum.ExceptionLog);
                    //LoggerHelper.Log("MsmkLogger", "数据库字符串:" + args[0].ToString(), LogEnum.ExceptionLog);
                    //LoggerHelper.Log("MsmkLogger", GlobalSet.appinifile + "----" + GlobalSet.appname + "----" + GlobalSet.client_id + "----" + GlobalSet.access_token + "----" + GlobalSet.serverUrl, LogEnum.ExceptionLog);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                else
                {
                    MessageBox.Show("参数错误", GlobalSet.appname);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transfer.Program--->Main-->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        static void OnApplicationExit(object sender, EventArgs e)
        {
            if (Program.appMutex != null)
            {
                Program.appMutex.ReleaseMutex();
                Program.appMutex.Close();
            }
        }
    }
}
