namespace LL.Utility
{
    using System;
    using System.Configuration;
    public class AppSettingUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static AppSettingUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private AppSettingUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static AppSettingUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new AppSettingUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共函数
        /// <summary>
        /// 获取AppSetting中的值
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public String GetApplicationSettingValue(String appKey)
        {
            return ConfigurationManager.AppSettings[appKey];
        }
        #endregion
    }
}
