namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LL.POS.Common;
    using LL.POS.Model;
    using LL.Utility;
    /// <summary>
    /// 会员接口服务
    /// </summary>
    public class MemberService
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static MemberService _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private MemberService()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static MemberService Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new MemberService();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 准备参数，涉及到：会员服务key生成规则
        /// </summary>
        /// <param name="action">方法</param>
        /// <param name="_dic">键值</param>
        /// <returns></returns>
        private String PrepareParameters(String action, Dictionary<String, String> _dic)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            sb.AppendFormat("?job={0}&", action);
            sb.AppendFormat("way={0}&", Gattr.ServiceWay);
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            String time = Convert.ToInt64(ts.TotalSeconds).ToString();
            sb.AppendFormat("time={0}&", time);
            String[] keys = new String[_dic.Count + 2];
            int index = 0;
            foreach (KeyValuePair<String, String> kv in _dic)
            {
                keys[index] = kv.Key;
                sb.AppendFormat("{0}={1}&", kv.Key, kv.Value);
                index++;
            }
            keys[_dic.Count] = "way";
            keys[_dic.Count + 1] = "time";
            String key = String.Empty;
            String[] keys1 = StringUtility.Instance.AsiicAsc(keys);
            for (int rindex = 0; rindex < keys1.Length; rindex++)
            {
                if (_dic.ContainsKey(keys1[rindex]))
                    sb1.AppendFormat("{0}={1}&", keys1[rindex], _dic[keys1[rindex]]);
                else
                {
                    if (keys1[rindex] == "way")
                    {
                        sb1.AppendFormat("way={0}&", Gattr.ServiceWay);
                    }
                    else
                    {
                        sb1.AppendFormat("time={0}&", time);
                    }
                }
            }
            sb1.AppendFormat("key={0}", Gattr.UseToken);
            key = SecurityUtility.Instance.GetMD5Hash(sb1.ToString()).ToLower();
            sb.AppendFormat("key={0}", key);
            return sb.ToString();
        }
        /// <summary>
        /// 根据会员账号获取会员信息 get_mem_info
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <returns></returns>
        public t_member_info GetMemberInfoByMemNo(String mem_no)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            t = InvokeMemberService("get_mem_info", _dic);
            return t;
        }
        /// <summary>
        /// 使用会员卡金额付款
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="money">扣减金额</param>
        /// <param name="ordername">关联订单号</param>
        /// <param name="memo">备注</param>
        /// <returns></returns>
        public t_member_info UseMemberCardPayNew(String mem_no, String money, String ordername, String vercode, String memo)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("money", SIString.TryStr(money));
            _dic.Add("ordername", ordername);
            _dic.Add("else", System.Web.HttpUtility.UrlEncode(memo).ToUpper());
            _dic.Add("vercode", vercode);
            t = InvokeMemberService("deduction", _dic);
            if (t.code == "1")
            {
                //t = AddMemberScore(mem_no, money, ordername);
                if (t.code == "1")
                {
                    t = GetMemberInfoByMemNo(mem_no);
                }
            }
            return t;
        }
        /// <summary>
        /// 获取支付使用的验证码
        /// </summary>
        /// <param name="mem_no">会员编号</param>
        /// <returns></returns>
        public t_member_info GetValidateCode(String mem_no)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            t = InvokeMemberService("get_consumer_vercode", _dic);
            return t;
        }
        /// <summary>
        /// 检测当前会员是否绑定利澜会会员
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <returns></returns>
        public t_member_info CheckMemberinfoForLL(String mem_no)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("type", "WEIXIN");
            t = InvokeMemberService("check_member_istoken", _dic);
            return t;
        }
        /// <summary>
        /// 根据会员账号、支付密码获取会员信息
        /// </summary>
        /// <param name="mem_no"></param>
        /// <param name="mem_pass"></param>
        /// <returns></returns>
        public t_member_info GetMemberInfoByMemNoAndPass(String mem_no, String mem_pass)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("mem_pass", mem_pass);
            t = InvokeMemberService("login", _dic);
            if (t.code == "1")
            {
                t = GetMemberInfoByMemNo(mem_no);
            }
            return t;
        }
        /// <summary>
        /// 根据会员卡号、原始支付密码、新支付密码修改支付密码
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="mem_pass">旧密码</param>
        /// <param name="mem_new_pass">新密码</param>
        /// <returns></returns>
        public t_member_info UpdateMemPayPass(String mem_no, String mem_pass, String mem_new_pass)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("mem_pass", mem_pass);
            _dic.Add("mem_new_pass", mem_new_pass);
            t = InvokeMemberService("passwordchange", _dic);
            if (t.code == "1")
            {
                t = GetMemberInfoByMemNo(mem_no);
            }
            return t;
        }
        /// <summary>
        /// 根据会员卡号、新支付密码重置支付密码
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="mem_new_pass">新密码</param>
        /// <returns></returns>
        public t_member_info ResetPayPass(String mem_no, String mem_new_pass)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("mem_new_pass", mem_new_pass);
            t = InvokeMemberService("password_reset", _dic);
            if (t.code == "1")
            {
                t = GetMemberInfoByMemNo(mem_no);
            }
            return t;
        }
        /// <summary>
        /// 增加会员积分
        /// </summary>
        /// <param name="mem_no">会员号</param>
        /// <param name="score">增加积分</param>
        /// <param name="ordername">关联订单号</param>
        /// <returns></returns>
        public t_member_info AddMemberScore(String mem_no, String score, String ordername, String el = "")
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("else", System.Web.HttpUtility.UrlEncode(el).ToUpper());
            _dic.Add("mem_no", mem_no);
            _dic.Add("score", score);
            _dic.Add("ordername", ordername);
            t = InvokeMemberService("addscore", _dic);
            if (t.code == "1")
            {
                t = GetMemberInfoByMemNo(mem_no);
            }
            return t;
        }
        /// <summary>
        /// 减少会员积分
        /// </summary>
        /// <param name="mem_no">会员号</param>
        /// <param name="score">增加积分</param>
        /// <param name="ordername">关联订单号</param>
        /// <returns></returns>
        public t_member_info UseMemberScore(String mem_no, String score, String ordername, String el = "")
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("else", System.Web.HttpUtility.UrlEncode(el).ToUpper());
            _dic.Add("mem_no", mem_no);
            _dic.Add("score", score);
            _dic.Add("ordername", ordername);
            t = InvokeMemberService("usescore", _dic);
            if (t.code == "1")
            {
                t = GetMemberInfoByMemNo(mem_no);
            }
            return t;
        }
        /// <summary>
        /// 使用会员卡金额付款
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="money">扣减金额</param>
        /// <param name="ordername">关联订单号</param>
        /// <param name="memo">备注</param>
        /// <returns></returns>
        public t_member_info UseMemberCardPay(String mem_no, String money, String ordername, String memo)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("money", SIString.TryStr(money));
            _dic.Add("ordername", ordername);
            _dic.Add("else", memo);
            t = InvokeMemberService("deduction", _dic);
            if (t.code == "1")
            {
                t = AddMemberScore(mem_no, money, ordername);
                if (t.code == "1")
                {
                    t = GetMemberInfoByMemNo(mem_no);
                }
            }
            return t;
        }
        /// <summary>
        /// 会员充值第一步，先充值到第三方中
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="money">充值金额</param>
        /// <param name="ordername">订单号</param>
        /// <param name="paytype">支付方式</param>
        /// <returns>是否成功</returns>
        public t_member_info RechargeMoney(String mem_no, String money, String ordername, String paytype)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("money", money);
            _dic.Add("ordername", ordername);
            _dic.Add("paytype", paytype);
            t = InvokeMemberService("memb_recharge_order_add", _dic);
            return t;
        }
        /// <summary>
        /// 充值完成
        /// </summary>
        /// <param name="mem_no">会员卡号</param>
        /// <param name="ordername">订单号</param>
        /// <returns></returns>
        public t_member_info RechargeMoneyFinsh(String mem_no, String ordername)
        {
            Dictionary<String, String> _dic = new Dictionary<string, string>();
            t_member_info t = null;
            _dic.Add("mem_no", mem_no);
            _dic.Add("ordername", ordername);
            t = InvokeMemberService("memb_recharge_order_finish", _dic);
            return t;
        }
        /// <summary>
        /// 根据要调用的方法和参数执行会员服务方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="_dic"></param>
        /// <returns></returns>
        private t_member_info InvokeMemberService(String method, Dictionary<String, String> _dic)
        {
            String parameter = String.Empty;
            String address = String.Empty;
            String response = String.Empty;
            t_member_info t = null;
            try
            {
                parameter = PrepareParameters(method, _dic);
                address = Gattr.MemberServiceUri + parameter;
                response = WebUtility.Instance.GetHttpWebResponse(address);
                if (response != "404")
                {
                    t = JsonUtility.Instance.JsonToObject<t_member_info>(response);
                    if (t == null)
                    {
                        t = new t_member_info() { code = "-1", info = response };
                    }
                }
                else
                {
                    t = new t_member_info() { code = "404", info = "远程服务器返回异常" };
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS->Services-->Memberserices-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return t;
        }
        #endregion
    }
}
