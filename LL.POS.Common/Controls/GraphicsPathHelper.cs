namespace LL.POS.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public static class GraphicsPathHelper
    {
        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePath(Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int num = correction ? 1 : 0;
            switch (style)
            {
                case RoundStyle.None:
                    path.AddRectangle(rect);
                    break;

                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
                    path.AddArc((rect.Right - radius) - num, rect.Y, radius, radius, 270f, 90f);
                    path.AddArc((rect.Right - radius) - num, (rect.Bottom - radius) - num, radius, radius, 0f, 90f);
                    path.AddArc(rect.X, (rect.Bottom - radius) - num, radius, radius, 90f, 90f);
                    break;

                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
                    path.AddLine(rect.Right - num, rect.Y, rect.Right - num, rect.Bottom - num);
                    path.AddArc(rect.X, (rect.Bottom - radius) - num, radius, radius, 90f, 90f);
                    break;

                case RoundStyle.Right:
                    path.AddArc((rect.Right - radius) - num, rect.Y, radius, radius, 270f, 90f);
                    path.AddArc((rect.Right - radius) - num, (rect.Bottom - radius) - num, radius, radius, 0f, 90f);
                    path.AddLine(rect.X, rect.Bottom - num, rect.X, rect.Y);
                    break;

                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
                    path.AddArc((rect.Right - radius) - num, rect.Y, radius, radius, 270f, 90f);
                    path.AddLine(rect.Right - num, rect.Bottom - num, rect.X, rect.Bottom - num);
                    break;

                case RoundStyle.Bottom:
                    path.AddArc((rect.Right - radius) - num, (rect.Bottom - radius) - num, radius, radius, 0f, 90f);
                    path.AddArc(rect.X, (rect.Bottom - radius) - num, radius, radius, 90f, 90f);
                    path.AddLine(rect.X, rect.Y, rect.Right - num, rect.Y);
                    break;
            }
            path.CloseFigure();
            return path;
        }
    }
}

