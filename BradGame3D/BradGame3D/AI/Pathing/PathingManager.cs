using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace BradGame3D.AI.Pathing
{
    public struct PathData
    {
        public Vector3 start;
        public Vector3 end;
        public Entities.LivingEntity b;
    }
    public static class PathingManager
    {

        public static World2 w;
        public static List<PathData> data = new List<PathData>();
        private static Object myLock = new Object();
        
        public static void findPaths()
        {
            
            while (true)
            {
                if (data.Count() > 0)
                {
                    lock (myLock)
                    {

                        PathData p = data[0];
                        p.b.followPath(Pathing.findPath(p.start, p.end, w));
                        data.RemoveAt(0);
                        Thread.Sleep(20);
                    }
                }
                else
                    Thread.Sleep(50);
                
            }
        }
    }
}
