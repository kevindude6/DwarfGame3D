using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D.AI.Pathing
{
    public class Path
    {
        //Node start;
        //Node end;
        public List<Node> nodeList;
        public bool endsSolid = false;
        public Path()
        {
            nodeList = new List<Node>();
        }

        public override string ToString()
        {
            String s;
            s = "Path: ";
            foreach (Node n in nodeList)
            {
                s = s + n.ToString();
            }
            return s;
        }
    } 
}
