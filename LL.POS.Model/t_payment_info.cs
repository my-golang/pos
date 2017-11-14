namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 支付方式信息实体定义
    /// </summary>
    public class t_payment_info
    {
        private String _pay_way;
        /// <summary>
        /// 支付方式代码
        /// </summary>
        public String pay_way
        {
            get { return _pay_way; }
            set { _pay_way = value; }
        }
        private String _pay_flag;
        /// <summary>
        /// 支付标示
        /// </summary>
        public String pay_flag
        {
            get
            {
                return _pay_flag;
            }
            set
            {
                _pay_flag = value;
            }
        }
        private String _pay_name;
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public String pay_name
        {
            get
            {
                return _pay_name;
            }
            set
            {
                _pay_name = value;
            }
        }
        private Decimal _rate;
        /// <summary>
        /// 人民币汇率
        /// </summary>
        public Decimal rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
            }
        }
        private String _memo;
        /// <summary>
        /// 描述
        /// </summary>
        public String memo
        {
            get
            {
                return _memo;
            }
            set
            {
                _memo = value;
            }
        }
    }
}
