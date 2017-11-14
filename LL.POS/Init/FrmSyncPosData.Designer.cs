using LL.POS.Common;
namespace LL.POS
{
    partial class FrmSyncPosData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbMessage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnOk = new ButtonEx(this.components);
            this.buttonEx2 = new ButtonEx(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 40);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(385, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据传输";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMessage
            // 
            this.lbMessage.Location = new System.Drawing.Point(22, 50);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(348, 12);
            this.lbMessage.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 71);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(352, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.ForeColor = System.Drawing.Color.Transparent;
            this.btnOk.Location = new System.Drawing.Point(184, 149);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 29);
            this.btnOk.TabIndex = 4;
            this.btnOk.Tag = "all";
            this.btnOk.Text = "同步下传(&F1)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // buttonEx2
            // 
            this.buttonEx2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonEx2.ForeColor = System.Drawing.Color.Transparent;
            this.buttonEx2.Location = new System.Drawing.Point(289, 149);
            this.buttonEx2.Name = "buttonEx2";
            this.buttonEx2.Size = new System.Drawing.Size(80, 29);
            this.buttonEx2.TabIndex = 6;
            this.buttonEx2.Text = "退出(&Esc)";
            this.buttonEx2.UseVisualStyleBackColor = true;
            this.buttonEx2.Click += new System.EventHandler(this.buttonEx2_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(20, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(350, 42);
            this.label2.TabIndex = 7;
            this.label2.Text = "注意：系统正在传输数据，可能需要比较长的时间，在此期间不要关闭电脑，以免造成数据混乱，请耐心等待...";
            // 
            // FrmSyncPosData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonEx2;
            this.ClientSize = new System.Drawing.Size(385, 185);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonEx2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbMessage);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(391, 210);
            this.MinimumSize = new System.Drawing.Size(391, 210);
            this.Name = "FrmSyncPosData";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private ButtonEx btnOk;
        private ButtonEx buttonEx2;
        private System.Windows.Forms.Label label2;
    }
}