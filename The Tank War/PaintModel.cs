using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    public class PaintModel
    {
        #region 构造函数
        public PaintModel(int X, int Y, ConsoleColor color)
        {
            repaint = new RepaintConsole(X, Y, color);
        }
        #endregion
        #region 成员变量
        /// <summary>
        /// 重绘字符串
        /// </summary>
        public char paintchar { get; set; }
        /// <summary>
        /// 重绘区域
        /// </summary>
        public RepaintConsole repaint { get; set; }
        #endregion
        #region 成员函数
        /// <summary>
        /// 清空上一片区域
        /// </summary>
        public virtual void ClearPartConsole() { }
        #endregion
    }

    public interface IMoveModel
    {
        /// <summary>
        /// 移动方向
        /// </summary>
        MoveDirection movedir { get; set; }
        /// <summary>
        /// 移动
        /// </summary>
        void Move(MoveDirection movedir, int step = 1);
    }
}
