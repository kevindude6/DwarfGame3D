using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.PlayerInteraction
{
    public class Utils
    {
        public static void drawWireBoundingBox(GraphicsDeviceManager graphics, BoundingBox bounds)
        {
            VertexPositionColorTexture[] verts;
            verts = new VertexPositionColorTexture[2];

            verts[0] = new VertexPositionColorTexture(bounds.Min, Color.White, new Vector2(0.05f,0.05f));
            verts[1] = new VertexPositionColorTexture(bounds.Max, Color.White, new Vector2(0.06f,0.06f));

            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.LineList, verts, 0, 1);
        }
    }
}
