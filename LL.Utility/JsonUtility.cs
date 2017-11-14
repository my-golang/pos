
namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    /// <summary>
    /// Json数据解析工具
    /// </summary>
    public class JsonUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static JsonUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private JsonUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static JsonUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new JsonUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 将json字符串转化成实体对象
        /// </summary>
        /// <param name="json"></param>
        public T JsonToObject<T>(String json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {

            }
            return default(T);
        }
        /// <summary>
        /// 将内存数据集返回成Json串
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public String DataTableToJson(DataTable table)
        {
            String json = String.Empty;
            try
            {
                json = JsonConvert.SerializeObject(table, new DataTableConverter());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return json;
        }
        /// <summary>
        /// 将对象转换成JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public String ObjectToJson<T>(List<T> t)
        {
            String json = String.Empty;
            try
            {
                json = JsonConvert.SerializeObject(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return json;
        }
        /// <summary>
        /// 将JSON字符串转换成二维数组
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>二维数组</returns>
        public DataTable JsonToDataTable(String json)
        {
            DataTable table = null;
            try
            {
                table = new DataTable();
                table=JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
        #endregion
    }
}
