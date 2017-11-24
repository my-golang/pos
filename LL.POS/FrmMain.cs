namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using LL.POS.Common;
    using LL.POS.Model;
    using LL.Utility;
    using System.IO;
    using Newtonsoft.Json.Linq;
    using System.Threading;
    /// <summary>
    /// 美食美客主窗体逻辑
    /// </summary>
    public partial class FrmMain : Form
    {
        #region 变量值
        private int _xItemNum; //商品列表横向显示数量
        private int _yItemNum;//商品列表纵向显示数量
        private Size _myButtonSize = new Size(80, 80);//商品按钮大小
        private int _spaceItemLen = 5; //商品列表按钮间隔距离
        private Font _myButtonFont = new Font("宋体", 15f, FontStyle.Bold); //商品按钮字体
        private Color _itemColor = Color.FromArgb(210, 210, 0xd7); //商品按钮背影色
        private int _bigClsNum; //大类显示数量
        private int _spaceLen = 2; //大类，小类按钮之产的间距
        /// <summary>
        /// 小类显示数量
        /// </summary>
        private int _smallClsNum;
        /// <summary>
        /// 大、小类背景色
        /// </summary>
        private Color _clsColor = Color.FromArgb(255, 232, 234);
        /// <summary>
        /// 流水GridView背景色
        /// </summary>
        private Color _colorGridBackGround = Color.FromArgb(255, 255, 255);
        /// <summary>
        /// 流水GridView选择行背景色
        /// </summary>
        private Color _colorGridRowSelected = Color.FromArgb(245, 245, 245);
        /// <summary>
        /// 销售商品
        /// </summary>
        private t_cur_saleflow _saleflow;
        /// <summary>
        /// 销售商品列表
        /// </summary>
        public List<t_cur_saleflow> _listSaleFlow;
        /// <summary>
        /// 付款金额列表
        /// </summary>
        public List<t_cur_payflow> _listPayFlow;
        /// <summary>
        /// 挂起付款金额列表
        /// </summary>
        public List<t_pending_payflow> _listPendingPayFlow;
        /// <summary>
        /// 备份销售商品列表
        /// </summary>
        public List<t_cur_saleflow> _backupSalesBeforeTot = null;
        /// <summary>
        /// 商品总价
        /// </summary>
        private decimal _saleTotalAmt = 0M;
        /// <summary>
        /// 商品折扣总价
        /// </summary>
        private decimal _saleZheKouAmt = 0M;
        bool IsInputNum = false;
        /// <summary>
        /// 选购商品总数量
        /// </summary>
        private string GlobalNum = "0";
        /// <summary>
        /// 公用自符串
        /// </summary>
        private string _inputText = "";
        /// <summary>
        /// 支付状态
        /// </summary>
        private bool IsPayState = false;
        /// <summary>
        /// 登录窗口
        /// </summary>
        private FrmLogin _frmLogin = null;
        /// <summary>
        /// POS机状态
        /// </summary>       
        private PosOpState _posState = PosOpState.PLU;
        /// <summary>
        /// 默认支付方式RMB
        /// </summary>
        string defaultPayWay = "RMB";
        /// <summary>
        /// 是否找零钱
        /// </summary>
        bool _isCHG = false;
        /// <summary>
        /// 是否返款
        /// </summary>
        bool _isFk = true;
        /// <summary>
        /// 打印字符串数组
        /// </summary>
        List<PrintString> _listWinPrtStr = new List<PrintString>();
        /// <summary>
        /// VIP状态
        /// </summary>
        bool _isVip = false;
        /// <summary>
        /// VIP信息
        /// </summary>
        //t_vip_info _vipInfo = null;
        /// <summary>
        /// 尾款
        /// </summary>
        decimal _balancePayAmt = 0M;
        /// <summary>
        /// 退货凭证号
        /// </summary>
        String _currentFlowNo = string.Empty;
        /// <summary>
        /// 退货操作余额付款金额
        /// </summary>
        decimal moneyCHA = 0;
        /// <summary>
        ///当前进行交易会员
        /// </summary>
        t_member_info _currentMember = null;
        private Size _sizeInputText; //大键盘
        private int _inputDisplayMax = 13;//大键盘输入最大数
        /// <summary>
        /// 网络连接状态线程
        /// </summary>
        Thread nonParameterThread = null;
        /// <summary>
        /// 双屏窗体
        /// </summary>
        private FrmDoubleDisplay frmdouble = null;

        private FrmStart _frmStart = null;
        #endregion
        #region 窗体构造
        public FrmMain(FrmLogin frmLogin, FrmStart frmStart)
        {
            InitializeComponent();
            this._frmLogin = frmLogin;
            this._frmStart = frmStart;
        }
        #endregion
        #region 窗体事件处理逻辑
        #region 窗体LOAD事件
        /// <summary>
        /// 窗体LOAD事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            lbApplication.Text = Gattr.AppTitle;
            ///设置大键盘
            this.tlpCenter.Controls.Add(this.tblNum, 1, 0);

            this.InitTouchScreenForItem();
            this.InitTouchScreenForClsBig();
            this.InitTouchScreenForClsSmall();
            this.SetGridViewColumns();
            if (this.GvSaleFlow.Width >= this.tblPanelBalance.Width)
            {
                this.tblPanelBalance.Left = ((base.Width - this.tblPanelBalance.Width) / 2) + 5;
            }
            else
            {
                this.tblPanelBalance.Left = (base.Width - this.tblPanelBalance.Width) / 2;
            }
            this.tblPanelBalance.Top = (base.Height - this.tblPanelBalance.Height) / 2;
            this.plOtherFunc.Location = new Point((this.tlpBottom.Width / 10) * 8, (Screen.PrimaryScreen.Bounds.Height - this.tlpBottom.Height) - this.plOtherFunc.Height);
            Gfunc.InitPosKeyTag();
            Gfunc.SetFlowNo();
            Gattr.FunKeys = Gattr.Bll.GetFuncKeys(false);
            this.SetBtnKey();
            Gattr.Bll.DeleteTempNonSaleData();
            this.ClsBigPage(true, true);
            this._listSaleFlow = Gattr.Bll.GetTempSaleRow();
            this._listPayFlow = Gattr.Bll.GetTempPayRow();
            this.bindingSaleFlow.DataSource = this._listSaleFlow;
            this.GvSaleFlow.DataSource = this.bindingSaleFlow;
            this.bindingPayFlow.DataSource = this._listPayFlow;
            this.GvPayFlow.DataSource = this.bindingPayFlow;
            this.ResetSaleState(true, false);
            this.ShowMessage("商品开始交易...");
            this.ResetPayCursor();
            this.ResetSaleCursor();
            this.SetCursorText();
            this.MouseClickReset(this);
            this.lbCheckStandValue.Text = "【" + Gattr.PosId + "】";
            this.lbShopAssistantValue.Text = "【" + Gattr.OperId + "】" + Gattr.OperFullName;
            if (Gattr.NetStatus == "【联网】")
            {
                this.labelNetStatus.ForeColor = Color.Green;
            }
            else
            {
                this.labelNetStatus.ForeColor = Color.Red;
            }
            this.labelNetStatus.Text = Gattr.NetStatus;
            this.InitDouble();
            GetNetStatus();
        }
        #endregion
        /// <summary>
        /// 销售流水控件行重绘事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvSaleFlow_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts = DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.Border | DataGridViewPaintParts.Background;
        }
        /// <summary>
        /// 销售流水控件行添加事件逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvSaleFlow_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < this.GvSaleFlow.RowCount; i++)
            {
                if (this.GvSaleFlow.Rows[i].Height < (this.GvSaleFlow.Height / 8))
                {
                    this.GvSaleFlow.Rows[i].Height = this.GvSaleFlow.Height / 8;
                }
            }
        }
        /// <summary>
        /// 获取网络连接状态
        /// </summary>
        private void GetNetStatus()
        {
            // update controls on UI thread  
            nonParameterThread = new Thread(new ThreadStart(SetNetStatus));
            nonParameterThread.Start(); 
        }
        /// <summary>
        /// 网络连接状态显示
        /// </summary>
        private void SetNetStatus()
        {
            while (this.IsHandleCreated)
            {
                Dictionary<string, object> _dic = Gfunc.GetServiceParameter();
                string errorMessage = string.Empty;
                bool isok = true;
                string res = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/Testconn", _dic, ref isok, ref errorMessage);
                if (isok && res == "1")
                {
                   
                    labelNetStatus.Invoke((MethodInvoker)delegate
                    {
                        this.labelNetStatus.Text = "【联网】";
                        this.labelNetStatus.ForeColor = Color.Green;
                    });
                }
                else
                {
                        labelNetStatus.Invoke((MethodInvoker)delegate
                        {
                            this.labelNetStatus.Text = "【断网】";
                            this.labelNetStatus.ForeColor = Color.Red;
                        });
                    
                }
                Thread.Sleep(10000);
            }
        }
        #endregion
        #region 窗体私有方法
        #region 操作业务验证
        /// <summary>
        /// 自动检测当前操作是否可以继续进行
        /// </summary>
        /// <param name="_keyId">操作时键标示</param>
        /// <returns>可以继续true否则false</returns>
        bool CheckIsCanOperate(PosOpType posType)
        {
            bool isCan = true;
            switch (posType)
            {
                case PosOpType.数量:
                case PosOpType.删除:
                case PosOpType.单价:
                case PosOpType.折扣:
                    if (_posState != PosOpState.PLU)
                    {
                        isCan = false;
                    }
                    if (this._listSaleFlow.Count == 0)
                    {
                        isCan = false;
                    }
                    else
                    {
                        if (posType == PosOpType.折扣)
                        {
                            if ((_posState != PosOpState.PLU) && (_posState != PosOpState.PAY))
                            {
                                isCan = false;
                            }//折扣只在销售和付款情况下启用
                        }
                    }
                    break;
                case PosOpType.会员:
                    if (_posState != PosOpState.PLU)
                    {
                        isCan = false;
                        this.ClearInput();
                    }
                    for (int index = 0; index < _listSaleFlow.Count; index++)//退货时不能进行该操作
                    {
                        if (_listSaleFlow[index].sale_way == "B")
                        {
                            isCan = false;
                            break;
                        }
                    }
                    break;
                case PosOpType.PLU:
                    for (int index = 0; index < _listSaleFlow.Count; index++)//退货时不能进行该操作
                    {
                        if (_listSaleFlow[index].sale_way == "B")
                        {
                            isCan = false;
                            break;
                        }
                    }
                    break;
                case PosOpType.退货://退货时不能有没有退货的商品存在
                    BeforePLU();
                    for (int index = 0; index < _listSaleFlow.Count; index++)
                    {
                        if (_listSaleFlow[index].sale_way != "B")
                        {
                            isCan = false;
                            break;
                        }
                    }
                    break;
                case PosOpType.交易查询:
                    BeforePLU();
                    if (_listSaleFlow.Count > 0)
                    {
                        isCan = false;
                    }
                    break;
                case PosOpType.营业员://结算过程中不能切换营业员
                    if (_posState == PosOpState.PAY)
                    {
                        isCan = false;
                    }
                    break;
                case PosOpType.挂单:
                    if (_posState != PosOpState.PLU)
                    {
                        isCan = false;
                    }
                    else
                    {
                        BeforePLU();
                        for (int index = 0; index < _listSaleFlow.Count; index++)//退货时不能进行该操作
                        {
                            if (_listSaleFlow[index].sale_way == "B")
                            {
                                isCan = false;
                            }
                        }     
                    }
                    break;
                case PosOpType.其他:
                    if (_posState != PosOpState.PLU)
                    {
                        isCan = false;
                    }
                    break;
                default:
                    break;
            }
            return isCan;
        }
        #endregion
        #region 商品分类、商品初始化
        /// <summary>
        /// 初始化商品大类页面
        /// </summary>
        /// <returns></returns>
        private bool InitTouchScreenForClsBig()
        {
            this._bigClsNum = this.plClsBig.Height / (this._myButtonSize.Height + this._spaceLen);
            if (this._bigClsNum <= 0)
            {
                return false;
            }
            this.plClsBig.Controls.Clear();
            for (int i = 0; i < this._bigClsNum; i++)
            {
                ButtonForPos pos = new ButtonForPos
                {
                    BackColor = Color.Transparent,
                    Font = this._myButtonFont,
                    Location = new Point(3, (this._myButtonSize.Height * i) + (this._spaceLen * (i + 1))),
                    Size = new Size(80, 80),//this.btnClsNextPage.Width, this._myButtonSize.Height),
                    Text = "大类" + i.ToString()
                };
                pos.Click += new EventHandler(this.btnTouchScreenForBigCls_Click);
                pos.Visible = false;
                pos.BaseColor = this._clsColor;
                this.plClsBig.Controls.Add(pos);
            }
            return true;
        }
        /// <summary>
        /// 初始化大类数据和分页
        /// </summary>
        /// <param name="isNextPage"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        private bool ClsBigPage(bool isNextPage, bool isFirst)
        {
            string clsNo = string.Empty;
            if (!isFirst)
            {
                ButtonForPos panelButton = this.GetPanelButton(this.plClsBig, isNextPage);
                if ((panelButton == null) && (this.plClsBig.Controls.Count > 0))
                {
                    panelButton = this.plClsBig.Controls[this.plClsBig.Controls.Count - 1] as ButtonForPos;
                }
                if (panelButton.Tag == null)
                {
                    return false;
                }
                clsNo = panelButton.Tag.ToString().Trim();
            }
            //获取分类数据
            DataTable itemDt = Gattr.Bll.GetBigClsPage(clsNo, this._bigClsNum, isNextPage);
            if (itemDt.Rows.Count > 0)
            {
                this.SetTouchScreen(this.plClsBig, itemDt, "typeno", "typename", 0);
            }
            else
            {
                return false;
            }
            if (this.plClsBig.Controls.Count > 0)
            {
                if ((this.plClsBig.Controls[0] as ButtonForPos).Tag.ToString().Trim() == string.Empty)
                {
                    return false;
                }
                this.btnTouchScreenForBigCls_Click(this.plClsBig.Controls[0] as ButtonForPos, null);
            }
            return false;
        }
        #endregion
        #endregion
        #region 商品大类分页事件
        /// <summary>
        /// 商品大类分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTouchScreenForBigCls_Click(object sender, EventArgs e)
        {
            ButtonForPos pos = sender as ButtonForPos;
            //this.plOtherFunc.Visible = false;
            if ((pos != null) && (pos.Tag != null))
            {
                string str = SIString.TryStr(pos.Tag).Trim();
                switch (str)
                {
                    case "BigCls&PriorPage":
                        this.ClsBigPage(false, false);
                        return;

                    case "BigCls&NextPage":
                        this.ClsBigPage(true, false);
                        return;
                }
                //设置大类面板的Tag不等空，初始化小类列表
                if (str != string.Empty)
                {
                    this.plClsBig.Tag = str;
                    //设置小类
                    if (this.ClsSmallPage(true, true))
                    {
                        int count = this.plClsSmall.Controls.Count;
                    }
                }
            }
        }
        #endregion
        #region 初始化商品小类页面
        /// <summary>
        /// 初始化商品小类页面
        /// </summary>
        /// <returns></returns>
        private bool InitTouchScreenForClsSmall()
        {
            //小类显示数量
            this._smallClsNum = this.plClsSmall.Width / (this._myButtonSize.Width + this._spaceLen);
            if (this._smallClsNum <= 0)
            {
                return false;
            }
            this.plClsSmall.Controls.Clear();
            //循环向面板上添加按钮。
            for (int i = 0; i < this._smallClsNum; i++)
            {
                ButtonForPos pos = new ButtonForPos
                {
                    Location = new Point((this._myButtonSize.Width * i) + (this._spaceLen * (i + 1)), this.btnClsSmallRight.Location.Y),
                    Width = this._myButtonSize.Width,
                    Height = this.btnClsSmallRight.Height,
                    Font = this._myButtonFont,
                    Text = "小类" + i.ToString()
                };
                pos.Click += new EventHandler(this.btnTouchScreenForSmallCls_Click);
                pos.Visible = false;
                pos.BaseColor = this._clsColor;
                this.plClsSmall.Controls.Add(pos);
            }
            return true;
        }
        #endregion
        #region 初始化商品小类数据和分页
        /// <summary>
        /// 始化商品小类数据和分页
        /// </summary>
        /// <param name="isNextPage"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        private bool ClsSmallPage(bool isNextPage, bool isFirst)
        {

            if (this.plClsBig.Tag != null)
            {
                string clsNo = string.Empty;
                if (!isFirst)
                {
                    ButtonForPos panelButton = this.GetPanelButton(this.plClsSmall, isNextPage);
                    if (panelButton == null)
                    {
                        if (this.plClsSmall.Controls.Count <= 0)
                        {
                            return false;
                        }
                        panelButton = this.plClsSmall.Controls[this.plClsSmall.Controls.Count - 1] as ButtonForPos;
                    }
                    if (panelButton.Tag == null)
                    {
                        return false;
                    }
                    clsNo = SIString.TryStr(panelButton.Tag).Trim();
                }
                if (string.IsNullOrEmpty(clsNo))
                {
                    clsNo = ExtendUtility.Instance.ParseToString(this.plClsBig.Tag);
                }
                DataTable itemDt = Gattr.Bll.GetClsPage(SIString.TryStr(this.plClsBig.Tag).Trim(), clsNo, this._smallClsNum, isNextPage);
                if (itemDt.Rows.Count > 0)
                {
                    this.SetTouchScreen(this.plClsSmall, itemDt, "typeno", "typename", 0);
                }
                else
                {
                    //this.plClsSmall.Controls.Clear();
                    for (int i = 0; i < this.plClsSmall.Controls.Count; i++)
                    {
                        this.plClsSmall.Controls[i].Visible = false;
                    }
                    this.plClsSmall.Tag = clsNo;
                    if (this.ItemPage(true, true))
                    {
                        int count = this.plItem.Controls.Count;
                    }
                    return false;
                }
                if (this.plClsSmall.Controls.Count > 0)
                {
                    if (SIString.TryStr((this.plClsSmall.Controls[0] as ButtonForPos).Tag).Trim() == string.Empty)
                    {
                        return false;
                    }
                    this.btnTouchScreenForSmallCls_Click(this.plClsSmall.Controls[0] as ButtonForPos, null);
                }
            }
            return false;
        }
        #endregion
        #region 商品小类点击事件
        private void btnTouchScreenForSmallCls_Click(object sender, EventArgs e)
        {
            ButtonForPos pos = sender as ButtonForPos;
            //this.plOtherFunc.Visible = false;
            if ((pos != null) && (pos.Tag != null))
            {
                string str = SIString.TryStr(pos.Tag).Trim();
                switch (str)
                {
                    case "SmallCls&PriorPage":
                        this.ClsSmallPage(false, false);
                        return;

                    case "SmallCls&NextPage":
                        this.ClsSmallPage(true, false);
                        return;
                }
                //如果小类的tag不等空,加载商品列表
                if (str != string.Empty)
                {
                    this.plClsSmall.Tag = str;
                    if (this.ItemPage(true, true))
                    {
                        int count = this.plItem.Controls.Count;
                    }
                }
            }
        }
        #endregion
        #region 初始化商品列表
        /// <summary>
        /// 初始化商品列表
        /// </summary>
        /// <returns></returns>
        private bool InitTouchScreenForItem()
        {
            this._xItemNum = this.plItem.Width / (this._myButtonSize.Width + this._spaceItemLen);
            this._yItemNum = this.plItem.Height / (this._myButtonSize.Height + this._spaceItemLen);
            int num = (this.plItem.Width - ((this._myButtonSize.Width + this._spaceItemLen) * this._xItemNum)) / 2;
            int num2 = (this.plItem.Height - ((this._myButtonSize.Height + this._spaceItemLen) * this._yItemNum)) / 2;
            if (num > num2)
            {
                num = num2;
            }
            else
            {
                num2 = num;
            }
            if ((this._xItemNum <= 0) && (this._yItemNum <= 0))
            {
                return false;
            }
            this.plItem.Controls.Clear();
            for (int i = 0; i < this._yItemNum; i++)
            {
                for (int j = 0; j < this._xItemNum; j++)
                {
                    ButtonForPos pos = new ButtonForPos
                    {
                        Location = new Point((((this._spaceItemLen / 2) + num) + (this._myButtonSize.Width * j)) + (this._spaceItemLen * j), (((this._spaceItemLen / 2) + num2) + (this._myButtonSize.Height * i)) + (this._spaceItemLen * i)),
                        Width = this._myButtonSize.Width,
                        Height = this._myButtonSize.Height,
                        Font = this._myButtonFont,
                        Text = "商品" + i.ToString() + "_" + j.ToString()
                    };
                    pos.Click += new EventHandler(this.btnTouchScreenForItem_Click);
                    pos.BaseColor = this._itemColor;
                    pos.IsItem = true;
                    pos.Visible = false;
                    if (i == (this._yItemNum - 1))
                    {
                        if (j == (this._xItemNum - 1))
                        {
                            pos.Visible = true;
                            pos.MyText = ">";
                            pos.Tag = "Item&NextPage";
                        }
                        else if (j == (this._xItemNum - 2))
                        {
                            pos.Visible = true;
                            pos.MyText = "<";
                            pos.Tag = "Item&PriorPage";
                        }
                    }
                    this.plItem.Controls.Add(pos);
                }
            }
            return true;
        }
        #endregion
        #region 商品数据和分页
        /// <summary>
        ///  商品数据和分页
        /// </summary>
        /// <param name="isNextPage"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        private bool ItemPage(bool isNextPage, bool isFirst)
        {
            if (this.plClsSmall.Tag != null)
            {
                // string itemNo = string.Empty;
                string itemNo = ExtendUtility.Instance.ParseToString(this.plClsSmall.Tag);
                if (!isFirst)
                {
                    ButtonForPos panelButton = this.GetPanelButton(this.plItem, isNextPage);
                    if ((panelButton == null) && (this.plItem.Controls.Count > 0))
                    {
                        panelButton = this.plItem.Controls[this.plItem.Controls.Count - 3] as ButtonForPos;
                    }
                    if (panelButton.Tag == null)
                    {
                        return false;
                    }
                    itemNo = SIString.TryStr(panelButton.Tag).Trim();
                }
                DataTable itemDt = Gattr.Bll.GetItemPage(SIString.TryStr(this.plClsSmall.Tag).Trim(), itemNo, (this._xItemNum * this._yItemNum) - 2, isNextPage);
                if (itemDt.Rows.Count > 0)
                {
                    this.SetTouchScreen(this.plItem, itemDt, "item_no", "item_name", Gattr.Bll.GetClsItemCount(SIString.TryStr(this.plClsSmall.Tag).Trim()));
                }
            }
            return false;
        }

        #endregion
        #region 商品分页事件
        /// <summary>
        /// 商品分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTouchScreenForItem_Click(object sender, EventArgs e)
        {
            ButtonForPos pos = sender as ButtonForPos;
            //this.plOtherFunc.Visible = false;
            if ((pos != null) && (pos.Tag != null))
            {
                string str = SIString.TryStr(pos.Tag).Trim();
                if (str == "Item&PriorPage")
                {
                    this.ItemPage(false, false);
                }
                else if (str == "Item&NextPage")
                {
                    this.ItemPage(true, false);
                }
                else
                {
                    this._inputText = str;
                    t_attr_function keyFunc = this.GetKeyFunc("plu");
                    if (keyFunc != null)
                    {
                        this.CallKeyFunc(keyFunc);
                    }
                }
            }
        }
        #endregion
        #region 查找控件
        /// <summary>
        /// 查找控件
        /// </summary>
        /// <param name="myPl">要查找的Panel</param>
        /// <param name="isNextPage">是否分页</param>
        /// <returns></returns>
        private ButtonForPos GetPanelButton(PanelEx myPl, bool isNextPage)
        {
            if (myPl.Controls.Count > 0)
            {
                if (!isNextPage)
                {
                    if ((myPl.Controls[0] as ButtonForPos).Visible)
                    {
                        return (myPl.Controls[0] as ButtonForPos);
                    }
                    return null;
                }
                foreach (Control control in myPl.Controls)
                {
                    if ((control is ButtonForPos) && !(control as ButtonForPos).Visible)
                    {
                        return (control as ButtonForPos);
                    }
                }
            }
            return null;
        }
        #endregion
        #region 设置触摸屏
        /// <summary>
        /// 设置触摸屏
        /// </summary>
        /// <param name="myPl">要设置的Panel</param>
        /// <param name="itemDt">表信息</param>
        /// <param name="colForNo">类别ID</param>
        /// <param name="colForName">类别名称</param>
        /// <param name="realQty"></param>
        /// <returns></returns>
        private bool SetTouchScreen(PanelEx myPl, DataTable itemDt, string colForNo, string colForName, int realQty)
        {
            if (itemDt == null)
            {
                return false;
            }
            if (itemDt.Rows.Count == 0)
            {
                return false;
            }
            if (myPl.Controls.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < myPl.Controls.Count; i++)
            {
                ButtonForPos pos = myPl.Controls[i] as ButtonForPos;
                // MessageBox.Show(pos.Name);
                if ((SIString.TryStr(pos.Tag).Trim() == "Item&NextPage") || (SIString.TryStr(pos.Tag).Trim() == "Item&PriorPage"))
                {
                    if (realQty > (myPl.Controls.Count - 2))
                    {
                        pos.Visible = true;
                    }
                    else
                    {
                        pos.Visible = false;
                    }
                }
                else if (i < itemDt.Rows.Count)
                {
                    ButtonForPos pos2 = myPl.Controls[i] as ButtonForPos;
                    pos2.MyUpText = itemDt.Rows[i][colForNo].ToString().Trim();
                    pos2.MyText = itemDt.Rows[i][colForName].ToString().Trim();
                    pos2.Tag = itemDt.Rows[i][colForNo].ToString().Trim();
                    pos2.Visible = true;
                }
                else
                {
                    ButtonForPos pos3 = myPl.Controls[i] as ButtonForPos;
                    pos3.MyText = string.Empty;
                    pos3.Tag = null;
                    pos3.Visible = false;
                }
            }
            return true;
        }
        #endregion
        #region 初始化销售列表
        /// <summary>
        /// 初始化销售列表
        /// </summary>
        private void SetGridViewColumns()
        {
            DataGridViewCellStyle style = null;
            Font font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                HeaderText = "行号",
                Name = "flow_id",
                DataPropertyName = "Flow_id",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = 50,
                Resizable = DataGridViewTriState.False,

            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Font = font
            };
            column.DefaultCellStyle = style;
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn
            {
                HeaderText = "品名",
                Name = "item_name",
                DataPropertyName = "Last_item_name",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Font = font,
                WrapMode = DataGridViewTriState.True
            };
            column2.DefaultCellStyle = style;
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn
            {
                HeaderText = "数量",
                Name = "sale_qnty",
                DataPropertyName = "Sale_qnty",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvSaleFlow.Width - column.Width) * 0.12M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = Gattr.PosSaleNumPoint,
                Font = font
            };
            column3.DefaultCellStyle = style;
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn
            {
                HeaderText = "原价",
                Name = "source_price",
                DataPropertyName = "Sale_price",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvSaleFlow.Width - column.Width) * 0.13M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = Gattr.PosSalePrcPoint,
                Font = font
            };
            column4.DefaultCellStyle = style;
            column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn
            {
                HeaderText = "单价",
                Name = "sale_price1",
                DataPropertyName = "Unit_price",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvSaleFlow.Width - column.Width) * 0.13M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = Gattr.PosSalePrcPoint,
                Font = font
            };
            column5.DefaultCellStyle = style;
            column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn
            {
                HeaderText = "小计",
                Name = "sale_money",
                DataPropertyName = "Sale_money",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = Gattr.PosSaleAmtPoint,
                Font = font
            };
            column6.DefaultCellStyle = style;
            column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.Width = 30;
            column2.MinimumWidth = (int)((this.GvSaleFlow.Width - column.Width) * 0.3M);
            column3.MinimumWidth = (int)((this.GvSaleFlow.Width - column.Width) * 0.13M);
            column4.MinimumWidth = (int)((this.GvSaleFlow.Width - column.Width) * 0.17M);
            column5.MinimumWidth = (int)((this.GvSaleFlow.Width - column.Width) * 0.17M);
            column6.MinimumWidth = (int)((this.GvSaleFlow.Width - column.Width) * 0.22M);
            this.GvSaleFlow.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            DataGridViewCellStyle style2 = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(0, 4, 0, 4),
                Font = font
            };
            this.GvSaleFlow.ColumnHeadersDefaultCellStyle = style2;
            this.GvSaleFlow.BorderStyle = BorderStyle.None;
            this.GvSaleFlow.ColumnHeadersVisible = false;
            this.GvSaleFlow.RowHeadersVisible = false;
            this.GvSaleFlow.Columns.AddRange(new DataGridViewColumn[] { column, column2, column3, column4, column5, column6 });
            this.GvSaleFlow.AutoGenerateColumns = false;
            this.GvSaleFlow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.GvSaleFlow.Enabled = true;
            this.GvSaleFlow.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.GvSaleFlow.BackgroundColor = this._colorGridBackGround;
            this.GvSaleFlow.GridColor = this._colorGridBackGround;
            DataGridViewTextBoxColumn column7 = new DataGridViewTextBoxColumn
            {
                HeaderText = "行号",
                Name = "flow_id",
                DataPropertyName = "flow_id",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = 60,
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Font = font
            };
            column7.DefaultCellStyle = style;
            DataGridViewTextBoxColumn column8 = new DataGridViewTextBoxColumn
            {
                HeaderText = "付款方式",
                Name = "pay_name",
                DataPropertyName = "pay_name",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvPayFlow.Width - column7.Width) * 0.23M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Font = font
            };
            column8.DefaultCellStyle = style;
            DataGridViewTextBoxColumn column9 = new DataGridViewTextBoxColumn
            {
                HeaderText = "汇率",
                Name = "coin_rate",
                DataPropertyName = "coin_rate",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvPayFlow.Width - column7.Width) * 0.25M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = "N2",
                Font = font
            };
            column9.DefaultCellStyle = style;
            DataGridViewTextBoxColumn column10 = new DataGridViewTextBoxColumn
            {
                HeaderText = "付款金额",
                Name = "pay_amount",
                DataPropertyName = "pay_amount",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = (int)((this.GvPayFlow.Width - column7.Width) * 0.2M),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = "N2",
                Font = font
            };
            column10.DefaultCellStyle = style;
            DataGridViewTextBoxColumn column11 = new DataGridViewTextBoxColumn
            {
                HeaderText = "折算金额",
                Name = "convert_amt",
                DataPropertyName = "convert_amt",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Width = ((((this.GvPayFlow.Width - column7.Width) - column8.Width) - column9.Width) - column10.Width),
                Resizable = DataGridViewTriState.False
            };
            style = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                BackColor = this._colorGridBackGround,
                ForeColor = this.ForeColor,
                SelectionBackColor = this._colorGridRowSelected,
                SelectionForeColor = this.ForeColor,
                Format = "N2",
                Font = font
            };
            column11.DefaultCellStyle = style;
            this.GvPayFlow.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            DataGridViewCellStyle style3 = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(0, 4, 0, 4),
                Font = font
            };
            this.GvPayFlow.ColumnHeadersDefaultCellStyle = style3;
            this.GvSaleFlow.BorderStyle = BorderStyle.None;
            this.GvPayFlow.RowHeadersVisible = false;
            this.GvPayFlow.Columns.AddRange(new DataGridViewColumn[] { column7, column8, column10, column9, column11 });
            this.GvPayFlow.ScrollBars = ScrollBars.Vertical;
            this.GvPayFlow.AutoGenerateColumns = false;
            this.GvPayFlow.BackgroundColor = this._colorGridBackGround;
            this.GvPayFlow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.GvPayFlow.Enabled = true;
            this.GvPayFlow.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }
        #endregion
        #region 初始化销售列表数据
        /// <summary>
        /// 初始化销售列表数据
        /// </summary>
        /// <param name="bOnLoading"></param>
        /// <param name="bSetFlowNo"></param>
        private void ResetSaleState(bool bOnLoading, bool bSetFlowNo)
        {
            if (bOnLoading)
            {
                this.SetTotalAmt();
            }
        }
        #endregion
        #region 初始化 数量、删除、退出等按钮
        /// <summary>
        /// 初始化 数量、删除、退出等按钮
        /// </summary>
        private void SetBtnKey()
        {
            try
            {
                this.btnFastNum.MyText = string.Format("数量 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastNum.Tag)).pos_key).ToLower());
                this.btnFastDel.MyText = string.Format("删除 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastDel.Tag)).pos_key).ToLower());
                this.btnFastPrc.MyText = string.Format("单价 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastPrc.Tag)).pos_key).ToLower());
                this.btnFastDct.MyText = string.Format("折扣 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastDct.Tag)).pos_key).ToLower());
                this.btnFastPnr.MyText = string.Format("挂单 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastPnr.Tag)).pos_key).ToLower());
                this.btnFastVip.MyText = string.Format("会员 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastVip.Tag)).pos_key).ToLower());
                this.btnFastDou.MyText = string.Format("营业员 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastDou.Tag)).pos_key).ToLower());
                this.btnFastTot.MyText = string.Format("结算 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastTot.Tag)).pos_key).ToLower());
                this.btnFastOther.MyText = string.Format("其它 {0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnFastOther.Tag)).pos_key).ToLower());
                this.btnFastExit.MyText = "退出 Esc";
                this.btnPicFuncPayEsc.MyUpText = "Esc";
                //if (this.btnPicFunAllcan.Tag.ToString() == "can")
                //{
                //    this.btnPicFunAllcan.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr("del")).pos_key).ToLower());
                //}
                //if (this.btnPicFunAllDis.Tag.ToString() == "ads")
                //{
                //    this.btnPicFunAllDis.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr("dct")).pos_key).ToLower());
                //}
                //if (this.btnPicFunPayOther.Tag.ToString() == "oth")
                //{
                //    this.btnPicFunPayOther.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr("oth")).pos_key).ToLower());
                //}
                //this.btnPicFunBcd.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnPicFunBcd.Tag)).pos_key).ToLower());
                //this.btnPicFunSav.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnPicFunSav.Tag)).pos_key).ToLower());
                //this.btnPicFunCou.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnPicFunCou.Tag)).pos_key).ToLower());
                //this.btnPicFunRmb.MyUpText = string.Format("{0}", SIString.TryStr(this.GetKeyFunc(SIString.TryStr(this.btnPicFunRmb.Tag)).pos_key).ToLower());
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }
        }
        #endregion
        #region 查询快捷键方法
        /// <summary>
        /// 查询快捷键方法
        /// </summary>
        /// <param name="funcName"></param>
        /// <returns></returns>
        private t_attr_function GetKeyFunc(string funcName)
        {
            foreach (KeyValuePair<string, t_attr_function> pair in Gattr.FunKeys)
            {
                if (SIString.TryStr(pair.Value.func_id).Trim().ToLower() == funcName.ToLower())
                {
                    return pair.Value;
                }
            }
            return null;
        }

        #endregion
        #region 双屏显示
        /// <summary>
        /// 出初始化双屏
        /// </summary>
        /// <returns></returns>
        public bool InitDouble()
        {
            Screen[] sc = Screen.AllScreens;
            if (sc.Length == 2)
            {
                //双屏开关
                if (Gattr.IsDoubleModule)
                {
                    if (this._frmStart != null && this._frmStart.InvokeRequired)
                    {
                        this._frmStart.Invoke(new EventHandler(delegate
                        {
                            this.frmdouble = new FrmDoubleDisplay(this, _frmStart);
                            this.frmdouble.Show();
                        }));
                    }
                    else
                    {
                        this.frmdouble = new FrmDoubleDisplay(this, _frmStart);
                        this.frmdouble.Show();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 双屏
        /// </summary>
        /// <param name="state"></param>
        /// <param name="TotalAmt"></param>
        /// <param name="RemainAmt"></param>
        /// <param name="PaedAmt"></param>
        /// <param name="ChgAmt"></param>
        /// <param name="TotalQty"></param>
        /// <param name="cardid"></param>
        public void SetDoubleDisplay(string state, string TotalAmt, string RemainAmt, string PaedAmt, string ChgAmt, string TotalQty, string cardid, Decimal money, Decimal score)
        {
            Screen[] sc = Screen.AllScreens;
            if (sc.Length == 2)
            {
                if (Gattr.IsDoubleModule && (this.frmdouble != null))
                {
                    if (this._frmStart != null && this._frmStart.InvokeRequired)
                    {
                        this._frmStart.Invoke(new EventHandler(delegate
                        {
                            this.frmdouble.SetDoubleDisplay(state, TotalAmt, RemainAmt, PaedAmt, ChgAmt, TotalQty, cardid, money.ToString("f2"), score.ToString("f2"));
                        }));
                    }
                    else
                    {
                        this.frmdouble.SetDoubleDisplay(state, TotalAmt, RemainAmt, PaedAmt, ChgAmt, TotalQty, cardid, money.ToString("f2"), score.ToString("f2"));
                    }
                }
            }
        }
        /// <summary>
        /// 设置二维码显示
        /// </summary>
        /// <param name="url"></param>
        public void SetDoubleQrcode(string html)
        {
            Screen[] sc = Screen.AllScreens;
            if (sc.Length == 2)
            {
                if (Gattr.IsDoubleModule && (this.frmdouble != null))
                {
                    if (this._frmStart != null && this._frmStart.InvokeRequired)
                    {
                        this._frmStart.Invoke(new EventHandler(delegate
                        {
                            this.frmdouble.SetWebDocument(html);
                        }));
                    }
                    else
                    {
                        this.frmdouble.SetWebDocument(html);
                    }
                }
            }
        }
        /// <summary>
        /// 设置URL显示
        /// </summary>
        /// <param name="url"></param>
        public void SetDoubleUrl(string url)
        {
            Screen[] sc = Screen.AllScreens;
            if (sc.Length == 2)
            {
                if (Gattr.IsDoubleModule && (this.frmdouble != null))
                {
                    if (this._frmStart != null && this._frmStart.InvokeRequired)
                    {
                        this._frmStart.Invoke(new EventHandler(delegate
                        {
                            this.frmdouble.SetWebBrowserUrl(url);
                        }));
                    }
                    else
                    {
                        this.frmdouble.SetWebBrowserUrl(url);
                    }
                }
            }
        }
        
        #endregion
        #region 绑定销售商品列表数据
        /// <summary>
        /// 绑定销售商品列表数据
        /// </summary>
        /// <param name="item_no"></param>
        private void SetGridViewColumnsValue(string item_no)
        {
            if (this.tblPanelBalance.Visible)
            {
                this.tblPanelBalance.Visible = false;
                this.ResetSaleState();
            }
            if (item_no != null && !item_no.Equals(""))
            {
                this.ClearInput();
                //称重
                if (item_no.Length == 13 && item_no.Substring(0, 2).Equals("21"))
                {
                    this._saleflow = Gattr.Bll.GetItemInfo(item_no.Substring(2, 5));
                    string strPrice = item_no.Substring(7, 3) + "." + item_no.Substring(10, 2);
                    this._saleflow.sale_money = Convert.ToDecimal(strPrice);
                    //数量
                    this._saleflow.sale_qnty = this._saleflow.sale_money / this._saleflow.sale_price;
                }
                else
                {
                    //this._saleflow = Gattr.Bll.GetItemInfo(item_no);
                    List<t_cur_saleflow> saleInfos = Gattr.Bll.GetItemsForPos(item_no);
                    if (saleInfos.Count == 0)
                    {
                        MessageBox.Show("货号" + item_no + "不存在", Gattr.AppTitle);
                        this.ShowMessage("商品开始交易....");
                        this.ClearInput();
                        return;
                    }
                    else if (saleInfos.Count > 1)
                    {
                        this.ClearInput();
                        FrmItemQuery query = new FrmItemQuery(item_no, saleInfos);
                        query.ShowDialog();
                        if (query.itemF5 != string.Empty)
                        {
                            this._inputText = query.itemF5.ToString();
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Enter));
                        }
                        return;
                    }
                    else
                    {
                        this._saleflow = saleInfos[0];
                    }
                }
                _saleflow.flow_id = this.bindingSaleFlow.Count + 1;
                #region 促销代码添加
                List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                _saleInfos.Add(_saleflow);
                _listSaleFlow = SetSinglePlanRule(_saleInfos);
                this.ResetSaleFlowData();
                this.bindingSaleFlow.DataSource = this._listSaleFlow;
                this.bindingSaleFlow.ResetBindings(true);
                #endregion

                //绑定数据
                //this.bindingSaleFlow.Add(_saleflow);
                this.bindingSaleFlow.MoveLast();
                this.GvSaleFlow.DataSource = this.bindingSaleFlow;
                this.ShowMessage("交易商品<" + item_no + ">,数量<" + _saleflow.sale_qnty + ">");
                //总价
                decimal amtTotal = 0M;
                //数量
                decimal qntyTotal = 0M;
                foreach (t_cur_saleflow saleflow in this.bindingSaleFlow)
                {
                    amtTotal = amtTotal + saleflow.sale_money;
                    qntyTotal = qntyTotal + saleflow.sale_qnty;
                    saleflow.oper_id = Gattr.OperId;
                    saleflow.sale_way = saleflow.sale_way == "B" ? "B" : "A";
                    saleflow.branch_no = Gattr.BranchNo;
                    saleflow.pos_id = Gattr.PosId;
                }
                Gattr.Bll.SaveTempSaleRow(_listSaleFlow);

                this.SetTotalAmt();
                this.ClearInput();
            }
            else
            {
                this.bindingSaleFlow.DataSource = this._listSaleFlow;
                this.GvSaleFlow.DataSource = this.bindingSaleFlow;
                this.GvSaleFlow.Refresh();
            }

        }
        #endregion
        #region 销售商品向下翻页事件
        private void btnSaleDown_Click(object sender, EventArgs e)
        {
            if (this.bindingSaleFlow.Count != 0)
            {
                int num = 8;
                if (num < 0)
                {
                    num = 8;
                }
                if (this.bindingSaleFlow.Count <= num)
                {
                    this.bindingSaleFlow.MoveLast();
                }
                int num2 = this.bindingSaleFlow.Position + num;
                if (this.bindingSaleFlow.Count > num2)
                {
                    this.bindingSaleFlow.Position = num2;
                }
                else
                {
                    this.bindingSaleFlow.MoveLast();
                }
                this.GvSaleFlow.Refresh();
            }
        }
        #endregion
        #region 销售商品向上翻页事件
        private void btnSaleUp_Click(object sender, EventArgs e)
        {
            if (this.bindingSaleFlow.Count != 0)
            {
                int num = 8;
                if (num < 0)
                {
                    num = 8;
                }
                if (this.bindingSaleFlow.Count <= num)
                {
                    this.bindingSaleFlow.MoveFirst();
                }
                int num2 = this.bindingSaleFlow.Position - num;
                if (num2 > 0)
                {
                    this.bindingSaleFlow.Position = num2;
                }
                else
                {
                    this.bindingSaleFlow.MoveFirst();
                }
                this.GvSaleFlow.Refresh();
            }
        }
        #endregion
        #region 按钮点击事件，数量、会员、营业员、结算等。
        /// <summary>
        /// 按钮点击事件，数量、会员、营业员、结算等。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaleFunc_Click(object sender, EventArgs e)
        {
            PictureBox box = sender as PictureBox;
            if (box != null)
            {
                string str = SIString.TryStr(box.Tag).Trim();
                if (str != string.Empty)
                {
                    t_attr_function keyfunc = null;
                    if (str.ToLower() != "oth")
                    {
                        this.plOtherFunc.Visible = false;
                    }
                    switch (str.ToLower())
                    {
                        case "0":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad0));
                            break;
                        case "1":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad1));
                            break;
                        case "2":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad2));
                            break;
                        case "3":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad3));
                            break;
                        case "4":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad4));
                            break;
                        case "5":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad5));
                            break;
                        case "6":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad6));
                            break;
                        case "7":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad7));
                            break;
                        case "8":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad8));
                            break;
                        case "9":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad9));
                            break;
                        case "dot":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Decimal));
                            break;
                        case "div":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Subtract));
                            break;
                        case "enter":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Enter));
                            break;
                        case "backspace":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Back));
                            break;
                        case "reset":
                            this._inputText = string.Empty;
                            this.SetCursorText();
                            this.ShowMessage("取消输入！");
                            break;
                        case "exit":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Escape));
                            break;
                        case "num"://数量修改
                        case "can"://删除
                        case "prc"://单价
                        case "dct"://折扣
                        case "gdj"://挂单
                        case "vip"://会员
                        case "dou"://营业员
                        case "tot"://结算
                        case "oth"://其他
                        case "ret"://退货
                        case "lok"://交易查询
                            if (str.ToLower() == "oth")
                            {
                                this.plOtherFunc.Visible = !this.plOtherFunc.Visible;
                                this.lbxFunc.Focus();
                                this.lbxFunc.SelectionMode = SelectionMode.One;
                                this.lbxFunc.SelectedIndex = 0;
                            }
                            else
                            {
                                keyfunc = this.GetKeyFunc(str.ToLower());
                            }
                            break;
                    }
                    if (keyfunc != null)
                    {
                        this.CallKeyFunc(keyfunc);
                    }
                }
            }
        }
        #endregion
        #region 结算按钮事件
        /// <summary>
        /// 结算按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPayNum_Click(object sender, EventArgs e)
        {
            PictureBox box = sender as PictureBox;
            if (box != null)
            {
                string str = SIString.TryStr(box.Tag).Trim();
                if (str != string.Empty)
                {
                    this.plOtherFunc.Visible = false;
                    t_attr_function keyfunc = null;
                    if (((str.ToLower() == "enter") || (str.ToLower() == "payesc")) || (this._posState != PosOpState.PLU))
                    {
                        bool flag = false;
                        switch (str.ToLower())
                        {
                            case "0":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad0));
                                break;
                            case "1":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad1));
                                break;
                            case "2":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad2));
                                break;
                            case "3":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad3));
                                break;
                            case "4":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad4));
                                break;
                            case "5":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad5));
                                break;
                            case "6":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad6));
                                break;
                            case "7":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad7));
                                break;
                            case "8":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad8));
                                break;
                            case "9":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad9));
                                break;
                            case "-":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Subtract));
                                break;
                            case "dot":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Decimal));
                                break;
                            case "backspace":
                                this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Back));
                                break;
                            case "reset":
                                this._inputText = string.Empty;
                                this.SetCursorText();
                                break;
                            case "yezf":
                            case "enter":
                            case "rmb":
                            case "cdt":
                            case "csc":
                            case "payother":
                            case "ads":
                            case "azs"://整单赠送
                            case "agz"://挂账
                            case "zfb"://支付宝
                            case "aca":
                                this.CallKeyFunc(new t_attr_function() { func_id = str.ToLower() });
                                break;
                            case "payesc":
                                if (this._listPayFlow.Count <= 0)
                                {
                                    if (!Gattr.IsTotRollBack)
                                    {
                                        Gattr.IsTotRollBack = true;
                                        flag = true;
                                    }
                                    keyfunc = this.GetKeyFunc("tot");
                                    if (keyfunc != null)
                                    {
                                        this.CallKeyFunc(keyfunc);
                                    }
                                    if (flag)
                                    {
                                        Gattr.IsTotRollBack = false;
                                        this.tlpMain.Enabled = true;
                                        this._posState = PosOpState.PLU;
                                    }
                                    return;
                                }
                                this.ShowMessage("已存在付款记录，不能退出！");
                                return;
                        }
                        if (keyfunc != null)
                        {
                            this.CallKeyFunc(keyfunc);
                        }
                    }
                }
            }
        }
        #endregion
        #region 窗体键盘监听事件
        /// <summary>
        /// 键盘监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (_posState == PosOpState.LOOK)
            {
                ResetSaleState();
                ShowMessage("商品开始交易...");
                return;
            }
            try
            {
                if (e.Control)
                {
                    return;
                }
                switch (e.KeyCode)
                {
                    case Keys.Back:
                        if (this._inputText.Length >= 1)
                        {
                            this._inputText = this._inputText.Substring(0, this._inputText.Length - 1);
                            this.SetCursorText();
                            if (this._inputText.Length == 0)
                            {
                                this.ShowMessage(string.Format("已键入{0}字符.", "0"));
                            }
                            else
                            {
                                this.ShowMessage(string.Format("已键入{0}字符.", this._inputText.Length.ToString()));
                            }
                        }
                        return;

                    case Keys.Enter:
                        if (!this.plOtherFunc.Visible)
                        {
                            if (this._posState == PosOpState.PLU)
                            {
                                t_attr_function keyfunc = new t_attr_function
                                {
                                    pos_key = "Enter",
                                    func_id = "plu",
                                    type = '1'
                                };
                                this.CallKeyFunc(keyfunc);
                            }
                            else if ((this._posState == PosOpState.PAY) || (this._posState == PosOpState.CHG))
                            {
                                this.BeforePLU();
                                this.CallKeyFunc(new t_attr_function() { func_id = "rmb" });
                            }
                        }
                        else
                        {
                            this.plOtherFunc.Visible = false;
                            if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("会员查询"))
                            {
                                FrmMemberInfo frmMemberInfo = new FrmMemberInfo();
                                frmMemberInfo.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("商品查询"))
                            {
                                this.ItemQuery();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("修改支付"))
                            {
                                FrmCPayWay _frmCPayWay = new FrmCPayWay("",false);
                                _frmCPayWay.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("交易查询"))
                            {
                                t_attr_function keyfunc = this.GetKeyFunc("lok");
                                if (keyfunc == null)
                                {
                                    keyfunc = new t_attr_function() { func_id = "lok" };
                                }
                                this.CallKeyFunc(keyfunc);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("收银对账"))
                            {
                                this.CashAccount();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("库存查询"))
                            {
                                this.StockQuery(false);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("要 货 单"))
                            {
                                FrmDmSheet dmSheet = new FrmDmSheet();
                                dmSheet.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("收 货 单"))
                            {
                                FrmSHSheet shSheet = new FrmSHSheet();
                                shSheet.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("数据下载"))
                            {
                                FrmSyncPosData _postData = new FrmSyncPosData(false);
                                _postData.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("退货"))
                            {
                                t_attr_function keyfunc = this.GetKeyFunc("ret");
                                this.CallKeyFunc(keyfunc);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("会员查询"))
                            {
                                FrmMemberInfo frmMemberInfo = new FrmMemberInfo();
                                frmMemberInfo.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("项目储值"))
                            {
                                MessageBox.Show("项目储值功能尚未实现", Gattr.AppTitle);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("更改仓库"))
                            {
                                MessageBox.Show("更改仓库功能尚未实现", Gattr.AppTitle);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("客户订货"))
                            {
                                MessageBox.Show("客户订货功能尚未实现", Gattr.AppTitle);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("寄存提货"))
                            {
                                MessageBox.Show("寄存提货功能尚未实现", Gattr.AppTitle);
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("挂账回款"))
                            {
                                FrmArrearage frmArrearage = new FrmArrearage();
                                frmArrearage.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("更改密码"))
                            {
                                this.UpdatePwd();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("非交易"))
                            {
                                this.NonTrading();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("赠送"))
                            {
                                this.FunKeyGift();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("微信活动"))
                            {
                                this.FunKeyWeiXin();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("打印设置"))
                            {
                                FrmPrint _frmPrint = new FrmPrint();
                                _frmPrint.ShowDialog();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("操作手册"))
                            {
                                this.FunKeyMyHelp();
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("切换键盘"))
                            {
                                this.tblNum.Visible = !this.tblNum.Visible;
                                this.tlpCenterRight.Visible = !this.tlpCenterRight.Visible;
                                if (this.tblNum.Visible)
                                {
                                    this.ResetSaleCursor();
                                }
                            }
                            else if ((e.KeyCode == Keys.Return) && this.lbxFunc.SelectedItem.ToString().Contains("提交反馈"))
                            {
                                FrmFeedBack _frmFeedBack = new FrmFeedBack();
                                _frmFeedBack.ShowDialog();
                            }
                        }
                        break;
                    case Keys.PageUp:
                    case Keys.Next:
                    case Keys.Left:
                    case Keys.Right:
                        return;

                    case Keys.Up:
                        if (!this.GvSaleFlow.Focused)
                        {
                            this.bindingSaleFlow.MovePrevious();
                        }
                        return;

                    case Keys.Down:
                        if (!this.GvSaleFlow.Focused)
                        {
                            this.bindingSaleFlow.MoveNext();
                        }
                        return;

                    case Keys.Escape:
                        ///判断当前是否有挂单商品，销售商品是否为空
                        //if (this._posState == PosOpState.PLU)
                        //{
                        //    this._frmLogin.LoginParm = "RETURN LOGIN";
                        //    base.Close();
                        //}
                        //else if (this.tblPanelBalance.Visible)
                        //{
                        //    this.picPayNum_Click(this.btnPicFuncPayEsc, null);
                        //}
                        if (this.tblPanelBalance.Visible)
                        {
                            this.picPayNum_Click(this.btnPicFuncPayEsc, null);
                        }
                        else if (this._posState == PosOpState.PLU)
                        {
                            //if(未对账&&配置文件中不允许更换)
                            this._frmLogin.LoginParm = "RETURN LOGIN";
                            base.Close(); 
                        }
                        else
                        {
                            this.Close();
                            Application.Exit();
                        }
                        return;
                    case Keys.F1:
                        MessageBox.Show("功能尚未实现", Gattr.AppTitle);
                        return;
                    case Keys.F2:
                        return;

                    case Keys.F3:
                        return;

                    case Keys.F4:

                        return;

                    case Keys.F5:
                        Form f = Application.OpenForms["FrmText"];  //查找是否打开过Form1窗体  
                        if (f == null)  //没打开过  
                        {
                            FrmText _frmText = new FrmText();
                            _frmText.Show();   //重新new一个Show出来  
                        }  
                        else  
                        {  
                             f.Focus();   //打开过就让其获得焦点  
                        } 
                        return;

                    case Keys.F6:

                        return;

                    case Keys.F7:

                        return;

                    case Keys.F8:

                        return;

                    case Keys.F9:

                        return;

                    case Keys.F10:

                        return;

                    case Keys.F11:


                        return;

                    case Keys.F12:

                        return;
                    default:
                        goto LableKeys;

                }

            LableKeys:
                string str = Gfunc.ParseKeyCode(e.Shift, e.KeyCode);
                if (str.Length > 0)
                {
                    //if (this.plOtherFunc.Visible)
                    //{
                    //    this.plOtherFunc.Visible = false;
                    //}
                    //处理主操作按钮事件
                    if (Gattr.FunKeys.ContainsKey(str))
                    {
                        this.ShowMessage(string.Format("已键入{0}字符.", "0"));
                        if (str.Equals("."))
                        {
                            this.ShowMessage("小数点\".\"不允许使用该功能键，请更改POS功能键设置。");
                        }
                        else
                        {
                            this.CallKeyFunc(Gattr.FunKeys[str]);
                        }
                    }
                    else if (str.Length == 1)
                    {
                        if (str.ToLower() == "r" || str.ToLower() == "i" || str.ToLower() == "c" || str.ToLower() == "=" || str.ToLower() == "," || str.ToLower() == "j" || str.ToLower() == "e")
                        {
                            this.CallKeyFunc(new t_attr_function() { func_id = Gattr.UsedDicKeyTag[str.ToLower()] });
                        }
                        else
                        {
                            //处理“结算”事件
                            Regex regex = new Regex(@"[^A-Za-z0-9\.-]");
                            if (!regex.IsMatch(str))
                            {
                                this._inputText = this._inputText + str;
                            }
                            regex = null;
                            this.SetCursorText();
                            this.ShowMessage(string.Format("已键入{0}字符.", this._inputText.Length.ToString()));
                        }
                    }
                    else
                    {
                        string _msgString = string.Format("功能键[{0}]未定义！", str);
                        this.ShowMessage(_msgString);
                    }
                }


            }

            catch (Exception ex)
            {
                this.ClearInput();
                this.ShowMessage("输入错误！");
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }

        }
        #endregion
        #region CallKeyFunc集中处理事件方法
        /// <summary>
        /// 集中处理事件方法
        /// </summary>
        /// <param name="keyfunc"></param>
        private void CallKeyFunc(t_attr_function keyfunc)
        {
            if (keyfunc != null)
            {
                string str = keyfunc.func_id.ToLower();
                PosOpType postype = Gattr.DicPosOpType[str];
                if (!CheckIsCanOperate(postype))
                {
                    ShowMessage("当前业务状态下该快捷键无效");
                    return;
                }
                switch (postype)
                {
                    case PosOpType.PLU:
                        BeforePLU();
                        this.SetGridViewColumnsValue(_inputText);
                        break;
                    case PosOpType.数量:
                        this.IsInputNum = true;
                        this.FunKeyNum(null);
                        this.IsInputNum = false;
                        break;
                    case PosOpType.删除:
                        this.FunKeyDelSaleFlow();
                        break;
                    case PosOpType.单价:
                        this.FunKeyPrc();
                        break;
                    case PosOpType.折扣:
                    case PosOpType.整单折扣:
                        this.FunKeyDct();
                        break;
                    case PosOpType.挂单:
                        this.FunKeyPnr("normal");
                        break;
                    case PosOpType.会员:
                        this.FunKeyVipSale(false);
                        break;
                    case PosOpType.营业员:
                        MessageBox.Show("功能暂未开通", Gattr.AppTitle);
                        //this.FunKeySaleMan();
                        break;
                    case PosOpType.结算:
                        this.FunKeyTot();
                        break;
                    case PosOpType.其他:
                        this.plOtherFunc.Visible = !this.plOtherFunc.Visible;
                        this.lbxFunc.Focus();
                        this.lbxFunc.SelectionMode = SelectionMode.One;
                        this.lbxFunc.SelectedIndex = 0;
                        break;
                    case PosOpType.现金:
                        this.FunKeyRmb();
                        break;
                    case PosOpType.银行卡:
                        this.FunKeyBcd();
                        break;
                    case PosOpType.储值卡:
                        this.FunKeyCsc();
                        break;
                    case PosOpType.支付宝:
                        //支付宝支付
                        this.FunKeyZFB();
                        break;
                    case PosOpType.余额支付:
                        this.FunKeyYezf();
                        break;
                    case PosOpType.整单赠送:
                        this.FunKeyGiftAll();
                        break;
                    case PosOpType.挂账:
                        this.FunKeyArrearage();
                        break;
                    case PosOpType.购物券:
                        MessageBox.Show(keyfunc.func_name + "功能尚未实现！", Gattr.AppTitle);
                        break;
                    case PosOpType.其他方式:
                        this.FunKeyFkf();
                        break;
                    case PosOpType.整单取消:
                        DialogResult dr = MessageBox.Show("是否取消本次订单交易？",Gattr.AppTitle,MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                        if (dr==DialogResult.Yes)
                        {
                            Gattr.Bll.DeleteTempData();
                            ResetSaleState();
                            LoggerHelper.Log("MsmkLogger", Gattr.OperId + "进行整单取消", LogEnum.SysLog);
                            ShowMessage("商品开始交易...");
                        }
                        break;
                    case PosOpType.退货:
                        PayBackButtonEvent();
                        LoggerHelper.Log("MsmkLogger", Gattr.OperId+ "进行退货操作！", LogEnum.SysLog);
                        break;
                    case PosOpType.交易查询:
                        PaySaleInfoLook();
                        LoggerHelper.Log("MsmkLogger", Gattr.OperId + "进行交易查询", LogEnum.SysLog);
                        break;
                    case PosOpType.开钱箱:
                        this.FunKeyOPD(); 
                        break;

                }
            }
        }
        #endregion
        #region 退货按钮事件逻辑
        /// <summary>
        /// 退货按钮事件逻辑
        /// </summary>
        void PayBackButtonEvent()
        {
            FrmPayBack payback = new FrmPayBack();
            String _reasonId = string.Empty;
            List<t_cur_saleflow> _currentSaleList = new List<t_cur_saleflow>();
            List<t_cur_saleflow> _returnSaleList = new List<t_cur_saleflow>();
            bool _isFlowNo = false;
            payback.EventSetReasonID += (id) => { _reasonId = id; };
            if (payback.ShowDialog() == System.Windows.Forms.DialogResult.OK)//需要获取到退货原因标示
            {
                FrmFlowNoSearch _frmFlow = new FrmFlowNoSearch();
                _currentFlowNo = string.Empty;
                _isFlowNo = false;
                _frmFlow.EventSetFlowNo += (no, isflow, ls) => { _currentFlowNo = no; _isFlowNo = isflow; _returnSaleList = ls; };
                if (_frmFlow.ShowDialog() == System.Windows.Forms.DialogResult.OK)//需要获取到退货凭证号
                {
                    if (_isFlowNo)
                    {
                        FrmSaleDetails _frmSale = new FrmSaleDetails(_returnSaleList);
                        _frmSale.EventSetSaleList += (ls,mCHA) =>
                        {
                            if (ls != null && ls.Count > 0)
                            {
                                moneyCHA = 0;
                                moneyCHA = mCHA;
                                foreach (t_cur_saleflow sale in ls)
                                {
                                    sale.sale_qnty = sale.sale_qnty * (-1);
                                    sale.sale_money = sale.sale_qnty * sale.unit_price;
                                    sale.flow_old_id = sale.flow_id;
                                    sale.reasonid = _reasonId;
                                    sale.sale_way = "B";
                                    sale.branch_no = Gattr.BranchNo;
                                    sale.oper_id = Gattr.OperId;
                                    sale.pos_id = Gattr.PosId;
                                }
                                if (_currentSaleList.Count == 0)
                                {
                                    _currentSaleList.AddRange(ls);
                                }
                            }
                        };
                        if (_frmSale.ShowDialog() == System.Windows.Forms.DialogResult.OK)//需要获取到退货商品信息
                        {
                            //TODO:将退货的商品详细信息列表绑定到销售列表中，金额为负数
                            //this._listSaleFlow.AddRange(_currentSaleList);
                            if (_currentSaleList.Count == 0)
                            {
                                _currentFlowNo = string.Empty;
                                ShowMessage("没有可以进行退货的商品");
                                return;
                            }
                            if (_listSaleFlow.Count == 0)
                            {
                                foreach (t_cur_saleflow sale in _currentSaleList)
                                {
                                    if (_listSaleFlow.Count == 0)
                                    {
                                        sale.flow_id = 1;
                                    }
                                    else
                                    {
                                        sale.flow_id = _listSaleFlow[_listSaleFlow.Count - 1].flow_id + 1;
                                    }
                                    _listSaleFlow.Add(sale);
                                }
                            }
                            else
                            {
                                foreach (t_cur_saleflow sale in _currentSaleList)
                                {
                                    if (IsCanAdd(sale))
                                    {
                                        sale.flow_id = _listSaleFlow[_listSaleFlow.Count - 1].flow_id + 1;
                                        _listSaleFlow.Add(sale);
                                    }
                                }
                            }
                            _currentSaleList.Clear();
                            this.bindingSaleFlow.ResetBindings(false);
                            this.GvSaleFlow.DataSource = this.bindingSaleFlow;
                            string card = Gattr.Bll.GetVipCardByNo(_currentFlowNo);
                            if (!string.IsNullOrEmpty(card))
                            {
                                //t_member_info memeber = MemberService.Instance.GetMemberInfoByMemNo(card);
                                t_member_info meminfo = MemberService.Instance.GetMemberInfoByMemNo(card);
                                if (meminfo.code == "-1")
                                {
                                    MessageBox.Show(meminfo.info, Gattr.AppTitle);
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
                                                this.lbVipValue.Text = "【" + meminfo.mem_no + "】" + meminfo.username + "[供货港微会员]";
                                                SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, meminfo.mem_no + "[利澜微会员]", meminfo.balance, meminfo.score);
                                            }
                                            else
                                            {
                                                this.lbVipValue.Text = "【" + meminfo.mem_no + "】" + meminfo.username;
                                                SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, meminfo.mem_no, meminfo.balance, meminfo.score);
                                            }
                                            _isVip = true;
                                            this.ShowMessage("会员：" + meminfo.mem_no + "成功记录");
                                        }
                                    }
                                }
                                _currentMember = meminfo;
                                //t_member_info tempMember = MemberService.Instance.CheckMemberinfoForLL(card);
                                //if (tempMember.code == "1")
                                //{
                                //    _currentMember.mem_type = "WX";
                                //}
                            }

                            this.SetTotalAmt(false);
                            this.GvSaleFlow.Refresh();

                        }//if (_frmSale.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            ShowMessage("正在进行退货");
                        }
                    }
                    else
                    {
                        ShowMessage(_currentFlowNo);
                        _currentFlowNo = string.Empty;
                    }
                }///if (_frmFlow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                else
                {
                    _currentFlowNo = string.Empty;
                    ShowMessage("取消退货");
                }
            }//if (payback.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            else
            {
                ShowMessage("取消退货");
            }
        }
        /// <summary>
        /// 验证当前的销售商品流水是否可以添加到销售列表中
        /// </summary>
        /// <param name="sale">销售列表</param>
        /// <returns></returns>
        bool IsCanAdd(t_cur_saleflow sale)
        {
            bool isAdd = true;
            for (int index = 0; index < this._listSaleFlow.Count; index++)
            {
                t_cur_saleflow sale1 = _listSaleFlow[index];
                if (!string.IsNullOrEmpty(sale1.flow_no))
                {
                    if (sale1.flow_no == sale.flow_no)
                    {
                        if (sale1.flow_old_id == sale.flow_old_id)
                        {
                            return false;
                        }
                    }
                }
            }
            return isAdd;
        }
        #endregion
        #region 数量修改按钮
        /// <summary>
        /// 数量修改按钮
        /// </summary>
        /// <param name="saleflow"></param>
        private void FunKeyNum(t_cur_saleflow saleflow)
        {
            //修改当前行的数量，计算小计，更改总价格，更改总数量，更新流水表数据。

            if (this.bindingSaleFlow.Count == 0)
            {
                this.ShowMessage("当前无商品销售，操作取消!");//Language.GetString("#0053"));
            }
            else
            {

                decimal num = 0M; //当前数量

                if (saleflow == null)
                {
                    saleflow = this.bindingSaleFlow.Current as t_cur_saleflow;
                }

                //if (saleflow.num3 == 1)
                //{
                //    this.ShowMessage("计重商品不能修改商品数量!");//Language.GetString("#0053"));
                //    return;
                //}

                decimal num2 = saleflow.sale_qnty;
                if (this.IsInputNum)
                {
                    FrmNum frmNum = new FrmNum();
                    if (frmNum.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    this.GlobalNum = frmNum.Num.ToString(Gattr.PosSaleNumPoint);
                    //输入的数量
                    num = Gfunc.TypeToDecimal(this.GlobalNum, 0M);
                }
                else
                {
                    num = Gfunc.TypeToDecimal("", 0M);
                }

                //小计
                if (saleflow.sale_way == "B")
                {
                    if (Math.Abs(saleflow.sale_qnty) < num)
                    {
                        this.ShowMessage("超过退货商品数量");//Language.GetString("#0053"));
                        return;
                    }
                    else
                    {
                        saleflow.sale_qnty = num * (-1);
                        saleflow.sale_money = (decimal)(saleflow.sale_qnty * saleflow.unit_price) * (-1);
                    }
                }
                else
                {
                    saleflow.sale_qnty = num;
                    saleflow.sale_money = (decimal)(saleflow.sale_qnty * saleflow.unit_price);
                }

                this.bindingSaleFlow.ResetBindings(false);

                #region 促销代码添加
                List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                _listSaleFlow = SetSinglePlanRule(_saleInfos);
                this.ResetSaleFlowData();
                this.bindingSaleFlow.DataSource = this._listSaleFlow;
                this.bindingSaleFlow.ResetBindings(true);
                #endregion

                this.ResetSaleFlowData();
                this.SetTotalAmt();
                this.GvSaleFlow.Refresh();
                //向销售商品流水表插入数据
                Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                this.ShowMessage("商品<" + saleflow.item_no + ">，改数量<" + saleflow.sale_qnty + ">！");
                //this.ClearInput();
            }
        }
        #endregion
        #region 删除商品按钮
        /// <summary>
        /// 删除商品按钮
        /// </summary>
        private void FunKeyDelSaleFlow()
        {
            if (this._posState == PosOpState.PLU)
            {
                if (this.bindingSaleFlow.Count > 0)
                {

                    int index = this.bindingSaleFlow.Position;

                    t_cur_saleflow saleflow = this.bindingSaleFlow[index] as t_cur_saleflow;

                    this.bindingSaleFlow.RemoveAt(index);
                    this.bindingSaleFlow.MoveLast();

                    if (saleflow.unit_price != 0M)
                    {
                        this.DelListSaleFlow(saleflow);
                    }

                    #region 促销代码添加
                    List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                    _listSaleFlow = SetSinglePlanRule(_saleInfos);
                    this.ResetSaleFlowData();
                    this.bindingSaleFlow.DataSource = this._listSaleFlow;
                    this.bindingSaleFlow.ResetBindings(true);
                    #endregion

                    this.ResetSaleFlowData();
                    //this.bindingSaleFlow.ResetBindings(false);

                    this.bindingSaleFlow.Position = this.bindingSaleFlow.Count - 1;
                    this.GvSaleFlow.Refresh();
                    this.SetTotalAmt();

                    Gattr.Bll.SaveTempSaleRow(this._listSaleFlow);

                    this.ShowMessage("商品<" + saleflow.item_no + ">，已被删除！");
                }
            }
            else if (this._posState == PosOpState.PAY)
            {
                Gattr.Bll.DeleteTempData();
                this.IsPayState = true;
                this.ClearInput();
                this.SetPayTotelAmt();
                this.ResetSaleState();
            }
            if (this._listSaleFlow.Count == 0 && !string.IsNullOrEmpty(_currentFlowNo) && _currentMember != null && !string.IsNullOrEmpty(_currentMember.mem_no))
            {
                _currentMember = null;
                _currentFlowNo = string.Empty;
            }

        }
        #endregion
        #region 修改单价按钮
        /// <summary>
        /// 修改单价按钮
        /// </summary>
        private void FunKeyPrc()
        {
            //if (Gattr.OperGrant.Substring(1, 1) == "1")
            //{
                if (this.bindingSaleFlow.Count > 0)
                {
                    int index = this.bindingSaleFlow.Position;
                    t_cur_saleflow saleflow = this.bindingSaleFlow[index] as t_cur_saleflow;
                    if (saleflow.isYh)
                    {
                        this.ShowMessage("已经优惠商品，不能改价！");
                        return;
                    }
                    string text = "商品改价";//"【" + saleflow.Item_no + "】,原价【" + Convert.ToDecimal(saleflow.Sale_price.ToString(Gattr.PosSalePrcPoint)) + "】,请输入新的价格:";
                    string lbName = "价   格：";
                    string itemName = saleflow.item_no + "\r\n" + saleflow.item_name;
                    string itemPrice = saleflow.unit_price.ToString("0.00");
                    FrmPriceInput frm = new FrmPriceInput(lbName, text, itemName, itemPrice);
                    if (frm.ShowDialog() == DialogResult.Cancel)
                    {
                        this.ShowMessage("取消改价！");
                        return;
                    }
                    if (frm.GetNumber <= 0)
                    {
                        this.ShowMessage("价格输入错误！");
                        return;
                    }
                    saleflow.unit_price = Convert.ToDecimal(frm.GetNumber.ToString(Gattr.PosSalePrcPoint));
                    saleflow.sale_money = saleflow.unit_price * saleflow.sale_qnty;
                    this.bindingSaleFlow.ResetBindings(false);
                    this.ResetSaleFlowData();
                    this.SetTotalAmt();
                    this.GvSaleFlow.Refresh();
                    //向销售商品流水表插入数据
                    Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                    this.ShowMessage("商品<" + saleflow.item_no + ">，改价<" + saleflow.unit_price + ">！");
                }
                else
                {
                    this.ShowMessage("当前无商品销售，操作取消!");
                }
            //}
            //else
            //{
            //    MessageBox.Show("权限不足，无法操作！");
            //}
        }
        #endregion
        #region 修改折扣按钮
        private void FunKeyDct()
        {
            //if (Gattr.OperGrant.Substring(0, 1) == "1")
            //{
                FrmPriceInput frm;
                if (this._posState != PosOpState.PLU)
                {
                    string lbName = "折扣值：";
                    string text = "整单折扣";
                    frm = new FrmPriceInput(lbName, text);

                    if (frm.ShowDialog() == DialogResult.Cancel)
                    {
                        this.ShowMessage("取消折扣！");
                        return;
                    }
                    decimal rebates = frm.GetNumber;
                    if (rebates >= Gattr.OperDiscount)
                    {
                        if (rebates <= 0 || rebates > 99)
                        {
                            this.ShowMessage("折扣值输入错误，值应在1-99之间！");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("您的最大折扣权限为：" + Gattr.OperDiscount);
                        return;
                    }

                    rebates = rebates.ToString().Length >= 2 ? rebates / 100 : rebates / 10;

                    foreach (t_cur_saleflow saleflow in this._listSaleFlow)
                    {
                        saleflow.unit_price = saleflow.unit_price * rebates;
                        saleflow.sale_money = saleflow.unit_price * saleflow.sale_qnty;
                        saleflow.discount_rate = frm.GetNumber;
                    }

                    this.bindingSaleFlow.ResetBindings(false);
                    this.bindingSaleFlow.Position = this.bindingSaleFlow.Count - 1;
                    this.SetTotalAmt();
                    this.SetPayTotelAmt();
                    LoggerHelper.Log("MsmkLogger", System.DateTime.Now.ToString() + "折扣操作！", LogEnum.SysLog);
                }
                else
                {
                    t_cur_saleflow saleflow = this.bindingSaleFlow.Current as t_cur_saleflow;
                    string text = "单品折扣";
                    string lbName = "折扣值：";
                    string itemName = saleflow.item_no + "\r\n" + saleflow.item_name;
                    string itemPrice = saleflow.unit_price.ToString("0.00");
                    frm = new FrmPriceInput(lbName, text, itemName, itemPrice);


                    if (frm.ShowDialog() == DialogResult.Cancel)
                    {
                        this.ShowMessage("取消折扣！");
                        return;
                    }
                    decimal rebates = frm.GetNumber;
                    if (rebates >= Gattr.OperDiscount)
                    {
                        if (rebates <= 0 || rebates > 99)
                        {
                            this.ShowMessage("折扣值输入错误，值应在1-99之间！");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("您的最大折扣权限为：" + Gattr.OperDiscount);
                        return;
                    }
                    rebates = rebates.ToString().Length >= 2 ? rebates / 100 : rebates / 10;
                    saleflow.unit_price = saleflow.unit_price * rebates;
                    saleflow.sale_money = saleflow.sale_qnty * saleflow.unit_price;
                    saleflow.discount_rate = rebates;
                    this.bindingSaleFlow.ResetBindings(false);
                    this.ResetSaleFlowData();
                    this.SetTotalAmt();
                    this.GvSaleFlow.Refresh();
                    //向销售商品流水表插入数据
                    Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                    this.ShowMessage("商品<" + saleflow.item_no + ">，折扣值<" + saleflow.discount_rate + ">！");
                }
            //}
            //else
            //{
            //    MessageBox.Show("权限不足，无法操作！");
            //}

        }
        #endregion
        #region 挂单操作按钮
        /// <param name="pType">挂起类型</param>
        private void FunKeyPnr(string pType)
        {
            FrmPendingOrder frm = new FrmPendingOrder();
            //销售列表不为空
            if (this.bindingSaleFlow.Count <= 0 && pType == "normal")
            {
                //判断是否有挂单的商品，如果有调出来

                if (frm.PenOrdCount > 0)
                {

                    if (frm.PenOrdCount != 1)
                    {
                        frm.ShowDialog();
                        if (frm.PenOrdCount <= 0)
                        {
                            return;
                        }
                    }
                    this.bindingSaleFlow.Clear();
                    foreach (t_cur_saleflow _saleflow in frm.ListSaleFlows)
                    {
                        _saleflow.oper_id = Gattr.OperId;
                        this.bindingSaleFlow.Add(_saleflow);
                    }
                    /*////////////////获取支付金额列表//////////////////*/
                    //清空临时付款表
                    if (_listPayFlow != null && _listPayFlow.Count > 0)
                    {
                        _listPayFlow.Clear();
                    }
                    _listPayFlow = frm.ListPendingPayFlow;
                    if(frm.Vip_No!=string.Empty)
                        VipShow(frm.Vip_No);
                    if (_listPayFlow != null && _listPayFlow.Count > 0)
                    {
                        int flowId = 1;
                        foreach (t_cur_payflow _payflow in frm.ListPendingPayFlow)
                        {
                            _payflow.flow_id = flowId;
                            this.bindingPayFlow.Add(_payflow);
                            flowId++;
                        }
                        this.bindingPayFlow.ResetBindings(false);
                        this.GvPayFlow.Refresh();
                    }
                    ////////////////////////////////////////////
                    this.bindingSaleFlow.ResetBindings(false);
                    this.bindingSaleFlow.Position = this._listSaleFlow.Count - 1;
                    this.GvSaleFlow.Refresh();
                    this.SetTotalAmt();
                    Gattr.Bll.SaveTempSaleRow(this._listSaleFlow);
                    frm.DelPendingOrder();
                }
                else
                {
                    this.ShowMessage("当前无商品销售，操作取消!");
                }
                //LoggerHelper.Log("MsmkLogger", System.DateTime.Now.ToString() + "进行取单操作！", LogEnum.SysLog);
            }
            else
            {
                if (pType == "normal" && this.bindingPayFlow.Count > 0) {

                    MessageBox.Show("本单为余额支付挂单，无法从本页面再次挂起！",Gattr.AppTitle);
                }
                else
                {
                    string strFlow = "0001";
                    int maxPendingOrderNo = Gattr.Bll.GetMaxPendingOrderFlowNo();
                    int flowNo = 0;
                    while (++flowNo <= (maxPendingOrderNo + 1))
                    {
                        if (flowNo > Gattr.PendingOrderMaxNo)
                        {
                            this.ShowMessage("已经达到最大挂单数量限制，不能再挂单。");
                            return;
                        }
                        if (!Gattr.Bll.CheckPendingOrder(flowNo))
                        {
                            strFlow = flowNo.ToString("0000");
                            break;
                        }
                    }
                    t_pending_order penOrd = new t_pending_order();
                    penOrd.flow_no = strFlow;
                    penOrd.sale_man = Gattr.OperId;
                    penOrd.pending_type = pType;//挂起类型
                    if (_currentMember != null)
                    {
                        penOrd.vip_no = _currentMember.mem_no;
                    }
                    Gattr.Bll.InsertPendingOrder(strFlow, _listSaleFlow, penOrd);
                    this.bindingSaleFlow.Clear();
                    this.SetTotalAmt();
                    this.ResetSaleFlowData();
                    //向销售商品流水表插入数据
                    Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                    if (pType == "yezf")
                    {
                        //余额支付时挂起
                        Gattr.Bll.SavePendingPayRow(_listPayFlow, strFlow);
                    }
                    decimal m = moneyCHA;
                    this.ResetSaleState();
                    moneyCHA = m;
                    LoggerHelper.Log("MsmkLogger",Gattr.OperId + "进行挂单操作", LogEnum.SysLog);
                }
            }
        }
        #endregion
        #region 会员操作按钮
        private void FunKeyVipSale(bool isYE)
        {
            if (((this.bindingSaleFlow.Count > 0) && (this._posState != PosOpState.PAY)) && (this._posState != PosOpState.PLU))
            {
                this.ShowMessage("已有业务发生，不能再使用会员卡！");
                this.ClearInput();
            }
            else if (this._posState == PosOpState.PAY && this.bindingPayFlow.Count > 0)
            {
                this.ShowMessage("已部分付款，不能再使用会员卡！");
                this.ClearInput();
            }
            else
            {
                if (_currentMember != null)
                {
                    if (MessageBox.Show("是否取消当前会员的交易", Gattr.AppTitle, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.ShowMessage("取消会员" + _currentMember.mem_no + "的交易");
                        _currentMember = null;
                        this.lbVipValue.Text = string.Empty;
                        _isVip = false;
                        #region 取消所有的商品折扣和活动
                        //_listSaleFlow.Clear();
                        //this.ResetSaleFlowData();
                        //this.bindingSaleFlow.DataSource = this._listSaleFlow;
                        //this.bindingSaleFlow.ResetBindings(true);
                        ResetSaleState(false);
                        #endregion
                        this.ClearInput();
                    }
                }
                else
                {
                    FrmVipInput frm = new FrmVipInput();
                    frm.SetMemberEvent += (minfo) =>
                    {
                        _currentMember = minfo;
                        if (_currentMember.mem_type == "WX")
                        {
                            SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, _currentMember.mem_no + "[利澜微会员]",_currentMember.balance,_currentMember.score);
                        }
                        else
                        {
                            SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, _currentMember.mem_no, _currentMember.balance, _currentMember.score);
                        }
                    };
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_currentMember != null)
                        {
                            if (_currentMember.mem_type == "WX")
                            {
                                this.lbVipValue.Text = "【" + _currentMember.mem_no + "】" + _currentMember.username + "[供货港微会员]";
                            }
                            else
                            {
                                this.lbVipValue.Text = "【" + _currentMember.mem_no + "】" + _currentMember.username;
                            }
                            _isVip = true;
                            this.ShowMessage("会员："+_currentMember.mem_no+"成功记录");
                            #region 取消所有的商品折扣和活动
                            List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                            List<t_cur_saleflow> tList = SetSinglePlanRule(_saleInfos);
                            _listSaleFlow.Clear();
                            _listSaleFlow = tList;
                            this.ResetSaleFlowData();
                            this.bindingSaleFlow.DataSource = this._listSaleFlow;
                            this.bindingSaleFlow.ResetBindings(true);
                            this.SetPayTotelAmt();
                            #endregion
                            // 是否是在余额支付中打开
                            if (isYE)
                            {
                                this.FunKeyYezf();
                            }
                            //FunKeyRmb();
                            //TODO: 根据会员的打折信息进行打折
                        }
                    }
                }
            }
        }
        #endregion
        #region 挂账操作按钮
        /// <summary>
        /// 挂账操作
        /// </summary>
        private void FunKeyArrearage()
        {
            if (Gattr.OperGrant.Substring(2, 1) == "1")
            {
                if (_currentMember != null)
                {
                    //应收账款
                    decimal payAmtRec = Convert.ToDecimal(String.Format("{0:F}", SIString.TryDec(this.lbPayAmtRec.Text)));
                    t_payment_info payment = null;
                    t_cur_payflow payflow = null;
                    payment = new t_payment_info() { rate = 1M, pay_way = "GZ", pay_name = "挂账" };
                    payflow = SetPayFlowInfo(payAmtRec, payment, this._saleTotalAmt, _currentFlowNo, string.Empty, string.Empty);
                    //挂账的销售方式为 E
                    payflow.sale_way = "E";
                    this._listPayFlow.Add(payflow);
                    Gattr.Bll.SaveTempPayRow(_listPayFlow);
                    this.bindingPayFlow.DataSource = this._listPayFlow;
                    this.bindingPayFlow.ResetBindings(false);
                    this.SetPayTotelAmt();
                    this.ClearInput();
                }
                else
                {
                    MessageBox.Show("请登录会员账户才可挂账！", Gattr.AppTitle);
                }
            }
            else
            {
                MessageBox.Show("您没有权限进行此操作！");
            }

        }
        #endregion
        #region 微信活动
        private void FunKeyWeiXin()
        {
            if (((this.bindingSaleFlow.Count > 0) && (this._posState != PosOpState.PAY)) && (this._posState != PosOpState.PLU))
            {
                this.ShowMessage("已有业务发生，不能再进行微信活动！");
                this.ClearInput();
            }
            else if (this._posState == PosOpState.PAY && this.bindingPayFlow.Count > 0)
            {
                this.ShowMessage("已部分付款，不能再进行微信活动！");
                this.ClearInput();
            }
            else
            {
                if (_currentMember != null)
                {
                    //if (MessageBox.Show("是否取消当前会员的交易", Gattr.AppTitle, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    this.ShowMessage("取消会员" + _currentMember.mem_no + "的交易");
                    //    _currentMember = null;
                    //    this.lbVipValue.Text = string.Empty;
                    //    _isVip = false;
                    //    #region 取消所有的商品折扣和活动
                    //    //_listSaleFlow.Clear();
                    //    //this.ResetSaleFlowData();
                    //    //this.bindingSaleFlow.DataSource = this._listSaleFlow;
                    //    //this.bindingSaleFlow.ResetBindings(true);
                    //    ResetSaleState(false);
                    //    #endregion
                    //    this.ClearInput();
                    //}
                    #region 存在会员信息时领蛋判断
                    t_member_info _temp = MemberService.Instance.CheckMemberinfoForLL(_currentMember.mem_no);
                    if (_temp != null)
                    {
                        if (_temp.code == "1")
                        {
                            if (_temp.time.Date == System.DateTime.Now.Date)
                            {
                                //接口参数
                                Dictionary<string, object> _dic = Gfunc.GetServiceParameter();
                                _dic.Add("vip_no", _currentMember.mem_no);
                                _dic.Add("item_no", "80310891");
                                _dic.Add("plan_no", "WX80310891");
                                bool isok1 = true;
                                string errorMessage = string.Empty;
                                //参数提交
                                string result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/GetIsFirstSale", _dic, ref isok1, ref errorMessage);
                                if (isok1)
                                {
                                    if (result == "0")
                                    {
                                        bool iscz = false;//销售列表里是否存在本次活动的商品
                                        foreach (t_cur_saleflow saleflowWX in this.bindingSaleFlow)
                                        {
                                            if (saleflowWX.plan_no == "WX80310891")
                                            {
                                                iscz = true;
                                            }
                                        }
                                        if (!iscz)
                                        {
                                            //绑定赠送商品
                                            this.SetGridViewColumnsValue("80310891");
                                            //更改参数为赠送的参数
                                            t_cur_saleflow saleflow = this.bindingSaleFlow.Current as t_cur_saleflow;
                                            if (saleflow.sale_way == "A")
                                            {
                                                saleflow.unit_price = 0M;
                                                saleflow.sale_money = 0M;
                                                saleflow.sale_way = "C";
                                                saleflow.plan_no = "WX80310891";
                                                this.ShowMessage("增送商品<" + saleflow.item_name + ">，数量<" + saleflow.sale_qnty + ">");
                                            }
                                            this.bindingSaleFlow.ResetBindings(false);
                                            this.ResetSaleFlowData();
                                            this.SetTotalAmt();
                                            this.GvSaleFlow.Refresh();
                                            //向销售商品流水表插入数据
                                            Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                                            this.ClearInput();
                                        }
                                        else
                                        {
                                            this.ShowMessage("已存在活动商品！");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("您领取过本活动的赠品！", Gattr.AppTitle);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("接口信息获取错误！", Gattr.AppTitle);
                                }
                            }
                            else
                            {
                                MessageBox.Show("您不是今日绑定的会员信息！本次活动仅限于当日绑定的会员！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("您未将会员卡与微信绑定，无法参加活动，请使用微信绑定会员卡！", Gattr.AppTitle);
                        }
                    }
                    #endregion
                }
                else
                {
                    FrmHD frm = new FrmHD();
                    frm.SetMemberEvent += (minfo) =>
                    {
                        _currentMember = minfo;
                        if (_currentMember.mem_type == "WX")
                        {
                            SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, _currentMember.mem_no + "[利澜微会员]", _currentMember.balance, _currentMember.score);
                        }
                    };
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (_currentMember != null)
                        {
                            if (_currentMember.mem_type == "WX")
                            {
                                this.lbVipValue.Text = "【" + _currentMember.mem_no + "】" + _currentMember.username + "[供货港微会员]";
                                _isVip = true;
                                this.ShowMessage("会员：" + _currentMember.mem_no + "成功记录");

                                #region 取消所有的商品折扣和活动
                                List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                                List<t_cur_saleflow> tList = SetSinglePlanRule(_saleInfos);
                                _listSaleFlow.Clear();
                                _listSaleFlow = tList;
                                this.ResetSaleFlowData();
                                this.bindingSaleFlow.DataSource = this._listSaleFlow;
                                this.bindingSaleFlow.ResetBindings(true);
                                this.SetPayTotelAmt();
                                #endregion

                                //绑定赠送商品
                                this.SetGridViewColumnsValue("80310891");
                                //更改参数为赠送的参数
                                t_cur_saleflow saleflow = this.bindingSaleFlow.Current as t_cur_saleflow;
                                if (saleflow.sale_way == "A")
                                {
                                    saleflow.unit_price = 0M;
                                    saleflow.sale_money = 0M;
                                    saleflow.sale_way = "C";
                                    saleflow.plan_no = "WX80310891";
                                    this.ShowMessage("增送商品<" + saleflow.item_name + ">，数量<" + saleflow.sale_qnty + ">");
                                }
                                this.bindingSaleFlow.ResetBindings(false);
                                this.ResetSaleFlowData();
                                this.SetTotalAmt();
                                this.GvSaleFlow.Refresh();
                                //向销售商品流水表插入数据
                                Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                                this.ClearInput();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 更换营业员操作
        /// <summary>
        /// 更换营业员操作
        /// </summary>
        private void FunKeySaleMan()
        {
            string strLog = "";
            FrmSaleMan frmSm = new FrmSaleMan();
            frmSm.SetOperatorInfo += (id, name) =>
            {
                Gattr.OperId = id;
                Gattr.OperFullName = name;
                this.lbShopAssistantValue.Text = string.Format("【{0}】{1}", Gattr.OperId, Gattr.OperFullName);
            };
            if (frmSm.ShowDialog() == DialogResult.Cancel)
            {
                strLog = "输入营业员被取消！";
            }
            else
            {
                strLog = "当前营业员：" + this.lbShopAssistantValue.Text;
            }
            this.ShowMessage(strLog);
            this.ClearInput();
        }

        #endregion
        #region 结算操作按钮
        /// <summary>
        /// 结算操作
        /// </summary>
        private void FunKeyTot()
        {
            if (this.bindingSaleFlow.Count > 0)
            {
                this.ClearInput();
                this.SetPosState(PosOpState.PAY);
                this.IsPayState = false;
                this.SetCursorText();
                if ((this.bindingSaleFlow.Count == 0))
                {
                    this.tblPanelBalance.Visible = false;
                }
                else if (Gattr.IsTotRollBack)
                {
                    this._listSaleFlow.Clear();
                    foreach (t_cur_saleflow _saleflow in this._backupSalesBeforeTot)
                    {
                        this._listSaleFlow.Add(_saleflow.clone() as t_cur_saleflow);
                    }
                    this.bindingSaleFlow.ResetBindings(false);
                    this.GvSaleFlow.Refresh();
                    this.SetTotalAmt();
                    this.tblPanelBalance.Visible = false;
                }
                else
                {
                    this._backupSalesBeforeTot = new List<t_cur_saleflow>();

                    foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
                    {
                        this._backupSalesBeforeTot.Add(_saleflow.clone() as t_cur_saleflow);
                    }

                    string vip_type = "ALL";
                    if (_currentMember != null && !string.IsNullOrEmpty(_currentMember.mem_type))
                    {
                        vip_type = string.Empty;
                    }

                    //List<t_cur_saleflow> sales = PlanDear.Instance.DearPGSaleInfo(this._listSaleFlow);
                    List<t_cur_saleflow> sales = PlanDealer.Instance.DearPGSaleInfo(this._listSaleFlow, vip_type);
                    this._listSaleFlow.Clear();
                    this._listSaleFlow.AddRange(sales);
                    this.ResetSaleFlowData();
                    this.bindingSaleFlow.ResetBindings(false);
                    this.GvSaleFlow.Refresh();
                    this.SetTotalAmt();

                    List<t_rm_plan_master> _planMasters = GetPlanPluMasters(this._listSaleFlow);
                    if (_planMasters != null && _planMasters.Count > 0)
                    {
                        FrmPlan _plan = new FrmPlan(_planMasters);
                        List<t_rm_plan_master> currentPlanMaster = new List<t_rm_plan_master>();
                        _plan.EventSetPlanMaster += (p) =>
                        {
                            currentPlanMaster = p;
                        };
                        if (_plan.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            //减钱的
                            //加钱填商品的，需要弹出商品窗体
                            foreach (t_rm_plan_master plan in currentPlanMaster)
                            {
                                //MessageBox.Show(plan.plan_no);
                                List<CheckPara> _cps = plan.PlanPara.ResultParas;
                                if (_cps[0].check_operator == "s=")
                                {
                                    //List<t_cur_saleflow> saleInfos = PlanDear.Instance.GetSendSaleInfo(_cps, plan.plan_no);
                                    List<t_cur_saleflow> saleInfos = PlanDealer.Instance.DearSendInfo(plan.plan_no, _cps);
                                    //PlanDear.Instance.GetSendSaleInfo(_cps, plan.plan_no);
                                    FrmSend frmSend = new FrmSend(plan.rule_desc, saleInfos);
                                    List<t_cur_saleflow> sendInfos = new List<t_cur_saleflow>();
                                    frmSend.SetSend += (infos) =>
                                    {
                                        if (infos != null && infos.Count > 0)
                                        {
                                            sendInfos = infos;
                                        }
                                    };
                                    if (frmSend.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        //赠送的商品进行计算
                                        //List<t_cur_saleflow> sendItems = PlanDear.Instance.DearSendFlow(sendInfos, plan);
                                        List<t_cur_saleflow> sendItems = PlanDealer.Instance.DearSendFlow(plan, saleInfos);
                                        //PlanDear.Instance.DearSendFlow(sendInfos, plan);
                                        this._listSaleFlow.AddRange(sendItems);
                                        this.ResetSaleFlowData();
                                        this.bindingSaleFlow.ResetBindings(false);
                                        this.SetTotalAmt();
                                        goto NormalTOT;
                                    }
                                    else
                                    {
                                        goto NormalTOT;
                                    }
                                }
                                else
                                {
                                    //没有赠送信息的促销规则
                                    //List<t_cur_saleflow> sendItems = PlanDear.Instance.DearPluSaleFlow(plan, this._listSaleFlow);
                                    List<t_cur_saleflow> sendItems = PlanDealer.Instance.DearPluSaleFlow(plan, this._listSaleFlow);
                                    //PlanDear.Instance.DearPluSaleFlow(plan, this._listSaleFlow);
                                    this._listSaleFlow.Clear();
                                    this._listSaleFlow.AddRange(sendItems);
                                    this.ResetSaleFlowData();
                                    this.bindingSaleFlow.ResetBindings(false);
                                    this.SetTotalAmt();
                                    goto NormalTOT;
                                }
                            }
                        }
                        else
                        {
                            goto NormalTOT;
                        }
                    }
                    else
                    {
                        goto NormalTOT;
                    }
                NormalTOT:
                    //判断当前结算商品是否为空
                    this.ShowMessage("结算开始！！！！");
                    this.tblPanelBalance.Visible = true;
                    this.tlpMain.Enabled = false;
                    decimal d = 0M;
                    foreach (t_cur_saleflow saleflow in this.bindingSaleFlow)
                    {
                        d += saleflow.sale_money;
                    }
                    if (d == 0)
                    {
                        this.lbPayTotalAmt.Text ="0.00";
                        this.lbPayAmtRec.Text = "0.00";
                        //this.SetPosState(PosOpState.PLU);
                    }
                    else
                    {
                        this.SetPayTotelAmt();
                    }
                    Gattr.IsTotRollBack = false;
                }
            }

        }
        #endregion
        #region 支付方式
        #region 其他付款方式
        /// <summary>
        /// 其它付款方式
        /// </summary>
        public void FunKeyFkf()
        {
            if (this._posState != PosOpState.PAY || (this._posState != PosOpState.CHG))
            {
                FrmPayWay frmPayWay = new FrmPayWay(Convert.ToDecimal(this.lbPayAmtRec.Text), defaultPayWay, "A",_currentMember);

                if (frmPayWay.ShowDialog() == DialogResult.OK)
                {
                    if (this.PosPayAmt(frmPayWay.ReturnPayWay, frmPayWay.ReturnMoney, true,false))
                    {
                        LoggerHelper.Log("MsmkLogger", "其他支付方式支付！", LogEnum.SysLog);
                    }
                    else
                    {
                        this.ShowMessage("用户取消操作！");
                    }
                }

            }
            else
            {
                this.ShowMessage("当前状态，此功能键无效!");
            }
        }
        #endregion
        #region 余额支付
        /// <summary>
        /// 余额支付
        /// </summary>
        public void FunKeyYezf()
        {
            if (this._posState != PosOpState.PAY || (this._posState != PosOpState.CHG))
            {
                if (_currentMember != null)
                {
                    if (Convert.ToDecimal(this.lbPayAmtRec.Text) != 0)
                    {
                        FrmPayBalance frmPayBalance = new FrmPayBalance(Convert.ToDecimal(this.lbPayAmtRec.Text), "CHA", _currentMember,moneyCHA*-1);

                        if (frmPayBalance.ShowDialog() == DialogResult.OK)
                        {
                            if (this.PosPayAmt("CHA", frmPayBalance.ReturnMoney, true, false))
                            {
                                if (moneyCHA > 0 || Convert.ToDecimal(this.lbPayTotalAmt.Text)<0)
                                    moneyCHA = -1;
                                //LoggerHelper.Log("MsmkLogger", "会员" + _currentMember.mem_no + "使用余额支付！支付金额：" + frmPayBalance.ReturnMoney, LogEnum.SysLog);
                            }
                            else
                            {
                                this.ShowMessage("余额支付-->>用户取消操作！");
                            }
                        }
                    }
                    else
                    {
                        /********用于取消后重新进入余额支付的输入验证码状态**********/
                        bool ispay = true;
                        String flow_no = Gfunc.GetFlowNo();//Gfunc.GenerateId();
                        String errorMessge = string.Empty;
                        //MemeberPayInfo(ref ispay, memberNo, ref errorMessge);

                        if (!PayByCharge(flow_no))
                        {
                            return;
                        }
                        t_app_viplist vipinfo = null;
                        t_member_info memberinfo = UserMemberPay(flow_no, ref ispay, ref vipinfo);
                        if (memberinfo != null)
                        {
                            if (memberinfo.code == "404")//离线会员记录
                            {
                                ispay = Gattr.Bll.AddVipflow(vipinfo);
                            }
                            else
                            {
                                if (ispay)
                                {
                                    //正常会员消费记录
                                    vipinfo.com_flag = "0";
                                    ispay = Gattr.Bll.AddVipflow(vipinfo);
                                }
                                else
                                {
                                    //消费错误提示
                                    MessageBox.Show(memberinfo.info, Gattr.AppTitle);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            vipinfo.com_flag = "0";
                            if (vipinfo != null)
                            {
                                if (!string.IsNullOrEmpty(vipinfo.card_no))
                                {
                                    ispay = Gattr.Bll.AddVipflow(vipinfo);
                                }
                            }
                        }
                        //处理数据
                        if (!ispay)
                        {
                            MessageBox.Show("会员消费保存失败", Gattr.AppTitle);
                        }
                        else
                        {
                            Gattr.Bll.SavePosBill(flow_no, this._listSaleFlow, this._listPayFlow);
                            Gattr.Bll.DeleteTempData();
                            Gfunc.SetFlowNo();
                            this._isCHG = true;
                            this.IsPayState = true;
                            this.SetPosState(PosOpState.CHG);
                            this.ClearInput();
                            //****折扣返余额计算方式****//
                            /***********含有余额混合支付的不参与次活动，因此返余额无必要*/
                            this.ShowMessage("结算完成!");
                            //--------------------------打印操作-----------------
                            _listWinPrtStr.Clear();
                            Gattr.PosPrinter.OpenPrinter(Gattr.PosModel, Gattr.PosPort);
                            Gfunc.PrintHeader();
                            this.PrintHeaderDetail(flow_no);
                            this.PrintItemDetail(this._listSaleFlow);
                            this.PrintFooterDetail(vipinfo);
                            Gfunc.SendToAPIPrint();
                            _currentFlowNo = string.Empty;
                            this.lbPayAmtChange.Text = "0.00";
                            this.ResetSaleState(true);
                            //this.tblPanelBalance.Visible = false;
                        }
                    }
                }
                else
                {
                    DialogResult diaRes = MessageBox.Show("会员信息不存在，是否现在登陆！", Gattr.AppTitle, MessageBoxButtons.YesNo);
                    if (diaRes == DialogResult.Yes)
                    {
                        this.FunKeyVipSale(true);
                    }
                }
            }
            else
            {
                this.ShowMessage("当前状态，此功能键无效!");
            }
        }
        #endregion
        #region 测试方法
        /// <summary>
        ///  测试方法
        /// </summary>
        /// <param name="mon"></param>
        private void abc(decimal mon)
        {
            //美元 mon
            decimal a = Convert.ToDecimal(mon * 6.14M);
            if (a != _saleTotalAmt)
            {
                decimal b = a - this._saleTotalAmt;
                //处理数据
                FrmPayWay py = new FrmPayWay(Math.Round(b), "RMB", "A", _currentMember);
                DialogResult result = py.ShowDialog();
                defaultPayWay = py.ReturnPayWay;
                if (result == DialogResult.Cancel)
                {
                    ///处理数据
                }
            }
        }
        #endregion
        #region 人民币支付
        /// <summary>
        /// 人民币支付
        /// </summary>
        private void FunKeyRmb()
        {
            if (this._posState != PosOpState.PAY && this._posState != PosOpState.CHG)
            {
                this.ShowMessage("当前状态，此功能键无效!");
            }
            else
            {
                decimal result = 0M;
                if (this._inputText.Length == 0)
                {
                    //付款结束操作
                    if (this.IsPayState)
                    {
                        //付款结束
                        this.lbAmtTitle.Text = "总价：";
                        this._isCHG = false;
                        this.ResetSaleState();
                    }
                    else
                    {
                        if (this.bindingPayFlow.Count <= 0)
                        {
                            decimal payAmtPaid = Convert.ToDecimal(String.Format("{0:F}", SIString.TryDec(this.lbPayAmtRec.Text)));
                            if (this.PosPayAmt("RMB", payAmtPaid, false, false))
                            {
                                LoggerHelper.Log("MsmkLogger", System.DateTime.Now.ToString() + "进行人民币支付操作", LogEnum.SysLog);
                                return;
                            }
                        }
                        else
                        {
                            this.ShowMessage("请输入金额！！！！！");
                        }
                    }
                }
                decimal num2 = SIString.TryDec(this._inputText.Trim());
                decimal num5 = SIString.TryDec(this.lbPayAmtRec.Text.Trim());//应收金额
                if (num5 < 0)
                {
                    if (num2 >= 0)
                    {
                        MessageBox.Show("现在为退货操作，只能输入负数金额!", Gattr.AppTitle);
                        this.ClearInput();
                        return;
                    }
                    else if(num2<num5)
                    {
                        MessageBox.Show("退款金额不能大于所剩货物金额！", Gattr.AppTitle);
                        this.ClearInput();
                        return;
                    }
                }
                else
                {
                    if (num2<0)
                    {
                        MessageBox.Show("正常交易，请不要输入负数金额!", Gattr.AppTitle);
                        this.ClearInput();
                        return;
                    }
                }
                this._inputText = this._inputText.Trim();
                decimal num3 = Convert.ToDecimal(this.lbPayAmtChange.Text);
                if ((num2 == 0M) && (num2 != SIString.TryDec(this.lbPayAmtChange.Text)))
                {
                    this.ShowMessage("请输入付款金额!");
                    this.ClearInput();
                }
                else if (!decimal.TryParse(this._inputText, out result))
                {
                    this.ShowMessage("金额输入错误,请重新输入!");
                    this.ClearInput();
                }
                else
                {
                    try
                    {

                        if (!this.PosPayAmt("RMB", result, false, false))
                        {

                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }
                }
            }
        }
        #endregion
        #region 充值卡
        /// <summary>
        /// 充值卡
        /// </summary>
        private void FunKeyCsc()
        {
            if (this._posState != PosOpState.PAY)
            {
                this.ShowMessage("该状态下，此功能键无效！");
            }
            else
            {
                this.ClearInput();
                if (this.PosPayAmt("SAV", 0M, false, false))
                {
                    LoggerHelper.Log("MsmkLogger", System.DateTime.Now.ToString() + "使用充值卡支付！", LogEnum.SysLog);
                }
            }
        }
        #endregion
        #region 会员余额支付功能实现
        private bool PayByCharge(string flow_no)
        {
            bool isok = false;
            decimal payamt = 0M;

            try
            {
                bool isCha = false;
                foreach (t_cur_payflow payflow in this._listPayFlow)
                {
                    if (payflow.pay_way == "CHA")
                    {
                        isCha = true;
                        payamt += payflow.pay_amount;
                    }
                }
                if (isCha&&moneyCHA==0)
                {
                    FrmCharge frmCharge = new FrmCharge(_currentMember.mem_no);
                    string _vercode = string.Empty;
                    frmCharge.SetVerCode += (vercode) => {
                        _vercode = vercode;
                    };
                    DialogResult dlr = frmCharge.ShowDialog();//返回窗体响应值
                    if (dlr == System.Windows.Forms.DialogResult.OK)
                    {
                        t_member_info info = MemberService.Instance.UseMemberCardPayNew(_currentMember.mem_no,
                              payamt.ToString(),
                              flow_no,
                              _vercode,
                              "门店消费:" + flow_no.ToString());
                        if (info.code == "1")
                        {
                            isok = true;
                        }
                        else
                        {
                            MessageBox.Show(info.info, Gattr.AppTitle);
                            isok = false;
                        }
                    }
                    else if (dlr == System.Windows.Forms.DialogResult.Ignore)//挂单的返回响应值
                    {
                        //挂单
                        this.FunKeyPnr("yezf");
                        //ResetSaleState();
                    }
                    else
                    {
                        MessageBox.Show("请输入验证码", Gattr.AppTitle);
                        return false;
                    }
                }
                else if ((isCha && moneyCHA == -1 )|| (isCha && moneyCHA > 0))//直接余额退款
                {
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    String ordername = Convert.ToInt64(ts.TotalSeconds).ToString();
                    payamt = payamt * -1;
                    if (MessageBox.Show("确定将本单中的【" + payamt + "元】退还到会员余额？", Gattr.AppTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        t_member_info memberinfo = MemberService.Instance.RechargeMoney(_currentMember.mem_no, payamt.ToString(), ordername, "RMB");
                        if (memberinfo.code == "1")
                        {
                            t_member_info memberinfo1 = MemberService.Instance.RechargeMoneyFinsh(_currentMember.mem_no, ordername);
                            if (memberinfo1.code == "1")
                            {
                                MessageBox.Show("退货成功，卡号:[" + _currentMember.mem_no + "]充值金额[" + payamt + "]元", Gattr.AppTitle);
                                LoggerHelper.Log("MsmkLogger", "【" + Gattr.OperId + "】进行退货--->>余额支付退款！退款成功，卡号:[" + _currentMember.mem_no + "]充值金额[" + payamt + "]", LogEnum.SysLog);
                                moneyCHA = 0;
                                isok = true;
                            }
                            else
                            {
                                MessageBox.Show(memberinfo1.info, Gattr.AppTitle);
                                isok = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show(memberinfo.info, Gattr.AppTitle);
                            isok = false;
                        }
                    }
                }
                else
                {
                    isok = true;
                }
            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }
        #endregion
        #region 银行卡支付
        /// <summary>
        /// 银行卡支付
        /// </summary>
        private void FunKeyBcd()
        {
            if (this._posState != PosOpState.PAY)
            {
                this.ShowMessage("前状态，此功能键无效!");
            }
            else
            {
                decimal payAmt = 0M;
                payAmt = this.GetTotalPayedAmt();
                if (this.PosPayAmt("CRD", payAmt, false, false))
                {
                    LoggerHelper.Log("MsmkLogger", System.DateTime.Now.ToString() + "使用银行卡支付！", LogEnum.SysLog);
                }
            }
        }
        #endregion

        #region 支付宝支付
        /// <summary>
        /// 支付宝支付
        /// </summary>
        private void FunKeyZFB()
        {
            String Alipay_No = String.Empty;//订单号（提交支付宝用）
            try
            {
                if (this._posState == PosOpState.PAY)
                {
                    //生成订单号
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    String time = Convert.ToInt64(ts.TotalSeconds).ToString();     
                    Alipay_No="JILAN" +Gattr.OperId+ time;
                    //调用二维码支付接口获取二维码显示
                    string Qrcode_url = GetQrcode(Alipay_No,Convert.ToDecimal(this.lbPayAmtRec.Text));
                    //获取html文件内容
                    //string Qrcode_html = Gattr.PayHtml.Replace("%QrcodeURL%", Qrcode_url);
                    string Qrcode_html = Qrcode_url;
                    if (Qrcode_url != "-1" && Qrcode_html != string.Empty)
                    {
                        //FrmAliPay frm_alipay = new FrmAliPay(Convert.ToDecimal(this.lbPayAmtRec.Text), Alipay_No);
                        //this.SetDoubleQrcode(Qrcode_html);
                        //if (frm_alipay.ShowDialog() == DialogResult.OK)
                        //{
                        //    if (this.PosPayAmt("ZFB", frm_alipay.ReturnMoney, true, false))
                        //    {
                        //        LoggerHelper.Log("MsmkLogger", "使用支付宝支付！", LogEnum.SysLog);
                        //    }
                        //}
                        //else
                        //{
                        //    this.ShowMessage("取消支付宝支付！");
                        //}
                        ////恢复顾显URL显示
                        //this.SetDoubleUrl(Gattr.adUrl);
                        //双屏时显示
                        Screen[] sc = Screen.AllScreens;
                        if (sc.Length == 2)
                        {
                            //if (Gattr.IsDoubleModule && (this.frmdouble != null))
                                if (Gattr.IsDoubleModule)
                                {
                                //设置二维码顾显显示
                                this.SetDoubleQrcode(Qrcode_html);
                                //显示支付宝支付窗体
                                FrmAliPay frm_alipay = new FrmAliPay(Convert.ToDecimal(this.lbPayAmtRec.Text), Alipay_No);
                                if (frm_alipay.ShowDialog() == DialogResult.OK)
                                {
                                    if (this.PosPayAmt("ZFB", frm_alipay.ReturnMoney, true, false))
                                    {
                                        LoggerHelper.Log("MsmkLogger", "使用支付宝支付！", LogEnum.SysLog);
                                    }
                                }
                                else
                                {
                                    this.ShowMessage("取消支付宝支付！");
                                }
                                //恢复顾显URL显示
                                this.SetDoubleUrl(Gattr.adUrl);
                            }
                            else
                            {
                                MessageBox.Show("顾显未正常显示，无法使用此功能！", Gattr.AppTitle);
                            }
                        }
                        else
                        {
                            MessageBox.Show("未发现顾显，无法使用此功能！", Gattr.AppTitle);
                        }
                    }
                    else
                    {
                        MessageBox.Show("支付二维码生成失败，无法使用此功能！", Gattr.AppTitle);
                    }
                }
                else
                {
                    this.ShowMessage("当前状态，此功能键无效!");
                }
            }
            catch(Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "FrmMain ==> FunKeyZFB ==>支付宝二维码支付出现异常！" + ex.ToString(), LogEnum.ExceptionLog);
            }
        }

        /// <summary>
        /// 生成支付二维码
        /// </summary>
        /// <param name="ordername"></param>
        /// <returns></returns>
        public string GetQrcode(String flow_no, decimal pay_amt)
        {
            String isConnect = String.Empty;
            String address = String.Empty;
            String response = String.Empty;
            String serviceUrl = String.Empty;
            bool isok = true;
            string errorMessage = string.Empty;
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            try
            {
                _dic.Add("username", "");
                _dic.Add("password","");
                _dic.Add("access_token",Gattr.access_token);
                _dic.Add("client_id", Gattr.client_id);
                _dic.Add("pay_amount", pay_amt);
                _dic.Add("flow_no", flow_no);
               isConnect = PServiceProvider.Instance.InvokeMethod("http://esales/manager/api/ws/API/GetQrcode", _dic, ref isok, ref errorMessage);
                //isConnect = PServiceProvider.Instance.InvokeMethod("http://www.ghuog.com/erpalipay/index.php", _dic, ref isok, ref errorMessage);
                //var jObject = JObject.Parse(isConnect);
                //isConnect = jObject["data"]["imgUrl"].ToString();
                if (isConnect == "error")
                {
                    isConnect = "-1";
                    LoggerHelper.Log("MsmkLogger", Gattr.OperId + "支付二维码生成失败！", LogEnum.SysLog);
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
        #endregion

        #endregion

        #region 操作手册
        private void FunKeyMyHelp()
        {
            if (File.Exists(Gattr.MyHelp))
            {
                Help.ShowHelp(this, Gattr.MyHelp);
            }
            else
            {
                MessageBox.Show("操作手册不存在！");
            }
        }
        #endregion

        #region 会员信息绑定
        private void VipShow(string vipNo)
        {
            if (vipNo.Length == 0)
            {
                MessageBox.Show("请输入会员编号", Gattr.AppTitle);
            }
            else
            {
                t_member_info meminfo = MemberService.Instance.GetMemberInfoByMemNo(vipNo);
                if (meminfo.code == "-1")
                {
                    MessageBox.Show(meminfo.info, Gattr.AppTitle);
                }
                else
                {
                    if (meminfo.code == "1")
                    {
                        _currentMember = meminfo;
                        t_member_info _temp = MemberService.Instance.CheckMemberinfoForLL(meminfo.mem_no);
                        if (_temp != null)
                        {
                            if (_temp.code == "1")
                            {
                                SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, _currentMember.mem_no + "[利澜微会员]", _currentMember.balance, _currentMember.score);
                                this.lbVipValue.Text = "【" + _currentMember.mem_no + "】" + _currentMember.username + "[利澜微会员]";
                            }
                            else
                            {
                                SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, _currentMember.mem_no, _currentMember.balance, _currentMember.score);
                                this.lbVipValue.Text = "【" + _currentMember.mem_no + "】" + _currentMember.username;
                            }
                        }
                        _isVip = true;
                        #region 取消所有的商品折扣和活动
                        List<t_cur_saleflow> _saleInfos = this._listSaleFlow;
                        List<t_cur_saleflow> tList = SetSinglePlanRule(_saleInfos);
                        _listSaleFlow.Clear();
                        _listSaleFlow = tList;
                        this.ResetSaleFlowData();
                        this.bindingSaleFlow.DataSource = this._listSaleFlow;
                        this.bindingSaleFlow.ResetBindings(true);
                        this.SetPayTotelAmt();
                        #endregion
                    }
                    else
                    {
                        if (meminfo.code == "404")
                        {
                            MessageBox.Show("会员信息获取错误", Gattr.AppTitle);
                        }
                    }
                }
            }
        }
        #endregion

        #region 折扣返余额

        private bool DiscountToYE(decimal amount,string vip_no)
        {
            bool isok = false;
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            String ordername = Convert.ToInt64(ts.TotalSeconds).ToString();
            t_member_info memberinfo = MemberService.Instance.RechargeMoney(vip_no, amount.ToString(), ordername, "RMB");
            if (memberinfo.code == "1")
            {
                t_member_info memberinfo1 = MemberService.Instance.RechargeMoneyFinsh(vip_no, ordername);
                if (memberinfo1.code == "1")
                {
                    LoggerHelper.Log("MsmkLogger", "【" + Gattr.OperId + "】结算操作--->>折扣返余额！余额充值成功，卡号:[" + vip_no + "]充值金额[" + amount + "]", LogEnum.SysLog);
                    isok = true;
                }
                else
                {
                    MessageBox.Show(memberinfo1.info, Gattr.AppTitle);
                    isok = false;
                }
            }
            else
            {
                MessageBox.Show(memberinfo.info, Gattr.AppTitle);
                isok = false;
            }
            return isok;
        }
        #endregion

        /// <summary>
        /// 开钱箱
        /// </summary>
        private void FunKeyOPD()
        {
            string cashierNo = Gattr.OperId;
            //Gattr.Bll.WriteRetailLog("OPD", 0M, "", Gattr.OperId, "", 0M, true);
            string message = "现在在开钱箱...";
            // RetailGlobal.Bll.WriteSysLog(RetailGlobal.LogFile, "开钱箱", "开钱箱", 1);
            this.OpenCashbox();
            this.ShowMessage(message);
            this.ClearInput();
        }
        private void OpenCashbox()
        {
            ////if (!Gfunc.PosCheckGrant(ref RetailGlobal.CashierNo, PosGrant.OpenDraw, ref this._msgString))
            ////{
            ////    this.ShowMessage(this._msgString);
            ////    this.ClearInput();
            ////}
            ////else
            ////{
            //Gfunc.OpenCash();
            ////}
            PrintLPTUtility PrintLPT = new PrintLPTUtility();
            if (PrintLPT.Open(Gattr.PortName))
            {
                try
                {
                    if (PrintLPT.OpenMbox())
                    {
                        LoggerHelper.Log("MsmkLogger", Gattr.OperId + "通过测试LPT开钱箱！", LogEnum.SysLog);
                        //关闭端口
                        PrintLPT.Close();
                    }
                    else
                    {
                        //关闭端口
                        PrintLPT.Close();
                    }

                }
                catch (Exception exception)
                {
                    //关闭端口
                    PrintLPT.Close();
                    MessageBox.Show(exception.Message, "系统提示");
                    LoggerHelper.Log("MsmkLogger", "LPT开钱箱错误：" + exception.Message, LogEnum.SysLog);
                }

            }
            else
            {
                MessageBox.Show(Gattr.PortType + "端口无效！", Gattr.AppTitle);
                LoggerHelper.Log("MsmkLogger", "开钱箱==>"+Gattr.PortType + "端口无效！", LogEnum.SysLog);
            }
        }

        /// <summary>
        /// 支付金额处理方法
        /// </summary>
        /// <param name="payWay">支付方法：RMB</param>
        /// <param name="payAmt">支付金额：</param>
        /// <param name="isGift">是否整单赠送</param>
        /// <returns></returns>
        private bool PosPayAmt(string payWay, decimal payAmt, bool isRedirect, bool isGift)
        {
            //已付账款
            decimal payAmtPaid = payAmt;//Convert.ToDecimal(String.Format("{0:F}", SIString.TryDec(payAmt.ToString())));
            //应收账款
            decimal payAmtRec = Convert.ToDecimal(String.Format("{0:F}", SIString.TryDec(this.lbPayAmtRec.Text)));
            //decimal rate = 1M; //
            // 总付款金额
            decimal totalPayedAmt = this.GetTotalPayedAmt();
            t_payment_info payment = null;
            t_cur_payflow payflow = null;
            String _card = String.Empty;
            String _memo = String.Empty;
            try
            {
                switch (payWay.ToUpper())
                {
                    case "RMB":
                        #region 人民币付款
                        payment = new t_payment_info() { rate = 1M, pay_way = "RMB", pay_name = "人民币付款" };
                        if (payAmtRec < 0)//退货需要找钱
                        {
                            if (moneyCHA >0)
                            {
                                MessageBox.Show("本单含有余额支付！请先退余额，再退现金！");
                                this.FunKeyYezf();
                            }
                            else
                            {
                                if (((payAmtPaid - payAmtRec) % 1 == 0))//&& Math.Abs(payAmtRec) >= payAmt
                                {
                                    payflow = SetPayFlowInfo(payAmtPaid, payment, this._saleTotalAmt, _currentFlowNo, string.Empty, string.Empty);
                                    this._listPayFlow.Add(payflow);
                                }
                                else
                                {
                                    this.ClearInput();
                                    this.ShowMessage("金额输入错误！");
                                }
                            }
                        }
                        else
                        {
                            if (payAmtPaid - payAmtRec <= 100)
                            {
                                if (payAmtRec >= 0)
                                {
                                    payflow = SetPayFlowInfo(payAmtPaid, payment, this._saleTotalAmt, _currentFlowNo, string.Empty, string.Empty);
                                    //整单赠送的设置为C
                                    if(isGift)  payflow.sale_way = "C";
                                    this._listPayFlow.Add(payflow);
                                    if (this._posState == PosOpState.PAY)
                                    {
                                        decimal tmpPayTotalAmt = 0M;
                                        decimal tmpFlowId = 0;
                                        foreach (t_cur_payflow _tmpPayflow in this._listPayFlow)
                                        {
                                            tmpPayTotalAmt += _tmpPayflow.pay_amount;
                                            tmpFlowId = _tmpPayflow.flow_id + 1;
                                        }
                                        //找零
                                        if (tmpPayTotalAmt > Convert.ToDecimal(this.lbPayTotalAmt.Text))
                                        {
                                            t_cur_payflow _payflow2 = new t_cur_payflow
                                            {
                                                flow_id = tmpFlowId,
                                                pay_way = defaultPayWay,
                                                pay_name = payment.pay_name + "找零",
                                                coin_rate = 1,
                                                coin_type = defaultPayWay,
                                                sale_amount = this.SaleTotalAmt,
                                                pay_amount = tmpPayTotalAmt - Convert.ToDecimal(this.lbPayTotalAmt.Text),
                                                convert_amt = tmpPayTotalAmt - Convert.ToDecimal(this.lbPayTotalAmt.Text),
                                                card_no = "",
                                                sale_way = "D",
                                                oper_id = Gattr.OperId,
                                                oper_date = DateTime.Now.ToString("s"),
                                                pos_id = Gattr.PosId,
                                                branch_no = Gattr.BranchNo,
                                                voucher_no = _currentFlowNo
                                            };
                                            this._listPayFlow.Add(_payflow2);
                                        }
                                    }
                                }
                                else
                                {
                                    this.ClearInput();
                                    this.ShowMessage("金额输入错误！");
                                }
                            }
                            else
                            {
                                this.ClearInput();
                                this.ShowMessage("金额输入错误！");
                            }
                        }
                        #endregion
                        break;
                    case "BCD":
                    case "CRD":
                        #region 银行卡
                        payment = new t_payment_info() { rate = 1M, pay_way = payWay.ToUpper(), pay_name = "银行卡" };
                        if (payWay.ToUpper() == "CRD")
                        {
                            payment = new t_payment_info() { rate = 1M, pay_way = payWay.ToUpper(), pay_name = "人民币信用卡" };
                        }
                        decimal _tempAmt = _saleTotalAmt - GetTotalPayedAmt();
                        if (isRedirect)
                        {
                            _tempAmt = payAmt;
                        }
                        FrmPayCardNo frmPayCard = new FrmPayCardNo("信用（银行）卡号支付", "卡号：", _tempAmt);
                        frmPayCard.SetBCDCardAndMemo += (card, memo, pay) =>
                        {
                            _card = card;
                            _memo = memo;
                            payAmtPaid = SIString.TryDec(pay);
                        };
                        if (frmPayCard.ShowDialog() == DialogResult.OK)
                        {
                            payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, _card, _memo);
                            _listPayFlow.Add(payflow);
                        }
                        #endregion
                        break;
                    case "SAV":
                        #region 储值卡/会员卡
                        decimal _tempAmt1 = _saleTotalAmt - GetTotalPayedAmt();
                        if (isRedirect)
                        {
                            _tempAmt1 = payAmt;
                        }
                        payment = new t_payment_info() { rate = 1M, pay_way = "VIP", pay_name = "会员卡" };
                        FrmPaySavCard frmSaveCard = new FrmPaySavCard(_currentMember, _tempAmt1);
                        frmSaveCard.SetCardPayInfo += (card, pay, memo) =>
                        {
                            _card = card;
                            payAmtPaid = pay;
                            _memo = memo;
                        };
                        if (frmSaveCard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, _card, _memo);
                            _listPayFlow.Add(payflow);
                        }
                        #endregion
                        break;
                    case "HF":
                        #region  预付费卡
                        decimal _tempAmt2 = _saleTotalAmt - GetTotalPayedAmt();
                        if (isRedirect)
                        {
                            _tempAmt2 = payAmt;
                        }
                        payment = new t_payment_info() { rate = 1M, pay_way = "HF", pay_name = "预付费卡" };
                        FrmPayCardNo frmPayCard1 = new FrmPayCardNo("预付费卡", "卡号：", _tempAmt2);
                        frmPayCard1.SetBCDCardAndMemo += (card, memo, pay) =>
                        {
                            _card = card;
                            _memo = memo;
                            payAmtPaid = SIString.TryDec(pay);
                        };
                        if (frmPayCard1.ShowDialog() == DialogResult.OK)
                        {
                            payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, _card, _memo);
                            _listPayFlow.Add(payflow);
                        }
                        //payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, string.Empty, string.Empty);
                        //_listPayFlow.Add(payflow);
                        #endregion
                        break;
                    case "CHA":
                        decimal _tempAmt3 = _saleTotalAmt - GetTotalPayedAmt();
                        if (isRedirect)
                        {
                            _tempAmt3 = payAmt;
                        }
                        payAmtPaid = SIString.TryDec(_tempAmt3);
                        payment = new t_payment_info() { rate = 1M, pay_way = "CHA", pay_name = "余额支付" };
                        payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, _card, _memo);
                        _listPayFlow.Add(payflow);
                        break;
                    case "ZFB":
                        decimal _tempAmt4 = _saleTotalAmt - GetTotalPayedAmt();
                        if (isRedirect)
                        {
                            _tempAmt4 = payAmt;
                        }
                        payAmtPaid = SIString.TryDec(_tempAmt4);
                        payment = new t_payment_info() { rate = 1M, pay_way = "ZFB", pay_name = "支付宝支付" };
                        payflow = SetPayFlowInfo(payAmtPaid, payment, _saleTotalAmt, _currentFlowNo, _card, _memo);
                        _listPayFlow.Add(payflow);
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }
            if (payWay.Equals("COU"))
            {
                #region 购物券

                FrmPayCou card = new FrmPayCou(this.SaleTotalAmt);
                if (card.ShowDialog() == DialogResult.OK)
                {
                    if (card.ReturnMoney > 0)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    this.ShowMessage("用户取消操作！！！");
                }
                #endregion
            }
            else
            {
                //其它付款方式
                if (payWay.Equals("USD")) //美元处理方法
                {

                }
                else if (payWay.Equals("VIP")) //会员积分
                {
                    //判断积分金额
                }
                else if (payWay.Equals("HKD")) //港币处理
                {

                }
            }
            Gattr.Bll.SaveTempPayRow(_listPayFlow);
            this.bindingPayFlow.DataSource = this._listPayFlow;
            this.bindingPayFlow.ResetBindings(false);
            this.SetPayTotelAmt();
            this.ClearInput();
            return true;
        }
        /// <summary>
        /// 计算金额,保存所有数据
        /// </summary>
        private void SetPayTotelAmt()
        {
            if (this._posState != PosOpState.PAY)
            {
                return;
            }
            decimal payTotalAmt = 0M; //商品总价。付款
            //应收账款
            decimal payAmtRec = 0M;
            //已付账款
            decimal payAmtPaid = 0M;
            //找零账款
            decimal payAmtChange = 0M;
            foreach (t_cur_payflow payflow in this._listPayFlow)
            {
                if (!payflow.sale_way.Equals("D"))
                {
                    //if (payflow.sale_way.Equals("B"))
                    //{
                    //    payAmtPaid += (-payflow.pay_amount);
                    //}
                    //else
                    //{
                    payAmtPaid += payflow.pay_amount;
                    //}
                }
            }
            decimal payTotalAmt1 = Math.Abs(this._saleTotalAmt);

            if (_currentMember != null && _currentMember.mem_type == "WX")
            {
                //decimal sumAmt = OperateAmt(_saleTotalAmt);
                payTotalAmt1 = _saleTotalAmt;//商品总价
                payTotalAmt = Convert.ToDecimal(String.Format("{0:N}", _saleTotalAmt));
            }
            else
            {
                payTotalAmt = Convert.ToDecimal(String.Format("{0:N}", this._saleTotalAmt));
            }
            bool ispayback = false;
            if (this._saleTotalAmt < 0)
            {
                ispayback = true;
                //payAmtRec = payTotalAmt + payAmtPaid;
                if (this._listPayFlow.Count == 1 && Math.Abs(payAmtPaid) == payTotalAmt1)
                {
                    payAmtRec = payTotalAmt1 + payAmtPaid;
                }
                else
                {
                    decimal payTotalAmt2 = -payTotalAmt1;
                    if (payTotalAmt2 > 0)
                    {
                        payTotalAmt2 = -payTotalAmt2;
                    }

                    if (payAmtPaid < 0)
                    {
                        payAmtPaid = -payAmtPaid;
                    }
                    //应收账款 = 总金额 - 已付账款
                    payAmtRec = payAmtPaid + payTotalAmt2;
                    //if (payAmtPaid != 0)
                    //{

                    //}
                }
            }
            else
            {
                //应收账款 = 总金额 - 已付账款
                payAmtRec = payTotalAmt - payAmtPaid;
            }

            //合计金额
            //this.lbPayTotalAmt.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payTotalAmt.ToString("N1"))).ToString(Gattr.PosSaleAmtPoint);
            if (_currentMember != null && _currentMember.mem_type == "WX")
            {
                //decimal sumAmt = OperateAmt(payTotalAmt);
                //decimal sumAmt1 = OperateAmt(payAmtRec);
                this.lbPayTotalAmt.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payTotalAmt)).ToString(Gattr.PosSaleAmtPoint);
                this.lbPayAmtRec.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payAmtRec)).ToString(Gattr.PosSaleAmtPoint);
            }
            else
            {
                this.lbPayTotalAmt.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payTotalAmt)).ToString(Gattr.PosSaleAmtPoint);
                this.lbPayAmtRec.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payAmtRec)).ToString(Gattr.PosSaleAmtPoint);
            }
            //应付账款 
            //this.lbPayAmtRec.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payAmtRec)).ToString(Gattr.PosSaleAmtPoint);
            //已付账款 
            this.lbPayAmtPaid.Text = ExtendUtility.Instance.ParseToDecimal(String.Format("{0:N}", payAmtPaid)).ToString(Gattr.PosSaleAmtPoint);


            //已付大于应收
            if (payTotalAmt < 0)
            {
                if (_listPayFlow.Count == 0)
                {
                    if (ispayback)
                    {
                        if (payAmtPaid == 0)
                        {

                        }
                        else
                        {
                            payAmtChange = payAmtRec;
                        }
                    }
                    else
                    {
                        payAmtChange = payAmtRec;
                    }
                    if (payAmtChange > 0)
                    {
                        this.lbPayAmtChange.Text = Convert.ToDecimal(String.Format("{0:F}", payAmtChange)).ToString();
                    }
                    return;
                }
                else
                {
                    if (Math.Abs(payAmtPaid) >= Math.Abs(payTotalAmt))
                    {
                        this.lbAmtTitle.Text = "找零：";
                        this.lbPayAmtRec.Text = Convert.ToDecimal(String.Format("{0:F}", 0)).ToString();
                        this.lbAmtTotal.Text = Convert.ToDecimal(String.Format("{0:F}", -payAmtRec)).ToString();
                        this.lbPayAmtChange.Text = Convert.ToDecimal(String.Format("{0:F}", -payAmtRec)).ToString();
                        this._listPayFlow[this._listPayFlow.Count - 1].pay_amount = this._listPayFlow[this._listPayFlow.Count - 1].pay_amount - payAmtRec;
                        this._listPayFlow[this._listPayFlow.Count - 1].convert_amt = this._listPayFlow[this._listPayFlow.Count - 1].convert_amt;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (payAmtRec <= 0)
                {
                    payAmtChange = Math.Abs(payAmtRec);
                    //找零金额
                    this.lbPayAmtChange.Text = Convert.ToDecimal(String.Format("{0:F}", payAmtChange)).ToString();
                    this.lbPayAmtRec.Text = Convert.ToDecimal(String.Format("{0:F}", 0)).ToString();
                    this.lbAmtTitle.Text = "找零：";
                    this.lbAmtTotal.Text = this.lbPayAmtChange.Text;
                    //this.lbAmtTotal.Text = Convert.ToDecimal(String.Format("{0:F}", payAmtChange)).ToString();

                }
                else
                {
                    payAmtChange = 0;
                    this.lbPayAmtChange.Text = Convert.ToDecimal(String.Format("{0:F}", payAmtChange)).ToString();
                    //双屏
                    if (Gattr.IsDoubleModule && (this.frmdouble != null))
                    {
                        this.SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, "",0,0);
                    }
                    return;
                }
            }
            //双屏
            if (Gattr.IsDoubleModule && (this.frmdouble != null))
            {
                this.SetDoubleDisplay("2", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, "",0,0);
            }
            bool ispay = true;
            String flow_no = Gfunc.GetFlowNo();//Gfunc.GenerateId();
            String errorMessge = string.Empty;
            //MemeberPayInfo(ref ispay, memberNo, ref errorMessge);

            if (!PayByCharge(flow_no))
            {
                return;
            }

            t_app_viplist vipinfo = null;
            t_member_info memberinfo = UserMemberPay(flow_no, ref ispay, ref vipinfo);
            if (memberinfo != null)
            {
                if (memberinfo.code == "404")//离线会员记录
                {
                    ispay = Gattr.Bll.AddVipflow(vipinfo);
                }
                else
                {
                    if (ispay)
                    {
                        //正常会员消费记录
                        vipinfo.com_flag = "0";
                        ispay = Gattr.Bll.AddVipflow(vipinfo);
                    }
                    else
                    {
                        //消费错误提示
                        MessageBox.Show(memberinfo.info, Gattr.AppTitle);
                        return;
                    }
                }
            }
            else
            {
                vipinfo.com_flag = "0";
                if (vipinfo != null)
                {
                    if (!string.IsNullOrEmpty(vipinfo.card_no))
                    {
                        ispay = Gattr.Bll.AddVipflow(vipinfo);
                    }
                }
            }
            //处理数据
            if (!ispay)
            {
                MessageBox.Show("会员消费保存失败", Gattr.AppTitle);
            }
            else
            {
                Gattr.Bll.SavePosBill(flow_no, this._listSaleFlow, this._listPayFlow);
                Gattr.Bll.DeleteTempData();
                Gfunc.SetFlowNo();
                this._isCHG = true;
                this.IsPayState = true;
                this.SetPosState(PosOpState.CHG);
                this.ClearInput();
                //****折扣返余额计算方式****//
                //****在促销规则绑定时进行计算，小计金额为原价，单价为折扣后的单价
                //****付款时还是原价支付，折扣金额返还到会员余额账户

                //是否含有余额支付，含有则不能退款
                _isFk = true;
                foreach (t_cur_payflow pay in _listPayFlow)
                {
                    if (pay.pay_way == "CHA" || pay.pay_way == "GZ")
                        _isFk = false;
                }
                //折扣总金额 = 正常价小计总额 - 优惠价总额
                //_saleZheKouAmt = _saleflow.unit_price * _saleflow.sale_qnty 这是计算方式
                decimal amt = this._saleTotalAmt - this._saleZheKouAmt;
                //amt为负数时为退货扣除余额优惠返利
                if (amt < 0) _isFk = true;
                if (vipinfo != null && !string.IsNullOrEmpty(vipinfo.card_no) && amt != 0 && _isFk)
                {
                    string _msgSuccess = string.Empty;
                    string _msgFailed = string.Empty;
                    if (amt > 0)
                    {
                        _msgSuccess = "会员【" + vipinfo.card_no + "】折扣额已成功返到余额账户！金额：" + amt.ToString("f2");
                        _msgFailed = "会员【" + vipinfo.card_no + "】折扣返到余额账户失败！！！金额：" + amt.ToString("f2");
                    }
                    else
                    {
                        _msgSuccess = "会员【" + vipinfo.card_no + "】余额优惠已成功扣除！金额：" + amt.ToString("f2");
                        _msgFailed = "会员【" + vipinfo.card_no + "】余额优惠扣除失败！！！金额：" + amt.ToString("f2");
                    }

                    if (!this.DiscountToYE(amt, vipinfo.card_no))
                    {
                        MessageBox.Show(_msgFailed);
                        LoggerHelper.Log("MsmkLogger", Gattr.OperId + _msgFailed, LogEnum.SysLog);
                    }
                    else
                    {
                        MessageBox.Show(_msgSuccess);
                        LoggerHelper.Log("MsmkLogger", Gattr.OperId + _msgSuccess, LogEnum.SysLog);
                    }
                }
                this.ShowMessage("<"+flow_no+">结算完成!");
                
                //--------------------------打印操作-----------------
                _listWinPrtStr.Clear();
                Gattr.PosPrinter.OpenPrinter(Gattr.PosModel, Gattr.PosPort);
                Gfunc.PrintHeader();
                this.PrintHeaderDetail(flow_no);
                this.PrintItemDetail(this._listSaleFlow);
                this.PrintFooterDetail(vipinfo);
                Gfunc.SendToAPIPrint();
                _currentFlowNo = string.Empty;
                this.lbPayAmtChange.Text = "0.00";
                this.ResetSaleState(true);
                //this.tblPanelBalance.Visible = false;
                //开钱箱
                //}
            }
        }
        /// <summary>
        /// 已付金额
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalPayedAmt()
        {
            decimal num = 0M;
            foreach (t_cur_payflow _payflow in this._listPayFlow)
            {
                //num += Math.Round((decimal)(_payflow.convert_amt), Convert.ToInt32("0"));
                num += (decimal)(_payflow.convert_amt);

            }
            return num;


        }
        /// <summary>
        /// 总金额
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalAmt()
        {
            decimal num = 0M;
            foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
            {
                num += Math.Round(_saleflow.sale_money, Convert.ToInt32(Gattr.PosSaleAmtPoint.Substring(1, 1)));
            }
            return num;
        }
        /// <summary>
        /// 购买数据
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalQty()
        {
            decimal d = 0M;
            foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
            {
                d += _saleflow.sale_qnty;
            }
            return d;
        }
        private int SetPayFlowId()
        {
            int num = 1;
            foreach (t_cur_payflow payflow in this._listPayFlow)
            {
                num++;
            }
            return num;
        }
        private void ClearInput()
        {
            this._inputText = "";
            this.lbInput.Text = "";
            this.SetCursorText();
        }
        private void ResetSaleState(bool isOver = false)
        {

            this.bindingPayFlow.Clear();
            this.bindingSaleFlow.Clear();
            this.SetPosState(PosOpState.PLU);
            this._listSaleFlow.Clear();
            this._listPayFlow.Clear();

            this.bindingSaleFlow.ResetBindings(false);
            this.bindingPayFlow.ResetBindings(false);

            this.GvPayFlow.Refresh();
            this.GvSaleFlow.Refresh();
            if (!isOver)
            {
                this.lbAmtTitle.Text = "总价：";
                this.lbPayTotalAmt.Text = "0.00";
                this.lbPayAmtPaid.Text = "0.00";
                this.lbPayAmtRec.Text = "0.00";
                this.lbAmtTotal.Text = "0.00";
                this.lbQntyTotal.Text = "0.00";
                this.SetTotalAmt();
                SetDoubleDisplay("1", this.lbAmtTotal.Text, this.lbPayAmtRec.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, "",0,0);
            }
            else
            {
                this.SetTotalAmt(true);
                this.lbPayTotalAmt.Text = "0.00";
                this.lbPayAmtPaid.Text = "0.00";
                SetDoubleDisplay("2", "0.00", "0.00", "0.00", this.lbAmtTotal.Text, "0.00", "",0,0);
            }

            this.tblPanelBalance.Visible = false;
            this.tlpMain.Enabled = true;
            this.moneyCHA = 0;//余额支付
            this._currentFlowNo = null;
            this._isVip = false;
            this.lbVipValue.Text = "";
            //this._vipInfo = null;
            this._currentMember = null;
            this._saleTotalAmt = 0M;//销售商品的总价格
            this._saleZheKouAmt = 0M;//折扣后的商品总金额
        }
        #region 删除ListSaleFlow某个单行
        /// <summary>
        /// 删除销售产品bindingSaleFlow行,同时删除销售商品列表listSaleFlow中的数据
        /// </summary>
        /// <param name="saleflow"></param>
        private void DelListSaleFlow(t_cur_saleflow saleflow)
        {
            foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
            {
                if (_saleflow.item_no == saleflow.item_no && saleflow.flow_id == _saleflow.flow_id)
                {
                    this._listSaleFlow.RemoveAt(this._listSaleFlow.IndexOf(_saleflow));
                    this.ResetSaleFlowId();
                    this.bindingSaleFlow.ResetBindings(false);
                    break;
                }
            }
        }
        #endregion
        /// <summary>
        /// 重新绑定FLOWID
        /// </summary>
        private void ResetSaleFlowId()
        {
            int num = 1;
            foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
            {
                _saleflow.flow_id = num;
                num++;
            }
        }
        /// <summary>
        /// 保存销售数据
        /// </summary>
        private void ResetSaleFlowData()
        {
            int num = 1;
            foreach (t_cur_saleflow saleflow in this._listSaleFlow)
            {
                saleflow.flow_id = num;
                saleflow.oper_id = Gattr.OperId;
                saleflow.pos_id = Gattr.PosId;
                saleflow.branch_no = Gattr.BranchNo;
                //saleflow.sale_money = (decimal)(saleflow.unit_price * saleflow.sale_qnty);
                num++;
            }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="Message"></param>
        private void ShowMessage(string Message)
        {
            this.lbMessage.Text = "提示：" + Message;
            LoggerHelper.Log("MsmkLogger", Gattr.OperId + Message + this._posState.ToString(), LogEnum.SysLog);
        }
        /// <summary>
        /// 重新计算数量和价格
        /// </summary>
        /// <param name="isRefreshSaleAmt"></param>
        private void SetTotalAmt()
        {
            this.SetTotalAmt(false);
        }
        /// <summary>
        /// 重新计算数量和价格
        /// </summary>
        /// <param name="isRefreshSaleAmt"></param>
        private void SetTotalAmt(bool isRefreshSaleAmt)
        {

            decimal num = new decimal(0); //销售的商品总数量
            this._saleTotalAmt = 0M;//销售商品的总价格
            this._saleZheKouAmt = 0M;//折扣后的商品总金额
            foreach (t_cur_saleflow _saleflow in this._listSaleFlow)
            {
                //this._saleTotalAmt += Math.Round(_saleflow.Sale_money); //四舍五入
                this._saleTotalAmt += (_saleflow.sale_money);
                this._saleZheKouAmt += (_saleflow.unit_price * _saleflow.sale_qnty);
                num += _saleflow.sale_qnty;
            }
            if (!isRefreshSaleAmt)
            {
                //微信会员四舍五入
                //if (_currentMember != null && _currentMember.mem_type == "WX")
                //{
                    //decimal sumAmt = OperateAmt(_saleTotalAmt);
                    this.lbAmtTotal.Text = ExtendUtility.Instance.ParseToDecimal(_saleTotalAmt).ToString(Gattr.PosSaleAmtPoint);
                //}
                //else
                //{
                //    this.lbAmtTotal.Text = ExtendUtility.Instance.ParseToDecimal(Convert.ToDecimal(String.Format("{0:F}", _saleTotalAmt))).ToString(Gattr.PosSaleAmtPoint);
                //}
                this.lbQntyTotal.Text = Convert.ToDecimal(String.Format("{0:F}", num)).ToString();
            }
            //双屏
            if (Gattr.IsDoubleModule && (this.frmdouble != null))
            {
                if (_currentMember != null && _currentMember.mem_type == "WX")
                {
                    this.lbAmtTotal.Text = ExtendUtility.Instance.ParseToDecimal(_saleTotalAmt).ToString(Gattr.PosSaleAmtPoint);
                }
                this.SetDoubleDisplay("1", this.lbAmtTotal.Text, this.lbAmtTotal.Text, this.lbPayAmtPaid.Text, this.lbPayAmtChange.Text, this.lbQntyTotal.Text, "",0,0);
            }
        }

        #region 会员打折金额折算方案修改
        /// <summary>
        /// 处理会员优惠策略
        /// </summary>
        /// <param name="sumAmt"></param>
        /// <returns></returns>
        private decimal OperateAmt(decimal sumAmt)
        {
            bool ispayback = false;
            if (sumAmt < 0)
            {
                ispayback = true;
                sumAmt = Math.Abs(sumAmt);
            }
            decimal resultAmt = sumAmt;
            decimal tempAmt = Math.Floor(sumAmt);
            try
            {
                resultAmt = tempAmt;//保存当前的值
                tempAmt = Math.Abs(tempAmt);
                if (sumAmt >= (tempAmt + 0.5M))//大于5毛的话取到5毛
                {
                    resultAmt = tempAmt + 0.5M;
                }
                if (resultAmt > 0 && ispayback)
                {
                    resultAmt = -resultAmt;
                }
            }
            catch (Exception ex)
            {

            }
            return resultAmt;
        }
        #endregion

        Size _sizeInputPayText; //光标label标签
        private int _inputPayDisplayMax = 15;//金额输入最大框
        #region 金额支付光标设置
        /// <summary>
        /// 金额支付光标设置
        /// </summary>
        private void ResetPayCursor()
        {
            TextGraphics graphics = new TextGraphics(base.CreateGraphics());
            this._sizeInputPayText = graphics.MeasureText("B", this.lbPayInput.Font);
            graphics.Dispose();
            this._inputPayDisplayMax = (this.lbPayInput.Width / this._sizeInputPayText.Width) - 1;
        }
        #endregion
        /// <summary>
        /// 金额支付光标处内容
        /// </summary>
        private void SetCursorText()
        {
            if (this._posState == PosOpState.PAY)
            {
                //结算小键盘
                if (this._inputText.Length > this._inputPayDisplayMax)
                {
                    this.lbPayInput.Text = this._inputText.Substring(this._inputText.Length - this._inputPayDisplayMax, this._inputPayDisplayMax);
                }
                else
                {
                    this.lbPayInput.Text = this._inputText;
                }
                this.lbPayInput.Refresh();
                this.lbPayCursor.Visible = false;
                this.lbPayCursor.Left = ((int)(this._sizeInputPayText.Width * 0.65f)) + (this.lbPayInput.Text.Length * this._sizeInputPayText.Width);
                this.lbPayCursor.Visible = true;
            }
            else
            {
                //大键盘
                if (this._inputText.Length > this._inputDisplayMax)
                {
                    this.lbInput.Text = this._inputText.Substring(this._inputText.Length - this._inputDisplayMax, this._inputDisplayMax);
                }
                else
                {
                    this.lbInput.Text = this._inputText;
                }
                this.lbInput.Refresh();
                this.lbCursor.Visible = false;
                this.lbCursor.Left = ((int)(this._sizeInputText.Width * 0.5f)) + (this.lbInput.Text.Length * this._sizeInputText.Width);
            }
        }
        /// <summary>
        /// 设置POS状态
        /// </summary>
        /// <param name="opState"></param>
        private void SetPosState(PosOpState opState)
        {
            this._posState = opState;
        }
        #region 时间定时器
        /// <summary>
        /// 时间定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tNowTime_Tick(object sender, EventArgs e)
        {
            this.lbNowTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion
        #region 结算金额输入光标定时器
        /// <summary>
        /// 光标定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeCursor_Tick(object sender, EventArgs e)
        {
            if (this._posState == PosOpState.PAY)
            {
                this.lbPayCursor.Visible = !this.lbPayCursor.Visible;
            }
            else
            {
                this.lbCursor.Visible = !this.lbCursor.Visible;
            }
        }
        #endregion
        #region 结算VisibleChanged事件
        /// <summary>
        /// 结算VisibleChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblPanelBalance_VisibleChanged(object sender, EventArgs e)
        {

            if (this.tblPanelBalance.Visible)
            {
                this.plButton.Enabled = false;
                this.btnSaleUp.Enabled = false;
                this.btnSaleDown.Enabled = false;
                this.tlpCenterRight.Enabled = false;
            }
            else
            {
                this.plButton.Enabled = true;
                this.btnSaleUp.Enabled = true;
                this.btnSaleDown.Enabled = true;
                this.plClsSmall.Enabled = true;
                this.tlpCenterRight.Enabled = true;
            }
        }
        #endregion
        private void MouseClickReset(Control father)
        {
            if (father.Controls.Count > 0)
            {
                foreach (Control control in father.Controls)
                {
                    if (!(control is Button) && !(control is ButtonForPos))
                    {
                        control.MouseClick += new MouseEventHandler(this.all_MouseClick);
                        if (control.Controls.Count > 0)
                        {
                            this.MouseClickReset(control);
                        }
                    }
                }
            }
        }
        private void all_MouseClick(object sender, MouseEventArgs e)
        {
            if (!((!this.tblPanelBalance.Visible || (this._posState != PosOpState.CHG)) || this._isCHG))
            {
                this.ResetSaleState();
            }
            this.plOtherFunc.Visible = false;
            this._isCHG = false;
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bindingSaleFlow.Count > 0)
            {
                //this._frmLogin.LoginParm = "";
                this.ShowMessage("只能在正常销售状态下且无商品销售时，才退出收银状态！");
                e.Cancel = true;
            }
            else
            {
                //this._intIdle = 0;
                //this.timerIdle.Enabled = false;
                if (this.frmdouble != null)
                {
                    this.frmdouble.Close();
                }
                if (nonParameterThread != null)
                {
                    nonParameterThread.Abort();
                }

            }
        }
        /// <summary>
        /// 打印小票头部明细
        /// </summary>
        private void PrintHeaderDetail(string no)
        {
            int QtyLen = 6; //数量长度
            int PrcLen = 6; //单价长度
            int ItemLen = 13; //商品号

            string CashierNo = "收银员:" + Gattr.OperId;
            string number = "NO:" + no;
            string now = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
            this._listWinPrtStr.Add(new PrintString(CashierNo, CashierNo.Length));
            this._listWinPrtStr.Add(new PrintString(number, number.Length));
            this._listWinPrtStr.Add(new PrintString(now, CashierNo.Length));
            this._listWinPrtStr.Add(new PrintString("".PadLeft((Gattr.PrtLen < 0) ? 0 : Gattr.PrtLen, '-')));

            string hd1 = "货号/品名";
            string hd2 = "单价";
            string hd3 = "数量";
            string hd4 = "小计";

            hd1 = Gfunc.PrintStrAlign(hd1, ItemLen, TextAlign.Left);
            hd2 = Gfunc.PrintStrAlign(hd2, PrcLen, TextAlign.Right);
            hd3 = Gfunc.PrintStrAlign(hd3, QtyLen, TextAlign.Right);
            hd4 = Gfunc.PrintStrAlign(hd4, PrcLen, TextAlign.Right);

            _listWinPrtStr.Add(new PrintString(hd1 + hd2 + hd3 + hd4, Gattr.PrtLen, TextAlign.Left));

            Gfunc.PrintOut(_listWinPrtStr);
        }
        /// <summary>
        /// 获取不同收款方式的收款金额
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, decimal> GetPayInfos()
        {
            Dictionary<string, decimal> _pays = new Dictionary<string, decimal>();
            foreach (t_cur_payflow payflow in this._listPayFlow)
            {
                if (payflow.sale_way != "C")
                {
                    if (!_pays.ContainsKey(payflow.pay_way))
                    {
                        _pays.Add(payflow.pay_way, payflow.pay_amount);
                    }//
                    else
                    {
                        if (payflow.sale_way == "A")
                        {
                            _pays[payflow.pay_way] += payflow.pay_amount;
                        }
                        else
                        {
                            _pays[payflow.pay_way] -= payflow.pay_amount;
                        }
                    }
                }
            }
            return _pays;
        }
        /// <summary>
        /// 打印商品明细
        /// </summary>
        /// <param name="saleflow"></param>
        private bool PrintItemDetail(List<t_cur_saleflow> saleflow)
        {
            if (saleflow.Count <= 0)
            {
                return false;
            }
            List<PrintString> listPrtStr = new List<PrintString>();
            bool saleMore = false;

            foreach (t_cur_saleflow _saleflow in saleflow)
            {
                List<PrintString> list2 = this.PrintAtOnceSaleRow(_saleflow, false, saleMore);
                foreach (PrintString str3 in list2)
                {
                    listPrtStr.Add(str3);
                }
            }
            listPrtStr.Add(new PrintString("".PadLeft((Gattr.PrtLen < 0) ? 0 : Gattr.PrtLen, '-')));
            this.lbMessage.Text = this.lbMessage.Text + "... 打印 ...";

            Gfunc.PrintOut(listPrtStr);
            return true;
        }
        private List<PrintString> PrintAtOnceSaleRow(t_cur_saleflow saleItem, bool isPrint, bool saleMore)
        {
            List<PrintString> listPs = new List<PrintString>();
            int QtyLen = 5; //数量长度
            int PrcLen = 6; //单价长度
            int ItemLen = 13; //商品号
            //int SumLen = 6; //单价长度

            string itemNo = saleItem.item_no;
            string itemName = saleItem.item_name;
            string unitPrice = saleItem.unit_price.ToString("0.00");
            string Qnty = saleItem.sale_qnty.ToString("0.00");
            string sum = (saleItem.sale_qnty * saleItem.unit_price).ToString("0.00");

            string str1 = Gfunc.PrintStrAlign(itemNo, ItemLen, TextAlign.Left);
            string str2 = Gfunc.PrintStrAlign(unitPrice, PrcLen, TextAlign.Right);
            string str3 = Gfunc.PrintStrAlign(Qnty, QtyLen, TextAlign.Right);
            string str4 = Gfunc.PrintStrAlign(sum, PrcLen, TextAlign.Right);

            listPs.Add(new PrintString(str1 + str2 + str3 + str4, Gattr.PrtLen, TextAlign.Left));
            listPs.Add(new PrintString(itemName, Gattr.PrtLen, TextAlign.Left));
            t_cur_saleflow sale = Gattr.Bll.GetItemInfo(saleItem.item_no);
            if (sale.unit_price != saleItem.unit_price && saleItem.sale_way != "B")
            {
                listPs.Add(new PrintString("                     原价：" + sale.unit_price, Gattr.PrtLen, TextAlign.Left));
            }
            return listPs;
        }
        /// <summary>
        /// 打印小票底部部明细
        /// </summary>
        private void PrintFooterDetail(t_app_viplist memberinfo)
        {
            int footTitle = 16; //单价长度

            decimal iTotNum = 0M;
            decimal iTotSum = 0M;
            List<PrintString> listPs = new List<PrintString>();

            decimal decCoupon = 0M;
            decimal descYh = 0M;
            bool ispayback = false;
            foreach (t_cur_saleflow _saleflow in _listSaleFlow)
            {
                iTotNum += _saleflow.sale_qnty;
                if (_saleflow.sale_way == "B")
                {
                    iTotSum += _saleflow.sale_qnty * _saleflow.unit_price * (-1);
                }
                else
                {
                    iTotSum += _saleflow.sale_qnty * _saleflow.unit_price;
                }
                t_cur_saleflow sale = Gattr.Bll.GetItemInfo(_saleflow.item_no);
                if (_saleflow.sale_way != "B")
                {
                    descYh += _saleflow.sale_qnty * sale.unit_price;
                }
                else
                {
                    ispayback = true;
                    descYh += _saleflow.sale_qnty * sale.unit_price;
                }
            }

            if (ispayback)
            {
                decCoupon = Math.Abs(descYh) - Math.Abs(iTotSum);
            }
            else
            {
                decCoupon = descYh - iTotSum;
            }
            string strTotNum = "数量合计:" + iTotNum.ToString("0.00");
            strTotNum = Gfunc.PrintStrAlign(strTotNum, footTitle, TextAlign.Left);
            string strTotSum = "合计金额:" + iTotSum.ToString("0.00");


            strTotSum = Gfunc.PrintStrAlign(strTotSum, Gattr.PrtLen - footTitle, TextAlign.Left);

            listPs.Add(new PrintString(strTotNum + strTotSum, Gattr.PrtLen, TextAlign.Left));

            string strCoupon = "优惠金额:" + decCoupon.ToString("0.00");
            strCoupon = Gfunc.PrintStrAlign(strCoupon, footTitle, TextAlign.Left);
            string strCost = "原价金额:" + descYh.ToString("0.00");
            if (ispayback)
            {
                strCost = "原价金额:" + (-descYh).ToString("0.00");
            }
            strCost = Gfunc.PrintStrAlign(strCost, Gattr.PrtLen - footTitle, TextAlign.Left);

            listPs.Add(new PrintString(strCoupon + strCost, Gattr.PrtLen, TextAlign.Left));

            decimal chs = ExtendUtility.Instance.ParseToDecimal(this.lbAmtTotal.Text);
            if (chs > 0)
            {
                listPs.Add(new PrintString("找零金额:" + this.lbAmtTotal.Text, Gattr.PrtLen, TextAlign.Left));
            }
            ////计算抹零金额
            //decimal ml = this._listPayFlow[0].sale_amount - this._listPayFlow[0].pay_amount;
            //if (ml > 0)
            //{
            //    listPs.Add(new PrintString("抹零金额:" + ml.ToString("0.00"), Gattr.PrtLen, TextAlign.Left));
            //}

            //计算返款金额
            decimal fk = this._saleTotalAmt - this._saleZheKouAmt;
            if (fk > 0 && _isFk)
            {
                listPs.Add(new PrintString("折扣返款(返至余额账户):" + fk.ToString("0.00"), Gattr.PrtLen, TextAlign.Left));
            }
            else if(fk > 0&&!_isFk)
            {
                listPs.Add(new PrintString("(此类支付不享受折扣返款)", Gattr.PrtLen, TextAlign.Left));
            }
            Dictionary<string, decimal> _payInfos = GetPayInfos();
            foreach (KeyValuePair<string, decimal> kv in _payInfos)
            {
                string strPayWayName = "付款方式:";
                string strPayWay = "人民币现金";
                switch (kv.Key)
                {
                    case "SAV":
                        strPayWay = "会员卡";
                        break;
                    case "BCD":
                    case "CRD":
                        strPayWay = "银行(信用)卡";
                        break;
                    case "HF":
                        strPayWay = "预付费卡";
                        break;
                    case "CHA":
                        strPayWay = "钱包余额";
                        break;
                    case "GZ":
                        strPayWay = "挂账";
                        break;
                    case "ZFB":
                        strPayWay = "支付宝";
                        break;
                    default:
                        break;
                }
                string str1 = kv.Value.ToString("0.00"); //(iTotSum - decCoupon).ToString("0.00");
                if (ispayback)
                {
                    str1 = (-kv.Value).ToString("0.00"); //(iTotSum - decCoupon).ToString("0.00");
                }
                strPayWay = Gfunc.PrintStrAlign(strPayWay, (Gattr.PrtLen - strPayWayName.Length - iTotSum.ToString().Length) / 2, TextAlign.Center);
                str1 = Gfunc.PrintStrAlign(str1, str1.Length + 5, TextAlign.Right);
                listPs.Add(new PrintString(strPayWayName + strPayWay + str1, Gattr.PrtLen, TextAlign.Left));
            }
            string strMember = "会员卡号:";
            string strScore = "";
            if (memberinfo != null && !string.IsNullOrEmpty(memberinfo.card_no))
            {
                strMember += Gfunc.PrintStrAlign(memberinfo.card_no, footTitle, TextAlign.Left);
                strScore = "本次积分:" + Gfunc.PrintStrAlign(memberinfo.score.ToString(), footTitle, TextAlign.Left);
                listPs.Add(new PrintString(strMember, Gattr.PrtLen, TextAlign.Left));
                listPs.Add(new PrintString(strScore, Gattr.PrtLen, TextAlign.Left));
            }
            List<string> _strNo = new List<string>();
            if (_listPayFlow.Count > 0)
            {
                for (var i = _listPayFlow.Count - 1; i >= 0; i--)
                {
                    t_cur_payflow payflow = _listPayFlow[i];
                    if ((!string.IsNullOrEmpty(payflow.card_no)) || (!string.IsNullOrEmpty(payflow.vip_no)))
                    {
                        if (((!string.IsNullOrEmpty(payflow.card_no)) && (payflow.pay_way == "VIP")) || (!string.IsNullOrEmpty(payflow.vip_no)))
                        {
                        }
                        else
                        {
                            if (payflow.pay_way == "BCD")
                            {
                                strMember = "银行(信用)卡:";
                                strMember += payflow.card_no;
                                strScore = "备注:" + payflow.memo;
                                strMember = Gfunc.PrintStrAlign(strMember, footTitle, TextAlign.Left);
                                strScore = Gfunc.PrintStrAlign(strScore, footTitle, TextAlign.Left);
                                listPs.Add(new PrintString(strMember, Gattr.PrtLen, TextAlign.Left));
                                listPs.Add(new PrintString(strScore, Gattr.PrtLen, TextAlign.Left));
                                _strNo.Add(payflow.card_no);
                            }
                        }
                    }
                }
            }
            string strPrompt = "谢谢惠顾！欢迎下次光临！";
            strPrompt = Gfunc.PrintStrAlign(strPrompt, Gattr.PrtLen, TextAlign.Center);
            listPs.Add(new PrintString(strPrompt, Gattr.PrtLen, TextAlign.Left));

            Gfunc.PrintOut(listPs);
        }
        private void ListFunc_Click(object sender, EventArgs e)
        {
            try
            {
                //this._intIdle = 0;
                ListBox box = sender as ListBox;
                string str = SIString.TryStr(box.SelectedItem).Replace(" ", string.Empty);
                this.plOtherFunc.Visible = false;
                t_attr_function keyfunc = null;
                switch (str)
                {
                    case "数据下载":
                        FrmSyncPosData _postData = new FrmSyncPosData(false);
                        _postData.ShowDialog();
                        break;
                    case "退货":
                        keyfunc = this.GetKeyFunc("ret");
                        break;
                    case "会员查询":
                        FrmMemberInfo frmMemberInfo = new FrmMemberInfo();
                        frmMemberInfo.ShowDialog();
                        break;
                    case "商品查询":
                        this.ItemQuery();
                        break;
                    case "交易查询":
                        keyfunc = this.GetKeyFunc("lok");
                        if (keyfunc == null)
                        {
                            keyfunc = new t_attr_function() { func_id = "lok" };
                        }
                        break;
                    case "修改支付":
                         FrmCPayWay _frmCPayWay = new FrmCPayWay("",false);
                         _frmCPayWay.ShowDialog();
                        break;
                    case "收银对账":
                        this.CashAccount();
                        break;
                    case "库存查询":
                        this.StockQuery(false);
                        break;
                    case "客户订货":
                    case "寄存提货":
                    case "项目储值":
                    case "更改仓库":
                        MessageBox.Show(str + "功能尚未实现！", Gattr.AppTitle);
                        break;
                    case "挂账回款":
                        FrmArrearage frmArrearage = new FrmArrearage();
                        frmArrearage.ShowDialog();
                        break;
                    case "更改密码":
                        this.UpdatePwd();
                        break;
                    case "要货单":
                        FrmDmSheet dmSheet = new FrmDmSheet();
                        dmSheet.ShowDialog();
                        break;
                    case "收货单":
                        FrmSHSheet shSheet = new FrmSHSheet();
                        shSheet.ShowDialog();
                        break;
                    case "非交易":
                        this.NonTrading();
                        break;
                    case "赠送":
                        this.FunKeyGift();
                        break;
                    case "微信活动":
                        this.FunKeyWeiXin();
                        break;
                    case "打印设置":
                        FrmPrint _frmPrint = new FrmPrint();
                        _frmPrint.ShowDialog();
                        break;
                    case "操作手册":
                        this.FunKeyMyHelp();
                        break;
                    case "切换键盘":

                        this.tblNum.Visible = !this.tblNum.Visible;
                        this.tlpCenterRight.Visible = !this.tlpCenterRight.Visible;
                        if (this.tblNum.Visible)
                        {
                            this.ResetSaleCursor();
                        }
                        break;
                    case "提交反馈":
                        FrmFeedBack _frmFeedBack = new FrmFeedBack();
                        _frmFeedBack.ShowDialog();
                        break;

                }
                if (keyfunc != null)
                {
                    this.CallKeyFunc(keyfunc);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        private bool UpdatePwd()
        {
            if (this._posState == PosOpState.PAY)
            {
                this.ShowMessage("当前状态，此功能键无效!");
                return false;
            }
            else
            {
                FrmUpdatePwd fup = new FrmUpdatePwd();
                if (fup.ShowDialog() == DialogResult.OK)
                {
                    if (fup.UpdateStatus == true)
                    {
                        this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Escape));
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 收银对帐
        /// </summary>
        private void CashAccount()
        {
            string cashierNo = Gattr.OperId;
            //string msg = "";
            if (this._posState == PosOpState.PAY)
            {
                this.ShowMessage("付款状态下不能使用[收银对账]功能！");
            }
            else if (this.bindingSaleFlow.Count > 0)
            {
                this.ShowMessage("能在正常销售状态下且无商品销售时才能进行交班操作。");
            }
            else
            {
                //if (Gattr.IsExitHangBill)
                //{
                //    int num = RetailGlobal.Bll.CheckOperIdHungFlow(RetailGlobal.CashierNo);
                //    if (num > 0)
                //    {
                //        MessageBox.Show(string.Format(Language.GetString("#0515"), num), RetailGlobal.AppTitle);
                //        return;
                //    }
                //}
                FrmAccount2 account = new FrmAccount2();
                account.ShowDialog();
                if (account.IsLogout)
                {
                    //Gfunc.UserLogOut();
                    this._frmLogin.LoginParm = "CHANGE";
                    base.Close();
                }
            }
        }
        #region 赠送商品操作
        /// <summary>
        /// 赠送商品操作
        /// </summary>
        /// <param name="saleflow"></param>
        private void FunKeyGift()
        {
            //修改当前行的数量，计算小计，更改总价格，更改总数量，更新流水表数据。

            if (this.bindingSaleFlow.Count == 0)
            {
                this.ShowMessage("当前无商品销售，操作取消!");//Language.GetString("#0053"));
            }
            else
            {
                //decimal num = 0M; //当前数量
                t_cur_saleflow saleflow = this.bindingSaleFlow.Current as t_cur_saleflow;
                //小计
                if (saleflow.sale_way == "A")
                {
                    saleflow.unit_price = 0M;
                    saleflow.sale_money = 0M;
                    saleflow.sale_way = "C";
                    this.ShowMessage("增送商品<" + saleflow.item_name + ">，数量<" + saleflow.sale_qnty + ">");
                }
                else if (saleflow.sale_way == "C")
                {
                    MessageBox.Show("该商品赠送！！！", Gattr.AppTitle);
                }
                else
                {
                    MessageBox.Show("该商品不可以赠送！！！", Gattr.AppTitle);
                }
                this.bindingSaleFlow.ResetBindings(false);
                this.ResetSaleFlowData();
                this.SetTotalAmt();
                this.GvSaleFlow.Refresh();
                //向销售商品流水表插入数据
                Gattr.Bll.SaveTempSaleRow(_listSaleFlow);
                this.ClearInput();
            }
        }
        /// <summary>
        /// 整单赠送
        /// </summary>
        /// <param name="saleflow"></param>
        private void FunKeyGiftAll()
        {
            FrmSQ _frmSQ = new FrmSQ();
            //_frmSQ.SetIsOKEvent += (isok) =>
            //{
            //    success=isok;
            //};
           if (_frmSQ.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               //修改当前行的数量，计算小计，更改总价格，更改总数量，更新流水表数据。
               if (this._listSaleFlow != null && this._listSaleFlow.Count > 0)
               {
                   foreach (t_cur_saleflow saleflow in this._listSaleFlow)
                   {
                       saleflow.unit_price = 0M;
                       saleflow.sale_money = 0M;
                       saleflow.sale_way = "C";
                   }
                   this.bindingSaleFlow.ResetBindings(false);
                   this.ResetSaleFlowData();
                   this.PosPayAmt("RMB", 0, false, true);
                   this.SetTotalAmt();
                   this.SetPayTotelAmt();
                   this.GvSaleFlow.Refresh();
                   this.ClearInput();
                   LoggerHelper.Log("MsmkLogger", Gattr.OperId + "进行整单赠送！", LogEnum.SysLog);
               }
               else
               {
                   this.ShowMessage("当前无商品销售，操作取消!");//Language.GetString("#0053"));
               }
           }
        }
        #endregion
        /// <summary>
        /// 商品查询窗体
        /// </summary>
        /// <returns></returns>
        private bool ItemQuery()
        {
            if (this._posState == PosOpState.PAY)
            {
                this.ShowMessage("当前状态，此功能键无效!");
                return false;
            }
            else
            {
                this.ClearInput();
                FrmItemQuery query = new FrmItemQuery();
                query.ShowDialog();
                if (query.itemF5 != string.Empty)
                {
                    this._inputText = query.itemF5.ToString();
                    this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Enter));
                }
            }
            return true;
        }

        /// <summary>
        /// 非交易
        /// </summary>
        /// <returns></returns>
        private bool NonTrading()
        {
            if (this._posState == PosOpState.PAY)
            {
                this.ShowMessage("当前状态，此功能键无效!");
                return false;
            }
            else
            {
                FrmNonTrading fnt = new FrmNonTrading();
                fnt.ShowDialog();
                //if (query.itemF5 != string.Empty)
                //{
                //    this._inputText = query.itemF5.ToString();
                //    this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Enter));
                //}
            }
            return true;
        }
        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal SaleTotalAmt
        {
            get
            {
                return this._saleTotalAmt;
            }
        }
        /// <summary>
        /// 尾款
        /// </summary>
        public decimal BalancePayAmt
        {
            get { return _balancePayAmt; }
        }
        #region 操作业务逻辑
        /// <summary>
        /// 交易查询业务逻辑
        /// </summary>
        void PaySaleInfoLook()
        {
            _posState = PosOpState.LOOK;
            bool isReprint = false;
            String lookFlowNo = string.Empty;
            FrmPayFlowInfoSearch _pay = new FrmPayFlowInfoSearch();
            _pay.SetReprintEvent += (p, no) =>
            {
                isReprint = p;
                lookFlowNo = no;
            };
            if (_pay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (isReprint)
                {
                    //TODO:执行重新打印功能
                    //1、首先查询出支付流水信息和销售流水信息
                    //2、对支付流水信息和销售流水信息进行处理[退货：金额为负]
                    //3、将数据绑定到列表中
                    //4、重打印操作
                    List<t_cur_payflow> payList = Gattr.Bll.GetPayFlowInfoListByFlowNo(lookFlowNo);
                    List<t_cur_saleflow> saleList = Gattr.Bll.GetSaleFlowInfoListByFlowNo(lookFlowNo);
                    foreach (t_cur_payflow pay in payList)
                    {
                        pay.pay_amount = pay.voucher_no == null ? ((SIString.TryStr(pay.voucher_no) == String.Empty) ? pay.pay_amount : pay.pay_amount * (-1)) : pay.pay_amount;
                    }
                    foreach (t_cur_saleflow sale in saleList)
                    {
                        sale.sale_qnty = sale.sale_way == "B" ? sale.sale_qnty * (-1) : sale.sale_qnty;
                        sale.sale_money = sale.sale_price * sale.sale_qnty;
                    }
                    this._listSaleFlow.AddRange(saleList);
                    this._listPayFlow.AddRange(payList);
                    this.bindingPayFlow.DataSource = payList;
                    this.bindingSaleFlow.DataSource = saleList;
                    this.bindingSaleFlow.ResetBindings(false);
                    this.bindingPayFlow.ResetBindings(false);
                    if (this.bindingPayFlow.Count == 0)
                    {
                        ShowMessage("交易查询小票数据不存在或已退货");
                    }
                    else
                    {
                        Gattr.PosPrinter.OpenPrinter(Gattr.PosModel, Gattr.PosPort);
                        Gfunc.PrintHeader();
                        this.PrintHeaderDetail(lookFlowNo);
                        this.PrintItemDetail(this._listSaleFlow);
                        t_member_info _mem = new t_member_info();
                        t_app_viplist _vip = Gattr.Bll.GetVipflow(lookFlowNo);
                        if (_vip != null)
                        {
                            _mem.mem_no = _vip.card_no;
                            _mem.score = _vip.score;
                        }
                        this.PrintFooterDetail(_vip);
                        Gfunc.SendToAPIPrint();
                        this._listSaleFlow.Clear();
                        this._listPayFlow.Clear();
                        ShowMessage(string.Format("流水号:{0}交易时间:{1},按任意键继续", lookFlowNo, payList[payList.Count - 1].oper_date));
                    }
                }
            }//if (_pay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            else
            {
                ResetSaleState();
                ShowMessage("商品开始交易...");
            }
        }
        /// <summary>
        /// 根据付款信息和支付方式设置支付流水记录
        /// </summary>
        /// <param name="payAmount"></param>
        /// <param name="payInfo"></param>
        /// <param name="saleAmount"></param>
        /// <param name="flowNo"></param>
        /// <param name="cardNo"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        t_cur_payflow SetPayFlowInfo(Decimal payAmount, t_payment_info payInfo, Decimal saleAmount, String flowNo, String cardNo, String memo)
        {
            t_cur_payflow payflow = null;
            try
            {
                payflow = new t_cur_payflow() { flow_id = 1, pay_amount = payAmount, sale_amount = saleAmount, sale_way = "A" };
                if (_listPayFlow != null && _listPayFlow.Count > 0)
                {
                    payflow.flow_id = _listPayFlow[_listPayFlow.Count - 1].flow_id + 1;
                }
                if (flowNo != null && flowNo.ToString().Length > 0)//正常付款，否则为退货
                {
                    payflow.voucher_no = flowNo;
                    payflow.sale_way = "B";
                }
                if (_currentMember != null)
                {
                    payflow.vip_no = _currentMember.mem_no;
                }
                payflow.pay_way = payInfo.pay_way;
                payflow.pay_name = payInfo.pay_name;
                payflow.coin_type = payInfo.pay_way;
                payflow.coin_rate = payInfo.rate;
                payflow.convert_amt = SIString.TryDec(payInfo.rate * payAmount);
                payflow.branch_no = Gattr.BranchNo;
                payflow.card_no = cardNo;
                payflow.oper_date = System.DateTime.Now.ToString("s");
                payflow.oper_id = Gattr.OperId;
                payflow.pos_id = Gattr.PosId;
                payflow.memo = memo;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }
            return payflow;
        }
        /// <summary>
        /// 打印会员消费
        /// </summary>
        void PrintMemberPayInfo()
        {
            if (_listPayFlow.Count > 0)
            {
                foreach (t_cur_payflow payInfo in _listPayFlow)
                {
                    if (payInfo.pay_way == "VIP")
                    {

                    }
                }
            }
        }
        /// <summary>
        /// 会员操作流程
        /// </summary>
        /// <param name="flow_no"></param>
        /// <returns></returns>
        t_member_info UserMemberPay(string flow_no, ref bool ispay, ref t_app_viplist vipinfo)
        {
            t_member_info memberinfo = null;
            Dictionary<string, decimal> _dicPayinfo = new Dictionary<string, decimal>();
            List<t_cur_payflow> payList = new List<t_cur_payflow>();
            decimal pay_flow_id = 0;
            decimal s_flow_id = 1;
            string memo = string.Empty;
            string vip_no = string.Empty;
            try
            {
                vipinfo = new t_app_viplist();
                vipinfo.flow_no = flow_no;
                vipinfo.oper_date = DateTime.Now.ToString("s");
                foreach (t_cur_payflow payInfo in _listPayFlow)
                {
                    #region 统计需要进行更改的会员信息
                    //E为挂账状态
                    if (payInfo.sale_way == "A" || payInfo.sale_way == "E")
                    {
                        if (payInfo.pay_way == "VIP")
                        {
                            if (payInfo.flow_id > pay_flow_id)
                            {
                                pay_flow_id = payInfo.flow_id;
                            }
                            if (!_dicPayinfo.ContainsKey(payInfo.card_no))
                            {
                                _dicPayinfo.Add(payInfo.card_no, payInfo.pay_amount);
                            }
                            else
                            {
                                _dicPayinfo[payInfo.card_no] += payInfo.pay_amount;
                            }
                        }
                    }
                    #endregion
                }
                #region 付款操作和加减积分操作
                //付款操作
                if (_dicPayinfo.Count > 0)
                {
                    foreach (string key in _dicPayinfo.Keys)
                    {
                        memberinfo = MemberService.Instance.UseMemberCardPay(key, _dicPayinfo[key].ToString(), flow_no, "门店消费");
                        if (memberinfo.code == "1")
                        {
                            memo = string.Format("{0}:{1}", memberinfo.balance, memberinfo.score);
                        }
                        else
                        {
                            ispay = false;
                        }
                    }
                }
                //加减积分操作
                if (ispay)
                {
                    //for (int index = _listPayFlow.Count - 1; index >= 0; index--)
                    //{

                    decimal _acc_num = 0M;
                    foreach (t_cur_saleflow saleflow in _listSaleFlow)
                    {
                        if (saleflow.vip_acc_flag == "1")
                        {
                            _acc_num += saleflow.sale_money;
                        }
                    }

                    if (_acc_num != 0)
                    {
                        t_cur_payflow _pay = _listPayFlow[0];
                        if (!string.IsNullOrEmpty(_pay.vip_no))
                        {
                            //E为挂账状态
                            if (_pay.sale_way == "A" || _pay.sale_way == "E")
                            {
                                vipinfo.card_no = _pay.vip_no;
                                vipinfo.sale_amt = _pay.sale_amount;
                                vipinfo.score = _acc_num;
                                memberinfo = MemberService.Instance.AddMemberScore(_pay.vip_no, _acc_num.ToString(), flow_no, "门店消费:" + flow_no);
                                if (memberinfo.code == "1")
                                {
                                    vipinfo.card_amount = memberinfo.balance;
                                    vipinfo.card_score = memberinfo.score;
                                    memberinfo.score = _acc_num;
                                    vipinfo.over_flag = "1";
                                    memo = string.Format("余额:{0},本次积分:{1}", memberinfo.balance, memberinfo.score);
                                    LoggerHelper.Log("MsmkLogger",vipinfo.card_no + memo, LogEnum.SysLog);
                                }
                                else
                                {
                                    ispay = false;
                                    LoggerHelper.Log("MsmkLogger", vipinfo.card_no + "memberinfo.code!=1", LogEnum.SysLog);
                                }
                            }
                        }
                        if (ispay)
                        {
                            if (!string.IsNullOrEmpty(_pay.voucher_no))
                            {
                                vip_no = Gattr.Bll.GetVipCardByNo(_pay.voucher_no);
                                if (!string.IsNullOrEmpty(vip_no))
                                {
                                    vipinfo.card_no = vip_no;
                                    vipinfo.sale_amt = _pay.sale_amount;
                                    vipinfo.score = _pay.sale_amount;
                                    vipinfo.voucher_no = _pay.voucher_no;
                                    if (_pay.sale_way == "B")
                                    {
                                        memberinfo = MemberService.Instance.UseMemberScore(vip_no, Math.Abs(_acc_num).ToString().ToString(), flow_no, "门店退货:" + _pay.voucher_no);
                                        _currentMember = memberinfo;
                                        if (memberinfo.code == "1")
                                        {
                                            vipinfo.card_amount = memberinfo.balance;
                                            vipinfo.card_score = memberinfo.score;
                                            memberinfo.score = _acc_num;
                                            vipinfo.over_flag = "1";
                                            memo = string.Format("余额:{0},本次积分:{1}", memberinfo.balance, memberinfo.score);
                                            LoggerHelper.Log("MsmkLogger", vipinfo.card_no + memo, LogEnum.SysLog);
                                        }
                                        else
                                        {
                                            ispay = false;
                                            LoggerHelper.Log("MsmkLogger", vipinfo.card_no + "memberinfo.code!=1", LogEnum.SysLog);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //TODO：会员信息有，但是没有积分的商品信息，则也显示会员消费记录，否则查询不到会员信息,只是不进行积分
                        if (_currentMember != null)
                        {
                            vipinfo.card_no = _currentMember.mem_no;
                            vipinfo.card_amount = _currentMember.balance;
                            vipinfo.card_score = _currentMember.score;
                            vipinfo.sale_amt = _listPayFlow[0].sale_amount;
                        }
                    }
                }
                #endregion
                if (ispay)
                {
                    foreach (t_cur_payflow payinfo in _listPayFlow)
                    {
                        t_cur_payflow _pay = payinfo;
                        if (payinfo.flow_id == pay_flow_id)
                        {
                            _pay.memo = memo;
                        }
                        else if (payinfo.flow_id == s_flow_id)
                        {
                            _pay.memo = memo;
                        }
                        if (!string.IsNullOrEmpty(payinfo.voucher_no))
                        {
                            _pay.vip_no = vip_no;
                        }
                        payList.Add(_pay);
                    }
                    _listPayFlow.Clear();
                    _listPayFlow.AddRange(payList);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(Gattr.LoggerName, "LL.POS->FrmMain-->UserMemberPay->Exception:" +
                    ex.ToString(), LogEnum.ExceptionLog);
            }
            return memberinfo;
        }


        /// <summary>
        /// 会员信息支付
        /// </summary>
        Dictionary<String, Decimal> MemeberPayInfo(ref bool ispay, String flowNo, ref string errorMessage)
        {
            Dictionary<String, Decimal> _dicPay = new Dictionary<string, decimal>();
            t_member_info _memberinfo = null;
            try
            {
                //A 会员卡付款如果是（付款 加积分） 不是只加积分 B  只进行减积分
                Dictionary<string, decimal> _dicPayinfo = new Dictionary<string, decimal>();
                //Dictionary<string, decimal> _dicSrcore = new Dictionary<string, decimal>();
                List<t_cur_payflow> payList = new List<t_cur_payflow>();
                decimal pay_flow_id = 0;
                decimal s_flow_id = 1;
                string memo = string.Empty;
                foreach (t_cur_payflow payInfo in _listPayFlow)
                {
                    #region 统计需要进行更改的会员信息
                    if (payInfo.sale_way == "A")
                    {
                        if (payInfo.pay_way == "VIP")
                        {
                            if (payInfo.flow_id > pay_flow_id)
                            {
                                pay_flow_id = payInfo.flow_id;
                            }
                            if (!_dicPayinfo.ContainsKey(payInfo.card_no))
                            {
                                _dicPayinfo.Add(payInfo.card_no, payInfo.pay_amount);
                            }
                            else
                            {
                                _dicPayinfo[payInfo.card_no] += payInfo.pay_amount;
                            }
                        }
                    }
                    #endregion
                }
                #region 付款操作和加减积分操作
                //付款操作
                if (_dicPayinfo.Count > 0)
                {
                    foreach (string key in _dicPayinfo.Keys)
                    {
                        _memberinfo = MemberService.Instance.UseMemberCardPay(key, _dicPayinfo[key].ToString(), flowNo, "门店消费");
                        if (_memberinfo.code == "-1")
                        {
                            ispay = false;
                            errorMessage = _memberinfo.info;
                            LoggerHelper.Log("MsmkLogger", flowNo +"余额支付失败！"+ errorMessage, LogEnum.SysLog);
                            break;
                        }
                        else
                        {
                            memo = string.Format("{0}:{1}", _memberinfo.balance, _memberinfo.score);
                            LoggerHelper.Log("MsmkLogger", flowNo + "余额支付！" + memo, LogEnum.SysLog);
                        }
                    }
                }
                //加减积分操作
                if (ispay)
                {
                    //for (int index = _listPayFlow.Count - 1; index >= 0; index--)
                    //{
                    t_cur_payflow _pay = _listPayFlow[0];
                    if (!string.IsNullOrEmpty(_pay.vip_no))
                    {
                        if (_pay.sale_way == "A")
                        {
                            _memberinfo = MemberService.Instance.AddMemberScore(_pay.vip_no, _pay.sale_amount.ToString(), flowNo, "门店消费:" + flowNo);
                            if (_memberinfo.code == "-1")
                            {
                                ispay = false;
                                errorMessage = _memberinfo.info;
                                LoggerHelper.Log("MsmkLogger", flowNo + "积分失败！" + errorMessage, LogEnum.SysLog);
                            }
                            else
                            {
                                memo = string.Format("{0}:{1}", _memberinfo.balance, _memberinfo.score);
                                LoggerHelper.Log("MsmkLogger", flowNo + "积分添加！" + memo, LogEnum.SysLog);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(_pay.voucher_no))
                    {
                        string vip_no = Gattr.Bll.GetVipCardByNo(_pay.voucher_no);
                        if (!string.IsNullOrEmpty(vip_no))
                        {
                            if (_pay.sale_way == "B")
                            {
                                _memberinfo = MemberService.Instance.AddMemberScore(vip_no, (_pay.sale_amount).ToString(), flowNo, "门店退货:" + _pay.voucher_no);
                                _currentMember = _memberinfo;
                                if (_memberinfo.code == "-1")
                                {
                                    ispay = false;
                                    errorMessage = _memberinfo.info;
                                    LoggerHelper.Log("MsmkLogger", flowNo + "门店退货（-1）！" + errorMessage, LogEnum.SysLog);
                                }
                                else
                                {
                                    memo = string.Format("{0}:{1}", _memberinfo.balance, _memberinfo.score);
                                    LoggerHelper.Log("MsmkLogger", flowNo + "扣除积分成功！" + memo, LogEnum.SysLog);
                                }
                            }
                        }
                    }
                    //else 
                }
                #endregion
                if (ispay)
                {
                    foreach (t_cur_payflow payinfo in _listPayFlow)
                    {
                        t_cur_payflow _pay = payinfo;
                        if (payinfo.flow_id == pay_flow_id)
                        {
                            _pay.memo = memo;
                        }
                        else if (payinfo.flow_id == s_flow_id)
                        {
                            _pay.memo = memo;
                        }
                        payList.Add(_pay);
                    }
                    _listPayFlow.Clear();
                    _listPayFlow.AddRange(payList);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", ex.ToString(), LogEnum.ExceptionLog);
            }
            return _dicPay;
        }
        #endregion

        #region 大键盘事件
        /// <summary>
        /// 大键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picSaleFunc_Click(object sender, EventArgs e)
        {
            if (this._posState == PosOpState.CHG)
            {
                this.ResetSaleState();
            }
            else if (this._posState == PosOpState.LOOK)
            {
                this.ResetSaleState(false, false);
            }
            else if (this._posState == PosOpState.PLU)
            {
                this.GvSaleFlow.Focus();
            }
            PictureBox box = sender as PictureBox;
            if (box != null)
            {
                string str = SIString.TryStr(box.Tag).Trim();
                if (str != string.Empty)
                {
                    t_attr_function keyfunc = null;
                    if (str.ToLower() != "qt")
                    {
                        this.plOtherFunc.Visible = false;
                    }
                    switch (str.ToLower())
                    {
                        case "0":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad0));
                            break;

                        case "1":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad1));
                            break;

                        case "2":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad2));
                            break;

                        case "3":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad3));
                            break;

                        case "4":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad4));
                            break;

                        case "5":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad5));
                            break;

                        case "6":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad6));
                            break;

                        case "7":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad7));
                            break;

                        case "8":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad8));
                            break;

                        case "9":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.NumPad9));
                            break;

                        case "dot":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Decimal));
                            break;

                        case "div":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Subtract));
                            break;

                        case "enter":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Return));
                            break;

                        case "backspace":
                            this.FrmMain_KeyDown(null, new KeyEventArgs(Keys.Back));
                            break;

                        case "reset":
                            this._inputText = string.Empty;
                            this.SetCursorText();
                            this.ShowMessage("取消输入！");
                            break;
                    }
                    if (keyfunc != null)
                    {
                        this.CallKeyFunc(keyfunc);
                    }
                }
            }
        }
        #endregion

        #region 设置大键盘光标
        /// <summary>
        /// 设置大键盘光标
        /// </summary>
        private void ResetSaleCursor()
        {
            TextGraphics graphics = new TextGraphics(base.CreateGraphics());
            this._sizeInputText = graphics.MeasureText("A", this.lbInput.Font);
            graphics.Dispose();
            this.lbCursor.Top = (int)(((float)(this.lbInput.Height - this.lbCursor.Height)) / 2f);
            this._inputDisplayMax = (this.lbInput.Width / this._sizeInputText.Width) - 1;
        }
        #endregion

        #region 库存查询
        private void StockQuery(bool isThis)
        {
            if (this._posState == PosOpState.PAY)
            {
                this.ShowMessage("当前状态，此功能键无效!");
            }
            else
            {
                //if (!Gattr.ServerBll.TestConnection())
                //{
                //    MessageBox.Show("断网状态，操作取消!", Gattr.AppTitle);
                //}
                //else
                //{
                new FrmStockQuery(isThis, null, Gattr.BranchNo).ShowDialog();
                //}
            }
        }
        #endregion

        #region 促销操作

        private List<t_cur_saleflow> SetSinglePlanRule(List<t_cur_saleflow> _saleflow)
        {
            string vip_type = "ALL";
            if (_currentMember != null && !string.IsNullOrEmpty(_currentMember.mem_type))
            {
                vip_type = string.Empty;
            }
            return PlanDealer.Instance.DearBeforePLU(_saleflow, vip_type);
        }

        private List<t_rm_plan_master> GetPlanPluMasters(List<t_cur_saleflow> _saleflow)
        {
            string vip_type = "ALL";
            if (_currentMember != null && !string.IsNullOrEmpty(_currentMember.mem_type))
            {
                vip_type = string.Empty;
            }
            return PlanDealer.Instance.GetPlanPluMasters(_saleflow, vip_type);
        }
        #endregion

        private void GvPayFlow_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #region 重新卖商品时操作
        /// <summary>
        /// 重新交易时操作
        /// </summary>
        private void BeforePLU()
        {
            if (this.lbAmtTitle.Text == "找零：")
            {
                this.lbAmtTitle.Text = "总价：";
                this.lbAmtTotal.Text = "0.00";
                this.lbQntyTotal.Text = "0.00";
                SetDoubleDisplay("2", "0.00", "0.00", "0.00", "0.00", "0.00", "",0,0);
            }
            this.ShowMessage("商品开始交易...");
        }
        #endregion

    }
}
