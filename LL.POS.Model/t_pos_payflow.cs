namespace LL.POS.Model
{
    using System;
    public class t_pos_payflow
    {
        public t_pos_payflow()
        { }
        #region Model
        private int _id;
        private int? _flow_id;
        private string _flow_no;
        private decimal? _sale_amount;
        private string _pay_way;
        private decimal? _pay_amount;
        private string _coin_type;
        private string _pay_name;
        private decimal? _coin_rate;
        private decimal? _convert_amt;
        private string _card_no;
        private string _memo;
        private string _vip_no;
        private string _oper_date;
        private string _oper_id;
        private string _voucher_no;
        private string _branch_no;
        private string _pos_id;
        private string _sale_way;
        private string _com_flag;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? flow_id
        {
            set { _flow_id = value; }
            get { return _flow_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string flow_no
        {
            set { _flow_no = value; }
            get { return _flow_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? sale_amount
        {
            set { _sale_amount = value; }
            get { return _sale_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pay_way
        {
            set { _pay_way = value; }
            get { return _pay_way; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? pay_amount
        {
            set { _pay_amount = value; }
            get { return _pay_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string coin_type
        {
            set { _coin_type = value; }
            get { return _coin_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pay_name
        {
            set { _pay_name = value; }
            get { return _pay_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? coin_rate
        {
            set { _coin_rate = value; }
            get { return _coin_rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? convert_amt
        {
            set { _convert_amt = value; }
            get { return _convert_amt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string card_no
        {
            set { _card_no = value; }
            get { return _card_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string memo
        {
            set { _memo = value; }
            get { return _memo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vip_no
        {
            set { _vip_no = value; }
            get { return _vip_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string oper_date
        {
            set { _oper_date = value; }
            get { return _oper_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string oper_id
        {
            set { _oper_id = value; }
            get { return _oper_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string voucher_no
        {
            set { _voucher_no = value; }
            get { return _voucher_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string branch_no
        {
            set { _branch_no = value; }
            get { return _branch_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pos_id
        {
            set { _pos_id = value; }
            get { return _pos_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sale_way
        {
            set { _sale_way = value; }
            get { return _sale_way; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string com_flag
        {
            set { _com_flag = value; }
            get { return _com_flag; }
        }

        private string _pos_flag="0";
        /// <summary>
        /// 
        /// </summary>
        public string pos_flag
        {
            set { _pos_flag = value; }
            get { return _pos_flag; }
        }
        #endregion Model
    }
}
