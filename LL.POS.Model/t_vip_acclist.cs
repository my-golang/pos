namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 会员消费信息表
    /// </summary>
    [Serializable]
    public class t_vip_acclist
    {

        private string _flow_no;
        private string _card_id;
        private decimal _balance;
        private decimal _total_spending;
        private decimal _all_score;
        private decimal _use_score;
        private decimal _surplus_score;
        private DateTime _oper_time;
        /// <summary>
        /// 流水号
        /// </summary>
        public string flow_no
        {
            get
            {
                return this._flow_no;
            }
            set
            {
                this._flow_no = value;
            }
        }
        /// <summary>
        /// 会员卡
        /// </summary>
        public string card_id
        {
            get
            {
                return this._card_id;
            }
            set
            {
                this._card_id = value;
            }
        }
        /// <summary>
        /// 可用金额
        /// </summary>
        public decimal balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal total_spending
        {
            get
            {
                return this._total_spending;
            }
            set
            {
                this._total_spending = value;
            }
        }
        /// <summary>
        /// 总积分
        /// </summary>
        public decimal all_score
        {
            get
            {
                return this._all_score;
            }
            set
            {
                this._all_score = value;
            }
        }
        /// <summary>
        /// 已用积分
        /// </summary>
        public decimal use_score
        {
            get
            {
                return this._use_score;
            }
            set
            {
                this._use_score = value;
            }
        }
        /// <summary>
        /// 剩余积分
        /// </summary>
        public decimal surplus_score
        {
            get
            {
                return this._surplus_score;
            }
            set
            {
                this._surplus_score = value;
            }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime oper_time
        {
            get { return _oper_time; }
            set { _oper_time = value; }
        }
    }
}

