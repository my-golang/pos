namespace LL.POS.Model
{
    using System;
    /// <summary>
    /// 会员服务提供会员信息实体定义
    /// </summary>
    public class t_member_info
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public String mobile { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public String email { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public String mem_no { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String mem_pass { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public String mem_id { get; set; }
        /// <summary>
        /// 注册方式
        /// </summary>
        public String regtype { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public String username { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// 消费总额
        /// </summary>
        public Decimal total_consu { get; set; }
        /// <summary>
        /// 当前积分
        /// </summary>
        public Decimal score { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public String Level { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public String code { get; set; }
        /// <summary>
        /// info
        /// </summary>
        public String info { get; set; }
        /// <summary>
        /// time
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 会员是否绑定利澜会会员
        /// </summary>
        public String mem_type
        {
            get;
            set;
        }
    }
}
