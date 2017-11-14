namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    /// <summary>
    /// sqlserver数据访问工具
    /// </summary>
    public class DBUtilitySqlServer : IDBUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static DBUtilitySqlServer _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DBUtilitySqlServer()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static DBUtilitySqlServer Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new DBUtilitySqlServer();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 执行一条SQL，返回执行影响的记录数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public int ExecuteSql(String connectString, String sql, DbParameter[] parameters, ref String errorMessage)
        {
            int result = 0;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(connectString);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                errorMessage=sqlex.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString() ;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }
        /// <summary>
        /// 返回执行SQL后第一行第一列的值
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>第一行第一列的值</returns>
        public object GetSingleData(string connectionString, string sql, DbParameter[] parameters, ref String errorMessage)
        {
            object obj = null;
            SqlCommand cmd = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                obj = cmd.ExecuteScalar();
            }
            catch (SqlException sqlex)
            {
                errorMessage = sqlex.ToString();
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
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 返回执行SQL的泛型对象
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public System.Collections.Generic.List<T> GetDataList<T>(string connectionString, string sql, DbParameter[] parameters, ref String errorMessage)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            SqlConnection conn = null;
            List<T> tList = null;
            try
            {
                conn = new SqlConnection(connectionString);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                reader = cmd.ExecuteReader();
                tList = CommonUtility.Instance.GetListByDbReader<T>(reader);
            }
            catch (SqlException sqlex)
            {
                errorMessage = sqlex.ToString();
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
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return tList;
        }
        /// <summary>
        /// 返回执行SQL的内存数据
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns>内存数据集</returns>
        public System.Data.DataTable GetDataTable(string connectionString, string sql, DbParameter[] parameters, ref String errorMessage)
        {
            DataTable table = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                ds = new DataSet();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }
            catch (SqlException sqlex)
            {
                errorMessage = sqlex.ToString();
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
        /// <param name="connectionString">连接字符串</param>
        /// <param name="_dicParameters">SQL语句泛型</param>
        public int ExecuteSqlsByTrans(string connectionString, System.Collections.Generic.List<SqlParaEntity> _dicParameters, ref String errorMessage)
        {
            int result = 1;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlTransaction trans = null;
            try
            {
                conn = new SqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                trans = conn.BeginTransaction();
                foreach (SqlParaEntity kv in _dicParameters)
                {
                    cmd = new SqlCommand(kv.Sql, conn);
                    cmd.Parameters.Clear();
                    if (kv.parameters != null && kv.parameters.Length > 0)
                        cmd.Parameters.AddRange(kv.parameters);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (SqlException sqlex)
            {
                errorMessage = sqlex.ToString();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
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
        #endregion 
    }
}
