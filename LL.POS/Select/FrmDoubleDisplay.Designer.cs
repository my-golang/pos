
using LL.POS.Common;
namespace LL.POS
{
    partial class FrmDoubleDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any Properties.Resources being used.
        /// </summary>
        /// <param name="disposing">true if managed Properties.Resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.plSaleFlow = new System.Windows.Forms.Panel();
            this.GvSaleFlow = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelhead = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.plpay = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbChgAmt = new System.Windows.Forms.Label();
            this.lbPaedAmt = new System.Windows.Forms.Label();
            this.lbRemainAmt = new System.Windows.Forms.Label();
            this.lbBTotalAmt = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAd = new System.Windows.Forms.Label();
            this.plSale = new System.Windows.Forms.Panel();
            this.bindingSaleFlow = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tblCardInfo = new LL.POS.Common.TableLayoutPanelEx(this.components);
            this.lbScore = new System.Windows.Forms.Label();
            this.lbMoney = new System.Windows.Forms.Label();
            this.lbCard = new System.Windows.Forms.Label();
            this.lbTotalAmt = new System.Windows.Forms.Label();
            this.lbTotalQty = new System.Windows.Forms.Label();
            this.plSaleFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvSaleFlow)).BeginInit();
            this.tableLayoutPanelhead.SuspendLayout();
            this.plpay.SuspendLayout();
            this.plSale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSaleFlow)).BeginInit();
            this.tblCardInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // plSaleFlow
            // 
            this.plSaleFlow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(222)))), ((int)(((byte)(247)))));
            this.plSaleFlow.Controls.Add(this.GvSaleFlow);
            this.plSaleFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plSaleFlow.Location = new System.Drawing.Point(0, 43);
            this.plSaleFlow.Margin = new System.Windows.Forms.Padding(0);
            this.plSaleFlow.Name = "plSaleFlow";
            this.plSaleFlow.Size = new System.Drawing.Size(243, 203);
            this.plSaleFlow.TabIndex = 23;
            // 
            // GvSaleFlow
            // 
            this.GvSaleFlow.AllowUserToAddRows = false;
            this.GvSaleFlow.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(222)))), ((int)(((byte)(247)))));
            this.GvSaleFlow.ColumnHeadersHeight = 40;
            this.GvSaleFlow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GvSaleFlow.ColumnHeadersVisible = false;
            this.GvSaleFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GvSaleFlow.Location = new System.Drawing.Point(0, 0);
            this.GvSaleFlow.MultiSelect = false;
            this.GvSaleFlow.Name = "GvSaleFlow";
            this.GvSaleFlow.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvSaleFlow.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GvSaleFlow.RowHeadersVisible = false;
            this.GvSaleFlow.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GvSaleFlow.RowTemplate.Height = 40;
            this.GvSaleFlow.RowTemplate.ReadOnly = true;
            this.GvSaleFlow.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GvSaleFlow.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.GvSaleFlow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GvSaleFlow.Size = new System.Drawing.Size(243, 203);
            this.GvSaleFlow.TabIndex = 0;
            this.GvSaleFlow.TabStop = false;
            this.GvSaleFlow.VirtualMode = true;
            this.GvSaleFlow.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GvSaleFlow_DataError);
            this.GvSaleFlow.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.GvSaleFlow_RowPostPaint);
            this.GvSaleFlow.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GvSaleFlow_RowsAdded);
            // 
            // tableLayoutPanelhead
            // 
            this.tableLayoutPanelhead.ColumnCount = 3;
            this.tableLayoutPanelhead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelhead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelhead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelhead.Controls.Add(this.label13, 2, 1);
            this.tableLayoutPanelhead.Controls.Add(this.label11, 1, 1);
            this.tableLayoutPanelhead.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanelhead.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanelhead.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanelhead.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelhead.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelhead.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelhead.Name = "tableLayoutPanelhead";
            this.tableLayoutPanelhead.RowCount = 2;
            this.tableLayoutPanelhead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelhead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelhead.Size = new System.Drawing.Size(243, 43);
            this.tableLayoutPanelhead.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label13.Location = new System.Drawing.Point(169, 21);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 22);
            this.label13.TabIndex = 25;
            this.label13.Text = "金额";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label11.Location = new System.Drawing.Point(97, 21);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 22);
            this.label11.TabIndex = 24;
            this.label11.Text = "单价";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label12.Location = new System.Drawing.Point(0, 21);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 22);
            this.label12.TabIndex = 23;
            this.label12.Text = "数量(kg/件)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanelhead.SetColumnSpan(this.label9, 2);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label9.Location = new System.Drawing.Point(97, 0);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 21);
            this.label9.TabIndex = 21;
            this.label9.Text = "品名";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 21);
            this.label5.TabIndex = 20;
            this.label5.Text = "货号";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plpay
            // 
            this.plpay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(222)))), ((int)(((byte)(247)))));
            this.plpay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plpay.Controls.Add(this.label27);
            this.plpay.Controls.Add(this.label15);
            this.plpay.Controls.Add(this.lbChgAmt);
            this.plpay.Controls.Add(this.lbPaedAmt);
            this.plpay.Controls.Add(this.lbRemainAmt);
            this.plpay.Controls.Add(this.lbBTotalAmt);
            this.plpay.Controls.Add(this.label14);
            this.plpay.Controls.Add(this.label10);
            this.plpay.Controls.Add(this.label6);
            this.plpay.Controls.Add(this.label8);
            this.plpay.Controls.Add(this.label7);
            this.plpay.Controls.Add(this.label4);
            this.plpay.Controls.Add(this.label3);
            this.plpay.Controls.Add(this.label2);
            this.plpay.Controls.Add(this.label1);
            this.plpay.Location = new System.Drawing.Point(6, 356);
            this.plpay.Margin = new System.Windows.Forms.Padding(0);
            this.plpay.Name = "plpay";
            this.plpay.Size = new System.Drawing.Size(243, 190);
            this.plpay.TabIndex = 45;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Blue;
            this.label27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label27.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.ForeColor = System.Drawing.Color.Blue;
            this.label27.Location = new System.Drawing.Point(1, -164);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(205, 2);
            this.label27.TabIndex = 15;
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Blue;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Blue;
            this.label15.Location = new System.Drawing.Point(1, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(237, 2);
            this.label15.TabIndex = 14;
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbChgAmt
            // 
            this.lbChgAmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbChgAmt.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbChgAmt.ForeColor = System.Drawing.Color.Blue;
            this.lbChgAmt.Location = new System.Drawing.Point(119, 149);
            this.lbChgAmt.Name = "lbChgAmt";
            this.lbChgAmt.Size = new System.Drawing.Size(107, 23);
            this.lbChgAmt.TabIndex = 13;
            this.lbChgAmt.Text = "0.00";
            this.lbChgAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbPaedAmt
            // 
            this.lbPaedAmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPaedAmt.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPaedAmt.ForeColor = System.Drawing.Color.Blue;
            this.lbPaedAmt.Location = new System.Drawing.Point(119, 104);
            this.lbPaedAmt.Name = "lbPaedAmt";
            this.lbPaedAmt.Size = new System.Drawing.Size(107, 23);
            this.lbPaedAmt.TabIndex = 12;
            this.lbPaedAmt.Text = "0.00";
            this.lbPaedAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbRemainAmt
            // 
            this.lbRemainAmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbRemainAmt.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRemainAmt.ForeColor = System.Drawing.Color.Blue;
            this.lbRemainAmt.Location = new System.Drawing.Point(119, 59);
            this.lbRemainAmt.Name = "lbRemainAmt";
            this.lbRemainAmt.Size = new System.Drawing.Size(107, 23);
            this.lbRemainAmt.TabIndex = 11;
            this.lbRemainAmt.Text = "0.00";
            this.lbRemainAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbBTotalAmt
            // 
            this.lbBTotalAmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBTotalAmt.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBTotalAmt.ForeColor = System.Drawing.Color.Blue;
            this.lbBTotalAmt.Location = new System.Drawing.Point(119, 14);
            this.lbBTotalAmt.Name = "lbBTotalAmt";
            this.lbBTotalAmt.Size = new System.Drawing.Size(107, 23);
            this.lbBTotalAmt.TabIndex = 10;
            this.lbBTotalAmt.Text = "0.00";
            this.lbBTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Blue;
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(112, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(2, 180);
            this.label14.TabIndex = 9;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Blue;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(1, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(237, 2);
            this.label10.TabIndex = 8;
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(12, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "总计：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Blue;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(1, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(237, 2);
            this.label8.TabIndex = 6;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Blue;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(1, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(237, 2);
            this.label7.TabIndex = 5;
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Blue;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(1, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 2);
            this.label4.TabIndex = 3;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(12, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "找零：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "实收：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "应收：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAd
            // 
            this.tbAd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAd.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbAd.Location = new System.Drawing.Point(255, 434);
            this.tbAd.Name = "tbAd";
            this.tbAd.Size = new System.Drawing.Size(556, 112);
            this.tbAd.TabIndex = 46;
            this.tbAd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tbAd.Click += new System.EventHandler(this.tbAd_Click);
            // 
            // plSale
            // 
            this.plSale.Controls.Add(this.plSaleFlow);
            this.plSale.Controls.Add(this.tableLayoutPanelhead);
            this.plSale.Location = new System.Drawing.Point(6, 4);
            this.plSale.Name = "plSale";
            this.plSale.Size = new System.Drawing.Size(243, 246);
            this.plSale.TabIndex = 47;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(256, 4);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(548, 413);
            this.webBrowser1.TabIndex = 49;
            this.webBrowser1.Url = new System.Uri("http://www.geelan.cn/", System.UriKind.Absolute);
            // 
            // tblCardInfo
            // 
            this.tblCardInfo.ColumnCount = 2;
            this.tblCardInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCardInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCardInfo.Controls.Add(this.lbScore, 0, 3);
            this.tblCardInfo.Controls.Add(this.lbMoney, 0, 1);
            this.tblCardInfo.Controls.Add(this.lbCard, 0, 1);
            this.tblCardInfo.Controls.Add(this.lbTotalAmt, 1, 0);
            this.tblCardInfo.Controls.Add(this.lbTotalQty, 0, 0);
            this.tblCardInfo.Location = new System.Drawing.Point(6, 250);
            this.tblCardInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tblCardInfo.Name = "tblCardInfo";
            this.tblCardInfo.RowCount = 4;
            this.tblCardInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCardInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblCardInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblCardInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblCardInfo.Size = new System.Drawing.Size(243, 106);
            this.tblCardInfo.TabIndex = 48;
            // 
            // lbScore
            // 
            this.lbScore.AutoSize = true;
            this.lbScore.BackColor = System.Drawing.Color.Transparent;
            this.lbScore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblCardInfo.SetColumnSpan(this.lbScore, 2);
            this.lbScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbScore.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lbScore.Location = new System.Drawing.Point(0, 82);
            this.lbScore.Margin = new System.Windows.Forms.Padding(0);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(243, 24);
            this.lbScore.TabIndex = 6;
            this.lbScore.Text = "积 分:";
            this.lbScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMoney
            // 
            this.lbMoney.AutoSize = true;
            this.lbMoney.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblCardInfo.SetColumnSpan(this.lbMoney, 2);
            this.lbMoney.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMoney.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lbMoney.Location = new System.Drawing.Point(0, 58);
            this.lbMoney.Margin = new System.Windows.Forms.Padding(0);
            this.lbMoney.Name = "lbMoney";
            this.lbMoney.Size = new System.Drawing.Size(243, 24);
            this.lbMoney.TabIndex = 5;
            this.lbMoney.Text = "余 额:";
            this.lbMoney.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCard
            // 
            this.lbCard.AutoSize = true;
            this.lbCard.BackColor = System.Drawing.Color.Transparent;
            this.lbCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblCardInfo.SetColumnSpan(this.lbCard, 2);
            this.lbCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCard.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lbCard.Location = new System.Drawing.Point(0, 34);
            this.lbCard.Margin = new System.Windows.Forms.Padding(0);
            this.lbCard.Name = "lbCard";
            this.lbCard.Size = new System.Drawing.Size(243, 24);
            this.lbCard.TabIndex = 4;
            this.lbCard.Text = "卡 号:";
            this.lbCard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTotalAmt
            // 
            this.lbTotalAmt.AutoSize = true;
            this.lbTotalAmt.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTotalAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTotalAmt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lbTotalAmt.Location = new System.Drawing.Point(121, 0);
            this.lbTotalAmt.Margin = new System.Windows.Forms.Padding(0);
            this.lbTotalAmt.Name = "lbTotalAmt";
            this.lbTotalAmt.Size = new System.Drawing.Size(122, 34);
            this.lbTotalAmt.TabIndex = 1;
            this.lbTotalAmt.Text = "金额:0.00";
            this.lbTotalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTotalQty
            // 
            this.lbTotalQty.AutoSize = true;
            this.lbTotalQty.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTotalQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTotalQty.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lbTotalQty.Location = new System.Drawing.Point(0, 0);
            this.lbTotalQty.Margin = new System.Windows.Forms.Padding(0);
            this.lbTotalQty.Name = "lbTotalQty";
            this.lbTotalQty.Size = new System.Drawing.Size(121, 34);
            this.lbTotalQty.TabIndex = 0;
            this.lbTotalQty.Text = "数量:0.00";
            this.lbTotalQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmDoubleDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 555);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.tblCardInfo);
            this.Controls.Add(this.plSale);
            this.Controls.Add(this.tbAd);
            this.Controls.Add(this.plpay);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FrmDoubleDisplay";
            this.Text = "FrmDoubleDisplay";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmDoubleDisplay_Load);
            this.plSaleFlow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvSaleFlow)).EndInit();
            this.tableLayoutPanelhead.ResumeLayout(false);
            this.tableLayoutPanelhead.PerformLayout();
            this.plpay.ResumeLayout(false);
            this.plSale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSaleFlow)).EndInit();
            this.tblCardInfo.ResumeLayout(false);
            this.tblCardInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSaleFlow;
        public System.DateTime currNow;
        private System.Windows.Forms.DataGridView GvSaleFlow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbBTotalAmt;
        private System.Windows.Forms.Label lbChgAmt;
        private System.Windows.Forms.Label lbPaedAmt;
        private System.Windows.Forms.Label lbRemainAmt;
        private System.Windows.Forms.Label lbTotalAmt;
        private System.Windows.Forms.Label lbTotalQty;
        private System.Windows.Forms.Panel plpay;
        private System.Windows.Forms.Panel plSale;
        private System.Windows.Forms.Panel plSaleFlow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelhead;
        private System.Windows.Forms.Label tbAd;
        private TableLayoutPanelEx tblCardInfo;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label lbCard;
        private System.Windows.Forms.Label lbScore;
        private System.Windows.Forms.Label lbMoney;

    }
}