using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Blocks
{
    public class BlockDataManager
    {
        public BlockData[] blocks = new BlockData[256];
        int nextId = 0;
        public BlockDataManager()
        {
            
        }
        /*
        public void addBlock(string name, bool render, bool solid, Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Vector2 ttl, Vector2 ttr, Vector2 tbl, Vector2 tbr)
        {
            blocks[nextId] = new BlockData(nextId, name, render, solid, tl, tr, bl, br, ttl, ttr, tbl, tbr);
            nextId++;   
        }
         */
        public void addBlock(string name, bool render, bool solid, bool lightSource, int[] positions, int width, int sheetWidth)
        {
            Vector2[][] temp = new Vector2[6][];
            for(int i = 0; i < 6; i++)
            {
                temp[i] = new Vector2[4];
                float sw = sheetWidth;
                float spacing = width / sw;
                float xoffset = (positions[i]%(sw/width))*spacing;
                float yoffset =(float) (Math.Floor(positions[i] / (sw / width)) * spacing);
                
                
                temp[i][(int) BlockData.CORNER.TL] = new Vector2(xoffset,yoffset);
                temp[i][(int) BlockData.CORNER.TR] = new Vector2(xoffset+spacing,yoffset);
                temp[i][(int) BlockData.CORNER.BL] = new Vector2(xoffset,yoffset+spacing);
                temp[i][(int) BlockData.CORNER.BR] = new Vector2(xoffset+spacing,yoffset+spacing);
                
                /*
                temp[i][(int) BlockData.CORNER.TL] = new Vector2(0,0);
                temp[i][(int) BlockData.CORNER.TR] = new Vector2(0.125f,0);
                temp[i][(int) BlockData.CORNER.BL] = new Vector2(0,0.125f);
                temp[i][(int)BlockData.CORNER.BR] = new Vector2(0.125f, 0.125f);
                */
            }

            blocks[nextId] = new BlockData(nextId, name, render, solid,lightSource, temp);
            nextId++;
        }

    }
}
