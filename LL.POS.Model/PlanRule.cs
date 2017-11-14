
/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-07-25
 * Description:POS促销规则实体
 * Modify: 
 * ******************************************************/
namespace LL.POS.Model
{
    /// <summary>
    /// 促销规则定义实体
    /// </summary>
    public class PlanRule
    {
        /// <summary>
        /// 规则编号
        /// </summary>
        private string _ruleno;
        /// <summary>
        /// 规则使用范围
        /// </summary>
        private string _range_flag;
        /// <summary>
        /// 规则使用条件
        /// </summary>
        private string _rule_conditon;
        /// <summary>
        /// 规则计算结果
        /// </summary>
        private string _rule_result;
        /// <summary>
        /// 促销规则编号
        /// </summary>
        public string rule_no
        {
            get { return _ruleno; }
            set { _ruleno = value; }
        }
        /// <summary>
        /// 促销使用范围
        /// </summary>
        public string range_flag
        {
            get { return _range_flag; }
            set { _range_flag = value; }
        }
        /// <summary>
        /// 促销使用条件
        /// </summary>
        public string rule_condition
        {
            get { return _rule_conditon; }
            set { _rule_conditon = value; }
        }
        /// <summary>
        /// 促销计算结果
        /// </summary>
        public string rule_result
        {
            get { return _rule_result; }
            set { _rule_result = value; }
        }
    }
}
