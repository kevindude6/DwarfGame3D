using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BradGame3D.Art;

namespace BradGame3D.Entities
{
    public class BasicEntity
    {
        public Vector3 center;
        public float width = 1f;
        public float height = 1f;
        //bool verticesInitialized = false;
        public String currentAnim = "Idle";
        float animTime = 0;
        BillboardVertex[] vertices = new BillboardVertex[6];
        public SpriteSheet.Direction currentLook = SpriteSheet.Direction.RIGHT;

        public BasicEntity(Vector3 pos)
        {
            center = pos;

            vertices[0] = new BillboardVertex(pos, new Vector2(0, 0), new Vector2(-width/2, height/2));
            vertices[1] = new BillboardVertex(pos, new Vector2(1, 0), new Vector2(width/2, height/2));
            vertices[2] = new BillboardVertex(pos, new Vector2(0, 1), new Vector2(-width/2, -height/2));
            vertices[3] = vertices[2];
            vertices[4] = vertices[1];
            vertices[5] = new BillboardVertex(pos, new Vector2(1, 1), new Vector2(width/2, -height/2));
           

            //updateVertices();
            init();
        }
        public virtual void init()
        {

        }
        public bool collidesWithBlock(int x, int y, int z)
        {
            if (Math.Abs(center.X - x) < width / 2 + 0.5f)
                if (Math.Abs(center.Y - y) < height / 2 + 0.5f)
                    if (Math.Abs(center.Z - z) < width / 2 + 0.5f)
                        return true;
            return false;
        }
        public virtual void update(float gameTime, World2 w)
        {
            /*
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    int x = (int) Math.Round(center.X+a);
                    int y = (int) Math.Round(center.Y);
                    int z = (int) Math.Round(center.Z+b);
                    if (collidesWithBlock(x, y, z))
                    {
                        if(w.isSolid(x,y,z))
                        {
                            
                        }
                    }
                }
            }
             * */

            animTime += gameTime;
        }

        public virtual void updateVertices(SpriteSheet s)
        {
           
            int tile = s.getTile(currentAnim,currentLook,ref animTime);
            

            vertices[0] = new BillboardVertex(center, s.getTexCoords(tile,SpriteSheet.Corner.TL), new Vector2(-width / 2, height / 2));
            vertices[1] = new BillboardVertex(center, s.getTexCoords(tile, SpriteSheet.Corner.TR), new Vector2(width / 2, height / 2));
            vertices[2] = new BillboardVertex(center, s.getTexCoords(tile, SpriteSheet.Corner.BL), new Vector2(-width / 2, -height / 2));
            vertices[3] = vertices[2];
            vertices[4] = vertices[1];
            vertices[5] = new BillboardVertex(center, s.getTexCoords(tile, SpriteSheet.Corner.BR), new Vector2(width / 2, -height / 2));

        }
        public virtual void draw(GraphicsDeviceManager g, SpriteSheet s)
        {
            updateVertices(s);
            g.GraphicsDevice.DrawUserPrimitives<BillboardVertex>(PrimitiveType.TriangleList, vertices, 0, vertices.Length/3);
        }
    }
}
