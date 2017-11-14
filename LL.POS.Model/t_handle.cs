namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 数据增量更新标识实体类
    /// </summary>
    public class t_handle
    {
        private int _handle_id;
        private Decimal _t_base_code;
        private Decimal _t_base_code_type;
        private Decimal _t_bd_item_combsplit;
        private Decimal _t_operator;
        private Decimal _t_product_food;
        private Decimal _t_product_food_kc;
        private Decimal _t_product_food_jg;
        private Decimal _t_product_food_barcode;
        private Decimal _t_product_food_type;
        private Decimal _t_rm_plan_detail;
        private Decimal _t_rm_plan_master;
        /// <summary>
        /// 标识ID，本表只有一条记录
        /// </summary>
        public int handle_id
        {
            get { return _handle_id; }
            set { _handle_id = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public Decimal t_base_code
        {
            get
            {
                return _t_base_code;
            }
            set
            {
                _t_base_code = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal t_base_code_type
        {
            get
            {
                return _t_base_code_type;
            }
            set
            {
                _t_base_code_type = value;
            }
        }
        /// <summary>
        /// 组合商品
        /// </summary>
        public Decimal t_bd_item_combsplit
        {
            get
            {
                return _t_bd_item_combsplit;
            }
            set
            {
                _t_bd_item_combsplit = value;
            }
        }
        /// <summary>
        /// 营业员
        /// </summary>
        public Decimal t_operator
        {
            get
            {
                return _t_operator;
            }
            set
            {
                _t_operator = value;
            }
        }
        /// <summary>
        /// 商品
        /// </summary>
        public Decimal t_product_food
        {
            get
            {
                return _t_product_food;
            }
            set
            {
                _t_product_food = value;
            }
        }
        /// <summary>
        /// 库存
        /// </summary>
        public Decimal t_product_food_kc
        {
            get
            {
                return _t_product_food_kc;
            }
            set
            {
                _t_product_food_kc = value;
            }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public Decimal t_product_food_jg
        {
            get
            {
                return _t_product_food_jg;
            }
            set
            {
                _t_product_food_jg = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal t_product_food_barcode
        {
            get
            {
                return _t_product_food_barcode;
            }
            set
            {
                _t_product_food_barcode = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal t_product_food_type
        {
            get
            {
                return _t_product_food_type;
            }
            set
            {
                _t_product_food_type = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal t_rm_plan_detail
        {
            get
            {
                return _t_rm_plan_detail;
            }
            set
            {
                _t_rm_plan_detail = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal t_rm_plan_master
        {
            get
            {
                return _t_rm_plan_master;
            }
            set
            {
                _t_rm_plan_master = value;
            }
        }


    }
}
