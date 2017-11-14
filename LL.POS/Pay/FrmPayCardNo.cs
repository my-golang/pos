namespace LL.POS
{
    using System;
    using System.Windows.Forms;
    /// <summary>
    /// 银行卡支付界面逻辑
    /// </summary>
    public partial class FrmPayCardNo : FrmBase
    {
        #region 界面全局声名
        /// <summary>
        /// 设置银行卡和备注代理声名 
        /// </summary>
        /// <param name="bcdCard"></param>
        /// <param name="memo"></param>
        /// <param name="payAmount"></param>      
        public delegate void SetBCDCardAndMemoDelegate(String bcdCard, String memo, String payAmount);
        /// <summary>
        /// 设置银行卡和备注事件声名
        /// </summary>
        public event SetBCDCardAndMemoDelegate SetBCDCardAndMemo;
        #endregion
        #region 构造函数
        public FrmPayCardNo()
        {
            InitializeComponent();
        }
        public FrmPayCardNo(string title, string name, decimal payAmt)
        {
            InitializeComponent();
            this.lbTitle.Text = title;
            this.lbCardNo.Text = name;
            this.tbCardNo.Focus();
            this.tbPayAmt.Text = payAmt.ToString(Gattr.PosSaleAmtPoint);
        }
        #endregion
        #region  界面事件业务逻辑
        /// <summary>
        /// 键按下事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPayAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((char.IsNumber(e.KeyChar) || char.IsPunctuation(e.KeyChar)) || char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
            else if (char.IsPunctuation(e.KeyChar))
            {
                if (e.KeyChar == '.')
                {
                    if (((TextBox)sender).Text.LastIndexOf('.') != -1)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 确定按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SetBCDCardAndMemo != null)
            {
                SetBCDCardAndMemo(tbCardNo.Text.Trim(), tbMemo.Text.Trim(), tbPayAmt.Text.Trim());
            }
            DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// 取消按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            //this._isCancel = false;
            //base.Close();
        }
        #endregion


    }
}
