

namespace LL.POS.Model
{
    using System;
    [Serializable]
    public partial class t_item_info
    {
        public t_item_info()
		{}
		#region Model
		private string _item_no;
		private string _item_subno="0";
		private string _item_name;
		private string _item_subname;
		private string _item_clsno="LB";
		private string _item_brand="PP";
		private string _item_brandname="PM";
		private string _unit_no;
		private string _item_size;
		private string _product_area;
		private decimal _price=0M;
		private decimal _base_price=0M;
		private decimal _sale_price=0M;
		private string _combine_sta="0";
		private string _status="1";
		private string _display_flag="1";
		private string _other1;
		private string _other2;
		private string _other3;
		private decimal? _num1=0M;
		private decimal? _num2=0M;
		private decimal? _num3=0M;
		private int? _po_cycle=0;
		private int? _so_cycle=0;
		private string _automin_flag="1";
		private string _en_dis="0";
		private string _direct="2";
		private string _change_price="0";
		private decimal? _purchase_tax=0.17M;
		private decimal? _sale_tax=0.17M;
		private decimal? _purchase_spec=1M;
		private decimal? _shipment_spec=1M;
		private string _item_supcust;
		private string _main_supcust="0";
		private decimal? _lose_rate=0M;
		private string _item_sup_flag="A";
		private string _item_stock="0";
		private string _item_counter="9999";
		private decimal? _sup_ly_rate=0M;
		private DateTime _build_date;
		private DateTime? _modify_date;
		private DateTime? _stop_date;
		private decimal? _return_rate=0M;
		private string _abc="C";
		private string _branch_price="0";
		private string _item_rem;
		private decimal? _vip_price=0M;
		private decimal? _sale_min_price=0M;
		private string _branch_no;
		private string _cost_compute="0";
		private string _com_flag="0";
		private decimal? _base_price1=0M;
		private decimal? _base_price2=0M;
		private decimal? _base_price3=0M;
		private string _vip_acc_flag="1";
		private decimal? _vip_acc_num=0M;
		private string _dpfm_type="0";
		private decimal? _trans_price=0M;
		private decimal? _vip_price1=0M;
		private decimal? _vip_price2=0M;
		private decimal? _base_price4=0M;
		private string _build_man;
		private string _last_modi_man;
		private decimal? _order_man_rate=0M;
		private string _om_rate_type;
		private string _is_high="0";
		private string _is_focus="0";
		/// <summary>
		/// 
		/// </summary>
		public string item_no
		{
			set{ _item_no=value;}
			get{return _item_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_subno
		{
			set{ _item_subno=value;}
			get{return _item_subno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_name
		{
			set{ _item_name=value;}
			get{return _item_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_subname
		{
			set{ _item_subname=value;}
			get{return _item_subname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_clsno
		{
			set{ _item_clsno=value;}
			get{return _item_clsno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_brand
		{
			set{ _item_brand=value;}
			get{return _item_brand;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_brandname
		{
			set{ _item_brandname=value;}
			get{return _item_brandname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string unit_no
		{
			set{ _unit_no=value;}
			get{return _unit_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_size
		{
			set{ _item_size=value;}
			get{return _item_size;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string product_area
		{
			set{ _product_area=value;}
			get{return _product_area;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal base_price
		{
			set{ _base_price=value;}
			get{return _base_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal sale_price
		{
			set{ _sale_price=value;}
			get{return _sale_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string combine_sta
		{
			set{ _combine_sta=value;}
			get{return _combine_sta;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string display_flag
		{
			set{ _display_flag=value;}
			get{return _display_flag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string other1
		{
			set{ _other1=value;}
			get{return _other1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string other2
		{
			set{ _other2=value;}
			get{return _other2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string other3
		{
			set{ _other3=value;}
			get{return _other3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? num1
		{
			set{ _num1=value;}
			get{return _num1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? num2
		{
			set{ _num2=value;}
			get{return _num2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? num3
		{
			set{ _num3=value;}
			get{return _num3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? po_cycle
		{
			set{ _po_cycle=value;}
			get{return _po_cycle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? so_cycle
		{
			set{ _so_cycle=value;}
			get{return _so_cycle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string automin_flag
		{
			set{ _automin_flag=value;}
			get{return _automin_flag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string en_dis
		{
			set{ _en_dis=value;}
			get{return _en_dis;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string direct
		{
			set{ _direct=value;}
			get{return _direct;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string change_price
		{
			set{ _change_price=value;}
			get{return _change_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? purchase_tax
		{
			set{ _purchase_tax=value;}
			get{return _purchase_tax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? sale_tax
		{
			set{ _sale_tax=value;}
			get{return _sale_tax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? purchase_spec
		{
			set{ _purchase_spec=value;}
			get{return _purchase_spec;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? shipment_spec
		{
			set{ _shipment_spec=value;}
			get{return _shipment_spec;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_supcust
		{
			set{ _item_supcust=value;}
			get{return _item_supcust;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string main_supcust
		{
			set{ _main_supcust=value;}
			get{return _main_supcust;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? lose_rate
		{
			set{ _lose_rate=value;}
			get{return _lose_rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_sup_flag
		{
			set{ _item_sup_flag=value;}
			get{return _item_sup_flag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_stock
		{
			set{ _item_stock=value;}
			get{return _item_stock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_counter
		{
			set{ _item_counter=value;}
			get{return _item_counter;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? sup_ly_rate
		{
			set{ _sup_ly_rate=value;}
			get{return _sup_ly_rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime build_date
		{
			set{ _build_date=value;}
			get{return _build_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? modify_date
		{
			set{ _modify_date=value;}
			get{return _modify_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? stop_date
		{
			set{ _stop_date=value;}
			get{return _stop_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? return_rate
		{
			set{ _return_rate=value;}
			get{return _return_rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string abc
		{
			set{ _abc=value;}
			get{return _abc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string branch_price
		{
			set{ _branch_price=value;}
			get{return _branch_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_rem
		{
			set{ _item_rem=value;}
			get{return _item_rem;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? vip_price
		{
			set{ _vip_price=value;}
			get{return _vip_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? sale_min_price
		{
			set{ _sale_min_price=value;}
			get{return _sale_min_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string branch_no
		{
			set{ _branch_no=value;}
			get{return _branch_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cost_compute
		{
			set{ _cost_compute=value;}
			get{return _cost_compute;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string com_flag
		{
			set{ _com_flag=value;}
			get{return _com_flag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? base_price1
		{
			set{ _base_price1=value;}
			get{return _base_price1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? base_price2
		{
			set{ _base_price2=value;}
			get{return _base_price2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? base_price3
		{
			set{ _base_price3=value;}
			get{return _base_price3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string vip_acc_flag
		{
			set{ _vip_acc_flag=value;}
			get{return _vip_acc_flag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? vip_acc_num
		{
			set{ _vip_acc_num=value;}
			get{return _vip_acc_num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dpfm_type
		{
			set{ _dpfm_type=value;}
			get{return _dpfm_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? trans_price
		{
			set{ _trans_price=value;}
			get{return _trans_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? vip_price1
		{
			set{ _vip_price1=value;}
			get{return _vip_price1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? vip_price2
		{
			set{ _vip_price2=value;}
			get{return _vip_price2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? base_price4
		{
			set{ _base_price4=value;}
			get{return _base_price4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string build_man
		{
			set{ _build_man=value;}
			get{return _build_man;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string last_modi_man
		{
			set{ _last_modi_man=value;}
			get{return _last_modi_man;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? order_man_rate
		{
			set{ _order_man_rate=value;}
			get{return _order_man_rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string om_rate_type
		{
			set{ _om_rate_type=value;}
			get{return _om_rate_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string is_high
		{
			set{ _is_high=value;}
			get{return _is_high;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string is_focus
		{
			set{ _is_focus=value;}
			get{return _is_focus;}
		}
		#endregion Model
    }
}
