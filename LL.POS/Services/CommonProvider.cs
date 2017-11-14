using System;
using System.Collections.Generic;
using LL.POS.Common;

namespace LL.POS
{
    /// <summary>
    /// 统一接口访问入口
    /// </summary>
    public class CommonProvider
    {
        /// <summary>
        /// 访问API服务接口
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_dic"></param>
        /// <returns></returns>
        public static String InvokePOSAPI(String action, Dictionary<String, Object> _dic)
        {
            String result = string.Empty;
            String errorMessage = string.Empty;
            bool isok = true;
            result = PServiceProvider.Instance.InvokeMethod(Gattr.serverUrl + "/" + action, _dic, ref isok, ref errorMessage);
            if (errorMessage.Length > 0)
            {
                LoggerHelper.Log(Gattr.LoggerName, "LL.POS.CommonProvider-->Error:" + errorMessage, LogEnum.ExceptionLog);
            }
            return result;
        }
    }
}
