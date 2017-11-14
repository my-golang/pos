using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LL.POS.Common;

namespace LL.POS
{
    public partial class FrmFeedBack : LL.POS.FrmBase
    {
        public FrmFeedBack()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmFeedBack_Load(object sender, EventArgs e)
        {
            this.Text = Gattr.AppTitle + "_" + "提交反馈";
        }
        /// <summary>
        /// 提交反馈事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEx1_Click(object sender, EventArgs e)
        {
            bool isok = true;
            String errorMessage = String.Empty;
            try
            {
                if (this.richTextBox1.Text.Trim().Length == 0)
                {
                    MessageBox.Show("请输入反馈信息", Gattr.AppTitle);
                }
                else if (this.richTextBox1.Text.Trim().Length > 50)
                {
                    MessageBox.Show("反馈信息不能超过50个字", Gattr.AppTitle);
                }
                else
                {
                    Dictionary<string, object> _dic = Gfunc.GetServiceParameter();
                    _dic.Add("branch_no", Gattr.BranchNo);
                    _dic.Add("posid", Gattr.PosId);
                    _dic.Add("oper_id", Gattr.OperId);
                    _dic.Add("content", this.richTextBox1.Text.Trim());
                    String _result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Testconn", _dic, ref isok, ref errorMessage);
                    if (isok)
                    {
                        isok = false;
                        switch (_result)
                        {
                            case "-10":
                                errorMessage = "参数不正确";
                                break;
                            case "-20":
                                errorMessage = "权限不足";
                                break;
                            case "1":
                                isok = true;
                                String _content = this.richTextBox1.Text.Trim();
                                _result = string.Empty;
                                _result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Addfeedback", _dic, ref isok, ref errorMessage);
                                if (isok)
                                {
                                    isok = false;
                                    switch (_result)
                                    {
                                        case "-10":
                                            errorMessage = "参数不正确";
                                            break;
                                        case "-20":
                                            errorMessage = "权限不足";
                                            break;
                                        case "1":
                                            isok = true;
                                            errorMessage = "反馈提交成功";
                                            break;
                                        default:
                                            errorMessage = "反馈提交失败";
                                            break;
                                    }
                                }
                                break;
                            default:
                                if (errorMessage.Length == 0)
                                {
                                    errorMessage = "数据返回错误";
                                }
                                break;
                        }
                        MessageBox.Show(errorMessage, Gattr.AppTitle);
                        if (isok)
                        {
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                    else
                    {
                        LoggerHelper.Log("MsmkLogger", "LL.POS->FrmFeedBack-->Exception:" + errorMessage, LogEnum.ExceptionLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS->FrmFeedBack-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        /// <summary>
        /// 取消提交反馈事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEx2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
