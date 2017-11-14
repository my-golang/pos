

namespace LL.Utility
{
    using System;
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public class StringUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static StringUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private StringUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static StringUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new StringUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 将字符数组按照Asiic降序排列
        /// </summary>
        /// <param name="arr">排序数组</param>
        /// <returns></returns>
        public  String[] AsiicAsc(String[] arr)
        {
            String[] result = arr;
            String temp = String.Empty;
            for (int index = 0; index < result.Length - 1; index++)
            {
                for (int rindex = index + 1; rindex < result.Length; rindex++)
                {
                    if (!StringSortAsciic(result[index], result[rindex]))//如果前一个字符ASCIIC码大于后一个字符ASCIIC
                    {
                        temp = result[index];
                        result[index] = result[rindex];
                        result[rindex] = temp;
                    }
                }
            }
            return result;
        }

        public string GetSubString(string strSub, int start, int length)
        {
            string temp = strSub;
            int j = 0, k = 0, p = 0;

            CharEnumerator ce = temp.GetEnumerator();
            while (ce.MoveNext())
            {
                j += (ce.Current > 0 && ce.Current < 255) ? 1 : 2;

                if (j <= start)
                {
                    p++;
                }
                else
                {
                    if (j == GetLength(temp))
                    {
                        temp = temp.Substring(p, k + 1);
                        break;
                    }
                    if (j <= length + start)
                    {
                        k++;
                    }
                    else
                    {
                        temp = temp.Substring(p, k);
                        break;
                    }
                }
            }

            return temp;
        }

        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public String StringTrim(object obj)
        {
            String result = ExtendUtility.Instance.ParseToString(obj);
            return result.Trim();
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 将字符串转换成ASCIIC码值
        /// </summary>
        /// <param name="character">字符串</param>
        /// <returns></returns>
        private  int ASCIIC(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            return 0;
        }
        /// <summary>
        /// 比较两个字符串ASIIC
        /// </summary>
        /// <param name="beforeStr">前一个字符</param>
        /// <param name="endStr">后一个字符</param>
        /// <returns>如果前一个字符比后一个字符小，则为true，否则为false</returns>
        private  bool StringSortAsciic(string beforeStr, string endStr)
        {
            for (int i = 0; i < beforeStr.Length; i++)
            {
                if (endStr.Length > i)
                {
                    //如果当前字母相等则继续下个循环
                    if (ASCIIC(beforeStr.Substring(i, 1)) == ASCIIC(endStr.Substring(i, 1)))
                    {
                        continue;
                    }
                    //如果当前字母小于则代表beforeStr小于endStr
                    if (ASCIIC(beforeStr.Substring(i, 1)) < ASCIIC(endStr.Substring(i, 1)))
                    {
                        return true;
                    }
                    //如果beforeStr比endStr大 则返回false
                    if (ASCIIC(beforeStr.Substring(i, 1)) > ASCIIC(endStr.Substring(i, 1)))
                    {
                        return false;
                    }
                }
            }
            //如果上面字符都小于或者相等,前面字符长于后面字符,则代表比他大,
            if (beforeStr.Length > endStr.Length)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取指定字符串长度，汉字以2字节计算
        /// </summary>
        /// <param name="aOrgStr">要统计的字符串</param>
        /// <returns></returns>
        private  int GetLength(String aOrgStr)
        {
            int intLen = aOrgStr.Length;
            int i;
            char[] chars = aOrgStr.ToCharArray();
            for (i = 0; i < chars.Length; i++)
            {
                if (System.Convert.ToInt32(chars[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }
        #endregion
    }
}
