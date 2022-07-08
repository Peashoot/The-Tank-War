using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace The_Tank_War
{
    public class ConsoleWriter
    {
        /// <summary>
        /// 显示字符在控制台上
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="xloc">横向偏移字符数</param>
        /// <param name="yloc">纵向偏移行数</param>
        /// <param name="color">显示字符的颜色</param>
        public void ConsoleWrite(string str, int xloc, int yloc, ConsoleColor color = ConsoleColor.White)
        {
            var insert = new Action(() =>
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(xloc, yloc);
                Console.Write(str);
            });
            var current = thisActionId;
            dicActions[current] = insert;
            startWrite = true;
        }

        #region private

        /// <summary>
        /// 事件字典
        /// </summary>
        private ConcurrentDictionary<uint, Action> dicActions = new ConcurrentDictionary<uint, Action>();

        /// <summary>
        /// 当前事件id号
        /// </summary>
        private uint actionId;

        /// <summary>
        /// 当前事件id号
        /// </summary>
        private uint thisActionId
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { unchecked { return actionId++; } }
        }

        /// <summary>
        /// 循环写
        /// </summary>
        private bool writeEnable;

        private bool startWrite
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                //当启动状态从false变成true时，启动循环写方法
                if (!writeEnable && value) { Task.Factory.StartNew(ActuallyWrite); }
                writeEnable = value;
            }
            get { return writeEnable; }
        }

        /// <summary>
        /// 已执行的事件id
        /// </summary>
        private uint executed;

        /// <summary>
        /// 写到控制台
        /// </summary>
        private void ActuallyWrite()
        {
            while (dicActions.Count > 0)
            {
                Action next = null;
                if (dicActions.TryRemove(executed++, out next))
                {
                    try
                    {
                        next();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            startWrite = false;
        }

        #endregion private
    }

    public class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return _instance = _instance ?? new T(); }
        }
    }
}