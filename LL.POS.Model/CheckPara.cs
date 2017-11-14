
/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-07-25
 * Description:POS促销规则校验参数实体
 * Modify: 
 * ******************************************************/
namespace LL.POS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// 校验参数
    /// </summary>
    public class CheckPara
    {
        private string _check_type = "ALL";
        /// <summary>
        /// 校验类型 BRAND CLS ITEM
        /// </summary>
        public string check_type
        {
            get { return _check_type; }
            set { _check_type = value; }
        }
        private string _type_val = "";
        /// <summary>
        /// 校验时类型的值
        /// </summary>
        public string type_val
        {
            get { return _type_val; }
            set { _type_val = value; }
        }
        private string _check_operator = "";
        /// <summary>
        /// 校验时使用的操作符
        /// </summary>
        public string check_operator
        {
            get { return _check_operator; }
            set { _check_operator = value; }
        }
        private decimal _throld = 0M;
        /// <summary>
        /// 校验时验证的值
        /// </summary>
        public decimal throld
        {
            get { return _throld; }
            set { _throld = value; }
        }
        private bool _isAmt = false;
        /// <summary>
        /// 校验时是否校验金额
        /// </summary>
        public bool isAmt
        {
            get { return _isAmt; }
            set { _isAmt = value; }
        }
        private bool _isQty = false;
        /// <summary>
        /// 校验时是否校验数量
        /// </summary>
        public bool isQty
        {
            get { return _isQty; }
            set { _isQty = value; }
        }
    }
}
