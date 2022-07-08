using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    /// <summary>
    /// 一个类似System.Drawing.Point的坐标类
    /// </summary>
    public class PrintPoint
    {
        #region 构造函数
        public PrintPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion
        #region 成员变量
        /// <summary>
        /// 横坐标
        /// </summary>
        public int X = 0;
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int Y = 0;
        #endregion
    }
}
