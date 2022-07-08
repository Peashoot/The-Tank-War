using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleCountDown
{
    public class Program
    {
        #region 静态成员变量
        /// <summary>
        /// 控制台宽度
        /// </summary>
        public static int conswidth;
        /// <summary>
        /// 控制台高度
        /// </summary>
        public static int consheight;
        /// <summary>
        /// 玩家控制的坦克
        /// </summary>
        public static Tank newtank;
        /// <summary>
        /// 创建炮弹列表
        /// </summary>
        public static List<Bullet> bulletlist = new List<Bullet>();
        /// <summary>
        /// 命中的坦克列表
        /// </summary>
        public static List<Tank> hitlist = new List<Tank>();
        /// <summary>
        /// Tank机器人
        /// </summary>
        public static RobotTank robot;
        #endregion

        #region 主函数
        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region 设置控制台
            //固定控制台大小
            consheight = Console.LargestWindowHeight - 10;
            conswidth = Console.LargestWindowWidth - 50;
            //光标不可见
            Console.CursorVisible = false;
            //设置控制台窗口大小
            Console.SetWindowSize(conswidth, consheight);
            #endregion
            #region 创建坦克对象
            //生成若干个TankRobot
            robot = new RobotTank(3, true);
            //创建坦克对象
            newtank = new Tank(conswidth / 2, consheight / 2, MoveDirection.UP);
            #endregion
            //激活TankRobbot
            using (Timer robotaction = new Timer(new TimerCallback(robot.RobotAction), null, 0, 1000))
            {
                //定时器更新炮弹位置
                using (Timer bullettimer = new Timer(new TimerCallback(Bullet.BulletsMove), bulletlist, 0, 100))
                {
                    //定时检测炮弹命中
                    using (Timer destroytimer = new Timer(new TimerCallback(Tank.TanksExploding), null, 0, 100))
                    {
                        //定时器检测玩家坦克是否被命中
                        using (Timer EndGame = new Timer(new TimerCallback(Tank.MainTankExplode), null, 0, 100))
                        {
                            #region 画出控制台边框
                            //画出控制台边框
                            //上边界
                            newtank.repaint.WriteAt(new string('%', conswidth), -newtank.repaint.oripos.X, 0 - newtank.repaint.oripos.Y);
                            //下边界
                            newtank.repaint.WriteAt(new string('%', conswidth), -newtank.repaint.oripos.X, consheight - newtank.repaint.oripos.Y - 2);
                            for (int i = 1; i < consheight - 2; i++)
                            {
                                //左边界
                                newtank.repaint.WriteAt("%", -newtank.repaint.oripos.X, i - newtank.repaint.oripos.Y);
                                //右边界
                                newtank.repaint.WriteAt("%", conswidth - 1 - newtank.repaint.oripos.X, i - newtank.repaint.oripos.Y);
                            }
                            #endregion
                            #region 玩家坦克操作
                            //设置tank默认朝向为上
                            ConsoleKey revkey = ConsoleKey.UpArrow;
                            do
                            {
                                switch (revkey)
                                {
                                    //向上运动
                                    case ConsoleKey.UpArrow:
                                        newtank.movedir = MoveDirection.UP;
                                        newtank.Move();
                                        break;
                                    //向右运动
                                    case ConsoleKey.RightArrow:
                                        newtank.movedir = MoveDirection.RIGHT;
                                        newtank.Move();
                                        break;
                                    //向下运动
                                    case ConsoleKey.DownArrow:
                                        newtank.movedir = MoveDirection.DOWN;
                                        newtank.Move();
                                        break;
                                    //向左运动
                                    case ConsoleKey.LeftArrow:
                                        newtank.movedir = MoveDirection.LEFT;
                                        newtank.Move();
                                        break;
                                    //发射炮弹
                                    case ConsoleKey.Spacebar:
                                        Bullet newbullet = new Bullet(newtank.repaint.oripos.X, newtank.repaint.oripos.Y, newtank.movedir);
                                        lock (bulletlist)
                                        {
                                            bulletlist.Add(newbullet);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                revkey = Console.ReadKey().Key;
                            }
                            //按Esc退出游戏
                            while (revkey != ConsoleKey.Escape);
                            #endregion
                        }
                    }
                }
            }
        }
        #endregion
    }
}
