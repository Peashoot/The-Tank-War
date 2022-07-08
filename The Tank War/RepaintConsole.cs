using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    /// <summary>
    /// 重绘操作类
    /// </summary>
    public class RepaintConsole
    {
        #region 成员变量
        /// <summary>
        /// 光标当前位置
        /// </summary>
        public PrintPoint oripos;
        /// <summary>
        /// 控制台字体颜色
        /// </summary>
        public ConsoleColor color;
        #endregion
        #region 构造函数
        public RepaintConsole(int X, int Y, ConsoleColor color)
        {
            oripos = new PrintPoint(X, Y);
            this.color = color;
        }
        #endregion
        #region 局部刷新
        /// <summary>
        /// 局部刷新
        /// </summary>
        public void WriteAt(string s, int x = 0, int y = 0)
        {
            Singleton<ConsoleWriter>.Instance.ConsoleWrite(s, oripos.X + x, oripos.Y + y, color);
        }
        #endregion
    }
}
