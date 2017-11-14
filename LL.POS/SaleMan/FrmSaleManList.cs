namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using LL.POS.Model;
    /// <summary>
    /// 营业员列表窗体逻辑
    /// </summary>
    public partial class FrmSaleManList : FrmBase
    {
        #region 全局声名
        /// <summary>
        /// 操作人设置代理声名
        /// </summary>
        /// <param name="_operatorId">营业员ID</param>
        /// <param name="_operatorName">营业员名称</param>
        public delegate void SetOperatorInfoListDelegate(String _operatorId, String _operatorName);
        /// <summary>
        /// 操作人设置事件声名
        /// </summary>
        public event SetOperatorInfoListDelegate SetOperatorInfoList;
        /// <summary>
        /// 营业员列表
        /// </summary>
        List<t_operator> _currentList;
        #endregion
        #region 构造函数逻辑
        public FrmSaleManList(List<t_operator> list)
        {
            InitializeComponent();
            _currentList = list;
        }
        #endregion
        #region 窗体事件逻辑
        /// <summary>
        /// 窗体首次加载事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSaleManList_Load(object sender, EventArgs e)
        {
            this.dgvOperList.AutoGenerateColumns = false;
            this.bindingOperList.DataSource = _currentList;
            this.dgvOperList.DataSource = this.bindingOperList;
            this.dgvOperList.Refresh();
        }
        /// <summary>
        /// 确定按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectOperator();
        }
        /// <summary>
        /// 列表键弹起事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPenOrd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SelectOperator();
            }
        }
        /// <summary>
        /// 列表双击事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvOperList_DoubleClick(object sender, EventArgs e)
        {
            SelectOperator();
        }
        /// <summary>
        /// 取消/关闭按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion
        #region 窗体私有方法
        /// <summary>
        /// 选择营业员
        /// </summary>
        void SelectOperator()
        {
            String _operatorId = "9999";
            String _operatorName = "默认营业员";
            if (this.dgvOperList.CurrentRow.Index >= 0)
            {
                if (this.dgvOperList.Rows[this.dgvOperList.CurrentRow.Index].Cells["oper_id"].Value != null)
                {
                    _operatorId = dgvOperList.Rows[this.dgvOperList.CurrentRow.Index].Cells["oper_id"].Value.ToString();
                    _operatorName = dgvOperList.Rows[this.dgvOperList.CurrentRow.Index].Cells["full_name"].Value.ToString();
                }
            }
            if (SetOperatorInfoList != null)
            {
                SetOperatorInfoList(_operatorId, _operatorName);
            }
            DialogResult = DialogResult.OK;
        }
        #endregion
    }
}
