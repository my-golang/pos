
namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// t_base_code:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class t_base_code
    {
        public t_base_code()
        { }
        #region Model
        private string _type_no;
        private string _code_id;
        private string _code_name;
        private string _english_name;
        private string _code_type;
        private string _memo;
        /// <summary>
        /// 基本信息编码类型
        /// </summary>
        public string type_no
        {
            set { _type_no = value; }
            get { return _type_no; }
        }
        /// <summary>
        /// 基本信息编码标示
        /// </summary>
        public string code_id
        {
            set { _code_id = value; }
            get { return _code_id; }
        }
        /// <summary>
        /// 基本信息编码名称
        /// </summary>
        public string code_name
        {
            set { _code_name = value; }
            get { return _code_name; }
        }
        /// <summary>
        /// 基本信息英文名称
        /// </summary>
        public string english_name
        {
            set { _english_name = value; }
            get { return _english_name; }
        }
        /// <summary>
        /// 代码类型
        /// </summary>
        public string code_type
        {
            set { _code_type = value; }
            get { return _code_type; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string memo
        {
            set { _memo = value; }
            get { return _memo; }
        }
        #endregion Model
    }
}
