namespace LL.POS
{
    using System.Data;
    using System.Windows.Forms;
    using LL.Utility;
    public partial class FrmComSel : FrmBase
    {
        /// <summary>
        /// 设置查询公共控件的值代理
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public delegate void DelegateSetComSel(string code, string name);
        /// <summary>
        /// 设置公共查询的值事件
        /// </summary>
        public event DelegateSetComSel SetComSelEvent;
        /// <summary>
        /// 查询类型
        /// </summary>
        private string _type = "0";
        /// <summary>
        /// 查询名称
        /// </summary>
        private string _key = "分类";
        /// <summary>
        /// 公共查询窗体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="keyword">关键字</param>
        /// <param name="type">类型:商品分类0商品品牌1供应商2</param>
        /// <param name="condtion">查询条件，默认为空</param>
        public FrmComSel(string title, string keyword, string type, string condtion = "")
        {
            InitializeComponent();
            this.Text = Gattr.AppTitle + "-" + title;
            this.tbKey.Text = "关键字可以是" + keyword + "编号、" + keyword + "名称";
            this._key = keyword;
            this._type = type;
            this.dgvBranch.Columns[0].DataPropertyName = "code";
            this.dgvBranch.Columns[0].HeaderText = this._key + "编号";
            this.dgvBranch.Columns[0].Width = 100;
            this.dgvBranch.Columns[1].DataPropertyName = "name";
            this.dgvBranch.Columns[1].HeaderText = this._key + "名称";
            this.dgvBranch.Columns[1].Width = 180;
            btnSearch_Click(null, null);
        }
        /// <summary>
        /// 键盘按键事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmComSel_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.F1:
                    this.tbKey.Focus();
                    break;
                case System.Windows.Forms.Keys.Enter:
                    btnSearch_Click(null, null);
                    break;
                //case System.Windows.Forms.Keys.Escape:
                //DialogResult = System.Windows.Forms.DialogResult.No;
                //    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 文本框enter事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbKey_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
            else
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = System.Windows.Forms.DialogResult.No;
                }
            }
        }
        /// <summary>
        /// 查询按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            string keyword = string.Empty;
            if (this.tbKey.Text == "关键字可以是" + this._key + "编号、" + this._key + "名称")
            {
                keyword = "";
            }
            else
            {
                keyword = this.tbKey.Text.Trim();
            }
            DataTable table = Gattr.Bll.GetCommonSelData(this._type, keyword);
            this.dgvBranch.AutoGenerateColumns = false;
            this.dgvBranch.DataSource = table;
            this.dgvBranch.Focus();
        }
        private void dgvBranch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter || e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                dgvBranch_MouseClick(null, null);
            }
            else
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = System.Windows.Forms.DialogResult.No;
                }
            }
        }
        private void tbKey_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.tbKey.Text.Equals("关键字可以是" + this._key + "编号、" + this._key + "名称"))
            {
                this.tbKey.Text = string.Empty;
            }
        }
        private void dgvBranch_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void dgvBranch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.dgvBranch.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择" + this._key, Gattr.AppTitle);
            }
            else
            {
                DataGridViewRow dgvr = this.dgvBranch.SelectedRows[0];
                if (dgvr != null)
                {
                    if (dgvr.DataBoundItem != null)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        if (SetComSelEvent != null)
                        {
                            SetComSelEvent(ExtendUtility.Instance.ParseToString(dgvr.Cells["code"].Value),
                            ExtendUtility.Instance.ParseToString(dgvr.Cells["name"].Value));
                        }
                    }
                }
            }
        }
    }
}
