using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BradGame3D.Entities;

namespace BradGame3D.Art
{
    public class SpriteSheetEnhanced
    {
        public List<Entities.BasicEntity> ents = new List<Entities.BasicEntity>();
        World2 w;
        GameScreen g;
        public SpriteSheet s;
        public SpriteSheetEnhanced(World2 tw, GameScreen tg, SpriteSheet ts)
        {
            w = tw;
            g = tg;
            s = ts;
        }
        public void updateEnts(GameTime g)
        {
            foreach (BasicEntity b in ents)
                b.update(g.ElapsedGameTime.Milliseconds, w);
        }
        public void draw(GraphicsDeviceManager g)
        {
            foreach (BasicEntity b in ents)
            {
                if(b.parentChunk.isVisible)
                    b.draw(g, this.s);
            }
        }
        public void addEnt(BasicEntity b)
        {
            ents.Add(b);
        }
    }

    
}
