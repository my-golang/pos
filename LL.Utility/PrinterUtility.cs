namespace LL.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using LL.Common;
    /// <summary>
    /// 打印工具类
    /// </summary>
    public class PrinterUtility
    {
        #region 单例模式
        /// <summary>
        /// 实体锁
        /// </summary>
        private static object sync = new object();
        private static PrinterUtility _instance;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private PrinterUtility()
        {

        }
        /// <summary>
        /// 实体
        /// </summary>
        public static PrinterUtility Instance
        {
            get
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        lock (sync)
                        {
                            _instance = new PrinterUtility();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 在制定打印机上打印输出内容
        /// </summary>
        /// <param name="_parameters">打印内容列表</param>
        /// <param name="printerName">打印机名称</param>
        /// <param name="errMessage">错误消息，如果有</param>
        /// <returns>成功true否则false</returns>
        public bool Print(List<WinPrinterParameter> _parameters, String printerName, int _width, ref String errMessage)
        {
            bool isok = false;
            PrintDialog dialog = null;
            PrintDocument document = null;
            try
            {
                dialog = new PrintDialog();
                document = new PrintDocument() { DocumentName = "Pos" };
                document.PrinterSettings.PrinterName = printerName;
                document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                document.PrintController = new StandardPrintController();
                document.PrintPage += (obj, e) =>
                {
                    isok = PrintContent(_parameters, 0, e, _width);
                };
                dialog.Document = document;
                document.Print();
                isok = true;
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
                if (dialog != null)
                {
                    dialog.Dispose();
                }
            }
            return isok;
        }
      
        #endregion
        #region 私有方法
        
        /// <summary>
        /// 打印输出内容
        /// </summary>
        /// <param name="_paramters">打印内容列表</param>
        /// <param name="_startIndex">打印内容列表</param>
        /// <param name="PrintPageEventArgs">打印事件参数</param>
        private bool PrintContent(List<WinPrinterParameter> _paramters, int _startIndex, PrintPageEventArgs e, int _width)
        {
            bool isok = true;
            float y = e.MarginBounds.Y;
            float top = e.MarginBounds.Top;
            float height = 0f;
            try
            {
                for (int index = _startIndex; index < _paramters.Count; index++)
                {
                    WinPrinterParameter _para = _paramters[index];
                    using (Font font = new Font(_para.FontFamily, _para.FontSize, _para.FontStyle))
                    {
                        e.Graphics.DrawString(_para.FontText, font, Brushes.Black, (float)e.MarginBounds.X, y + 10);
                        height = font.GetHeight();
                        y += font.GetHeight();
                    }
                    //if ((((y + (height * 3f)) * 96f) / 100f) > e.PageSettings.PrintableArea.Height)
                    //{
                    //    e.HasMorePages = true;
                    //    PrintContent(_paramters, index + 1, e, _width);
                    //}
                }
            }
            catch
            {
                isok = false;
            }
            return isok;
        }
        
        #endregion
    }
}
