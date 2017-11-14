namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using LL.POS.Model;
    /// <summary>
    /// 退货订单号输入窗体逻辑
    /// </summary>
    public partial class FrmFlowNoSearch : FrmBase
    {
        #region
        /// <summary>
        /// 退货凭证号设置代理声名
        /// </summary>
        /// <param name="_flowNo"></param>
        public delegate void DelegateSetFlowNo(String _flowNo, bool isFlowNo, List<t_cur_saleflow> saleList);
        /// <summary>
        /// 退货凭证号设置事件声名
        /// </summary>
        public event DelegateSetFlowNo EventSetFlowNo;
        #endregion
        #region 窗体构造
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowNo">退货凭证号</param>
        public FrmFlowNoSearch()
        {
            InitializeComponent();
            this.ActiveControl = txtInput;
            txtInput.Focus();
        }
        #endregion
        #region 窗体事件逻辑
        /// <summary>
        /// 确定按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            Search();
        }
        /// <summary>
        /// 取消按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion
        #region 界面私有方法
        /// <summary>
        /// 查询方法
        /// </summary>
        void Search()
        {
            String flowNo = this.txtInput.Text.Trim();
            bool isFlowNo = true;
            List<t_cur_saleflow> salowflow = null;
            List<t_cur_payflow> payflowList = null;
            if (flowNo.Length == 0)
            {
                flowNo = "请输入原始单号";
                isFlowNo = false;
            }
            else
            {
                Int32 result = Gattr.Bll.GetVoucherNoCount(this.txtInput.Text.Trim());
                if (result > 0)
                {
                    flowNo = "当前单号是退单号，不能进行退货";
                    isFlowNo = false;
                }
                else
                {
                    payflowList = Gattr.Bll.GetPayFlowInfoByFlowNo(flowNo);
                    if (payflowList == null||payflowList.Count==0)
                    {
                        flowNo = "没有找到订单号为[" + flowNo + "]的订单信息";
                        isFlowNo = false;
                    }
                    else
                    {
                        salowflow = Gattr.Bll.GetSaleFlowInfoByFlowNo(flowNo);
                        if (salowflow == null || salowflow.Count == 0)
                        {
                            flowNo = "[" + flowNo + "]订单没有退货商品";
                            isFlowNo = false;
                        }
                    }
                }
            }
            if (EventSetFlowNo != null)
            {
                EventSetFlowNo(flowNo, isFlowNo, salowflow);
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion
    }
}
