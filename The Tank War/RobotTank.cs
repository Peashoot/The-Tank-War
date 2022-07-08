using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    #region 机器人坦克
    /// <summary>
    /// 机器人坦克
    /// </summary>
    public class RobotTank
    {
        #region 构造函数
        public RobotTank(int count, bool random = false)
        {
            for (int i = 0; i < count; i++)
            {
                int xpos = ran.Next(PublicObjects.ViewWidth - 7) + 3;
                int ypos = ran.Next(PublicObjects.ViewHeight - 6) + 3;
                MoveDirection movdir = (MoveDirection)(ran.Next(4) + 1);
                Tank tempbot = new Tank(xpos, ypos, movdir, random ? RandomColor() : ConsoleColor.White);
                tempbot.group = "robot";
                robotlist.Add(tempbot);
            }
        }
        #endregion
        #region 成员变量
        /// <summary>
        /// 产生随机数
        /// </summary>
        private Random ran = new Random();
        /// <summary>
        /// 坦克机器人列表
        /// </summary>
        public List<Tank> robotlist = new List<Tank>();
        /// <summary>
        /// 停止运动
        /// </summary>
        public bool stop = false;
        #endregion
        #region 机器人随机操作
        /// <summary>
        /// 机器人操作
        /// </summary>
        /// <param name="obj"></param>
        public void RobotAction(object obj)
        {
            if (!stop)
            {
                try
                {
                    PrintPoint tarpos1 = PublicObjects.GamerTank1.repaint.oripos;
                    PrintPoint tarpos2 = PublicObjects.GamerTank2 == null ? null : PublicObjects.GamerTank2.repaint.oripos;
                    foreach (Tank robot in robotlist)
                    {
                        int distance;
                        robot.Move(MoveToTarget(tarpos1, tarpos2, robot.repaint.oripos, out distance), 3);
                        robot.Shot();
                    }
                }
                catch
                {
                    stop = true;
                }
            }
        }
        /// <summary>
        /// 确定目标方位
        /// </summary>
        private MoveDirection MoveToTarget(PrintPoint tarpos1, PrintPoint tarpos2, PrintPoint oripos, out int distance)
        {
            distance = 0;
            List<MoveDirection> dirlist = new List<MoveDirection>();
            PrintPoint tarpos = tarpos2 == null ? tarpos1 : ran.Next(2) > 0 ? tarpos1 : tarpos2;
            dirlist.Clear();
            if (tarpos.X > oripos.X) { dirlist.Add(MoveDirection.RIGHT); distance = Math.Abs(tarpos.X - oripos.X); }
            else if (tarpos.X < oripos.X) { dirlist.Add(MoveDirection.LEFT); distance = Math.Abs(tarpos.X - oripos.X); }
            if (tarpos.Y > oripos.Y) { dirlist.Add(MoveDirection.DOWN); distance = Math.Abs(tarpos.Y - oripos.Y); }
            else if (tarpos.Y < oripos.Y) { dirlist.Add(MoveDirection.UP); distance = Math.Abs(tarpos.Y - oripos.Y); }
            return dirlist[ran.Next(dirlist.Count)];
        }
        #endregion
        #region 获取随机的颜色
        /// <summary>
        /// 获取随机的颜色
        /// </summary>
        private ConsoleColor RandomColor()
        {
            ConsoleColor color = ConsoleColor.White;
            do
            {
                color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ran.Next(Enum.GetNames(typeof(ConsoleColor)).Length).ToString());
            }
            while (color == ConsoleColor.Black || color == PublicObjects.GamerTank1.repaint.color);
            return color;
        }
        #endregion
    }
    #endregion
}
