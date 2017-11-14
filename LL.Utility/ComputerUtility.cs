namespace LL.Utility
{
    using System;
    using System.Management;
    using LL.Common;
    using System.Collections.Generic;
    /// <summary>
    /// 计算机信息工具类
    /// </summary>
    public class ComputerUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static ComputerUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ComputerUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static ComputerUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new ComputerUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 获取网卡
        /// </summary>
        /// <returns></returns>
        public String GetMacAddress()
        {
            String mac = "";
            ManagementObjectCollection moc = null;
            ManagementClass mc = null;
            try
            {
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                return mac;
            }
            catch
            {
                return GobalStaticString.UN_KNOWN_MAC;
            }
            finally
            {
                if (moc != null)
                {
                    moc.Dispose();
                }
                if (mc != null)
                {
                    mc.Dispose();
                }
            }
        }
        /// <summary>
        /// 获取硬盘码
        /// </summary>
        /// <returns></returns>
        public String GetDiskVolumeSerialNumber()
        {
            String disk = string.Empty;
            ManagementClass mc = null;
            ManagementObjectCollection moc = null;
            try
            {
                mc = new ManagementClass("Win32_DiskDrive");
                moc = mc.GetInstances();
                Dictionary<string, object> _dic = new Dictionary<string, object>();
                foreach (ManagementObject mo in moc)
                {
                    //HDid = (string)mo.Properties["SerialNumber"].Value;//SerialNumber在Win7以上系统有效
                    //PNPDeviceID:取\之后的值
                    foreach (PropertyData pd in mo.Properties)
                    {
                        if (!_dic.ContainsKey(pd.Name))
                        {
                            _dic.Add(pd.Name, pd.Value);
                        }
                    }
                }
                foreach (KeyValuePair<string, object> kv in _dic)
                {
                    if (kv.Key == "Signature")
                    {
                        disk = kv.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return GobalStaticString.UN_KNOWN_DISK;
            }
            finally
            {
                if (moc != null)
                {
                    moc.Dispose();
                }
                if (mc != null)
                {
                    mc.Dispose();
                }
            }
            return disk;
        }

        /// <summary>
        /// 获取硬盘码
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> GetDiskVolumeSerialNumber11()
        {
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            ManagementClass mc = null;
            ManagementObjectCollection moc = null;
            try
            {
                mc = new ManagementClass("Win32_DiskDrive");
                moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    foreach (PropertyData pd in mo.Properties)
                    {
                        if (!_dic.ContainsKey(pd.Name))
                        {
                            _dic.Add(pd.Name, pd.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return _dic;
            }
            finally
            {
                if (moc != null)
                {
                    moc.Dispose();
                }
                if (mc != null)
                {
                    mc.Dispose();
                }
            }
            return _dic;
        }

        public Dictionary<string, string> GetPrinterName()
        {
            string l_strSQL = string.Format("SELECT * from Win32_Printer where default = true ");
            Dictionary<string, string> _dic = new Dictionary<string, string>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(l_strSQL);
                ManagementObjectCollection printers = searcher.Get();
                foreach (ManagementObject print in printers)
                {
                    if (!_dic.ContainsKey(print["PortName"].ToString()))
                    {
                        _dic.Add(print["PortName"].ToString(), print["DriverName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return _dic;
            }
            return _dic;
        }
        #endregion
    }
}
