
/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-07-25
 * Description:POS促销规则参数实体
 * Modify: 
 * ******************************************************/
using System.Collections.Generic;
namespace LL.POS.Model
{
    /// <summary>
    /// 促销规则参数
    /// </summary>
    public class PlanPara
    {
        /// <summary>
        /// 促销规则编号
        /// </summary>
        private string _planNo = "";
        /// <summary>
        /// 促销规则详细编号
        /// </summary>
        private string _row_id = "";
        /// <summary>
        /// 规则类型:ALL/CLS/BRAND/ITEM
        /// </summary>
        private string _typeNo = "ALL";
        /// <summary>
        /// 是否是组合判断
        /// </summary>
        private bool _isN = false;
        /// <summary>
        /// 是否存在合并运算
        /// </summary>
        private bool _isAnd = false;
        private string _typeValue = "";
        /// <summary>
        /// 类型的值
        /// </summary>
        public string typeValue
        {
            get { return _typeValue; }
            set { _typeValue = value; }
        }
        /// <summary>
        /// 促销编号
        /// </summary>
        public string planNo
        {
            get { return _planNo; }
            set { _planNo = value; }
        }
        /// <summary>
        /// 促销规则详细编号
        /// </summary>
        public string row_id
        {
            get { return _row_id; }
            set { _row_id = value; }
        }
        /// <summary>
        /// 规则类型
        /// </summary>
        public string typeNo
        {
            get { return _typeNo; }
            set { _typeNo = value; }
        }
        /// <summary>
        /// 是否有多个判断条件
        /// </summary>
        public bool isAnd
        {
            get { return _isAnd; }
            set { _isAnd = value; }
        }
        /// <summary>
        /// 是否组合商品
        /// </summary>
        public bool isN
        {
            get { return _isN; }
            set { _isN = value; }
        }
        private List<CheckPara> _checkParas;
        /// <summary>
        /// 校验的数据
        /// </summary>
        public List<CheckPara> CheckParas
        {
            get { return _checkParas; }
            set { _checkParas = value; }
        }
        private List<CheckPara> _resultParas;
        /// <summary>
        /// 计算结果的参数
        /// </summary>
        public List<CheckPara> ResultParas
        {
            get { return _resultParas; }
            set { _resultParas = value; }
        }
        public List<CheckPara> _addPara;
        /// <summary>
        /// 附加参数列表
        /// </summary>
        public List<CheckPara> AddPara
        {
            get { return _addPara; }
            set { _addPara = value; }
        }
        private PlanRule _planRule;
        /// <summary>
        /// 使用的规则
        /// </summary>
        public PlanRule PlanRule
        {
            get
            {
                return _planRule;
            }
            set {
                _planRule = value;
            }
        }
    }
}
