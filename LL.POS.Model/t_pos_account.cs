namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 收银员对账记录实体
    /// </summary>
    public class t_pos_account
    {
        public t_pos_account()
        { }
        #region Model
        private string _branch_no;
        private string _pos_id;
        private string _oper_id;
        private string _oper_date;
        private string _start_time;
        private string _end_time;
        private decimal? _sale_amt = 0M;
        private decimal? _hand_amt = 0M;
        private string _com_flag = "0";

        /// <summary>
        /// 门店编码
        /// </summary>
        public string branch_no
        {
            set { _branch_no = value; }
            get { return _branch_no; }
        }
        /// <summary>
        /// POS机编码
        /// </summary>
        public string pos_id
        {
            set { _pos_id = value; }
            get { return _pos_id; }
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
        /// 操作时间
        /// </summary>
        public string oper_date
        {
            set { _oper_date = value; }
            get { return _oper_date; }
        }
        /// <summary>
        /// 首笔交易时间
        /// </summary>
        public string start_time
        {
            set { _start_time = value; }
            get { return _start_time; }
        }
        /// <summary>
        /// 对账结束时间
        /// </summary>
        public string end_time
        {
            set { _end_time = value; }
            get { return _end_time; }
        }
        /// <summary>
        /// 对账金额
        /// </summary>
        public decimal? sale_amt
        {
            set { _sale_amt = value; }
            get { return _sale_amt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? hand_amt
        {
            set { _hand_amt = value; }
            get { return _hand_amt; }
        }
        /// <summary>
        /// 上传标志
        /// </summary>
        public string com_flag
        {
            set { _com_flag = value; }
            get { return _com_flag; }
        }
        #endregion Model
    }
}
