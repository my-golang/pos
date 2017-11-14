
/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-08-05
 * Description:赠品选择窗体
 * Modify:
 * ******************************************************/
namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using LL.POS.Model;
    using System.Windows.Forms;
    public partial class FrmSend : FrmBase
    {
        /// <summary>
        /// 设置赠送信息的代理
        /// </summary>
        /// <param name="saleInfos"></param>
        public delegate void SetSendItem(List<t_cur_saleflow> saleInfos);
        /// <summary>
        /// 声明设置赠送信息的事件
        /// </summary>
        public event SetSendItem SetSend;

        public FrmSend(string showMessage, List<t_cur_saleflow> saleinfos)
        {
            InitializeComponent();
            BindData(showMessage, saleinfos);
        }
        #region 私有方法
        /// <summary>
        /// 绑定赠送商品数据
        /// </summary>
        /// <param name="saleinfos"></param>
        private void BindData(string showMessage, List<t_cur_saleflow> saleinfos)
        {
            this.lbMessage.Text = showMessage;
            this.dgItem.AutoGenerateColumns = false;
            this.dgItem.DataSource = saleinfos;
            for (int i = 0; i < this.dgItem.Rows.Count; i++)
            {
                this.dgItem.Rows[i].Cells[0].Value = true;
            }
        }
        #endregion
        #region 事件逻辑代码
        /// <summary>
        /// 全部商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEx1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgItem.Rows.Count; i++)
            {
                this.dgItem.Rows[i].Cells[0].Value = true;
            }
        }
        /// <summary>
        /// 全部选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEx2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgItem.Rows.Count; i++)
            {
                this.dgItem.Rows[i].Cells[0].Value = false;
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDm_Click(object sender, EventArgs e)
        {
            List<t_cur_saleflow> saleInfos = new List<t_cur_saleflow>();
            for (int i = 0; i < this.dgItem.Rows.Count; i++)
            {
                t_cur_saleflow saleinfo = this.dgItem.Rows[i].DataBoundItem as t_cur_saleflow;
                if (saleinfo.isSelect)
                {
                    saleInfos.Add(saleinfo);
                }
            }
            if (saleInfos.Count == 0)
            {
                if (MessageBox.Show("确定不选择促销商品?", Gattr.AppTitle, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
                else
                {
                    base.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            }
            else
            {
                if (SetSend != null)
                {
                    SetSend(saleInfos);
                }
                base.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        private void dgItem_DataError(object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
        {

        }
        #endregion
    }
}
