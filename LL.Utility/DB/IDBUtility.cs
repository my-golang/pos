

namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    /// <summary>
    /// 数据访问接口定义
    /// </summary>
    public interface IDBUtility
    {
        /// <summary>
        /// 返回执行SQL影响的记录条数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns>成功1，否则为0</returns>
        int ExecuteSql(String connectString, String sql, DbParameter[] parameters,ref String errorMessage);
        /// <summary>
        /// 返回执行SQL的第一行第一列的值
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        object GetSingleData(String connectionString, string sql, DbParameter[] parameters, ref String errorMessage);
        /// <summary>
        /// 返回执行SQL的泛型对象
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        List<T> GetDataList<T>(String connectionString, string sql, DbParameter[] parameters, ref String errorMessage);
        /// <summary>
        /// 返回执行SQL的内存数据
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns>内存数据集</returns>
        DataTable GetDataTable(String connectionString, string sql, DbParameter[] parameters, ref String errorMessage);
        /// <summary>
        /// 使用事物执行一组SQL语句
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="_dicParameters">SQL语句泛型</param>
        int ExecuteSqlsByTrans(String connectionString, List<SqlParaEntity> _dicParameters, ref String errorMessage);

    }
    /// <summary>
    /// SQL参数结构体
    /// </summary>
    public struct SqlParaEntity
    {
        public String Sql
        {
            get;
            set;
        }
        public DbParameter[] parameters
        {
            get;
            set;
        }
    }
}
