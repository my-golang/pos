using System;
namespace LL.POS.Model
{
    /// <summary>
    /// t_app_viplist:会员本地消费记录表
    /// </summary>
    [Serializable]
    public partial class t_app_viplist
    {
        public t_app_viplist()
        { }
        #region Model
        private string _flow_no;
        private string _card_no;
        private decimal _score;
        private decimal _sale_amt;
        private string _oper_date;
        private string _voucher_no;
        private string _over_flag = "0";
        private string _com_flag = "0";
        private decimal _card_score = 0M;
        private decimal _card_amount = 0M;
        /// <summary>
        /// 消费流水
        /// </summary>
        public string flow_no
        {
            set { _flow_no = value; }
            get { return _flow_no; }
        }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string card_no
        {
            set { _card_no = value; }
            get { return _card_no; }
        }
        /// <summary>
        /// 本次积分
        /// </summary>
        public decimal score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 本次消费金额
        /// </summary>
        public decimal sale_amt
        {
            set { _sale_amt = value; }
            get { return _sale_amt; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string oper_date
        {
            set { _oper_date = value; }
            get { return _oper_date; }
        }
        /// <summary>
        /// 退货凭证号
        /// </summary>
        public string voucher_no
        {
            set { _voucher_no = value; }
            get { return _voucher_no; }
        }
        /// <summary>
        /// 同步到会员系统标示0未同步1已同步
        /// </summary>
        public string over_flag
        {
            set { _over_flag = value; }
            get { return _over_flag; }
        }
        /// <summary>
        /// 同步到服务器标示0未同步1已同步
        /// </summary>
        public string com_flag
        {
            set { _com_flag = value; }
            get { return _com_flag; }
        }
        /// <summary>
        /// 会员本次消费后积分
        /// </summary>
        public decimal card_score
        {
            set { _card_score = value; }
            get { return _card_score; }
        }
        /// <summary>
        /// 会员本次消费后余额
        /// </summary>
        public decimal card_amount
        {
            set { _card_amount = value; }
            get { return _card_amount; }
        }
        #endregion Model
    }
}

