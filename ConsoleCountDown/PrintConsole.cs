using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;

namespace ConsoleCountDown
{
    #region 移动方向集合
    /// <summary>
    /// 移动方向集合
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>
        /// 向上
        /// </summary>
        UP = 1,
        /// <summary>
        /// 向右
        /// </summary>
        RIGHT = 2,
        /// <summary>
        /// 向下
        /// </summary>
        DOWN = 3,
        /// <summary>
        /// 向左
        /// </summary>
        LEFT = 4,
    }
    #endregion

    #region 基类
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseClass
    {
        #region 构造函数
        public BaseClass(int X, int Y, MoveDirection movdir, ConsoleColor color)
        {
            repaint = new RepaintConsole(X, Y, color);
            movedir = movdir;
        }
        #endregion
        #region 成员变量
        /// <summary>
        /// 重绘字符串
        /// </summary>
        public char paintchar;
        /// <summary>
        /// 重绘区域
        /// </summary>
        public RepaintConsole repaint;
        /// <summary>
        /// 移动方向
        /// </summary>
        public MoveDirection movedir;
        #endregion
        #region 成员函数
        /// <summary>
        /// 移动
        /// </summary>
        public virtual void Move() { }
        /// <summary>
        /// 清空上一片区域
        /// </summary>
        public virtual void ClearPartConsole() { }
        #endregion
    }
    #endregion

    #region 坦克类
    /// <summary>
    /// 坦克类
    /// </summary>
    public class Tank : BaseClass
    {
        #region 构造函数
        public Tank(int X, int Y, MoveDirection movdir, ConsoleColor color = ConsoleColor.White)
            : base(X, Y, movdir, color)
        {
            ClearPartConsole();
            paintchar = '@';
            ReDrawShape();
        }
        #endregion
        #region 坦克移动
        /// <summary>
        /// 坦克移动
        /// </summary>
        public override void Move()
        {
            ClearPartConsole();
            switch (movedir)
            {
                case MoveDirection.UP:
                    if (repaint.oripos.Y > 2)
                        repaint.oripos.Y--;
                    break;
                case MoveDirection.RIGHT:
                    if (repaint.oripos.X < Program.conswidth - 5)
                        repaint.oripos.X += 2;
                    break;
                case MoveDirection.DOWN:
                    if (repaint.oripos.Y < Program.consheight - 4)
                        repaint.oripos.Y++;
                    break;
                case MoveDirection.LEFT:
                    if (repaint.oripos.X > 4)
                        repaint.oripos.X -= 2;
                    break;
                default:
                    break;
            }
            ReDrawShape();
        }
        #endregion
        #region 重绘坦克形状
        /// <summary>
        /// 重绘坦克形状
        /// </summary>
        public void ReDrawShape()
        {
            switch (movedir)
            {
                case MoveDirection.UP:
                    repaint.WriteAt(paintchar.ToString(), 0, -1);
                    repaint.WriteAt(new string(paintchar, 5), -2, 0);
                    repaint.WriteAt(new string(paintchar, 5), -2, 1);
                    break;
                case MoveDirection.RIGHT:
                    repaint.WriteAt(new string(paintchar, 3), -2, -1);
                    repaint.WriteAt(new string(paintchar, 5), -2, 0);
                    repaint.WriteAt(new string(paintchar, 3), -2, 1);
                    break;
                case MoveDirection.DOWN:
                    repaint.WriteAt(new string(paintchar, 5), -2, -1);
                    repaint.WriteAt(new string(paintchar, 5), -2, 0);
                    repaint.WriteAt(paintchar.ToString(), 0, 1);
                    break;
                case MoveDirection.LEFT:
                    repaint.WriteAt(new string(paintchar, 3), 0, -1);
                    repaint.WriteAt(new string(paintchar, 5), -2, 0);
                    repaint.WriteAt(new string(paintchar, 3), 0, 1);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 清除上一次坦克所在的位置
        /// <summary>
        /// 清除上一次坦克所在的位置
        /// </summary>
        public override void ClearPartConsole()
        {
            for (int i = 0; i < 3; i++)
                repaint.WriteAt("     ", -2, i - 1);
        }
        #endregion
        #region 判断炮弹是否命中
        /// <summary>
        /// 判断炮弹是否命中
        /// </summary>
        /// <param name="pos"></param>
        public bool IsHit(Bullet movebullet)
        {
            bool ret = false;
            switch (movedir)
            {
                case MoveDirection.UP:
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y - 1 && movebullet.repaint.oripos.X == repaint.oripos.X;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y + 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    break;
                case MoveDirection.RIGHT:
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y - 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 1;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y + 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 1;
                    break;
                case MoveDirection.DOWN:
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y - 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y + 1 && movebullet.repaint.oripos.X == repaint.oripos.X;
                    break;
                case MoveDirection.LEFT:
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y - 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 1 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y && movebullet.repaint.oripos.X > repaint.oripos.X - 3 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    ret |= movebullet.repaint.oripos.Y == repaint.oripos.Y + 1 && movebullet.repaint.oripos.X > repaint.oripos.X - 1 && movebullet.repaint.oripos.X < repaint.oripos.X + 3;
                    break;
            }
            return ret;
        }
        #endregion
        #region 判断坦克是否发生碰撞
        /// <summary>
        /// 判断坦克是否发生碰撞
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsCollision(Tank movetank)
        {
            bool ret = false;
            switch (movedir)
            {
                case MoveDirection.UP:
                    switch (movetank.movedir)
                    {
                        case MoveDirection.UP:
                            ret |= repaint.oripos.Y + 1 == movetank.repaint.oripos.Y - 1 && repaint.oripos.X - 3 < movetank.repaint.oripos.X && movetank.repaint.oripos.X < repaint.oripos.X + 3;
                            break;
                        case MoveDirection.RIGHT:

                            break;
                        case MoveDirection.DOWN:
                            break;
                        case MoveDirection.LEFT:
                            break;
                    }
                    break;
                case MoveDirection.RIGHT:
                    switch (movetank.movedir)
                    {
                        case MoveDirection.UP:
                            break;
                        case MoveDirection.RIGHT:
                            break;
                        case MoveDirection.DOWN:
                            break;
                        case MoveDirection.LEFT:
                            break;
                    }
                    break;
                case MoveDirection.DOWN:
                    switch (movetank.movedir)
                    {
                        case MoveDirection.UP:
                            break;
                        case MoveDirection.RIGHT:
                            break;
                        case MoveDirection.DOWN:
                            break;
                        case MoveDirection.LEFT:
                            break;
                    }
                    break;
                case MoveDirection.LEFT:
                    switch (movetank.movedir)
                    {
                        case MoveDirection.UP:
                            break;
                        case MoveDirection.RIGHT:
                            break;
                        case MoveDirection.DOWN:
                            break;
                        case MoveDirection.LEFT:
                            break;
                    }
                    break;
            }
            return ret;
        }
        #endregion
        #region 单个坦克爆炸
        /// <summary>
        /// 坦克爆炸
        /// </summary>
        public void TankExplosion()
        {
            for (int i = 0; i < 5; i++)
            {
                if (i % 2 == 1)
                {
                    repaint.WriteAt("* * *", -2, -1);
                    repaint.WriteAt(" * * ", -2, 0);
                    repaint.WriteAt("* * *", -2, 1);
                }
                else
                {
                    repaint.WriteAt(" * * ", -2, -1);
                    repaint.WriteAt("* * *", -2, 0);
                    repaint.WriteAt(" * * ", -2, 1);
                }
                Thread.Sleep(100);
            }
            ClearPartConsole();
        }
        #endregion
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
                    t.TankExplosion();
                    if (t != Program.newtank)
                        Program.robot.robotlist.Remove(t);
                }
                hitlist.Clear();
            }
        }
        /// <summary>
        /// 一堆坦克爆炸线程
        /// </summary>
        public static void TanksExploding(object obj)
        {
            if (Program.hitlist != null && Program.hitlist.Count > 0)
            {
                Thread newthread = new Thread(new ParameterizedThreadStart(TanksExplode));
                newthread.Start(Program.hitlist);
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
            if (Program.hitlist.Contains(Program.newtank))
            {
                Program.robot.stop = true;
                Console.WriteLine("You lose!");
            }
        }
        #endregion
    }
    #endregion

    #region 炮弹类
    /// <summary>
    /// 炮弹类
    /// </summary>
    public class Bullet : BaseClass
    {
        #region 成员变量
        /// <summary>
        /// 炮弹清除原来位置判断是否清除两位
        /// </summary>
        public bool step = false;
        /// <summary>
        /// 炮弹停止运动标志位
        /// </summary>
        public bool stop = false;
        #endregion
        #region 构造函数
        public Bullet(int X, int Y, MoveDirection movdir)
            : base(X, Y, movdir, ConsoleColor.White)
        {
            paintchar = 'o';
            switch (movedir)
            {
                case MoveDirection.UP:
                    repaint.oripos.Y -= 2;
                    break;
                case MoveDirection.DOWN:
                    repaint.oripos.Y += 2;
                    break;
                case MoveDirection.RIGHT:
                    step = false;
                    repaint.oripos.X += 3;
                    break;
                case MoveDirection.LEFT:
                    step = true;
                    repaint.oripos.X -= 3;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 单个炮弹运动
        /// <summary>
        /// 炮弹运动
        /// </summary>
        public override void Move()
        {
            ClearPartConsole();
            switch (movedir)
            {
                case MoveDirection.UP:
                    if (repaint.oripos.Y > 1)
                        repaint.oripos.Y--;
                    else
                        stop = true;
                    break;
                case MoveDirection.RIGHT:
                    if (repaint.oripos.X < Program.conswidth - 3)
                    {
                        step = repaint.oripos.X == Program.conswidth - 4;
                        repaint.oripos.X += 2;
                    }
                    else
                        stop = true;
                    break;
                case MoveDirection.DOWN:
                    if (repaint.oripos.Y < Program.consheight - 3)
                        repaint.oripos.Y++;
                    else
                        stop = true;
                    break;
                case MoveDirection.LEFT:
                    if (repaint.oripos.X > 2)
                    {
                        repaint.oripos.X -= 2;
                        step = false;
                    }
                    else
                        stop = true;
                    break;
                default:
                    break;
            }
            Tank hittarget = GetHitTank();
            if (hittarget != null)
            {
                lock (Program.hitlist)
                {
                    Program.hitlist.Add(hittarget);
                }
                stop = true;
            }
            if (!stop)
                repaint.WriteAt(paintchar.ToString(), 0, 0);
        }
        #endregion
        #region 清除上一次炮弹停留的位置
        /// <summary>
        /// 清除上一次炮弹停留的位置
        /// </summary>
        public override void ClearPartConsole()
        {
            switch (movedir)
            {
                case MoveDirection.DOWN:
                case MoveDirection.UP:
                    repaint.WriteAt("  ", 0, 0);
                    break;
                case MoveDirection.RIGHT:
                    repaint.WriteAt(step ? " " : "  ", 0, 0);
                    break;
                case MoveDirection.LEFT:
                    repaint.WriteAt(step ? "" : "  ", 0, 0);
                    break;
                default:
                    break;
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
                        b.Move();
                    bulletlist.RemoveAll(i => i.stop);
                }
            }
        }
        #endregion
        #region 获取炮弹命中的目标
        /// <summary>
        /// 获取炮弹命中的目标
        /// </summary>
        /// <returns></returns>
        public Tank GetHitTank()
        {
            foreach (Tank t in Program.robot.robotlist)
                if (t.IsHit(this))
                    return t;
            if (Program.newtank.IsHit(this))
                return Program.newtank;
            return null;
        }
        #endregion
    }
    #endregion

    #region 坐标类
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
    #endregion

    #region 重绘操作类
    /// <summary>
    /// 重绘操作类
    /// </summary>
    public class RepaintConsole
    {
        #region 成员变量
        /// <summary>
        /// 线程锁对象
        /// </summary>
        private static object lockobj = new object();
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
        public bool WriteAt(string s, int x, int y)
        {
            lock (lockobj)
            {
                try
                {
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(oripos.X + x, oripos.Y + y);//col, row
                    Console.Write(s);
                    return true;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        #endregion
    }
    #endregion

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
                int xpos = ran.Next(Program.conswidth - 7) + 3;
                int ypos = ran.Next(Program.consheight - 6) + 3;
                MoveDirection movdir = (MoveDirection)(ran.Next(4) + 1);
                robotlist.Add(new Tank(xpos, ypos, movdir, random ? RandomColor() : ConsoleColor.White));
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
                    PrintPoint tarpos = Program.newtank.repaint.oripos;
                    List<MoveDirection> dirlist = new List<MoveDirection>();
                    dirlist.Clear();
                    foreach (Tank robot in robotlist)
                    {
                        switch (ran.Next(2))
                        {
                            case 0:
                                if (tarpos.X > robot.repaint.oripos.X)
                                    dirlist.Add(MoveDirection.RIGHT);
                                else if (tarpos.X < robot.repaint.oripos.X)
                                    dirlist.Add(MoveDirection.LEFT);
                                if (tarpos.Y > robot.repaint.oripos.Y)
                                    dirlist.Add(MoveDirection.DOWN);
                                else if (tarpos.Y < robot.repaint.oripos.Y)
                                    dirlist.Add(MoveDirection.UP);
                                robot.movedir = dirlist[ran.Next(dirlist.Count)];
                                robot.Move();
                                break;
                            default:
                                lock (Program.bulletlist)
                                {
                                    Program.bulletlist.Add(new Bullet(robot.repaint.oripos.X, robot.repaint.oripos.Y, robot.movedir));
                                }
                                break;
                        }
                    }
                }
                catch
                {
                    stop = true;
                }
            }
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
                int count = Enum.GetNames(typeof(ConsoleColor)).Length;
                color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ran.Next(count).ToString());
            } 
            while (color == ConsoleColor.Black);
            return color;
        }
        #endregion
    }
    #endregion
}
