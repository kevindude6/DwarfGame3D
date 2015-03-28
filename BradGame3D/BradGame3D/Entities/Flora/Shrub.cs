using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.Entities.Flora
{
    public class Shrub : Plant
    {
        public static float texWidth = (float) (128.0/1024.0);
        public static float texHeight = (float) (128.0/1024.0);
        public static float xOff = texWidth;
        public static float yOff = 0;
        public static Vector2 texTopLeft = new Vector2(xOff, yOff);
        public static  Vector2 texTopRight = new Vector2(texWidth+xOff, yOff);
        public static  Vector2 texBottomLeft= new Vector2(xOff, texHeight+yOff);
        public static  Vector2 texBottomRight= new Vector2(texWidth+xOff, texHeight+yOff);

        public Shrub(Chunk c, int seed, Vector3 pos): base(c, seed, pos)
        {

        }
        public override void initVertices()
        {
            vertices = new VertexPositionColorTexture[24];
            float rotRad = (new Random().Next(0, 90)) / 57.29f;


            float x = width / 2;
            float z = length / 2;

            Quaternion q;
            q = Quaternion.CreateFromYawPitchRoll(rotRad, 0, 0);
            //Width
            vertices[0] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, height - 0.5f, 0), q), mColor, texTopLeft);
            vertices[1] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0), q), mColor, texBottomLeft);
            vertices[2] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0), q), mColor, texTopRight);

            vertices[3] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0), q), mColor, texBottomLeft);
            vertices[4] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, -0.5f, 0), q), mColor, texBottomRight);
            vertices[5] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0), q), mColor, texTopRight);

            vertices[8] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, height - 0.5f, 0), q), mColor, texTopLeft);
            vertices[7] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0), q), mColor, texBottomLeft);
            vertices[6] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0), q), mColor, texTopRight);

            vertices[11] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(-x, -0.5f, 0), q), mColor, texBottomLeft);
            vertices[10] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, -0.5f, 0), q), mColor, texBottomRight);
            vertices[9] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(x, height - 0.5f, 0), q), mColor, texTopRight);

            //Length
            vertices[12] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, -z), q), mColor, texTopLeft);
            vertices[13] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z), q), mColor, texBottomLeft);
            vertices[14] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z), q), mColor, texTopRight);

            vertices[15] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z), q), mColor, texBottomLeft);
            vertices[16] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, z), q), mColor, texBottomRight);
            vertices[17] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z), q), mColor, texTopRight);

            vertices[20] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, -z), q), mColor, texTopLeft);
            vertices[19] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z), q), mColor, texBottomLeft);
            vertices[18] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z), q), mColor, texTopRight);

            vertices[23] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, -z), q), mColor, texBottomLeft);
            vertices[22] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, -0.5f, z), q), mColor, texBottomRight);
            vertices[21] = new VertexPositionColorTexture(baseBlock + Vector3.Transform(new Vector3(0, height - 0.5f, z), q), mColor, texTopRight);




        }
        
        public override void genFromSeed()
        {
            Random r = new Random(rSeed);
            mColor = RandomFunctions.randSaturatedColor(r);
            height = RandomFunctions.Normal(r, 0.1f, 1); //6
            width = RandomFunctions.Normal(r, 0.1f, 1); //3
            length = width;
        }
    }
}
