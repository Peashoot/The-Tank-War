using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace The_Tank_War
{
    #region 坦克类
    /// <summary>
    /// 坦克类
    /// </summary>
    public class Tank : PaintModel, IMoveModel
    {
        #region 构造函数
        public Tank(int X, int Y, MoveDirection movdir, ConsoleColor color = ConsoleColor.White)
            : base(X, Y, color)
        {
            this.movedir = movedir;
            ClearPartConsole();
            paintchar = '@';
            ReDrawShape();
        }
        #endregion

        public string group { get; set; }
        public MoveDirection movedir { get; set; }

        private AutoResetEvent waitHandle = new AutoResetEvent(false);
        #region 坦克移动
        /// <summary>
        /// 坦克移动
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
                        if (repaint.oripos.Y > 2)
                            repaint.oripos.Y--;
                        break;
                    case MoveDirection.RIGHT:
                        if (repaint.oripos.X < PublicObjects.ViewWidth - 5)
                            repaint.oripos.X += 2;
                        break;
                    case MoveDirection.DOWN:
                        if (repaint.oripos.Y < PublicObjects.ViewHeight - 4)
                            repaint.oripos.Y++;
                        break;
                    case MoveDirection.LEFT:
                        if (repaint.oripos.X > 4)
                            repaint.oripos.X -= 2;
                        break;
                    default:
                        break;
                }
                step--;
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
                    repaint.WriteAt(new string(paintchar, 1), 0, -1);
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
                    repaint.WriteAt(new string(paintchar, 1), 0, 1);
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

        /// <summary>
        /// 射击
        /// </summary>
        public void Shot()
        {
            lock (PublicObjects.BulletList)
            {
                PublicObjects.BulletList.Add(new Bullet(repaint.oripos.X, repaint.oripos.Y, movedir, this));
            }
        }

        #region 清除上一次坦克所在的位置
        /// <summary>
        /// 清除上一次坦克所在的位置
        /// </summary>
        public override void ClearPartConsole()
        {
            for (int i = 0; i < 3; i++)
            {
                repaint.WriteAt(new string(' ', 5), -2, i - 1);
            }
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
            //如果是同一类的，就忽略不计
            if (group.Equals(movebullet.from.group)) { return false; }
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
        #region 单个坦克爆炸
        /// <summary>
        /// 坦克爆炸
        /// </summary>
        public void Explose()
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
                waitHandle.WaitOne(100);
            }
            ClearPartConsole();
        }
        #endregion
    }
    #endregion
}
