

namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 商品支付临时表
    /// </summary>
    [Serializable]
    public class t_operator
    {
        private string _oper_id;
        private string _oper_name;
        private string _oper_pwd;
        private string _full_name;
        private string _oper_role;
        private string _oper_state;
        private string _branch_id;
        private DateTime _last_date;
        private string _oper_flag;
        private string _group_id;
        private string _cash_grant;
        private decimal _discount_rate;
        

        /// <summary>
        /// 操作人ID
        /// </summary>
        public string oper_id
        {
            get { return _oper_id; }
            set { _oper_id = value; }
        }
        /// <summary>
        /// 操作人登录名
        /// </summary>
        public string oper_name
        {
            get { return _oper_name; }
            set { _oper_name = value; }
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string oper_pwd
        {
            get { return _oper_pwd; }
            set { _oper_pwd = value; }
        }
        /// <summary>
        /// 全名
        /// </summary>
        public string full_name
        {
            get { return _full_name; }
            set { _full_name = value; }
        }
        /// <summary>
        /// 操作的角色
        /// </summary>
        public string oper_role
        {
            get { return _oper_role; }
            set { _oper_role = value; }
        }
        /// <summary>
        /// 操作状态
        /// </summary>
        public string oper_state
        {
            get { return _oper_state; }
            set { _oper_state = value; }
        }
        /// <summary>
        /// 操作人所属门店
        /// </summary>
        public string branch_id
        {
            get { return _branch_id; }
            set { _branch_id = value; }
        }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        public DateTime last_date
        {
            get { return _last_date; }
            set { _last_date = value; }
        }
        /// <summary>
        /// 用户标记
        /// </summary>
        public string oper_flag
        {
            get { return _oper_flag; }
            set { _oper_flag = value; }
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public string group_id
        {
            get { return _group_id; }
            set { _group_id = value; }
        }
        /// <summary>
        /// 用户权限
        /// </summary>
        public string cash_grant
        {
            get { return _cash_grant; }
            set { _cash_grant = value; }
        }
        /// <summary>
        /// 用户打折折扣率
        /// </summary>
        public decimal discount_rate
        {
            get { return _discount_rate; }
            set { _discount_rate = value; }
        }
    }
}
