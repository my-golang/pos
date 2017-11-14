namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// POS机设置
    /// </summary>
    [Serializable]
    public class t_sys_pos_set
    {
        /// <summary>
        /// 设置内容
        /// </summary>
        public String sys_var_id { get; set; }
        /// <summary>
        /// 设置值
        /// </summary>
        public String sys_var_value { get; set; }
    }
}
