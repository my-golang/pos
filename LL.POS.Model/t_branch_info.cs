﻿

namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// t_branch_info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class t_branch_info
    {
        public t_branch_info()
        { }
        #region Model
        private decimal? _com_no;
        private string _branch_no;
        private string _branch_name;
        private string _property;
        private string _branch_man;
        private string _address;
        private string _zip;
        private string _branch_email;
        private string _branch_tel;
        private string _branch_fax;
        private string _display_flag = "1";
        private string _other1;
        private string _other2;
        private string _other3;
        private string _sheet_tag;
        private decimal? _area;
        private string _trade_type = "1";
        private string _com_grant = "1";
        private string _account = "0";
        private string _com_init = "0";
        private DateTime? _init_date;
        private string _indep_bal = "0";
        private string _dc_no;
        private DateTime? _com_date_up;
        private DateTime? _com_date_down;
        private string _com_oper_up;
        private string _com_oper_down;
        private string _check_flag = "0";
        private string _check_sheet_no;
        private string _check_branch_no;
        private string _price_type = "1";
        private decimal? _price_rate = 0M;
        private string _tran_flag;
        private string _oper_flag = "0";
        private string _allow_cz = "0";
        private decimal? _order_num;
        private decimal? _branch_mj;
        /// <summary>
        /// 
        /// </summary>
        public decimal? com_no
        {
            set { _com_no = value; }
            get { return _com_no; }
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
        public string branch_name
        {
            set { _branch_name = value; }
            get { return _branch_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string property
        {
            set { _property = value; }
            get { return _property; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string branch_man
        {
            set { _branch_man = value; }
            get { return _branch_man; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zip
        {
            set { _zip = value; }
            get { return _zip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string branch_email
        {
            set { _branch_email = value; }
            get { return _branch_email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string branch_tel
        {
            set { _branch_tel = value; }
            get { return _branch_tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string branch_fax
        {
            set { _branch_fax = value; }
            get { return _branch_fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string display_flag
        {
            set { _display_flag = value; }
            get { return _display_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string other1
        {
            set { _other1 = value; }
            get { return _other1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string other2
        {
            set { _other2 = value; }
            get { return _other2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string other3
        {
            set { _other3 = value; }
            get { return _other3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sheet_tag
        {
            set { _sheet_tag = value; }
            get { return _sheet_tag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? area
        {
            set { _area = value; }
            get { return _area; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string trade_type
        {
            set { _trade_type = value; }
            get { return _trade_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string com_grant
        {
            set { _com_grant = value; }
            get { return _com_grant; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string com_init
        {
            set { _com_init = value; }
            get { return _com_init; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? init_date
        {
            set { _init_date = value; }
            get { return _init_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string indep_bal
        {
            set { _indep_bal = value; }
            get { return _indep_bal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dc_no
        {
            set { _dc_no = value; }
            get { return _dc_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? com_date_up
        {
            set { _com_date_up = value; }
            get { return _com_date_up; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? com_date_down
        {
            set { _com_date_down = value; }
            get { return _com_date_down; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string com_oper_up
        {
            set { _com_oper_up = value; }
            get { return _com_oper_up; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string com_oper_down
        {
            set { _com_oper_down = value; }
            get { return _com_oper_down; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string check_flag
        {
            set { _check_flag = value; }
            get { return _check_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string check_sheet_no
        {
            set { _check_sheet_no = value; }
            get { return _check_sheet_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string check_branch_no
        {
            set { _check_branch_no = value; }
            get { return _check_branch_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string price_type
        {
            set { _price_type = value; }
            get { return _price_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? price_rate
        {
            set { _price_rate = value; }
            get { return _price_rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tran_flag
        {
            set { _tran_flag = value; }
            get { return _tran_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string oper_flag
        {
            set { _oper_flag = value; }
            get { return _oper_flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string allow_cz
        {
            set { _allow_cz = value; }
            get { return _allow_cz; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? order_num
        {
            set { _order_num = value; }
            get { return _order_num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? branch_mj
        {
            set { _branch_mj = value; }
            get { return _branch_mj; }
        }
        #endregion Model

    }
}
