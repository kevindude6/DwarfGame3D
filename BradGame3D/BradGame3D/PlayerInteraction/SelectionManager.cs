using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BradGame3D.Entities.Creatures;
using System.Diagnostics;

namespace BradGame3D.PlayerInteraction
{
    public class SelectionManager
    {
        public enum JOBTYPE {MINING, FARMING, CRAFTING, MASONRY, CARPENTRY};
        /*
        public struct SelectionData
        {
            public int x;
            public int y;
            public int z;
            public JOBTYPE job;
            public MouseIndicator m;
        }
        */
        public List<Selection> selections = new List<Selection>();
        public GameScreen gameScreen;
        public World2 world;
        public SelectionManager(GameScreen g, World2 w)
        {
            gameScreen = g;
            world = w;
        }
        public void workJobs()
        {
            while (1 == 1)
            {
                if (selections.Count() > 0)
                {
                    Selection s;
                    for(int j = 0; j < selections.Count; j++)
                    {
                        s = selections[j];
                        if (s != null)
                        {
                            if (s.jobsInSelection.Count != 0)
                            {
                                Selection.Job currentJob;
                                for (int i = 0; i < s.jobsInSelection.Count; i++)
                                {
                                    currentJob = s.jobsInSelection[i];
                                    if (accessibleBlock(currentJob.x, currentJob.y, currentJob.z))
                                    {
                                        Citizen a = findLeastBusyCitizen(currentJob);
                                        //Debug.WriteLine("Check 1");
                                        if (a != null)
                                        {

                                            s.jobsInSelection.RemoveAt(i);
                                            i--;
                                            a.tasks.Add(currentJob);
                                            //currentJob.m.updateColor(Color.Red);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                selections.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }
            }
        }
        public Citizen findLeastBusyCitizen(Selection.Job j)
        {
            Citizen h = null;
            float leastScore = 9999;
            foreach (Citizen temp in gameScreen.citizenList)
            {
                //Debug.WriteLine("Check 1: " + temp.jobEnabled[0]);
                if (temp.jobEnabled[(int)j.jobType] && temp.doingTask == false)
                {
                    //.WriteLine("Check 2");
                    float score = temp.tasks.Count() + temp.distTo(j);
                    if (score < leastScore)
                    {
                        leastScore = score;
                        h = temp;
                    }
                }
            }
            return h;
        }
        public bool accessibleBlock(int x, int y, int z)
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
            else if(!world.isSolid(x, y - 1, z) && world.isSolid(x,y-2,z) )
                return true;
            else
                return false;
        }
        public void addSelection(Vector3 a, Vector3 b, JOBTYPE j)
        {
            addSelection((int) a.X, (int) a.Y, (int) a.Z, (int) b.X, (int) b.Y, (int) b.Z, j);
        }
        public void addSelection(int xa, int ya, int za, int xb, int yb, int zb, JOBTYPE j)
        {
            Selection s =new Selection(new BoundingBox(new Vector3(xa,ya,za),new Vector3(xb,yb,zb)),j);

            int xSign = Math.Sign(xb - xa); if (xSign == 0) xSign = 1;
            int ySign = Math.Sign(yb - ya); if (ySign == 0) ySign = 1;
            int zSign = Math.Sign(zb - za); if (zSign == 0) zSign = 1;
            for (int y = ya; y != yb + ySign; y += 1 * ySign)
            {
                for (int x = xa; x != xb + xSign; x += 1 * xSign)
                {
                    for (int z = za; z != zb + zSign; z += 1 * zSign)
                    {

                        if (world.isRender(x, y, z))
                        {
                            MouseIndicator tempInd = new MouseIndicator(gameScreen.game);
                            tempInd.setPosition(new Vector3(x, y, z));
                            Selection.Job tempJob;
                            tempJob.x = x;
                            tempJob.y = y;
                            tempJob.z = z;
                            tempJob.jobType = j;
                            tempJob.m = tempInd;
                            s.addJob(tempJob);
                            //w.setBlockData(0, (int) Chunk.DATA.ID, new Vector3(x,y,z));
                        }

                    }
                }
            }
            selections.Add(s);
        }
    }
}
