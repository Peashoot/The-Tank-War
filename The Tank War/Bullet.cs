using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Tank_War
{
    #region 炮弹类
    /// <summary>
    /// 炮弹类
    /// </summary>
    public class Bullet : PaintModel, IMoveModel
    {
        #region 成员变量
        /// <summary>
        /// 炮弹清除原来位置判断是否清除两位
        /// </summary>
        public bool erase = false;
        /// <summary>
        /// 炮弹停止运动标志位
        /// </summary>
        public bool stop = false;
        /// <summary>
        /// 运动方向
        /// </summary>
        public MoveDirection movedir { get; set; }
        /// <summary>
        /// 来自某坦克
        /// </summary>
        public Tank from { get; set; }
        #endregion
        #region 构造函数
        public Bullet(int X, int Y, MoveDirection movedir, Tank from)
            : base(X, Y, ConsoleColor.White)
        {
            this.movedir = movedir;
            paintchar = 'o';
            this.from = from;
            switch (movedir)
            {
                case MoveDirection.UP:
                    repaint.oripos.Y -= 2;
                    break;
                case MoveDirection.DOWN:
                    repaint.oripos.Y += 2;
                    break;
                case MoveDirection.RIGHT:
                    erase = false;
                    repaint.oripos.X += 3;
                    break;
                case MoveDirection.LEFT:
                    erase = true;
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
        public void Move(MoveDirection movedir, int step = 1)
        {
            this.movedir = movedir;
            ClearPartConsole();
            while (step > 0)
            {
                switch (this.movedir)
                {
                    case MoveDirection.UP:
                        if (repaint.oripos.Y > 1)
                        {
                            repaint.oripos.Y--;
                        }
                        else
                        {
                            stop = true;
                        }
                        break;
                    case MoveDirection.RIGHT:
                        if (repaint.oripos.X < PublicObjects.ViewWidth - 3)
                        {
                            erase = repaint.oripos.X == PublicObjects.ViewWidth - 4;
                            repaint.oripos.X += 2;
                        }
                        else
                        {
                            stop = true;
                        }
                        break;
                    case MoveDirection.DOWN:
                        if (repaint.oripos.Y < PublicObjects.ViewHeight - 3)
                        {
                            repaint.oripos.Y++;
                        }
                        else
                        {
                            stop = true;
                        }
                        break;
                    case MoveDirection.LEFT:
                        if (repaint.oripos.X > 2)
                        {
                            repaint.oripos.X -= 2;
                            erase = false;
                        }
                        else
                        {
                            stop = true;
                        }
                        break;
                    default:
                        break;
                }

                Tank hittarget = GetHitTank();
                if (hittarget != null)
                {
                    lock (PublicObjects.HittedList)
                    {
                        PublicObjects.HittedList.Add(hittarget);
                    }
                    stop = true;
                }
                if (!stop)
                {
                    repaint.WriteAt(paintchar.ToString());
               }
                step--;
            }
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
                    repaint.WriteAt("  ");
                    break;
                case MoveDirection.RIGHT:
                    repaint.WriteAt(erase ? " " : "  ");
                    break;
                case MoveDirection.LEFT:
                    repaint.WriteAt(erase ? "" : "  ");
                    break;
                default:
                    break;
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
            foreach (Tank t in PublicObjects.Robot.robotlist)
            {
                if (t.IsHit(this))
                {
                    return t;
                }
            }
            if (PublicObjects.GamerTank1.IsHit(this))
            {
                return PublicObjects.GamerTank1;
            }
            if (PublicObjects.GamerTank2 != null && PublicObjects.GamerTank2.IsHit(this))
            {
                return PublicObjects.GamerTank2;
            }
            return null;
        }
        #endregion
    }
    #endregion
}
