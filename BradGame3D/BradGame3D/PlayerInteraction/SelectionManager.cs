using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.PlayerInteraction
{
    public class SelectionManager
    {
        public enum JOBTYPE {DIG,CHOP};
        public struct SelectionData
        {
            public int x;
            public int y;
            public int z;
            public JOBTYPE job;
            public MouseIndicator m;
        }

        public List<SelectionData> selections = new List<SelectionData>();
        public GameScreen gameScreen;
        public World2 world;
        public SelectionManager(GameScreen g, World2 w)
        {
            gameScreen = g;
            world = w;
        }
        public void addSelection(int xa, int ya, int za, int xb, int yb, int zb, JOBTYPE j)
        {
            int xSign = Math.Sign(xb - xa); if (xSign == 0) xSign = 1;
            int ySign = Math.Sign(yb - ya); if (ySign == 0) ySign = 1;
            int zSign = Math.Sign(zb - za); if (zSign == 0) zSign = 1;
            for (int x = xa; x != xb + xSign; x += 1 * xSign)
            {
                for (int z = za; z != zb + zSign; z += 1 * zSign)
                {
                    for (int y = ya; y != yb + ySign; y += 1 * ySign)
                    {
                        if (world.isRender(x, y, z))
                        {
                            MouseIndicator tempInd = new MouseIndicator(gameScreen.game);
                            tempInd.setPosition(new Vector3(x, y, z));
                            SelectionData s;
                            s.x = x;
                            s.y = y;
                            s.z = z;
                            s.job = j;
                            s.m = tempInd;
                            selections.Add(s);
                            //w.setBlockData(0, (int) Chunk.DATA.ID, new Vector3(x,y,z));
                        }
                    }
                }
            }
        }
    }
}
