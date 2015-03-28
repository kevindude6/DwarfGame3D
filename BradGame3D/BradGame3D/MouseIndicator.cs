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
        
        private static Vector3 frontTopLeft = new Vector3(-0.6f, 0.6f, -0.6f);
        private static Vector3 frontTopRight = new Vector3(0.6f, 0.6f, -0.6f);
        private static Vector3 frontBottomLeft = new Vector3(-0.6f, -0.6f, -0.6f);
        private static Vector3 frontBottomRight = new Vector3(0.6f, -0.6f, -0.6f);

        private static Vector3 backTopLeft = new Vector3(-0.6f, 0.6f, 0.6f);
        private static Vector3 backTopRight = new Vector3(0.6f, 0.6f, 0.6f);
        private static Vector3 backBottomLeft = new Vector3(-0.6f, -0.6f, 0.6f);
        private static Vector3 backBottomRight = new Vector3(0.6f, -0.6f, 0.6f);

        private static Vector2 BL = new Vector2(0, 1);
        private static Vector2 BR = new Vector2(1, 1);
        private static Vector2 TL = new Vector2(0, 0);
        private static Vector2 TR = new Vector2(1, 0);

        Game1 game;
        VertexPositionColorTexture[] verts = new VertexPositionColorTexture[12];
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
            verts[0] = new VertexPositionColorTexture(position + frontBottomLeft, Color.White, new Vector2(1, 0));
            verts[1] = new VertexPositionColorTexture(position + backBottomLeft, Color.White, new Vector2(1, 1));
            verts[2] = new VertexPositionColorTexture(position + backBottomRight, Color.White, new Vector2(0,1));
            /*
            verts[3] = new VertexPositionColorTexture(position + backBottomRight, Color.White, new Vector2(1, 0));
            verts[4] = new VertexPositionColorTexture(position + frontBottomRight, Color.White, new Vector2(1, 0));
            verts[5] = new VertexPositionColorTexture(position + frontBottomLeft, Color.White, new Vector2(1, 0));
            */

            verts[3] = new VertexPositionColorTexture(backBottomRight + position, Color.White, BL);
            verts[4] = new VertexPositionColorTexture(frontBottomRight + position, Color.White, TL);
            verts[5] = new VertexPositionColorTexture(frontBottomLeft + position, Color.White, TR);



            verts[6] = new VertexPositionColorTexture(frontTopLeft + position, Color.White, TL);
            verts[7] = new VertexPositionColorTexture(frontTopRight + position, Color.White, TR);
            verts[8] = new VertexPositionColorTexture(backTopRight + position, Color.White, BR);

            verts[9] = new VertexPositionColorTexture(backTopRight + position, Color.White, BR);
            verts[10] = new VertexPositionColorTexture(backTopLeft + position, Color.White, BL);
            verts[11] = new VertexPositionColorTexture(frontTopLeft + position, Color.White, TL);
        }

        public void Draw(GraphicsDeviceManager g)
        {
            g.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, verts, 0, verts.Length / 3);
        }
    }
}
