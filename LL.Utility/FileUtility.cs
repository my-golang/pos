using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LL.Utility
{
    public class FileUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static FileUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private FileUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static FileUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new FileUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="file">文件全路径</param>
        /// <param name="errorMessage">文件内容</param>
        /// <returns>文件内容</returns>
        public String GetFileContent(string file, ref String errorMessage)
        {
            FileStream fs = null;
            StreamReader reader = null;
            String content = string.Empty;
            try
            {
                if (File.Exists(file))
                {
                    fs = new FileStream(file, FileMode.Open);
                    reader = new StreamReader(fs);
                    content = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            return content;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public void WriteFile(string Path, string Strings)
        {

            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(Strings);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();


        }
        public string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="file">要创建文件全路径</param>
        /// <param name="autoDel">如果存在是否自动删除</param>
        /// <param name="errorMessage">创建失败时错误信息</param>
        /// <returns>成功返回TRUE否则返回FALSE</returns>
        public bool CreateFile(string file, bool autoDel, ref string errorMessage)
        {
            bool isCreate = true;
            FileStream fs = null;
            try
            {
                if (File.Exists(file))
                {
                    if (autoDel)
                    {
                        File.Delete(file);
                    }
                }
                if (!File.Exists(file))
                {
                    fs = File.Create(file);
                    isCreate = fs.CanRead;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                isCreate = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return isCreate;
        }
    }
}
