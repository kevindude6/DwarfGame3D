﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BradGame3D.AI.Pathing
{

    public class DuplicateKeyComparer<TKey>:IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }

        #endregion
    }

    public class Pathing
    {
      //  public static EnhancedNode[] temp = new EnhancedNode[26];
        public static EnhancedNode[] temp = new EnhancedNode[6];
        public static Path findPath(Vector3 start, Vector3 end, World2 w)
        {
            return findPath(new Node((int)Math.Round(start.X), (int)Math.Round(start.Y), (int)Math.Round(start.Z)), new Node((int)Math.Round(end.X), (int)Math.Round(end.Y), (int)Math.Round(end.Z)), w);
        }
        public static Path findPath(Node start, Node end, World2 w)
        {
            SortedList<float, EnhancedNode> openList = new SortedList<float, EnhancedNode>(new DuplicateKeyComparer<float>());
            List<EnhancedNode> closedList = new List<EnhancedNode>();

            //if (GameScreen.blockDataManager.blocks[(int)w.getBlockData((int)Chunk.DATA.ID, end.x, end.y, end.z)].getSolid())
            
            if(!accessibleBlock(end.x,end.y,end.z,w))
            {
                return null;
            }
            if (w.isSolid(end.x, end.y - 2, end.z) == false && w.isSolid(end.x,end.y-1,end.z)==false)
            {
                return null;
            }
            EnhancedNode st = new EnhancedNode(start.x, start.y, start.z);
            st.cost = 0;
            st.parent = null;
            openList.Add(st.cost, st);
            bool finding = true;
            EnhancedNode tempEnd = null ;
            int count = 0;
            Path p = new Path();
            if (w.isSolid(end.x, end.y, end.z)) p.endsSolid = true;

            //Debug.WriteLine("Start = " + st.toNode().ToString());
           // Debug.WriteLine("End = " + end.ToString());
            
            while (finding)
            {
                if (openList.Count() == 0)
                {
                    finding = false;
                    return null;
                }
                EnhancedNode current = openList.First().Value;
                if (current.Equals(end) && p.endsSolid == false)
                {
                    finding = false;
                    tempEnd = current;
                }
                else if (getFlatHeuristic(current, end) == 1 && p.endsSolid == true)
                {
                    finding = false;
                    tempEnd = current;
                }

                else if (count > 25000) //EARLY EXIT 14000
                {
                    //Debug.WriteLine("Early exit");
                    finding = false;
                    tempEnd = current;
                    //find closest
                    float min = 99999999;

                    foreach (EnhancedNode n in openList.Values)
                    {
                        if (getYModdedHeuristic(n, end) < min)
                        {
                            min = getYModdedHeuristic(n, end);
                            tempEnd = n;
                        }
                    }
                    Node tnode = new Node(tempEnd.x, tempEnd.y, tempEnd.z);
                    return findPath(start, tnode, w);
                    //Debug.WriteLine(n);
                }
                else
                {
                    getNeighbors(current);
                    foreach (EnhancedNode t in temp)
                    {
                        EnhancedNode n = new EnhancedNode(t.x, t.y, t.z);

                        if (!GameScreen.blockDataManager.blocks[(int)w.getBlockData((int)Chunk.DATA.ID, n.x, n.y, n.z)].getSolid() && (w.isSolid(n.x, n.y - 1, n.z) || w.isSolid(n.x, n.y - 2, n.z)))
                        {
                            float newCost = current.cost + getHeuristic(current, n.toNode());

                            if (openList.ContainsValue(n) == false)
                            {
                                n.cost = newCost;
                                n.parent = current;
                                //Debug.WriteLine("Adding node " + n.toNode().ToString() + " with cost " + n.cost + " and priority " + (newCost + getHeuristic(n, end)));
                                openList.Add(newCost + getHeuristic(n, end), n);

                            }
                            if (newCost < openList.ElementAt(openList.IndexOfValue(n)).Value.cost)
                            {
                                openList.RemoveAt(openList.IndexOfValue(n));
                                n.parent = current;
                                n.cost = newCost;
                                //Debug.WriteLine("Replacing node " + n.toNode().ToString() + " with cost " + n.cost + " and priority " + (newCost + getHeuristic(n, end)));
                                openList.Add(newCost + getHeuristic(n, end), n);
                            }
                            count++;
                        }
                    }
                    openList.RemoveAt(openList.IndexOfValue(current));
                }
            }
          
          
            p.nodeList.Add(tempEnd.toNode());
           // EnhancedNode t = tempEnd.parent;
            while (tempEnd.Equals(st) != true)
            {
                tempEnd = tempEnd.parent;
                p.nodeList.Add(tempEnd.toNode());
            }
            p.nodeList.Reverse();
            //Debug.WriteLine("count: " + count);
            
            return p;
            
        }
        public static float getFlatHeuristic(EnhancedNode a, Node b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
            //return (float)Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2) + Math.Pow((a.z - b.z), 2));
           
        }
        public static float getYModdedHeuristic(EnhancedNode a, Node b)
        {
            //return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
            return (float)Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 3) + Math.Pow((a.z - b.z), 2));
        }
        public static float getHeuristic(EnhancedNode a, Node b)
        {
            //return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
            return (float) Math.Sqrt(Math.Pow((a.x - b.x),2) + Math.Pow((a.y - b.y),2) + Math.Pow((a.z - b.z),2));
        }
        public EnhancedNode getMin(ref List<EnhancedNode> nodes)
        {
            float min = 99999;
            EnhancedNode temp = null;
            foreach (EnhancedNode n in nodes) 
            {
                if (n.cost < min)
                {
                    min = n.cost;
                    temp = n;
                }
            }
            return temp;
        }
        public static EnhancedNode[] getNeighbors(EnhancedNode n)
        {
            //EnhancedNode[] temp = new EnhancedNode[6];
            if (temp[0] == null)
            {
                temp[0]= new EnhancedNode(n.x + 1, n.y, n.z);
                temp[1]= new EnhancedNode(n.x - 1, n.y, n.z);
                temp[2]= new EnhancedNode(n.x, n.y + 1, n.z);
                temp[3]= new EnhancedNode(n.x, n.y - 1, n.z);
                temp[4]= new EnhancedNode(n.x, n.y, n.z + 1);
                temp[5]= new EnhancedNode(n.x, n.y, n.z - 1);
                /*
                int count = 0;
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            if ((x == 0 && y == 0 && z == 0) || (x!=0 && z!= 0))
                            {
                            }
                            else
                            {
                                temp[count] = new EnhancedNode(n.x + x, n.y + y, n.z + z);
                                count++;
                            }
                        }
                    }
                }
                 */
                 
            }
            else
            {
                /*
                int count = 0;
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            if ((x == 0 && y == 0 && z == 0) || (x!=0 && z!= 0))
                            {
                            }
                            else
                            {
                                temp[count].setXYZ(n.x + x, n.y + y, n.z + z);
                                count++;
                            }
                        }
                    }
                }
                */
                
                temp[0].setXYZ(n.x + 1, n.y, n.z);
                temp[1].setXYZ(n.x - 1, n.y, n.z);
                temp[2].setXYZ(n.x, n.y + 1, n.z);
                temp[3].setXYZ(n.x, n.y - 1, n.z);
                temp[4].setXYZ(n.x, n.y, n.z + 1);
                temp[5].setXYZ(n.x, n.y, n.z - 1);
                /*
                temp[6].setXYZ(n.x + 1, n.y, n.z + 1);
                temp[7].setXYZ(n.x + 1, n.y, n.z - 1);
                temp[8].setXYZ(n.x - 1, n.y, n.z + 1);
                temp[9].setXYZ(n.x - 1, n.y, n.z - 1);
                */
            }
            //Debug.WriteLine("Created 6 more nodes");
            return temp;
        }
        public static bool accessibleBlock(int x, int y, int z, World2 world)
        {
            if (!world.isSolid(x - 1, y, z))
                return true;
            else if (!world.isSolid(x + 1, y, z))
                return true;
            else if (!world.isSolid(x, y, z + 1))
                return true;
            else if (!world.isSolid(x, y, z - 1))
                return true;
            else if (!world.isSolid(x, y + 1, z))
                return true;
            else if (!world.isSolid(x, y - 1, z) && world.isSolid(x, y - 2, z))
                return true;
            else
                return false;
        }
    }
}
