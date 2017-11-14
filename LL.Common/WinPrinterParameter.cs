
namespace LL.Common
{
    using System;
    using System.Drawing;
    using System.Text;
    /// <summary>
    /// 打印机参数
    /// </summary>
    public class WinPrinterParameter
    {
        private Int32 _fontSize = 10;
        /// <summary>
        /// 文本大小
        /// </summary>
        public Int32 FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }
        private String _fontFamily = "宋体";
        /// <summary>
        /// 文本字体
        /// </summary>
        public String FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                _fontFamily = value;
            }
        }
        private FontStyle _fontStyle = FontStyle.Regular;
        /// <summary>
        /// 文本格式
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }
            set
            {
                _fontStyle = value;
            }
        }
        private String _fontText = String.Empty;
        /// <summary>
        /// 字体文本
        /// </summary>
        public String FontText
        {
            get
            {
                return _fontText;
            }
            set
            {
                _fontText = value;
            }
        }
        private TextAlign _textAlign = TextAlign.Left;
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlign TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }
    }
    /// <summary>
    /// 文本对其方式
    /// </summary>
    public enum TextAlign
    {
        Left = 0,
        Center = 1,
        Right = 2
    }


}
