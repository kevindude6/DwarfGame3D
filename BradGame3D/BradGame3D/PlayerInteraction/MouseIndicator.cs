using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BradGame3D
{
    
    public class MouseIndicator
    {
        private static float sizeOffset = 0.55f;
        private static Vector3 frontTopLeft = new Vector3(-sizeOffset, sizeOffset, -sizeOffset);
        private static Vector3 frontTopRight = new Vector3(sizeOffset, sizeOffset, -sizeOffset);
        private static Vector3 frontBottomLeft = new Vector3(-sizeOffset, -sizeOffset, -sizeOffset);
        private static Vector3 frontBottomRight = new Vector3(sizeOffset, -sizeOffset, -sizeOffset);

        private static Vector3 backTopLeft = new Vector3(-sizeOffset, sizeOffset, sizeOffset);
        private static Vector3 backTopRight = new Vector3(sizeOffset, sizeOffset, sizeOffset);
        private static Vector3 backBottomLeft = new Vector3(-sizeOffset, -sizeOffset, sizeOffset);
        private static Vector3 backBottomRight = new Vector3(sizeOffset, -sizeOffset, sizeOffset);

        private static Vector2 BL = new Vector2(0, 1);
        private static Vector2 BR = new Vector2(1, 1);
        private static Vector2 TL = new Vector2(0, 0);
        private static Vector2 TR = new Vector2(1, 0);

        public Color mColor = Color.Yellow;
        
        Game1 game;
        VertexPositionColorTexture[] verts = new VertexPositionColorTexture[36];
        public Vector3 position = new Vector3(0,0,0);
        public Texture2D myTex;
        public MouseIndicator(Game1 g)
        {
            game = g;
            myTex = g.Content.Load<Texture2D>("mouseIndicator");

            setPosition(new Vector3(0,0,0));
        }
        public void setPosition(Vector3 pos)
        {
            position = pos;
            //bottom
            verts[0] = new VertexPositionColorTexture(position + frontBottomLeft, mColor, TR);
            verts[1] = new VertexPositionColorTexture(position + backBottomLeft, mColor, BR);
            verts[2] = new VertexPositionColorTexture(position + backBottomRight, mColor, BL);
           

            verts[3] = new VertexPositionColorTexture(backBottomRight + position, mColor, BL);
            verts[4] = new VertexPositionColorTexture(frontBottomRight + position, mColor, TL);
            verts[5] = new VertexPositionColorTexture(frontBottomLeft + position, mColor, TR);

            //top
            verts[6] = new VertexPositionColorTexture(backTopRight + position, mColor, BR);
            verts[7] = new VertexPositionColorTexture(backTopLeft + position, mColor, BL);
            verts[8] = new VertexPositionColorTexture(frontTopLeft + position, mColor, TL);

            verts[9] = new VertexPositionColorTexture(frontTopLeft + position, mColor, TL);
            verts[10] = new VertexPositionColorTexture(frontTopRight + position, mColor, TR);
            verts[11] = new VertexPositionColorTexture(backTopRight + position, mColor, BR);

            //front
            verts[12] = new VertexPositionColorTexture(frontTopLeft + pos, mColor,TL);
            verts[13] = new VertexPositionColorTexture(frontBottomLeft + pos, mColor, BL);
            verts[14] = new VertexPositionColorTexture(frontTopRight + pos, mColor, TR);

            verts[15] = new VertexPositionColorTexture(frontBottomLeft + pos, mColor, BL);
            verts[16] = new VertexPositionColorTexture(frontBottomRight + pos, mColor, BR);
            verts[17] = new VertexPositionColorTexture(frontTopRight + pos, mColor, TR);

            //back
            verts[18] = new VertexPositionColorTexture(backTopRight + pos, mColor, TR);
            verts[19] = new VertexPositionColorTexture(backBottomRight + pos, mColor, BR);
            verts[20] = new VertexPositionColorTexture(backBottomLeft + pos, mColor, BL);

            verts[21] = new VertexPositionColorTexture(backTopRight + pos, mColor, TR);
            verts[22] = new VertexPositionColorTexture(backBottomLeft + pos, mColor, BL);
            verts[23] = new VertexPositionColorTexture(backTopLeft + pos, mColor, TL);

            //right
            verts[24] = new VertexPositionColorTexture(backTopLeft + pos,  mColor, TR);
            verts[25] = new VertexPositionColorTexture(backBottomLeft + pos, mColor, BR);
            verts[26] = new VertexPositionColorTexture(frontBottomLeft + pos, mColor, BL);

            verts[27] = new VertexPositionColorTexture(backTopLeft + pos, mColor, TR);
            verts[28] = new VertexPositionColorTexture(frontBottomLeft + pos, mColor, BL);
            verts[29] = new VertexPositionColorTexture(frontTopLeft + pos, mColor, TL);

            //left
            verts[30] = new VertexPositionColorTexture(frontTopRight + pos, mColor, TR);
            verts[31] = new VertexPositionColorTexture(frontBottomRight + pos, mColor, BR);
            verts[32] = new VertexPositionColorTexture(backTopRight + pos, mColor, TL);

            verts[33] = new VertexPositionColorTexture(frontBottomRight + pos, mColor, BR);
            verts[34] = new VertexPositionColorTexture(backBottomRight + pos, mColor, BL);
            verts[35] = new VertexPositionColorTexture(backTopRight + pos, mColor, TL);
        }

        public void Draw(GraphicsDeviceManager g)
        {
            g.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, verts, 0, verts.Length / 3);
        }
    }
}
