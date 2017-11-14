

namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using System.IO;
    using LL.POS.Model;
    using LL.POS.Common;
    using System.Security.Permissions;
    /// <summary>
    /// 美食美客主窗体逻辑
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class FrmDoubleDisplay : Form
    {
        private List<t_cur_saleflow> _listSaleFlow;
        public List<string> adp_list;
        public List<string> av_file;
        private FrmMain _frmMain;
        public int curradp;
        public int currfil;
        public int l_height;
        public int l_width;
        public string alipay_no = string.Empty;
        public string html2 = string.Empty;

        public FrmDoubleDisplay(FrmDoubleDisplay frmLogin)
        {
            InitializeComponent();
        }
        public FrmDoubleDisplay()
        {
            this._listSaleFlow = null;
            this._frmMain = null;
            this.currfil = 0;
            this.curradp = 0;
            this.components = null;
            this.InitializeComponent();
        }

        public FrmDoubleDisplay(FrmMain frmmain, FrmStart _frmStart)
        {
            this._listSaleFlow = null;
            this._frmMain = null;
            this.currfil = 0;
            this.curradp = 0;
            this.components = null;
            this.InitializeComponent();
            this._frmMain = frmmain;
            if (Gattr.IsDoubleSetwh)
            {
                try
                {
                    this.l_width = Gattr.DoubleSetwidth;
                    this.l_height = Gattr.DoubleSetheight;
                }
                catch
                {
                    this.l_width = this._frmMain.Width;
                    this.l_height = this._frmMain.Height;
                }
                base.Width = this.l_width;
                base.Height = this.l_height;
                base.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width, 0);
            }
            else
            {
                base.Width = this._frmMain.Width;
                base.Height = this._frmMain.Height;
                base.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width, 0);
            }

            this.plpay.Location = new Point(2, (base.Height - this.plpay.Height) - 2);
            this.tblCardInfo.Location = new Point(2, ((base.Height - this.plpay.Height) - this.tblCardInfo.Height) - 3);
            this.tblCardInfo.Width = this.plSale.Width;
            this.plSale.Location = new Point(2, 0x29);
            this.plSale.Height = ((base.Height - this.plpay.Height) - this.tblCardInfo.Height) - 6;
            this.plpay.Width = this.plSale.Width;

            this.webBrowser1.Location = new Point((this.plSale.Location.X + this.plSale.Width) + 2, this.plSale.Location.Y);
            this.webBrowser1.Width = (base.Width - this.webBrowser1.Location.X) - 2;
            this.webBrowser1.Height = ((base.Height - this.plSale.Location.Y) - this.tbAd.Height) - 4;
            this.tbAd.Location = new Point(this.webBrowser1.Location.X, (this.webBrowser1.Height + 2) + this.plSale.Location.Y);
            this.tbAd.Width = this.webBrowser1.Width;
            this.av_file = new List<string>();
            this.adp_list = new List<string>();
            try
            {
                //this.webBrowser1.Url = new Uri(Gattr.adUrl, UriKind.Absolute);
                if (_frmStart.InvokeRequired)
                {
                    _frmStart.Invoke(new EventHandler(delegate
                     {
                         this.webBrowser1.Navigate(Gattr.adUrl);
                     }));
                }
                else
                {
                    this.webBrowser1.Navigate(Gattr.adUrl);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "FrmDoubleDisplay--->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }

        private long DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan span = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts = new TimeSpan(DateTime2.Ticks);
            TimeSpan span3 = span.Subtract(ts).Duration();
            return (long)((((((span3.Days * 0x18) * 60) * 60) + ((span3.Hours * 60) * 60)) + (span3.Minutes * 60)) + span3.Seconds);
        }
        private void FrmDoubleDisplay_Load(object sender, EventArgs e)
        {
            ///设置多屏
            Screen[] sc = Screen.AllScreens;
            if (sc.Length == 2)
            {
                this.Location = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ColorChange));
                thread.IsBackground = true;
                thread.Start();
                string str4;
                this._listSaleFlow = this._frmMain._listSaleFlow;
                this.bindingSaleFlow.DataSource = this._listSaleFlow;
                this.GvSaleFlow.DataSource = this.bindingSaleFlow;
                base.ShowInTaskbar = false;
                short doubleColorRed = Gattr.DoubleColorRed;
                short doubleColorGreen = Gattr.DoubleColorGreen;
                short doubleColorBlue = Gattr.DoubleColorBlue;
                this.BackColor = Color.FromArgb(doubleColorRed, doubleColorGreen, doubleColorBlue);
                this.plSaleFlow.BackColor = Color.FromArgb(doubleColorRed, doubleColorGreen, doubleColorBlue);
                this.plpay.BackColor = Color.FromArgb(doubleColorRed, doubleColorGreen, doubleColorBlue);
                this.tbAd.BackColor = Color.FromArgb(doubleColorRed, doubleColorGreen, doubleColorBlue);
                this.GvSaleFlow.BackgroundColor = Color.FromArgb(doubleColorRed, doubleColorGreen, doubleColorBlue);
                int length = 0;
                string str3 = "欢迎光临";//Gattr.Adp_list;
                length = str3.IndexOf("&");
                if (length >= 0)
                {
                    while (length >= 0)
                    {
                        str4 = str3.Substring(0, length);
                        this.adp_list.Add(str4);
                        length = str3.Substring(length + 1).IndexOf("&");
                    }
                }
                else
                {
                    this.adp_list.Add("欢迎您的光临！！！");
                }
                if (this.adp_list.Count > 0)
                {
                    this.tbAd.Text = this.adp_list[0];
                }

                this.timer1.Enabled = true;
                this.timer1.Interval = 3000;
                this.currNow = DateTime.Now;
            }
        }
        //lable字体颜色变化
        private void ColorChange()
        {
            Random random = new Random();
            while (true)
            {
                int red = random.Next(1, 256);
                int green = random.Next(1, 256);
                int blue = random.Next(1, 256);
                this.tbAd.ForeColor = Color.FromArgb(red, green, blue);
                System.Threading.Thread.Sleep(500);
            }
        }
        private void GvSaleFlow_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                t_cur_saleflow _saleflow = this.bindingSaleFlow[e.RowIndex] as t_cur_saleflow;
                SolidBrush brush = new SolidBrush(Color.FromArgb(0xff, 0xfb, 240));
                if (this.bindingSaleFlow.Position == e.RowIndex)
                {
                    brush = new SolidBrush(Color.FromArgb(224, 224, 224));
                }
                e.Graphics.FillRectangle(brush, e.RowBounds);
                SolidBrush brush2 = new SolidBrush(Color.FromArgb(0x80, 0x80, 0x80));
                SolidBrush brush3 = new SolidBrush(Color.Black);
                e.Graphics.DrawRectangle(new Pen(brush2), e.RowBounds);
                Rectangle layoutRectangle = new Rectangle(e.RowBounds.X, e.RowBounds.Y + 5, (e.RowBounds.Width / 3) + 0x10, e.RowBounds.Height / 2);
                e.Graphics.DrawString(_saleflow.item_no, this.Font, brush3, layoutRectangle);
                Rectangle rectangle2 = new Rectangle(layoutRectangle.X + layoutRectangle.Width, layoutRectangle.Y, e.RowBounds.Width - layoutRectangle.Width, layoutRectangle.Height);
                e.Graphics.DrawString(_saleflow.item_name, this.Font, brush3, rectangle2);
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Far
                };
                Rectangle rectangle3 = new Rectangle(layoutRectangle.X, layoutRectangle.Y + layoutRectangle.Height, layoutRectangle.Width, layoutRectangle.Height);
                e.Graphics.DrawString(_saleflow.sale_qnty.ToString(Gattr.PosSaleNumPoint), this.Font, brush3, rectangle3, format);
                Rectangle rectangle4 = new Rectangle(rectangle2.X, rectangle3.Y, rectangle2.Width / 2, layoutRectangle.Height);
                e.Graphics.DrawString(_saleflow.sale_price.ToString(Gattr.PosSalePrcPoint), this.Font, brush3, rectangle4, format);
                Rectangle rectangle5 = new Rectangle(rectangle2.X + rectangle4.Width, rectangle3.Y, rectangle2.Width / 2, layoutRectangle.Height);
                e.Graphics.DrawString(_saleflow.sale_money.ToString(Gattr.PosSaleAmtPoint), this.Font, brush3, rectangle5, format);
            }
        }

        private void GvSaleFlow_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < this.GvSaleFlow.RowCount; i++)
            {
                if (this.GvSaleFlow.Rows[i].Height < this.GvSaleFlow.ColumnHeadersHeight)
                {
                    this.GvSaleFlow.Rows[i].Height = this.GvSaleFlow.ColumnHeadersHeight;
                }
            }
        }
        /// <summary>
        /// 设置双屏显示
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="TotalAmt">总金额</param>
        /// <param name="RemainAmt">应收金额</param>
        /// <param name="PaedAmt">实收金额</param>
        /// <param name="ChgAmt">找零</param>
        /// <param name="TotalQty">总数量</param>
        /// <param name="cardid">会员卡号</param>
        ///<param name="money">会员余额</param>
        ///<param name="score">会员积分</param>
        public void SetDoubleDisplay(string state, string TotalAmt, string RemainAmt, string PaedAmt, string ChgAmt, string TotalQty, string cardid, string money,string score)
        {
            this._listSaleFlow = this._frmMain._listSaleFlow;
            this.bindingSaleFlow.DataSource = this._listSaleFlow;
            this.GvSaleFlow.DataSource = this.bindingSaleFlow;
            this.bindingSaleFlow.ResetBindings(true);
            this.bindingSaleFlow.Position = this.bindingSaleFlow.Count - 1;
            this.GvSaleFlow.Refresh();
            if (state == "1")
            {
                this.lbBTotalAmt.Text = TotalAmt;
                this.lbRemainAmt.Text = RemainAmt;
                this.lbPaedAmt.Text = "0.00";
                this.lbChgAmt.Text = "0.00";
            }
            else
            {
                this.lbBTotalAmt.Text = TotalAmt;
                this.lbRemainAmt.Text = RemainAmt;
                this.lbPaedAmt.Text = PaedAmt;
                this.lbChgAmt.Text = ChgAmt;
            }
            this.lbTotalQty.Text = "数量和:" + TotalQty;
            this.lbTotalAmt.Text = "金额和:" + TotalAmt;
            this.lbCard.Text = "卡 号:" + cardid;
            this.lbMoney.Text = "金 额：" + money;
            this.lbScore.Text = "积 分：" + score;
            //if (Gattr.IsVipNoInvisible && (cardid != string.Empty))
            //{
            //    this.lbCard.Text = "卡 号: ******* ";
            //}
        }
        /// <summary>
        /// 设置URL
        /// </summary>
        /// <param name="url"></param>
        public void SetWebBrowserUrl(string url)
        {
            //if (url == null)
            //{
            //    pictureBox1.Visible = false;
            //}
            //else
            //{
            //    pictureBox1.ImageLocation = url;
            //    pictureBox1.Visible = true;
            //}
            this.webBrowser1.Navigate(url);
        }
        /// <summary>
        /// 设置二维码显示
        /// </summary>
        /// <param name="url"></param>
        public void SetWebDocument(string html)
        {
            this.webBrowser1.DocumentText = html;
        }
        //截取顾显WebBrowser界面
        public Bitmap GetWebBrowserScreen()
        {
            //创建高度和宽度与网页相同的图片
            Bitmap bitmap = new Bitmap(webBrowser1.Width, webBrowser1.Height);
            //绘图区域
            Rectangle rectangle = new Rectangle(0, 0, webBrowser1.Width, webBrowser1.Height);
            //截图
            webBrowser1.DrawToBitmap(bitmap, rectangle);
            return bitmap;
        }
        private void tbAd_Click(object sender, EventArgs e)
        {
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if ((this.av_file.Count > 0) && (this.axWindowsMediaPlayer1.status.IndexOf("已停止") >= 0))
            //{
            //    if (this.currfil < (this.av_file.Count - 1))
            //    {
            //        this.currfil++;
            //    }
            //    else if (this.currfil == (this.av_file.Count - 1))
            //    {
            //        this.currfil = 0;
            //    }
            //    this.axWindowsMediaPlayer1.URL = this.av_file[this.currfil];
            //}
            if ((this.adp_list.Count > 0) && (this.DateDiff(DateTime.Now, this.currNow) > 3))
            {
                if (this.curradp < (this.adp_list.Count - 1))
                {
                    this.curradp++;
                }
                else if (this.curradp == (this.adp_list.Count - 1))
                {
                    this.curradp = 0;
                }
                this.tbAd.Text = this.adp_list[this.curradp];
                this.currNow = DateTime.Now;
            }
        }

        private void GvSaleFlow_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
