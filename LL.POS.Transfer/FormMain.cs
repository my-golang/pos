namespace LL.POS.Transfer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using LL.POS.Common;
    using LL.Utility;
    using LL.POS.Model;
    /// <summary>
    /// 传输服务主窗体逻辑
    /// </summary>
    public partial class FormMain : Form
    {
        const int WM_SYSCOMMAND = 0x112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        #region 全局声名
        /// <summary>
        /// 窗体是否创建过
        /// </summary>
        bool windowCreate = true;
        /// <summary>
        /// 处理数据线程
        /// </summary>
        Thread currentThread = null;
        /// <summary>
        /// 处理数据线程是否工作中
        /// </summary>
        bool isWork = false;
        #endregion
        #region 构造函数
        public FormMain()
        {
            InitializeComponent();
            Load += new EventHandler(FormMain_Load);
        }
        #endregion
        /// <summary>
        /// 加载事件逻辑 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormMain_Load(object sender, EventArgs e)
        {
            InitControl();
            InitData();
        }
        #region 窗体事件逻辑
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() == SC_MINIMIZE)
                {
                    windowCreate = false;
                    Visible = false;
                    Hide();
                    ShowInTaskbar = false;
                    return;
                }
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// 托盘图标双击事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ShowOrHide();
            }
        }
        /// <summary>
        /// 查看日志按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLog_Click(object sender, EventArgs e)
        {
            String _path = AppDomain.CurrentDomain.BaseDirectory + GlobalSet.appinifile;
            String _setfolder = "transferlog";
            String _settimespan = WindowsAPIUtility.GetLocalSysParam("posset", "transfer_folder", _setfolder, _path);
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + _settimespan);
        }
        /// <summary>
        /// 处理按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeal_Click(object sender, EventArgs e)
        {
            //TODO:处理数据
            btnDeal.Enabled = false;
            isWork = false;
            if (currentThread != null && currentThread.ThreadState == System.Threading.ThreadState.Running)
            {
                MessageBox.Show("当前程序正在上传数据，请耐心等待再停止！", GlobalSet.appname);
                btnDeal.Enabled = true;
                return;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            DealerData(false, 0);
            Thread.Sleep(1000);
            MessageBox.Show("即时处理完毕", GlobalSet.appname);
            btnDeal.Enabled = true;
            btnStart_Click(null, null);
        }
        /// <summary>
        /// 启动按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            //TODO:启动线程
            if (currentThread != null)
            {
                this.currentThread.Abort();
                this.currentThread = null;
            }
            currentThread = new Thread(new ThreadStart(DoDealerWork));
            currentThread.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }
        /// <summary>
        /// 停止按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            //TODO：停止线程
            isWork = false;
            btnStop.Enabled = false;
            if (currentThread != null && currentThread.ThreadState == System.Threading.ThreadState.Running)
            {
                MessageBox.Show("当前程序正在上传数据，请耐心等待再停止！", GlobalSet.appname);
                btnStop.Enabled = true;
                return;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
        /// <summary>
        /// 退出按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            //TODO:退出
            ExitApplication();
        }
        /// <summary>
        /// 设置按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (tbNum.Enabled)//可用情况下
            {
                //TODO:设置同步时间
                tbNum.Enabled = false;
                tbNum.ReadOnly = true;
                String _path = AppDomain.CurrentDomain.BaseDirectory + GlobalSet.appinifile;
                WindowsAPIUtility.WritePrivateProfileString("posset", "transfer_time", tbNum.Value.ToString(), _path);
            }
            else//不可用
            {
                //TODO:可用
                tbNum.Enabled = true;
                tbNum.ReadOnly = false;
            }
        }
        /// <summary>
        /// 显示/隐藏菜单事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowOrHide();
        }
        /// <summary>
        /// 退出菜单事件逻辑 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }
        /// <summary>
        /// 窗体关闭事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出当前传输程序？", GlobalSet.appname, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            isWork = false;
            if (currentThread != null)
            {
                if (currentThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    if (MessageBox.Show("当前程序正在上传数据，请确定是否退出？", GlobalSet.appname, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        btnStop.Enabled = false;
                        btnStart.Enabled = true;
                        return;
                    }
                }
                currentThread.Abort();
                currentThread = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            notifyIcon1.BalloonTipText = GlobalSet.appname;
            //notifyIcon1.ShowBalloonTip(100, GlobalSet.appname, GlobalSet.appname, ToolTipIcon.Info);
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 退出程序
        /// </summary>
        void ExitApplication()
        {
            Close();
        }
        /// <summary>
        /// 处理数据方法
        /// </summary>
        void DoDealerWork()
        {
            isWork = true;
            try
            {
                while (isWork)
                {
                    if (isWork)
                    {
                        DealerAccount(true);
                        DealerData(true, 0);
                        DealerVip(true);
                    }
                    else
                    {
                        break;
                    }
                    int millsecond = (int)tbNum.Value * 60 * 1000;
                    if (!(Thread.CurrentThread.ThreadState == System.Threading.ThreadState.Running))
                    {
                        Thread.Sleep(millsecond);
                    }
                }
            }
            catch (ThreadAbortException threadex)
            {
                LoggerHelper.Log("MsmkLogger", "FormMain--->DoDealerWork-->Error:" + threadex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "FormMain--->DoDealerWork-->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        /// <summary>
        /// 数据处理公共方法
        /// </summary>
        void DealerData(bool isWorkInvoke, int mark)
        {
            //TODO:处理数据
            //bool isok = true;
            String _branchNo = string.Empty;
            DataTable tableFlowNo = null;
            DataTable tablePayFlow = null;
            DataTable tableSaleFlow = null;
            // bool isok1 = true;
            try
            {
                tableFlowNo = UploadInfoBLL.Instance.GetUploadPayNo(GlobalSet.dbsaleconn);
                if (tableFlowNo != null && tableFlowNo.Rows.Count > 0)
                {
                    foreach (DataRow dr in tableFlowNo.Rows)
                    {
                        tablePayFlow = UploadInfoBLL.Instance.GetUploadPayInfoByFlowNo(GlobalSet.dbsaleconn, dr[0].ToString());
                        tableSaleFlow = UploadInfoBLL.Instance.GetUploadSaleInfoByFlowNo(GlobalSet.dbsaleconn, dr[0].ToString());
                        if ((tablePayFlow != null && tablePayFlow.Rows.Count > 0) || (tableSaleFlow != null && tableSaleFlow.Rows.Count > 0))
                        {
                            StringBuilder sb = new StringBuilder();
                            if (isWorkInvoke)
                            {
                                if (isWork == false)
                                {
                                    break;
                                }
                            }
                            //object result = PosServiceProvider.Instance.UploadPayAndSaleInfo(GlobalSet.posserviceurl, GlobalSet.posservicename, tablePayFlow, tableSaleFlow, ref isok1);
                            Dictionary<string, object> _dic = new Dictionary<string, object>();
                            _dic.Add("username", "");
                            _dic.Add("password", "");
                            _dic.Add("client_id", GlobalSet.client_id);
                            _dic.Add("access_token", GlobalSet.access_token);
                            _dic.Add("pay", JsonUtility.Instance.DataTableToJson(tablePayFlow));
                            _dic.Add("sale", JsonUtility.Instance.DataTableToJson(tableSaleFlow));
                            // LoggerHelper.Log("pay", _dic["pay"].ToString(), LogEnum.TransferLog);
                            // LoggerHelper.Log("sale", _dic["sale"].ToString(), LogEnum.TransferLog);
                            string errorMessage = string.Empty;
                            bool isok11 = true;
                            string isConnect = PServiceProvider.Instance.InvokeMethod(GlobalSet.serverUrl + "/Testconn", _dic, ref isok11, ref errorMessage);
                            if (isok11 && isConnect == "1")
                            {
                                string json = PServiceProvider.Instance.InvokeMethod(GlobalSet.serverUrl + "/" + "Addflow", _dic, ref isok11, ref errorMessage);
                                if (isok11)
                                {
                                    if (json != "-10" && json != "-20")
                                    {
                                        int result = ExtendUtility.Instance.ParseToInt32(json);
                                        if (result == 1)
                                        {
                                            UploadInfoBLL.Instance.UpdateFlowComFlag(GlobalSet.dbsaleconn, dr[0].ToString());
                                            //TODO:记录日志
                                            if (tablePayFlow != null && tablePayFlow.Rows.Count > 0)
                                            {
                                                sb.AppendLine("流水号:" + dr[0].ToString() + "支付记录条数:" + tablePayFlow.Rows.Count);
                                            }
                                            if (tableSaleFlow != null && tableSaleFlow.Rows.Count > 0)
                                            {
                                                sb.AppendLine("流水号:" + dr[0].ToString() + "销售记录条数:" + tableSaleFlow.Rows.Count);
                                            }
                                            LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                        }
                                        else
                                        {
                                            //TODO:记录日志 错误信息
                                            //TODO:记录日志
                                            sb.AppendLine("流水号:" + dr[0].ToString() + "Error:" + result.ToString());
                                            LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                        }
                                    }
                                    else
                                    {
                                        if (json == "-10")
                                        {
                                            sb.AppendLine("流水号:" + dr[0].ToString() + "Error:参数错误");
                                            LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                        }
                                        else
                                        {
                                            sb.AppendLine("流水号:" + dr[0].ToString() + "Error:权限不足");
                                            LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                        }
                                    }
                                }
                                else
                                {
                                    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:" + errorMessage);
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                            }
                            else
                            {
                                //LoggerHelper.Log("MsmkLogger", "断网情况下不进行数据传输", LogEnum.TransferLog);
                            }
                            //object result = PosUploadServiceProvider.Instance.UploadFlow(GlobalSet.ConnectString, ref isok1, tablePayFlow, tableSaleFlow);
                            //if (isok1 == true)
                            //{
                            //    isok = (bool)result;
                            //    if (isok)
                            //    {
                            //        UploadInfoBLL.Instance.UpdateFlowComFlag(GlobalSet.dbsaleconn, dr[0].ToString());
                            //        //TODO:记录日志
                            //        sb.AppendLine("流水号:" + dr[0].ToString() + "支付记录条数:" + tablePayFlow.Rows.Count + "销售记录条数:" + tableSaleFlow.Rows.Count);
                            //        LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                            //    }
                            //    else
                            //    {
                            //        //TODO:记录日志 错误信息
                            //        //TODO:记录日志
                            //        sb.AppendLine("流水号:" + dr[0].ToString() + "Error:" + result.ToString());
                            //        LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                            //    }
                            //}
                            //else
                            //{
                            //    //TODO:记录日志 错误信息
                            //    //TODO:记录日志
                            //    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:" + result.ToString());
                            //    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                            //}
                        }
                    }
                }
                else
                {
                    if (mark == 1)
                    {
                        MessageBox.Show("数据上传完毕", GlobalSet.appname);
                        //button1.Enabled = true;
                        //Application.Exit();
                    }
                }
                DataTable table = UploadInfoBLL.Instance.GetNoneUploadMember(GlobalSet.dbsaleconn);
                if (table != null && table.Rows.Count > 0)
                {
                    //SyncProcessor.Instance.SyncMember(table);
                    foreach (DataRow dr in table.Rows)
                    {
                        string mem_no = ExtendUtility.Instance.ParseToString(dr["card_no"]);
                        string flow_no = ExtendUtility.Instance.ParseToString(dr["flow_no"]);
                        string score = ExtendUtility.Instance.ParseToString(dr["score"]);
                        string voucher_no = ExtendUtility.Instance.ParseToString(dr["voucher_no"]);
                        DateTime oper_date = ExtendUtility.Instance.ParseToDateTime(dr["oper_date"]);
                        string memo = "门店消费";
                        if (!string.IsNullOrEmpty(voucher_no))
                        {
                            memo = "门店退货" + voucher_no;
                        }
                        Dictionary<String, String> _dic = new Dictionary<string, string>();
                        t_member_info t = null;
                        _dic.Add("mem_no", mem_no);
                        if (isWorkInvoke)
                        {
                            if (isWork == false)
                            {
                                break;
                            }
                        }
                        t = SyncProcessor.Instance.InvokeMemberService("get_mem_info", _dic);
                        if (t.code == "1")
                        {
                            //有效会员进行有效积分计算
                            _dic.Add("else", System.Web.HttpUtility.UrlEncode(memo).ToUpper());
                            _dic.Add("score", score);
                            _dic.Add("ordername", flow_no);
                            t = SyncProcessor.Instance.InvokeMemberService("addscore", _dic);
                            if (t.code == "1")
                            {
                                //更新同步标志
                                UploadInfoBLL.Instance.UpdateUploadMember(GlobalSet.dbsaleconn, flow_no, mem_no, "1", "1");
                                LoggerHelper.Log("MsmkLogger", "离线会员消费信息上传成功，流水号:" + flow_no + "会员卡号:" + mem_no,
                         LogEnum.TransferLog);
                            }
                            else
                            {
                                if (t.code == "-1")
                                {
                                    //UploadInfoBLL.Instance.UpdateUploadMember(GlobalSet.dbsaleconn, flow_no, mem_no, "1", "2");
                                    LoggerHelper.Log("MsmkLogger", "离线会员消费信息上传失败，流水号:" + flow_no + "会员卡号:" + mem_no,
                             LogEnum.TransferLog);
                                }
                            }
                        }
                        else
                        {
                            if (t.code == "-1")
                            {
                                //无效会员
                                UploadInfoBLL.Instance.UpdateUploadMember(GlobalSet.dbsaleconn, flow_no, mem_no, "1", "0");
                                LoggerHelper.Log("MsmkLogger", "离线会员消费信息验证失败，流水号:" + flow_no + "会员卡号:" + mem_no,
                         LogEnum.TransferLog);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transfer->FormMain-->DealerData--->" + ex.ToString(), LogEnum.TransferLog);
            }
        }
        /// <summary>
        /// 处理收银员对账信息
        /// </summary>
        /// <param name="isWorkInvoke"></param>
        void DealerAccount(bool isWorkInvoke)
        {
            DataTable tableAcc = null;
            bool isok = true;
            string errorMessage = string.Empty;
            try
            {
                tableAcc = UploadInfoBLL.Instance.GetUploadAccount(GlobalSet.dbsaleconn);
                if (tableAcc != null && tableAcc.Rows.Count > 0)
                {
                    foreach (DataRow dr in tableAcc.Rows)
                    {
                        if (isWorkInvoke)
                        {
                            if (isWork == false)
                            {
                                break;
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        t_pos_account account = new t_pos_account();
                        account.branch_no = ExtendUtility.Instance.ParseToString(dr["branch_no"]);
                        account.end_time = ExtendUtility.Instance.ParseToString(dr["end_time"]);
                        account.oper_date = ExtendUtility.Instance.ParseToString(dr["oper_date"]);
                        account.oper_id = ExtendUtility.Instance.ParseToString(dr["oper_id"]);
                        account.pos_id = ExtendUtility.Instance.ParseToString(dr["pos_id"]);
                        account.sale_amt = ExtendUtility.Instance.ParseToDecimal(dr["sale_amt"]);
                        account.hand_amt = ExtendUtility.Instance.ParseToDecimal(dr["hand_amt"]);
                        account.start_time = ExtendUtility.Instance.ParseToString(dr["start_time"]);
                        List<t_pos_account> acList = new List<t_pos_account>();
                        acList.Add(account);
                        string ac = JsonUtility.Instance.ObjectToJson<t_pos_account>(acList);
                        Dictionary<string, object> _dic = new Dictionary<string, object>();
                        _dic.Add("username", "");
                        _dic.Add("password", "");
                        _dic.Add("client_id", GlobalSet.client_id);
                        _dic.Add("access_token", GlobalSet.access_token);
                        _dic.Add("ac", ac);
                        string json = PServiceProvider.Instance.InvokeMethod(GlobalSet.serverUrl + "/" + "Addposac", _dic, ref isok, ref errorMessage);
                        if (isok)
                        {
                            if (json != "-10" && json != "-20")
                            {
                                int result = ExtendUtility.Instance.ParseToInt32(json);
                                if (result == 1)
                                {
                                    UploadInfoBLL.Instance.UpdateAccountFlag(GlobalSet.dbsaleconn, dr[0].ToString());
                                    sb.Append("门店：" + account.branch_no);
                                    sb.Append("POS机：" + account.pos_id);
                                    sb.Append("收银员：" + account.oper_id);
                                    sb.Append(" 对账上传成功");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                                else
                                {
                                    sb.Append("门店：" + account.branch_no);
                                    sb.Append("POS机：" + account.pos_id);
                                    sb.Append("收银员：" + account.oper_id);
                                    sb.Append(" 对账上传失败" + "Error:" + errorMessage);
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                            }
                            else
                            {
                                if (json == "-10")
                                {
                                    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:参数错误");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                                else
                                {
                                    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:权限不足");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 处理收银员对账信息
        /// </summary>
        /// <param name="isWorkInvoke"></param>
        void DealerVip(bool isWorkInvoke)
        {
            DataTable tableAcc = null;
            bool isok = true;
            string errorMessage = string.Empty;
            try
            {
                tableAcc = UploadInfoBLL.Instance.GetUploadVip(GlobalSet.dbsaleconn);
                if (tableAcc != null && tableAcc.Rows.Count > 0)
                {
                    foreach (DataRow dr in tableAcc.Rows)
                    {
                        if (isWorkInvoke)
                        {
                            if (isWork == false)
                            {
                                break;
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        t_app_viplist vip = new t_app_viplist();
                        vip.flow_no = ExtendUtility.Instance.ParseToString(dr["flow_no"]);
                        vip.card_no = ExtendUtility.Instance.ParseToString(dr["card_no"]);
                        vip.score = ExtendUtility.Instance.ParseToDecimal(dr["score"]);
                        vip.sale_amt = ExtendUtility.Instance.ParseToDecimal(dr["sale_amt"]);
                        vip.voucher_no = ExtendUtility.Instance.ParseToString(dr["voucher_no"]);
                        vip.card_score = ExtendUtility.Instance.ParseToDecimal(dr["card_score"]);
                        vip.card_amount = ExtendUtility.Instance.ParseToDecimal(dr["card_amount"]);
                        vip.oper_date = ExtendUtility.Instance.ParseToString(dr["oper_date"]);
                        List<t_app_viplist> acList = new List<t_app_viplist>();
                        acList.Add(vip);
                        string ac = JsonUtility.Instance.ObjectToJson<t_app_viplist>(acList);
                        Dictionary<string, object> _dic = new Dictionary<string, object>();
                        _dic.Add("username", "");
                        _dic.Add("password", "");
                        _dic.Add("client_id", GlobalSet.client_id);
                        _dic.Add("access_token", GlobalSet.access_token);
                        _dic.Add("vip", ac);
                        string json = PServiceProvider.Instance.InvokeMethod(GlobalSet.serverUrl + "/" + "Addvip", _dic, ref isok, ref errorMessage);
                        if (isok)
                        {
                            if (json != "-10" && json != "-20")
                            {
                                int result = ExtendUtility.Instance.ParseToInt32(json);
                                if (result == 1)
                                {
                                    UploadInfoBLL.Instance.UpdateVipFlag(GlobalSet.dbsaleconn, vip.flow_no, vip.card_no);
                                    sb.Append("流水号：" + vip.flow_no);
                                    sb.Append("会员卡：" + vip.card_no);
                                    sb.Append(" 会员消费上传成功");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                                else
                                {
                                    sb.Append("流水号：" + vip.flow_no);
                                    sb.Append("会员卡：" + vip.card_no);
                                    sb.Append(" 会员消费上传失败" + "Error:" + errorMessage);
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                            }
                            else
                            {
                                if (json == "-10")
                                {
                                    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:参数错误");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                                else
                                {
                                    sb.AppendLine("流水号:" + dr[0].ToString() + "Error:权限不足");
                                    LoggerHelper.Log("MsmkLogger", sb.ToString(), LogEnum.TransferLog);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 显示or隐藏窗体
        /// </summary>
        void ShowOrHide()
        {
            if (windowCreate)//隐藏
            {
                windowCreate = false;
                Hide();
                ShowInTaskbar = false;
            }
            else//显示 
            {
                Visible = true;
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
                BringToFront();
                windowCreate = true;
                Activate();
            }
        }
        /// <summary>
        /// 组件初始化
        /// </summary>
        void InitControl()
        {
            windowCreate = false;
            Visible = false;
            Hide();
            ShowInTaskbar = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }
        /// <summary>
        /// 数据初始化
        /// </summary>
        void InitData()
        {
            try
            {
                String _path = AppDomain.CurrentDomain.BaseDirectory + GlobalSet.appinifile;
                String _xmlpath = "Application.xml";
                string _appname = WindowsAPIUtility.GetLocalSysParam("posset", "transfer_appname", "", _path);
                //GlobalSet.appname = _appname;
                Text = GlobalSet.appname;
                notifyIcon1.Text = GlobalSet.appname;
                notifyIcon1.BalloonTipText = GlobalSet.appname;
                notifyIcon1.ShowBalloonTip(100, GlobalSet.appname, GlobalSet.appname, ToolTipIcon.Info);
                String _applicationXml = WindowsAPIUtility.GetLocalSysParam("posset", "app_xml", _xmlpath, _path); ;
                _xmlpath = AppDomain.CurrentDomain.BaseDirectory + _applicationXml;
                Dictionary<String, String> _dic = XMLUtility.Instance.GetNodesText(_xmlpath, new string[] { "//applicationConfig/BranchNo" });
                if (_dic == null || _dic["//applicationConfig/BranchNo"] == null || _dic["//applicationConfig/BranchNo"].ToString() == string.Empty)
                {
                    MessageBox.Show("POS机注册信息不存在或有误，请运行程序注册POS机！", GlobalSet.appname);
                    Process.GetCurrentProcess().Kill();
                }
                currentThread = new Thread(DoDealerWork);
                currentThread.Start();
                Decimal _time = 1M;
                String _timespan = "1";
                String _settimespan = WindowsAPIUtility.GetLocalSysParam("posset", "transfer_time", _timespan, _path);
                Decimal.TryParse(_settimespan, out _time);
                _time = _time < 1M ? 1M : _time;
                tbNum.Value = _time;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "FormMain--->InitData-->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        #endregion

        private void FormMain_MinimumSizeChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isWork = false;
            //btnStop.Enabled = false;
            if (currentThread != null && currentThread.ThreadState == System.Threading.ThreadState.Running)
            {
                MessageBox.Show("当前程序正在上传数据，请耐心等待再停止！", GlobalSet.appname);
                btnStop.Enabled = true;
                return;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            try
            {
                //button1.Enabled = false;
                DealerVip(false);
                DealerAccount(false);
                DealerData(false, 1);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "FormMain--->手动上传-->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }

    }
}
