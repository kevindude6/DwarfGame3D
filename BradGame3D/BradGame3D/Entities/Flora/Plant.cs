using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.Entities.Flora
{
    abstract public class Plant
    {
        public float height = 1f; //Y
        public float width = 1f; //X
        public float length = 1f; // Z
        public int rSeed = 0;
        public Vector3 baseBlock = new Vector3(0,0,0);
        public bool faceme = false;
        public bool collideable = true;
        protected Texture2D tex;
        protected VertexPositionColorTexture[] vertices;
        public Color mColor = Color.Red; 
        protected Chunk c;

        public Plant()
        {

        }
        public Plant(Chunk tchunk, int seed, Vector3 pos)
        {
            c = tchunk;
            rSeed = seed;
            baseBlock = pos;
            genFromSeed();
            initVertices();
            setPlaceholders();
            
        }
        public void setTexture(Texture2D temp)
        {
            tex = temp;
        }
        public void setPlaceholders()
        {
            for (int i = (int) baseBlock.Y; i < baseBlock.Y + (float)(Math.Round(height)); i++)
            {
                c.world.setBlockData(6, (int) Chunk.DATA.ID, baseBlock + new Vector3(0,i,0));
            }
        }
        public abstract void genFromSeed();
        public virtual void initVertices()
        {
            vertices = new VertexPositionColorTexture[24];
            float rotRad = (new Random().Next(0, 90))/57.29f;


            float x = width / 2;
            float z = length / 2;

            Quaternion q;
            q = Quaternion.CreateFromYawPitchRoll(rotRad, 0, 0);
            //Width
            vertices[0] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x,height-0.5f,0),q),mColor, new Vector2(0f,0f));
            vertices[1] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x,-0.5f, 0),q), mColor, new Vector2(0f, 1f));
            vertices[2] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height-0.5f,0),q), mColor, new Vector2(1f, 0f));
            
            vertices[3] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0),q), mColor, new Vector2(0f, 1f));
            vertices[4] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, -0.5f, 0),q), mColor, new Vector2(1f, 1f));
            vertices[5] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f,0),q), mColor, new Vector2(1f, 0f));

            vertices[8] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, height - 0.5f, 0),q), mColor, new Vector2(0f, 0f));
            vertices[7] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0),q), mColor, new Vector2(0f, 1f));
            vertices[6] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0),q), mColor, new Vector2(1f, 0f));

            vertices[11] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0),q), mColor, new Vector2(0f, 1f));
            vertices[10] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, -0.5f, 0),q), mColor, new Vector2(1f, 1f));
            vertices[9] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0),q), mColor, new Vector2(1f, 0f));

            //Length
            vertices[12] = new VertexPositionColorTexture(baseBlock+ Vector3.Transform(new Vector3(0,height-0.5f,-z),q),mColor, new Vector2(0f,0f));
            vertices[13] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0,-0.5f, -z),q), mColor, new Vector2(0f, 1f));
            vertices[14] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height-0.5f,z),q), mColor, new Vector2(1f, 0f));
            
            vertices[15] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z),q), mColor, new Vector2(0f, 1f));
            vertices[16] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, z),q), mColor, new Vector2(1f, 1f));
            vertices[17] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f,z),q), mColor, new Vector2(1f, 0f));

            vertices[20] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, -z),q), mColor, new Vector2(0f, 0f));
            vertices[19] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z),q), mColor, new Vector2(0f, 1f));
            vertices[18] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z),q), mColor, new Vector2(1f, 0f));

            vertices[23] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z),q), mColor, new Vector2(0f, 1f));
            vertices[22] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, z),q), mColor, new Vector2(1f, 1f));
            vertices[21] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z),q), mColor, new Vector2(1f, 0f));


            

        }
        public void addVertices(ref List<VertexPositionColorTexture> temp)
        {
            for (int i = 0; i < vertices.Length; i++)
                temp.Add(vertices[i]);
        }
        public void draw(GraphicsDeviceManager g)
        {

            if (!faceme)
            {
                    
                //e.Texture = tex;
                //g.GraphicsDevice.
                g.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
            else
            {
            }

        }
    }
}
