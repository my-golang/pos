

namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 销售商品临时表
    /// </summary>
    [Serializable]
    public class t_cur_saleflow
    {

        private decimal _flow_id;
        private string _flow_no;
        private string _item_no;
        private decimal _unit_price; //单价
        private decimal _sale_price;//售价
        private decimal _price;//进价
        private string _item_subname;
        private string _item_status = "1";
        private string _item_name;
        private string _item_clsno;
        private string _item_subno;
        private string _oper_date;
        private string _oper_id;
        private decimal _sale_money;//小计
        private decimal _sale_qnty = 1;//数量
        private decimal _discount_rate;
        private string _last_item_name;
        private decimal _num3; //商品交易类型
        private string _unit_no; //商品类型
        private string _item_brand;

        private string _plan_no;
        private string _row_id;


        public object clone()
        {
            return base.MemberwiseClone();
        }

        public decimal price
        {
            get { return _price; }
            set { _price = value; }
        }
        public decimal flow_id
        {
            get { return _flow_id; }
            set { _flow_id = value; }
        }

        public string flow_no
        {
            get { return _flow_no; }
            set { _flow_no = value; }
        }

        public string item_no
        {
            get { return _item_no; }
            set { _item_no = value; }
        }

        public decimal unit_price
        {
            get { return _unit_price; }
            set { _unit_price = value; }
        }

        public decimal sale_price
        {
            get { return _sale_price; }
            set { _sale_price = value; }
        }


        public decimal sale_qnty
        {
            get { return _sale_qnty; }
            set { _sale_qnty = value; }
        }

        public decimal sale_money
        {
            get { return _sale_money; }
            set { _sale_money = value; }
        }

        public string oper_id
        {
            get { return _oper_id; }
            set { _oper_id = value; }
        }

        public string oper_date
        {
            get { return _oper_date; }
            set { _oper_date = value; }
        }

        public string item_subno
        {
            get { return _item_subno; }
            set { _item_subno = value; }
        }

        public string item_clsno
        {
            get { return _item_clsno; }
            set { _item_clsno = value; }
        }

        public string item_name
        {
            get { return _item_name; }
            set { _item_name = value; }
        }

        public string item_status
        {
            get { return _item_status; }
            set { _item_status = value; }
        }

        public string item_subname
        {
            get { return _item_subname; }
            set { _item_subname = value; }
        }
        public string last_item_name
        {
            get { return _last_item_name; }
            set { _last_item_name = value; }
        }
        /// <summary>
        /// 扣折率
        /// </summary>
        public decimal discount_rate
        {
            get { return _discount_rate; }
            set { _discount_rate = value; }
        }

        private string _reasonid;

        //退货原因ID
        public string reasonid
        {
            get { return _reasonid; }
            set { _reasonid = value; }
        }

        private string _sale_way = "A";
        /// <summary>
        /// 销售类型：A正常B退货C赠送
        /// </summary>
        public string sale_way
        {
            get { return _sale_way; }
            set { _sale_way = value; }
        }

        private decimal _flow_old_id;
        public decimal flow_old_id
        {
            get { return _flow_old_id; }
            set { _flow_old_id = value; }
        }
        private decimal _source_qnty;
        public decimal source_qnty
        {
            get { return _source_qnty; }
            set { _source_qnty = value; }
        }

        private string _branch_no;
        /// <summary>
        /// 门店编码
        /// </summary>
        public string branch_no
        {
            get
            {
                return _branch_no;
            }
            set
            {
                _branch_no = value;
            }
        }
        private string _pos_id;
        /// <summary>
        /// 收银机编码
        /// </summary>
        public string pos_id
        {
            get
            {
                return _pos_id;
            }
            set
            {
                _pos_id = value;
            }
        }
        public decimal num3
        {
            get { return _num3; }
            set { _num3 = value; }
        }
        public string unit_no
        {
            get { return _unit_no; }
            set { _unit_no = value; }
        }
        /// <summary>
        /// 品牌
        /// </summary>
        public string item_brand
        {
            get { return _item_brand; }
            set { _item_brand = value; }
        }
        /// <summary>
        /// 命中优惠的编号
        /// </summary>
        public string plan_no
        {
            get { return _plan_no; }
            set { _plan_no = value; }
        }
        /// <summary>
        /// 命中优惠规则的编号
        /// </summary>
        public string row_id
        {
            get { return _row_id; }
            set { _row_id = value; }
        }

        private bool _isYh;
        /// <summary>
        /// 是否命中优惠规则
        /// </summary>
        public bool isYh
        {
            get { return _isYh; }
            set { _isYh = value; }
        }

        private bool _isSelect;
        /// <summary>
        /// 
        /// </summary>
        public bool isSelect
        {
            get { return _isSelect; }
            set { _isSelect = value; }
        }

        private bool _isLimit = false;
        /// <summary>
        /// 是否可以重复优惠
        /// </summary>
        public bool isLimit
        {
            get
            {
                return _isLimit;
            }
            set
            {
                _isLimit = value;
            }
        }

        private string _item_rem;
        public string item_rem
        {
            get
            {
                return _item_rem;
            }
            set
            {
                _item_rem = value;
            }
        }

        private string _product_area;
        public string product_area
        {
            get
            {
                return _product_area;
            }
            set
            {
                _product_area = value;
            }
        }

        private string _item_size;
        public string item_size
        {
            get
            {
                return _item_size;
            }
            set
            {
                _item_size = value;
            }
        }
        private string _main_supcust;
        public string main_supcust
        {
            get
            {
                return _main_supcust;
            }
            set
            {
                _main_supcust = value;
            }
        }

        private string _change_price = "0";
        /// <summary>
        /// 是否可以议价
        /// </summary>
        public string change_price
        {
            get
            {
                return _change_price;
            }
            set
            {
                _change_price = value;
            }
        }
        private string _vip_acc_flag = "0";
        /// <summary>
        /// 是否可以积分
        /// </summary>
        public string vip_acc_flag
        {
            get
            {
                return _vip_acc_flag;
            }
            set
            {
                _vip_acc_flag = value;
            }
        }
        private decimal _vip_acc_num;
        /// <summary>
        /// 会员积分数量
        /// </summary>
        public decimal vip_acc_num
        {
            get
            {
                return _vip_acc_num;
            }
            set
            {
                _vip_acc_num = value;
            }
        }
        private string _scheme_price = "0";
        /// <summary>
        /// 是否可以打折
        /// </summary>
        public string scheme_price
        {
            get
            {
                return _scheme_price;
            }
            set
            {
                _scheme_price = value;
            }
        }
    }
}
