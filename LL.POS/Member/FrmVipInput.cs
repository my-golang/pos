namespace LL.POS
{
    using System;
    using System.Windows.Forms;
    using LL.POS.Model;
    /// <summary>
    /// 会员查询窗体逻辑
    /// </summary>
    public partial class FrmVipInput : FrmBase
    {
        #region 窗体全局声名
        /// <summary>
        /// 设置会员信息代理声名
        /// </summary>
        /// <param name="meminfo">会员信息实体</param>
        public delegate void SetMemberInfo(t_member_info meminfo);
        /// <summary>
        /// 设置会员信息事件声名
        /// </summary>
        public event SetMemberInfo SetMemberEvent;
        #endregion
        #region 窗体初始化
        public FrmVipInput()
        {
            InitializeComponent();
        }
        #endregion
        #region 窗体事件逻辑
        /// <summary>
        /// 输入控件键弹起事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchMember();
            }
        }
        /// <summary>
        /// 禁止输入字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))// && e.KeyChar!=46) 小数点
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 查询按钮事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeach_Click(object sender, EventArgs e)
        {
            SearchMember();
        }
        #endregion
        #region 窗体私有方法
        /// <summary>
        /// 会员查询
        /// </summary>
        public void SearchMember()
        {
            String mem_no = this.txtInput.Text.Trim();
            if (mem_no.Length == 0)
            {
                MessageBox.Show("请输入会员编号", Gattr.AppTitle);
                this.txtInput.Focus();
            }
            else
            {
                t_member_info meminfo = MemberService.Instance.GetMemberInfoByMemNo(mem_no);
                if (meminfo.code == "-1")
                {
                    MessageBox.Show(meminfo.info, Gattr.AppTitle);
                    this.txtInput.Focus();
                }
                else
                {
                    if (meminfo.code == "1")
                    {
                        t_member_info _temp = MemberService.Instance.CheckMemberinfoForLL(meminfo.mem_no);
                        if (_temp != null)
                        {
                            if (_temp.code == "1")
                            {
                                meminfo.mem_type = "WX";
                            }
                        }
                    }
                    else
                    {
                        if (meminfo.code == "404")
                        {
                            meminfo.code = "1";
                            meminfo.mem_no = mem_no;
                        }
                    }
                    if (SetMemberEvent != null)
                    {
                        SetMemberEvent(meminfo);
                    }
                    DialogResult = DialogResult.OK;
                }
                //if (meminfo.code == "1")
                //{
                //    if (SetMemberEvent != null)
                //    {
                //        SetMemberEvent(meminfo);
                //    }
                //    DialogResult = DialogResult.OK;
                //}
                //else
                //{
                //    MessageBox.Show(meminfo.info, Gattr.AppTitle);
                //    this.txtInput.Focus();
                //}
            }
        }
        #endregion

    }
}
