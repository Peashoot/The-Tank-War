using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace The_Tank_War
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args != null)
            {
                PublicObjects.RobotCount = 5;
                if (args.Length > 3)
                {
                    try
                    {
                        PublicObjects.RobotCount = Convert.ToInt32(args[2]);
                    }
                    catch { }
                }
                PublicObjects.ViewHeight = 40;
                if (args.Length > 2)
                {
                    try
                    {
                        PublicObjects.ViewHeight = Convert.ToInt32(args[1]);
                    }
                    catch { }
                }
                PublicObjects.ViewWidth = 200;
                if (args.Length > 1)
                {
                    try
                    {
                        PublicObjects.ViewHeight = Convert.ToInt32(args[0]);
                    }
                    catch { }
                }
            }
            Functions.StartGame(PublicObjects.ViewWidth, PublicObjects.ViewHeight, PublicObjects.RobotCount);
        }
    }
}
