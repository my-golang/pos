namespace LL.POS.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    public class TicketSet
    {
        public struct keyAndValue
        {
            public String keyStr;
            public String valueStr;
        }

        private List<keyAndValue> keyAndValueListTop = new List<keyAndValue>();
        /// <summary>
        /// 小票头部信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListTop
        {
            get { return keyAndValueListTop; }
        }
        /// <summary>
        /// 增加小票头部键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueTop(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListTop.Add(keyandvale);
        }

        private List<keyAndValue> keyAndValueListFoot = new List<keyAndValue>();
        /// <summary>
        /// 小票底部信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListFoot
        {
            get { return keyAndValueListFoot; }
        }
        /// <summary>
        /// 增加小票底部键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueFoot(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListFoot.Add(keyandvale);
        }
        private String ticketSignature;
        /// <summary>
        /// 小票顶部签名
        /// </summary>
        public String TicketSignature
        {
            get { return ticketSignature; }
            set { ticketSignature = value; }
        }

        private String ticketTitle;
        /// <summary>
        /// 小票的标题
        /// </summary>
        public String TicketTitle
        {
            get { return ticketTitle; }
            set { ticketTitle = value; }
        }
        private String ticketFooter;

        /// <summary>
        /// 小票底部签名
        /// </summary>
        public String TicketFooter
        {
            get { return ticketFooter; }
            set { ticketFooter = value; }
        }
        private DataTable dtGoodsList;

        /// <summary>
        /// 商品列表信息
        /// </summary>
        public DataTable DtGoodsList
        {
            get { return dtGoodsList; }
            set { dtGoodsList = value; }
        }
        private int ticketWidth;

        /// <summary>
        /// 小票宽度,按字符数计算
        /// </summary>
        public int TicketWidth
        {
            get { return ticketWidth; }
            set { ticketWidth = value; }
        }
        private Decimal colper1;

        /// <summary>
        /// 商品列表中第一个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper1
        {
            get { return colper1; }
            set { colper1 = value; }
        }
        private Decimal colper2;
        /// <summary>
        /// 商品列表中第二个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper2
        {
            get { return colper2; }
            set { colper2 = value; }
        }
        private Decimal colper3;
        /// <summary>
        /// 商品列表中第三个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper3
        {
            get { return colper3; }
            set { colper3 = value; }
        }

        private Decimal colper4;

        /// <summary>
        /// 商品列表中第四个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper4
        {
            get { return colper4; }
            set { colper4 = value; }
        }

        private Char signWeight;
        /// <summary>
        /// 重要分隔符的样式
        /// </summary>
        public Char SignWeight
        {
            get { return signWeight; }
            set { signWeight = value; }
        }
        private Char signLight;
        /// <summary>
        /// 一般分隔符的样式
        /// </summary>
        public Char SignLight
        {
            get { return signLight; }
            set { signLight = value; }
        }
        /// <summary>
        /// 商品列表设置
        /// </summary>
        /// <param name="ticket">TicketSet对象</param>
        /// <returns>带格式的商品列表</returns>
        private String ItemsList()
        {
            String result = string.Empty;

            if (this.dtGoodsList != null && this.dtGoodsList.Columns.Count > 0 && this.dtGoodsList.Rows.Count > 0)
            {
                result = CreateLine(this.TicketWidth, this.SignWeight);

                result += ArrangeArgPosition(this.dtGoodsList.Columns[1].Caption, this.TicketWidth, this.Colper1);

                result += ArrangeArgPosition(this.dtGoodsList.Columns[2].Caption, this.TicketWidth, this.Colper2);

                result += ArrangeArgPosition(this.dtGoodsList.Columns[3].Caption, this.TicketWidth, this.Colper3);

                result += ArrangeArgPosition(this.dtGoodsList.Columns[4].Caption, this.TicketWidth, this.Colper4);

                result += "\r\n";
                result += CreateLine(this.ticketWidth, this.SignLight);

                for (int i = 0; i < this.dtGoodsList.Rows.Count; i++)
                {
                    result += SetArgPosition(this.dtGoodsList.Rows[i][1].ToString(), this.TicketWidth, false);

                    result += ArrangeArgPosition(this.dtGoodsList.Rows[i][0].ToString(), this.TicketWidth, this.Colper1);

                    result += ArrangeArgPosition(this.dtGoodsList.Rows[i][2].ToString(), this.TicketWidth, this.Colper2);

                    result += ArrangeArgPosition(this.dtGoodsList.Rows[i][3].ToString(), this.TicketWidth, this.Colper3);

                    result += ArrangeArgPosition(this.dtGoodsList.Rows[i][4].ToString(), this.TicketWidth, this.Colper4);

                    result += "\r\n";
                }
                result += CreateLine(this.ticketWidth, this.SignLight);
            }
            return result;
        }
        /// <summary>
        /// 排列商品列表的表头信息
        /// </summary>
        /// <param name="arg">表头标题</param>
        /// <param name="charNum">标题所占总字符数，一般按照小票总宽度的百分比来设置</param>
        /// <returns>带有格式的标题</returns>
        private String ArrangeArgPosition(String arg, int ticketwidth, Decimal colPer)
        {
            String result = String.Empty;
            int charNum = Convert.ToInt32(ticketWidth * colPer);
            if (0 != charNum)
            {
                int argcount = System.Text.Encoding.Default.GetByteCount(arg);

                for (int i = 0; i < charNum - argcount; i++)
                {
                    arg = arg + " ";
                }
                result = result + arg;
            }
            return result;

        }
        /// <summary>
        /// 设置小票头部信息，可以自动区分汉字还是英文，格式只限2行
        /// </summary>
        /// <param name="arg">小票头部内容</param>
        /// <param name="ticketwidth">小票宽度，按照字符个数计算</param>
        /// <param name="isMiddle">是否中间对齐</param>
        /// <returns>带格式的小票头部信息</returns>
        private String SetArgPosition(String arg, int ticketwidth, bool isMiddle)
        {
            String result = String.Empty;
            int argnum = System.Text.Encoding.Default.GetByteCount(arg);
            if (argnum <= ticketwidth)
            {
                if (isMiddle)
                {
                    for (int i = 0; i < (ticketwidth - argnum) / 2; i++)
                    {
                        result = " " + result;
                    }
                }
                result = result + arg + "\r\n";

            }
            else
            {
                for (int i = 0; i <= ticketwidth / 2; i++)
                {
                    int temp = ticketwidth / 2 + i;
                    if (ticketwidth == System.Text.Encoding.Default.GetByteCount(arg.Substring(0, temp)) || ticketwidth == System.Text.Encoding.Default.GetByteCount(arg.Substring(0, temp)) - 1)
                    {
                        result = result + arg.Substring(0, temp);
                        result = result + "\r\n";
                        result = result + arg.Substring(temp, arg.Length - (temp)) + "\r\n";
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 设置小票各部分的分隔线
        /// </summary>
        /// <param name="ticketwidth">小票的宽度，按照字符个数计算</param>
        /// <param name="signChar">分隔线的样式</param>
        /// <returns>小票的分隔线</returns>
        private String CreateLine(int ticketwidth, Char signChar)
        {
            String result = String.Empty;
            for (int i = 0; i < ticketwidth; i++)
            {
                result += signChar;
            }
            result += "\r\n";

            return result;
        }

        /// <summary>
        /// 生成小票
        /// </summary>
        /// <param name="ticket">TicketSet对象</param>
        /// <returns>最终小票结果</returns>
        public String Ticket()
        {
            String ticketStr = String.Empty;
            //小票头部
            ticketStr = SetArgPosition(this.TicketSignature, this.TicketWidth, true);
            ticketStr += SetArgPosition(this.TicketTitle, this.TicketWidth, true);
            ticketStr += CreateLine(this.TicketWidth, this.signWeight);

            //小票上部内容
            for (int i = 0; i < this.KeyAndValueListTop.Count; i++)
            {
                ticketStr += this.KeyAndValueListTop[i].keyStr + this.KeyAndValueListTop[i].valueStr + "\r\n";
            }

            //商品列表
            ticketStr += ItemsList();

            //小票下部内容
            for (int i = 0; i < this.KeyAndValueListFoot.Count; i++)
            {
                ticketStr += this.KeyAndValueListFoot[i].keyStr + this.KeyAndValueListFoot[i].valueStr + "\r\n";
            }

            //小票底部
            ticketStr += CreateLine(this.TicketWidth, this.signWeight);
            ticketStr += this.TicketFooter + "\r\n";

            return ticketStr;
        }
    }
}
