/********************************************************
 * Company:LL
 * Author:yanhua
 * Date:2014-07-25
 * Description:NET访问Mysql的数据访问工具
 * Modify:
 * ******************************************************/
namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using MySql.Data.MySqlClient;
    /// <summary>
    /// NET访问Mysql的数据访问工具
    /// </summary>
    public class DbUtilityMySql : IDBUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static DbUtilityMySql _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DbUtilityMySql()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static DbUtilityMySql Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new DbUtilityMySql();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        /// <summary>
        /// 根据字符串测试数据库访问连接
        /// </summary>
        /// <param name="connectString"></param>
        /// <returns></returns>
        public bool TestConnect(string connectString)
        {
            bool isConnect = true;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectString);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception)
            {
                isConnect = false;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return isConnect;
        }
        /// <summary>
        /// 执行一条SQL语句
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="errorMessage">执行返回的错误信息</param>
        /// <returns>成功返回1失败返回0</returns>
        public int ExecuteSql(string connectString, string sql, DbParameter[] parameters, ref string errorMessage)
        {
            int result = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                conn = new MySqlConnection(connectString);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new MySqlCommand(sql, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (MySqlException mysql)
            {
                errorMessage = mysql.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }
        /// <summary>
        ///返回SQL语句执行后第一行第一列的值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="errorMessage">执行返回的错误信息</param>
        /// <returns>返回object数据默认为null</returns>
        public object GetSingleData(string connectionString, string sql, DbParameter[] parameters, ref string errorMessage)
        {
            object obj = null;
            MySqlCommand cmd = null;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                obj = cmd.ExecuteScalar();
            }
            catch (MySqlException mysql)
            {
                errorMessage = mysql.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 返回SQL执行的泛型数据对象
        /// </summary>
        /// <typeparam name="T">返回的泛型定义</typeparam>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="errorMessage">执行返回的错误信息</param>
        /// <returns>返回泛型列表，默认为null</returns>
        public List<T> GetDataList<T>(string connectionString, string sql, DbParameter[] parameters, ref string errorMessage)
        {
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;
            MySqlConnection conn = null;
            List<T> tList = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                reader = cmd.ExecuteReader();
                tList = GetModelList<T>(reader);
            }
            catch (MySqlException mysql)
            {
                errorMessage = mysql.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader.Close();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return tList;
        }
        /// <summary>
        /// 返回SQL执行的数据源
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="errorMessage">执行返回的错误信息</param>
        /// <returns>返回数据表</returns>
        public DataTable GetDataTable(string connectionString, string sql, DbParameter[] parameters, ref string errorMessage)
        {
            DataTable table = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter da = null;
            DataSet ds = null;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                ds = new DataSet();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }
            catch (MySqlException mysql)
            {
                errorMessage = mysql.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return table;
        }
        /// <summary>
        /// 使用事物执行一组SQL语句
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="_dicParameters">SQL数组</param>
        /// <param name="errorMessage">执行时错误信息</param>
        /// <returns>成功返回1失败返回0</returns>
        public int ExecuteSqlsByTrans(string connectionString, List<SqlParaEntity> _dicParameters, ref string errorMessage)
        {
            int result = 1;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlTransaction trans = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                trans = conn.BeginTransaction();
                foreach (SqlParaEntity kv in _dicParameters)
                {
                    cmd = new MySqlCommand(kv.Sql, conn);
                    cmd.Parameters.Clear();
                    if (kv.parameters != null && kv.parameters.Length > 0)
                        cmd.Parameters.AddRange(kv.parameters);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (MySqlException mysql)
            {
                errorMessage = mysql.ToString();
                trans.Rollback();
                result = 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                trans.Rollback();
                result = 0;
            }
            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 根据数据读取返回制定数据实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<T> GetModelList<T>(MySqlDataReader reader)
        {
            List<T> tList = new List<T>();
            if (reader != null && reader.HasRows)
            {
                T t = default(T);
                int index = -1;
                try
                {
                    while (reader.Read())
                    {
                        t = Activator.CreateInstance<T>();
                        Type type = t.GetType();
                        PropertyInfo[] pinfos = type.GetProperties();
                        DataTable table = reader.GetSchemaTable();
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
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToInt32(reader[pi.Name.ToLower()]), null);
                                            break;
                                        case "decimal":
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToDecimal(reader[pi.Name.ToLower()]), null);
                                            break;
                                        case "datetime":
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToDateTime(reader[pi.Name.ToLower()]), null);
                                            break;
                                        default:
                                            pi.SetValue(t, ExtendUtility.Instance.ParseToString(reader[pi.Name.ToLower()]), null);
                                            break;
                                    }//switch (pi.PropertyType.Name.ToLower())
                                }//if (index > 0) 
                                index = -1;
                            }//foreach (DataColumn dc in table.Columns)
                        }//if (table != null && table.Rows.Count > 0)
                        tList.Add(t);
                    }//while (reader.Read())
                }
                catch (MySqlException mysql)
                {
                   // errorMessage = mysql.ToString();
                }
                catch (Exception ex)
                {
                   // errorMessage = ex.ToString();
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Dispose();
                        reader.Close();
                    }
                }
            }
            return tList;
        }

        /// <summary>
        /// 根据数据读取返回制定数据实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<T> GetModelListByDataTable<T>(DataTable table)
        {
            List<T> tList = new List<T>();
            if (table != null && table.Rows.Count > 0)
            {
                T t = default(T);
                int index = -1;
                try
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        t = Activator.CreateInstance<T>();
                        Type type = t.GetType();
                        PropertyInfo[] pinfos = type.GetProperties();
                        foreach (DataColumn dc in table.Columns)
                        {
                            index = GetPropertyIndex(pinfos, dc.ColumnName);
                            if (index >= 0)
                            {
                                PropertyInfo pi = pinfos[index];
                                String fieldName = pi.Name;
                                switch (pi.PropertyType.Name.ToLower())
                                {
                                    case "int":
                                    case "int32":
                                        pi.SetValue(t, ExtendUtility.Instance.ParseToInt32(dr[pi.Name.ToLower()]), null);
                                        break;
                                    case "decimal":
                                        pi.SetValue(t, ExtendUtility.Instance.ParseToDecimal(dr[pi.Name.ToLower()]), null);
                                        break;
                                    case "datetime":
                                        pi.SetValue(t, ExtendUtility.Instance.ParseToDateTime(dr[pi.Name.ToLower()]), null);
                                        break;
                                    default:
                                        pi.SetValue(t, ExtendUtility.Instance.ParseToString(dr[pi.Name.ToLower()]), null);
                                        break;
                                }//switch (pi.PropertyType.Name.ToLower())
                            }//if (index > 0) 
                            index = -1;
                        }//foreach (DataColumn dc in table.Columns)
                        tList.Add(t);
                    }//while (reader.Read())
                }
                catch (MySqlException)
                {

                }
                catch (Exception)
                {

                }
            }
            return tList;
        }
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
    }
}
