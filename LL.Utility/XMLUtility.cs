namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    public class XMLUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static XMLUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private XMLUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static XMLUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new XMLUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region
        /// <summary>
        /// 获取制定文件中制定节点的内容
        /// </summary>
        /// <param name="xmlPath">XML文件</param>
        /// <param name="nodeName">要查找内容的节点数组</param>
        /// <returns></returns>
        public Dictionary<String, String> GetNodesText(String xmlPath, String[] nodeName)
        {
            Dictionary<String, String> result = null;
            XmlDocument xmlDocument = null;
            XmlNamespaceManager nsmgr = null;
            try
            {
                if (File.Exists(xmlPath))
                {
                    if (nodeName.Length > 0)
                    {
                        result = new Dictionary<String, String>();
                    }
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(Path.GetFileName(xmlPath));
                    nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                    for (int index = 0; index < nodeName.Length; index++)
                    {
                        XmlNode xmlNode = xmlDocument.SelectSingleNode(nodeName[index], nsmgr);
                        if (!result.ContainsKey(nodeName[index]))
                        {
                            result.Add(nodeName[index], xmlNode.InnerText);
                        }
                    }
                }
            }
            catch (IOException)
            {

            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return result;
        }
        /// <summary>
        /// 设置节点的值
        /// </summary>
        /// <param name="xmlPath">XML文件</param>
        /// <param name="nodeName">要设置的节点</param>
        /// <param name="nodeText">节点值</param>
        /// <returns></returns>
        public bool SetNodesText(String xmlPath, String nodeName, String nodeText)
        {
            bool isok = false;
            XmlDocument xmlDocument = null;
            XmlNamespaceManager nsmgr = null;
            try
            {
                if (File.Exists(xmlPath))
                {
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(Path.GetFileName(xmlPath));
                    nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                    XmlNode xmlNode = xmlDocument.SelectSingleNode(nodeName, nsmgr);
                    xmlNode.InnerText = nodeText;
                    XmlElement Xe = (XmlElement)xmlNode;
                    Xe.SetAttribute("value", nodeText);
                    isok = true;
                    xmlDocument.Save(xmlPath);
                }
            }
            catch (IOException)
            {

            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return isok;
        }
        /// <summary>
        /// 将指定的内容写入到XML的元素中,
        /// </summary>
        /// <param name="dicParameters">节点的元素</param>
        /// <param name="rootElement">根节点</param>
        /// <returns></returns>
        public bool WriteXmlFile(String xmlPath, Dictionary<String, Object> dicParameters, String rootElement)
        {
            bool isok = false;
            XmlTextWriter xmlTextWriter = null;
            try
            {
                if (File.Exists(xmlPath))
                {
                    File.Delete(xmlPath);
                }
                xmlTextWriter = new XmlTextWriter(xmlPath, System.Text.Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.Indentation = 4;
                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement(rootElement);
                foreach (KeyValuePair<String, Object> kv in dicParameters)
                {
                    xmlTextWriter.WriteStartElement(kv.Key);
                    xmlTextWriter.WriteValue(kv.Value);
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndDocument();
                isok = true;
            }
            catch (IOException)
            {
                isok = false;
            }
            catch (XmlException)
            {
                isok = false;
            }
            catch (Exception)
            {
                isok = false;
            }
            finally
            {
                if (xmlTextWriter != null)
                {
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }
            }
            return isok;
        }
        #endregion
    }
}
