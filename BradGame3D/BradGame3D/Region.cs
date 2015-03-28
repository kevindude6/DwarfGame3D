using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D
{
    public class Region
    {
        public Chunk[][] chunks = new Chunk[32][];
        Game1 game;
        public Region()
        {
            for (int i = 0; i < 32; i++)
            {
                chunks[i] = new Chunk[32];
            }
        }
        public Region(Game1 g)
        {
            game = g;
            for (int i = 0; i < 32; i++)
            {
                chunks[i] = new Chunk[32];
            }
        }
    }
}
