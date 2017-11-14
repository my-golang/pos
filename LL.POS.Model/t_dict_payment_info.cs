

namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 商品支付临时表
    /// </summary>
    [Serializable]
    public class t_dict_payment_info
    {
        private string _pay_way;
        private int _pay_flag;
        private string _pay_name;
        private decimal _rate;
        private string _pay_memo;

        /// <summary>
        /// 付款代码
        /// </summary>
        public string pay_way
        {
            get { return _pay_way; }
            set { _pay_way = value; }
        }
        /// <summary>
        /// 代码标记
        /// </summary>
        public int pay_flag
        {
            get { return _pay_flag; }
            set { _pay_flag = value; }
        }

        /// <summary>
        /// 代码名称
        /// </summary>
        public string pay_name
        {
            get { return _pay_name; }
            set { _pay_name = value; }
        }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal rate
        {
            get { return _rate; }
            set { _rate = value; }
        }
        /// <summary>
        /// 代码备注
        /// </summary>
        public string pay_memo
        {
            get { return _pay_memo; }
            set { _pay_memo = value; }
        }
    }
}
