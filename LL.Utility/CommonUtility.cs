
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Data;
namespace LL.Utility
{
    public class CommonUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static CommonUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private CommonUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static CommonUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new CommonUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region
        /// <summary>
        /// 根据进程名称杀掉进程
        /// </summary>
        /// <param name="processName"></param>
        public void KillProcesses(String processName)
        {
            Process[] processes = null;
            Process currentProcess = null;
            try
            {
                processes = Process.GetProcessesByName(processName);
                if (processes != null && processes.Length > 0)
                {
                    for (int index = 0; index < processes.Length; index++)
                    {
                        currentProcess = processes[index];
                        currentProcess.Kill();
                        currentProcess.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (currentProcess != null)
                {
                    currentProcess.Close();
                    currentProcess.Dispose();
                    currentProcess = null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public List<T> GetListByDbReader<T>(DbDataReader _reader)
        {
            List<T> tList = new List<T>();
            if (_reader != null && _reader.HasRows)
            {
                T t = default(T);
                int index = -1;
                try
                {
                    while (_reader.Read())
                    {
                        t = Activator.CreateInstance<T>();
                        Type type = t.GetType();
                        PropertyInfo[] pinfos = type.GetProperties();
                        DataTable table = _reader.GetSchemaTable();
                        if (table != null && table.Rows.Count > 0)
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                index = GetPropertyIndex(pinfos, dr["ColumnName"].ToString());
                                if (index >= 0)
                                {
                                    PropertyInfo pi = pinfos[index];
                                    String fieldName = pi.Name;
                                    switch (pi.PropertyType.Name.ToLower())
                                    {
                                        case "int":
                                        case "int32":
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToInt32(_reader[pi.Name.ToLower()]), null);
                                            break;
                                        case "decimal":
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToDecimal(_reader[pi.Name.ToLower()]), null);
                                            break;
                                        case "datetime":
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToDateTime(_reader[pi.Name.ToLower()]), null);
                                            break;
                                        default:
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToString(_reader[pi.Name.ToLower()]), null);
                                            break;
                                    }//switch (pi.PropertyType.Name.ToLower())
                                }//if (index > 0) 
                                index = -1;
                            }//foreach (DataColumn dc in table.Columns)
                        }//if (table != null && table.Rows.Count > 0)
                        tList.Add(t);
                    }//while (_reader.Read())
                }
                catch (DbException dbex)
                {
                    throw dbex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Dispose();
                        _reader.Close();
                    }
                }
            }
            return tList;
        }
        /// <summary>
        /// 获取去除重复datatable中指定列的值
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnName">指定进行去除重复的列</param>
        /// <returns>去除重复后的值</returns>
        public Dictionary<int, object> GetDistinctValue(DataTable table, string columnName)
        {
            Dictionary<int, object> _dic = new Dictionary<int, object>();
            try
            {
                if (table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Contains(columnName))
                    {
                        int index = 0;
                        foreach (DataRow dr in table.Rows)
                        {
                            foreach (DataColumn dc in table.Columns)
                            {
                                if (dc.ColumnName == columnName)
                                {
                                    string val = ExtendUtility.Instance.ParseToString(dr[dc]);
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        if (!_dic.ContainsValue(val))
                                        {
                                            _dic.Add(index, val);
                                            index++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _dic;
        }
        /// <summary>
        /// 将数据行转换成二维数据表
        /// </summary>
        /// <param name="drs">数据行数组</param>
        /// <returns>二维数据表</returns>
        public DataTable DataRowToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
            {
                DataRow dr = tmp.NewRow();
                foreach (DataColumn dc in tmp.Columns)
                {
                    dr[dc.ColumnName] = row[dc.ColumnName];
                }
                tmp.Rows.Add(dr);  // 将DataRow添加到DataTable中
            }
            return tmp;
        }
        /// <summary>
        /// 泛型对象中是否含有键的值
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="_tList">泛型对象列表</param>
        /// <param name="field">键</param>
        /// <param name="value">值</param>
        /// <returns>是否含有</returns>
        public int GetFieldIndex<T>(List<T> _tList, string field, object value)
        {
            int rindex = -1;
            try
            {
                for (int index = 0; index < _tList.Count; index++)
                {
                    T t = _tList[index];
                    Type type = t.GetType();
                    PropertyInfo[] pinfos = type.GetProperties();
                    foreach (PropertyInfo pi in pinfos)
                    {
                        if (pi.Name == field)
                        {
                            if (pi.GetValue(t, null).Equals(value))
                            {
                                rindex = index;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rindex;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 返回属性在属性数组中的索引
        /// </summary>
        /// <param name="pinfos">属性数组</param>
        /// <param name="pname">搜索的属性名称</param>
        /// <returns>-1未找到否则索引</returns>
        int GetPropertyIndex(PropertyInfo[] pinfos, String pname)
        {
            int index = -1;
            for (int rindex = 0; rindex < pinfos.Length; rindex++)
            {
                if (pinfos[rindex].Name.ToLower() == pname.ToLower())
                {
                    index = rindex;
                    break;
                }
            }
            return index;
        }
        #endregion
    }
}
