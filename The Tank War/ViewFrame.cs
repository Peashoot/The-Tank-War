using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    public class ViewFrame : PaintModel
    {
        public ViewFrame(int width, int height) : base(0, 0, ConsoleColor.White)
        {
            this.paintchar = '%';
            this.width = width;
            this.height = height;
        }

        public int width { get; set; }
        public int height { get; set; }
        /// <summary>
        /// 清除边框
        /// </summary>
        public override void ClearPartConsole()
        {
            char tempchar = paintchar;
            paintchar = ' ';
            DrawFrame();
            paintchar = tempchar;
        }
        /// <summary>
        /// 绘制边框
        /// </summary>
        public void DrawFrame()
        {
            //上边界
            repaint.WriteAt(new string(paintchar, PublicObjects.ViewWidth));
            //下边界
            repaint.WriteAt(new string(paintchar, PublicObjects.ViewWidth), 0, PublicObjects.ViewHeight - 2);
            for (int i = 1; i < PublicObjects.ViewHeight - 2; i++)
            {
                //左边界
                repaint.WriteAt(new string(paintchar, 1), 0, i);
                //右边界
                repaint.WriteAt(new string(paintchar, 1), PublicObjects.ViewWidth - 1, i);
            }
        }
    }
}
