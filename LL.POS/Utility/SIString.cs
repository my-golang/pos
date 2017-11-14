namespace LL.POS
{
    using System;
    /// <summary>
    /// 数据类型扩展方法
    /// </summary>
    public static class SIString
    {
        public static string TryStr(object obj)
        {
            return TryStr(obj, string.Empty);
        }
        public static string TryStr(object obj, string strDef)
        {
            if (!object.Equals(obj, DBNull.Value) && (obj != null))
            {
                return obj.ToString();
            }
            return strDef;
        }

        public static int TryInt(object obj)
        {
            return TryInt(obj, 0);
        }

        public static int TryInt(object obj, int decInt)
        {
            int result = 0;
            if (((!object.Equals(obj, DBNull.Value) && (obj != null)) && !object.Equals(obj, string.Empty)) && int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return decInt;
        }
        public static decimal TryDec(object obj)
        {
            return TryDec(obj, 0M);
        }
        public static decimal TryDec(object obj, decimal decDef)
        {
            decimal result = 0M;
            if (((!object.Equals(obj, DBNull.Value) && (obj != null)) && !object.Equals(obj, string.Empty)) && decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return decDef;
        }

        /// <summary>
        /// 时间类型的扩展方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(object obj)
        {
            DateTime time = DateTime.MinValue;
            try
            {
                if (DateTime.TryParse(TryStr(obj), out time))
                {
                    return time;
                }
            }
            catch
            {
                return time;
            }
            return time;
        }
        /// <summary>
        /// 判断是否为空的扩展方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyOrDBNull(object obj)
        {
            bool isok = true;
            if (obj != null || obj != DBNull.Value)
            {
                String str = TryStr(obj).Trim();
                if (str.Length != 0)
                {
                    isok = false;
                }
            }
            return isok;
        }
        /// <summary>
        /// 校验内存数据集是否为空扩展方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyDataSet(System.Data.DataSet ds)
        {
            return (ds == null || (ds != null && ds.Tables.Count == 0));
        }
        /// <summary>
        /// 校验数据集是否为空扩展方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyDataTable(System.Data.DataTable table)
        {
            return (table == null || (table != null && table.Rows.Count == 0));
        }
        /// <summary>
        /// 截取字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="interceptCount"></param>
        /// <returns></returns>
        public static string SubChar(string str, int interceptCount)
        {
            return SubChar(str, interceptCount, true);
        }
        public static string SubChar(string str, int interceptCount, bool beginTrim)
        {
            if (str == null)
            {
                return null;
            }
            if (str.Trim() == string.Empty)
            {
                return str;
            }
            if (interceptCount <= 0)
            {
                return string.Empty;
            }
            if (beginTrim)
            {
                str = str.Trim();
            }
            else
            {
                str = str.TrimEnd(new char[0]);
            }
            string[] strArray = toStringArray(str);
            if (strArray.Length == 0)
            {
                return string.Empty;
            }
            return SubChar(strArray, interceptCount);
        }
        public static string SubChar(string[] strArray, int interceptCount)
        {
            int num = 0;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int byteCount = 1;
            for (int i = 0; i < strArray.Length; i++)
            {
                byteCount = System.Text.Encoding.Default.GetByteCount(strArray[i].ToString());
                num += byteCount;
                if (num <= interceptCount)
                {
                    builder.Append(strArray[i]);
                }
                else
                {
                    break;
                }
            }
            if (builder.Length > 0)
            {
                return builder.ToString();
            }
            return string.Empty;
        }
        public static string[] toStringArray(string str)
        {
            string[] strArray = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strArray[i] = str.Substring(i, 1);
            }
            return strArray;
        }
    }
}
