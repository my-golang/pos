using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace LL.POS.GetDiskNumber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> _dic = LL.Utility.ComputerUtility.Instance.GetDiskVolumeSerialNumber11();
            foreach (KeyValuePair<string, object> _d in _dic)
            {
                this.label1.Text += _d.Key + ":" + _d.Value+"\r\n";
            }

            this.label2.Text = LL.Utility.ComputerUtility.Instance.GetDiskVolumeSerialNumber();
        }
    }
}
