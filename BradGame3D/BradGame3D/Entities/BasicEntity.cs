using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BradGame3D.Art;
using System.Diagnostics;

namespace BradGame3D.Entities
{
    public class BasicEntity
    {
        public Chunk parentChunk;

        public float lastX;
        public float lastY;
        public float lastZ;
        public Vector3 center;
        public float width = 1f;
        public float height = 1f;
        protected Vector3 netF = new Vector3();

        public bool isOnGround = false;
        public Vector3 velocity = new Vector3(0, 0, 0);
        public float mass = 75;

        //bool verticesInitialized = false;
        public String currentAnim = "Idle";
        float animTime = 0;
        BillboardVertex[] vertices = new BillboardVertex[6];
        public SpriteSheet.Direction currentLook = SpriteSheet.Direction.RIGHT;

        public BasicEntity(Vector3 pos)
        {
            center = pos;
            lastX = center.X;
            lastY = center.Y;
            lastZ = center.Z;
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
        public void addForce(Vector3 force)
        {
            netF += force;
        }
        protected virtual void calcForces()
        {

        }
        protected virtual void calcVelocity(float gameTime)
        {
            velocity += (netF / mass)*gameTime/1000;
            netF = Vector3.Zero;
            center += velocity * gameTime/1000;
        }
        public bool collidesWithBlock(int x, int y, int z)
        {
            float xPen = 9999;
            float yPen = 9999;
            float zPen = 9999;
            if (Math.Abs(center.X - x) < width / 2 + 0.5f)
            {
                xPen = (width / 2 + 0.5f) - (Math.Abs(center.X - x));
            }
            if (Math.Abs(center.Y - y) < height / 2 + 0.5f)
            {
                yPen = (height / 2 + 0.5f) - (Math.Abs(center.Y- y));
            }
            if (Math.Abs(center.Z - z) < width / 2 + 0.5f)
            {
                zPen = (width / 2 + 0.5f) - (Math.Abs(center.Z - z));
            }

            if (xPen == 9999 && yPen == 9999 && zPen == 9999)
            {
                return false;
            }
            else
            {
                if (xPen <= yPen && xPen <= zPen)
                {
                    //center.X = lastX;
                    if (center.X < x)
                        center.X = x - 0.5f - width / 2;
                    else
                        center.X = x + 0.5f + width / 2;
                    velocity.X = 0;
                    return true;
                }
                else if (yPen <= xPen && yPen <= zPen)
                {
                    //center.Y = lastY;
                    if (center.Y > y)
                        center.Y = y + 0.5f + height / 2;
                    else
                        center.Y = y - 0.5f - height / 2;
                    velocity.Y = 0;
                    
                    return true;
                }
                else if (zPen <= yPen && zPen <= xPen)
                {
                    //center.Z = lastZ;
                    if (center.Z > z)
                        center.Z = z + 0.5f + width / 2;
                    else
                        center.Z = z - 0.5f - width/2;
                    velocity.Z = 0;
                    return true;
                }
            }
            return false;
            
        }
        public void doCollisions(World2 w)
        {
            /*
            int x = (int) Math.Round(center.X);
            int y = (int) Math.Round(center.Y);
            int z = (int) Math.Round(center.Z);
            */
            int x = (int)Math.Round(center.X);
            int y = (int)Math.Round(center.Y);
            int z = (int)Math.Round(center.Z);
           
            /*
            if (w.isSolid(x, y, z))
            {
                if (collidesWithBlock(x, y, z))
                {
                    isOnGround = true;
                }
                else
                    isOnGround = false;
            }
            else
                isOnGround = false;
            */

            //BLOCK BELOW
            if (Math.Abs(center.Y - (y - 1)) < height / 2 + 0.5f)
            {
                if(w.isSolid(x,y-1,z))
                {
                  
                    center.Y = (y - 1) + height / 2 + 0.5f;
                    velocity.Y = 0;
                    isOnGround = true;
                }
                else
                    isOnGround = false;
            }
            else
                isOnGround = false;
            

            // X - 1
            if (Math.Abs(center.X - (x - 1)) < width / 2 + 0.5f)
            {
                if(w.isSolid(x-1,y,z))
                {
                    center.X = (x - 1) + width / 2 + 0.5f;
                    velocity.X = 0;
                }
            }

            // X + 1
            if (Math.Abs(center.X - (x + 1)) < width / 2 + 0.5f)
            {
                if (w.isSolid(x + 1, y, z))
                {
                    center.X = (x + 1) - width / 2 - 0.5f;
                    velocity.X = 0;
                }
            }
            
            // Z -1
            if (Math.Abs(center.Z - (z-1)) < width / 2 + 0.5f)
            {
                if (w.isSolid(x, y, z-1))
                {
                    center.Z = (z - 1) + width / 2 + 0.5f;
                    velocity.Z = 0;
                }
            }
            // Z + 1
            if (Math.Abs(center.Z - (z + 1)) < width / 2 + 0.5f)
            {
                if (w.isSolid(x, y, z + 1))
                {
                    center.Z = (z + 1) - width / 2 - 0.5f;
                    velocity.Z = 0;
                }
            }
             
         
         
             
        }
        public virtual void update(float gameTime, World2 w)
        {
            if (parentChunk == null)
            {
                w.addEntToChunk(this);
            }
            doCollisions(w);
            
            if ((int) (center.X / Chunk.xSize) != (int) (lastX / Chunk.xSize) || (int) (center.Z / Chunk.zSize) != (int) (lastZ / Chunk.zSize))
            {
                if(parentChunk!=null)
                    parentChunk.ents.Remove(this);
                w.addEntToChunk(this);
               // Debug.WriteLine("Ent moved over");
            }
            lastX = center.X;
            lastY = center.Y;
            lastZ = center.Z;
            animTime += gameTime;


            calcForces();

            calcVelocity(gameTime);
            
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
