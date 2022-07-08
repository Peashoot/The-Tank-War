using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace The_Tank_War
{
    public class Functions
    {
        public static void Welcome()
        {
            
        }

        public static void StartGame(int ConsoleWidth, int ConsoleHeight, int RobotCount)
        {
            #region 设置控制台
            //固定控制台大小
            PublicObjects.ViewHeight = ConsoleHeight;
            PublicObjects.ViewWidth = ConsoleWidth;
            //光标不可见
            Console.CursorVisible = false;
            //设置控制台窗口大小
            Console.SetWindowSize(PublicObjects.ViewWidth, PublicObjects.ViewHeight);
            PublicObjects.BulletList = new List<Bullet>();
            PublicObjects.HittedList = new List<Tank>();
            #endregion
            #region 创建坦克对象
            if (PublicObjects.GameKind == GameKind.SingleGamer)
            {
                //创建坦克对象
                PublicObjects.GamerTank1 = new Tank(PublicObjects.ViewWidth / 2, PublicObjects.ViewHeight / 2, MoveDirection.UP);
                PublicObjects.GamerTank1.group = "gamer";
            }
            else 
            {
                PublicObjects.GamerTank1 = new Tank(PublicObjects.ViewWidth / 4 * 3, PublicObjects.ViewHeight / 2, MoveDirection.UP);
                PublicObjects.GamerTank2 = new Tank(PublicObjects.ViewWidth / 4, PublicObjects.ViewHeight / 2, MoveDirection.UP);
                if (PublicObjects.GameKind == GameKind.DoubleWork)
                {
                    PublicObjects.GamerTank1.group = PublicObjects.GamerTank2.group = "gamer";
                }
                else
                {
                    PublicObjects.GamerTank1.group = "gamer1";
                    PublicObjects.GamerTank2.group = "gamer2";
                }
            }
            //生成若干个TankRobot
            PublicObjects.Robot = new RobotTank(RobotCount, true);
            #endregion
            //激活TankRobbot
            using (Timer robotaction = new Timer(new TimerCallback(PublicObjects.Robot.RobotAction), null, 0, 1000))
            {
                //定时器更新炮弹位置
                using (Timer bullettimer = new Timer(new TimerCallback(PublicObjects.BulletsMove), PublicObjects.BulletList, 0, 100))
                {
                    //定时检测炮弹命中
                    using (Timer destroytimer = new Timer(new TimerCallback(PublicObjects.TanksExploding), null, 0, 100))
                    {
                        //定时器检测玩家坦克是否被命中
                        using (Timer EndGame = new Timer(new TimerCallback(PublicObjects.MainTankExplode), null, 0, 100))
                        {
                            #region 画出控制台边框
                            ViewFrame frame = new ViewFrame(PublicObjects.ViewWidth, PublicObjects.ViewHeight);
                            frame.DrawFrame();
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
                                        PublicObjects.GamerTank1.Move(MoveDirection.UP);
                                        break;
                                    //向右运动
                                    case ConsoleKey.RightArrow:
                                        PublicObjects.GamerTank1.Move(MoveDirection.RIGHT);
                                        break;
                                    //向下运动
                                    case ConsoleKey.DownArrow:
                                        PublicObjects.GamerTank1.Move(MoveDirection.DOWN);
                                        break;
                                    //向左运动
                                    case ConsoleKey.LeftArrow:
                                        PublicObjects.GamerTank1.Move(MoveDirection.LEFT);
                                        break;
                                    //发射炮弹
                                    case ConsoleKey.Spacebar:
                                        PublicObjects.GamerTank1.Shot();
                                        break;
                                    case ConsoleKey.W:
                                        if (PublicObjects.GamerTank2 != null)
                                        {
                                            PublicObjects.GamerTank2.Move(MoveDirection.UP);
                                        }
                                        break;
                                    case ConsoleKey.D:
                                        if (PublicObjects.GamerTank2 != null)
                                        {
                                            PublicObjects.GamerTank2.Move(MoveDirection.RIGHT);
                                        }
                                        break;
                                    case ConsoleKey.S:
                                        if (PublicObjects.GamerTank2 != null)
                                        {
                                            PublicObjects.GamerTank2.Move(MoveDirection.DOWN);
                                        }
                                        break;
                                    case ConsoleKey.A:
                                        if (PublicObjects.GamerTank2 != null)
                                        {
                                            PublicObjects.GamerTank2.Move(MoveDirection.LEFT);
                                        }
                                        break;
                                    case ConsoleKey.Tab:
                                        if (PublicObjects.GamerTank2 != null)
                                        {
                                            PublicObjects.GamerTank2.Shot();
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
    }
}
