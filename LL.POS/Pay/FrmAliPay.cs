using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LL.POS.Common;
using LL.Common;

namespace LL.POS
{
    public partial class FrmAliPay : LL.POS.FrmBase
    {
        private decimal _payAmt=0M;//支付金额
        private decimal _returnMoney = 0M;
        private string Qrcode_url = string.Empty;//支付二维码
        private string flow_no = string.Empty;
        private string Qrcode_Url = string.Empty;
        public FrmAliPay()
        {
            InitializeComponent();
        }

        public FrmAliPay(decimal payAmt,string _flow_no)
        {
            InitializeComponent();
            this._payAmt = payAmt;
            tbPayAmt.Text = Convert.ToString(payAmt);
           // Qrcode_Url = qrcodeurl;
            flow_no = _flow_no;
        }
        /// <summary>
        /// 返回钱数
        /// </summary>
        public decimal ReturnMoney
        {
            get
            {
                return this._returnMoney;
            }
            set
            {
                this._returnMoney = value;
            }
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult msgDialog = MessageBox.Show("请确认客户已成功支付？", Gattr.AppTitle, MessageBoxButtons.YesNo);
            //tbPayAmt.Text
            if (msgDialog == DialogResult.Yes)
            {
                timer1.Enabled = false;
                this._returnMoney = this._payAmt;
                this.DialogResult = DialogResult.OK;
                
            }
            
        }

        /// <summary>
        /// 生成支付二维码
        /// </summary>
        /// <param name="ordername"></param>
        /// <returns></returns>
        public string GetQrcode(String flow_no,String pay_amt)
        {
            String isConnect = String.Empty;
            String address = String.Empty;
            String response = String.Empty;
            String serviceUrl = String.Empty;
            bool isok = true;
            string errorMessage = string.Empty;
            Dictionary<string, object> _dic = new Dictionary<string,object>();
            try
            {
                _dic.Add("username", "");
                _dic.Add("password", "");
                _dic.Add("client_id", Gattr.client_id);
                _dic.Add("access_token", Gattr.access_token);
                _dic.Add("flow_no", flow_no);
                _dic.Add("pay_amount", pay_amt);
                isConnect = PServiceProvider.Instance.InvokeMethod("http://www.jilan.cc/manager/api/ws/API/GetQrcode", _dic, ref isok, ref errorMessage);

                if (isConnect == "error")
                {
                    LoggerHelper.Log("MsmkLogger", Gattr.OperId+"支付二维码生成失败！", LogEnum.SysLog);
                }
                else
                {
                    LoggerHelper.Log("MsmkLogger", Gattr.OperId + "支付二维码生成成功！", LogEnum.SysLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS->FrmAliPay-->GetQrcode:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isConnect;
        }

        //取消支付
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult msgDialog = MessageBox.Show("是否取消支付宝支付？", Gattr.AppTitle, MessageBoxButtons.YesNo);
            //tbPayAmt.Text
            if (msgDialog == DialogResult.Yes)
            {
                timer1.Enabled = false;
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 支付状态查询
        /// </summary>
        /// <param name="ordername"></param>
        /// <returns></returns>
        public string GetPayStatus(String Qrcode)
        {
            String isConnect = String.Empty;
            String address = String.Empty;
            String response = String.Empty;
            String serviceUrl = String.Empty;
            bool isok = true;
            string errorMessage = string.Empty;
            Dictionary<string, object> _dic = new Dictionary<string,object>();
            try
            {
                _dic.Add("username", "");
                _dic.Add("password", "");
                _dic.Add("client_id", Gattr.client_id);
                _dic.Add("access_token", Gattr.access_token);
                _dic.Add("qrcode", Qrcode);
                isConnect = PServiceProvider.Instance.InvokeMethod("http://www.jilan.cc/manager/api/ws/API/GetPayStatus", _dic, ref isok, ref errorMessage);

                if (isConnect == "1")
                {
                    LoggerHelper.Log("MsmkLogger", Gattr.OperId + "查询支付宝支付状态成功！", LogEnum.SysLog);
                }
                else
                {
                    LoggerHelper.Log("MsmkLogger", Gattr.OperId + "查询支付宝支付状态失败！"+isConnect, LogEnum.SysLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS->FrmAliPay-->GetPayStatus:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isConnect;
        }
        //手动查询
        private void Btn_send_Click(object sender, EventArgs e)
        {
            //通过接口查询订单支付状态
            string result = GetPayStatus(flow_no);
            if (result == "1")
            {
                MessageBox.Show("支付成功！", Gattr.AppTitle);
            }
            else if (result == "0")
            {
                MessageBox.Show("支付未成功！",Gattr.AppTitle);
            }
            else
            {
                MessageBox.Show("查询错误！",Gattr.AppTitle);
            }
        }
        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            string result = GetPayStatus(flow_no);
            if (result == "1")
            {
                timer1.Enabled = false;
                this._returnMoney = this._payAmt;
                this.DialogResult = DialogResult.OK;
            }
        }
        //窗体装载
        private void FrmAliPay_Load(object sender, EventArgs e)
        {
            timer1.Interval = 900;
            timer1.Enabled = true;
        }

        private void FrmAliPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                btnCancel_Click(sender, e);
            }
        }
    }
}
