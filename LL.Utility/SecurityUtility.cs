

namespace LL.Utility
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    /// <summary>
    /// 安全工具类
    /// </summary>
    public class SecurityUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static SecurityUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private SecurityUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static SecurityUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new SecurityUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="input">要加密的字符串</param>
        /// <returns></returns>
        public  String GetMD5Hash(String input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        #endregion
    }
}
