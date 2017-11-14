/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-07-25
 * Description:POS促销帮助类
 * Modify:
 * ******************************************************/
namespace LL.POS.Model
{
    using System;
    using System.Collections.Generic;
    public class t_rm_plan_master
    {
        private string _plan_no;
        /// <summary>
        /// 方案编号
        /// </summary>
        public string plan_no
        {
            get { return _plan_no; }
            set { _plan_no = value; }
        }
        private string _plan_name;
        /// <summary>
        /// 方案名称
        /// </summary>
        public string plan_name
        {
            get { return _plan_name; }
            set { _plan_name = value; }
        }
        private string _plan_memo;
        /// <summary>
        /// 方案介绍
        /// </summary>
        public string plan_memo
        {
            get { return _plan_memo; }
            set { _plan_memo = value; }
        }
        private DateTime _begin_date;
        /// <summary>
        /// 方案开始时间
        /// </summary>
        public DateTime begin_date
        {
            get { return _begin_date; }
            set { _begin_date = value; }
        }
        private DateTime _end_date;
        /// <summary>
        /// 方案结束时间
        /// </summary>
        public DateTime end_date
        {
            get { return _end_date; }
            set { _end_date = value; }
        }
        private string _week;
        /// <summary>
        /// 方案实施星期
        /// </summary>
        public string week
        {
            get { return _week; }
            set { _week = value; }
        }
        private string _vip_type;
        /// <summary>
        /// 方案对应会员等级
        /// </summary>
        public string vip_type
        {
            get { return _vip_type; }
            set { _vip_type = value; }
        }
        private DateTime _stop_date;
        /// <summary>
        /// 方案终止日期
        /// </summary>
        public DateTime stop_date
        {
            get { return _stop_date; }
            set { _stop_date = value; }
        }
        private string _stop_man;
        /// <summary>
        /// 方案终止人
        /// </summary>
        public string stop_man
        {
            get { return _stop_man; }
            set { _stop_man = value; }
        }
        private string _approve_flag;
        /// <summary>
        /// 方案审核状态
        /// </summary>
        public string approve_flag
        {
            get { return _approve_flag; }
            set { _approve_flag = value; }
        }
        private string _rule_no;
        /// <summary>
        /// 方案对应规则编号
        /// </summary>
        public string rule_no
        {
            get { return _rule_no; }
            set { _rule_no = value; }
        }
        private string _range_flag;
        /// <summary>
        /// 方案对应规则使用范围
        /// </summary>
        public string range_flag
        {
            get { return _range_flag; }
            set { _range_flag = value; }
        }
        private string _rule_describe;
        /// <summary>
        /// 方案对应规则描述信息
        /// </summary>
        public string rule_describe
        {
            get { return _rule_describe; }
            set { _rule_describe = value; }
        }
        private string _rule_condition;
        /// <summary>
        /// 方案对应规则验证条件描述
        /// </summary>
        public string rule_condition
        {
            get { return _rule_condition; }
            set { _rule_condition = value; }
        }
        private string _rule_result;
        /// <summary>
        /// 方案对应规则计算规则
        /// </summary>
        public string rule_result
        {
            get { return _rule_result; }
            set { _rule_result = value; }
        }
        private string _plu_flag;
        /// <summary>
        /// 方案对应规则是否在付款时提示1是0不是
        /// </summary>
        public string plu_flag
        {
            get { return _plu_flag; }
            set { _plu_flag = value; }
        }
        private string _rule_desc;
        /// <summary>
        /// 方案对应规则具体详细信息描述
        /// </summary>
        public string rule_desc
        {
            get { return _rule_desc; }
            set { _rule_desc = value; }
        }
        private PlanPara _planPara;
        /// <summary>
        /// 方案对应的参数对象
        /// </summary>
        public PlanPara PlanPara
        {
            get { return _planPara; }
            set { _planPara = value; }
        }

        private bool _isSelect = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isSelect
        {
            get { return _isSelect; }
            set { _isSelect = value; }
        }
    }
}
