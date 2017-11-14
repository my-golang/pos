using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using LL.POS.Common;
using LL.Utility;
using LL.POS.Model;

namespace LL.POS.Transfer
{
    /// <summary>
    /// 同步处理器
    /// </summary>
    public class SyncProcessor
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static SyncProcessor _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private SyncProcessor()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static SyncProcessor Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new SyncProcessor();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 同步会员消费信息
        /// </summary>
        /// <returns></returns>
        public bool SyncMember(DataTable table)
        {
            bool isok = true;
            try
            {
                
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transfer->SyncProcessor-->Exception:" + ex.ToString(),
                     LogEnum.ExceptionLog);
            }
            return isok;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 根据要调用的方法和参数执行会员服务方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="_dic"></param>
        /// <returns></returns>
        public t_member_info InvokeMemberService(String method, Dictionary<String, String> _dic)
        {
            String parameter = String.Empty;
            String address = String.Empty;
            String response = String.Empty;
            t_member_info t = null;
            try
            {
                parameter = MemberServicesProvider.Instance.PrepareParameters(method, GlobalSet.access_way, GlobalSet.mem_access_token, _dic);
                address = GlobalSet.memberUrl + parameter;
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
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transfer->SyncProcessor-->InvokeMemberService-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return t;
        }
        #endregion
    }
}
