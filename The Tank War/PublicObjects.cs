using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Tank_War
{
    public class PublicObjects
    {
        /// <summary>
        /// 界面宽度
        /// </summary>
        public static int ViewWidth { get; set; }
        /// <summary>
        /// 界面高度
        /// </summary>
        public static int ViewHeight { get; set; }
        /// <summary>
        /// 机器人数量
        /// </summary>
        public static int RobotCount { get; set; }
        /// <summary>
        /// 玩家1
        /// </summary>
        public static Tank GamerTank1 { get; set; }
        /// <summary>
        /// 玩家2
        /// </summary>
        public static Tank GamerTank2 { get; set; }
        /// <summary>
        /// 子弹列表
        /// </summary>
        public static List<Bullet> BulletList { get; set; }
        /// <summary>
        /// 被击中列表
        /// </summary>
        public static List<Tank> HittedList { get; set; }
        /// <summary>
        /// 机器人
        /// </summary>
        public static RobotTank Robot { get; set; }
        /// <summary>
        /// 游戏模式
        /// </summary>
        public static GameKind GameKind { get; set; }


        #region 一堆坦克爆炸
        /// <summary>
        /// 一堆坦克爆炸
        /// </summary>
        /// <param name="hitlist"></param>
        public static void TanksExplode(object obj)
        {
            lock (obj)
            {
                List<Tank> hitlist = obj as List<Tank>;
                foreach (Tank t in hitlist)
                {
                    t.Explose();
                    if (t != GamerTank1 || (GamerTank2 != null && t != GamerTank2))
                    {
                        Robot.robotlist.Remove(t);
                    }
                }
                hitlist.Clear();
            }
        }
        /// <summary>
        /// 一堆坦克爆炸线程
        /// </summary>
        public static void TanksExploding(object obj)
        {
            if (PublicObjects.HittedList != null && HittedList.Count > 0)
            {
                Task newtask = new Task(TanksExplode, HittedList);
                newtask.Start();
            }
        }
        #endregion
        #region 主坦克爆炸
        /// <summary>
        /// 主坦克爆炸
        /// </summary>
        /// <param name="obj"></param>
        public static void MainTankExplode(object obj)
        {
            if (HittedList.Contains(GamerTank1))
            {
                Robot.stop = true;
                Console.WriteLine("You lose!");
            }
        }
        #endregion
        #region 所有炮弹运动
        /// <summary>
        /// 炮弹运动
        /// </summary>
        /// <param name="list"></param>
        public static void BulletsMove(object list)
        {
            lock (list)
            {
                List<Bullet> bulletlist = list as List<Bullet>;
                if (bulletlist != null && bulletlist.Count > 0)
                {
                    foreach (Bullet b in bulletlist)
                    {
                        b.Move(b.movedir);
                    }
                    bulletlist.RemoveAll(i => i.stop);
                }
            }
        }
        #endregion
    }

    public enum GameKind
    {
        /// <summary>
        /// 单人游戏
        /// </summary>
        SingleGamer = 1,
        /// <summary>
        /// 双人合作
        /// </summary>
        DoubleWork = 2,
        /// <summary>
        /// 双人对抗
        /// </summary>
        DoubleAlone = 3
    }
}
