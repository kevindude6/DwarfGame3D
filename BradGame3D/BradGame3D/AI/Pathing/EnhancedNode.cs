using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D.AI.Pathing
{
    public class EnhancedNode
    {
        public int x;
        public int y;
        public int z;
        public float cost;
        public float hscore;
        public EnhancedNode parent;

        public EnhancedNode(int tx, int ty, int tz)
        {
            x = tx;
            y = ty;
            z = tz;
        }
        public void setXYZ(int tx, int ty, int tz)
        {
            x = tx;
            y = ty;
            z = tz;
        }
        public Node toNode()
        {
            return new Node(x, y, z);
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            EnhancedNode n = obj as EnhancedNode;
            if ((System.Object)n == null)
            {
                Node a = obj as Node;
                if ((System.Object)a == null)
                {
                    return false;
                }
                else
                    return Equals(a);

            }
            else
                return Equals(n);

            // Return true if the fields match:
            
        }
        public bool Equals(EnhancedNode n)
        {
            // Return true if the fields match:
            return x == n.x && y == n.y && z == n.z;
        }
        public bool Equals(Node n)
        {
            // Return true if the fields match:
            return x == n.x && y == n.y && z == n.z;
        }
        public override int GetHashCode()
        {
            return x * y * z;
        }
    }
}
