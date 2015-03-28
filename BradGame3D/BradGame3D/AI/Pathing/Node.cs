using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.AI.Pathing
{
    public class Node
    {
        public int x;
        public int y;
        public int z;
        public Node(int tx, int ty, int tz)
        {
            x = tx;
            y = ty;
            z = tz;
        }
        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }
        public float distTo(Vector3 a)
        {
            double xdist = Math.Pow((float)x - a.X, 2);
            double ydist = Math.Pow((float)y - a.Y, 2);
            double zdist = Math.Pow((float)z - a.Z, 2);
            return (float) Math.Sqrt(xdist+ydist+zdist);
        }
    }
}
