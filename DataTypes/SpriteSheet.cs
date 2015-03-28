using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Diagnostics;

namespace BradGame3D.Art
{
    public class SpriteSheet
    {
        public enum Direction { RIGHT, LEFT, TOWARD, AWAY };
        public enum ANIMATION { Idle, Walk };
        public enum Corner { TL, TR, BL, BR };
        const int ANIMNUM = 2;

        public String name;
        public List<List<int>> animationTiles = new List<List<int>>();
        public List<string> animationNames = new List<string>();
        private Texture2D image;
        public int widthPx;
        public int heightPx;
        public int tileWidth;
        public int tileHeight;
        //private GameScreen gScreen;
        public String fileName;
        //XmlDocument data;
        public SpriteSheet()
        {
        }
        public SpriteSheet(String fn)
        {
            fileName = fn;
            
        }
        public void setImage(Texture2D img) { image = img; }
        public Texture2D getImage()
        {
            return image;
        }
        public int getTile(string animation, Direction d, ref float animationTime)
        {
            if (animationNames.Contains(animation))
            {
                int anim = animationNames.IndexOf(animation);
                if (animationTime > animationTiles[anim].Last())
                {
                    animationTime = animationTime - animationTiles[anim].Last();
                }
                for (int i = 1; i < animationTiles[anim].Count(); i += 2)
                {
                    if (animationTiles[anim][i] > animationTime)
                    {
                        return animationTiles[anim][i - 1] + ((int) d)*(widthPx/tileWidth);
                    }
                }
                return 0;
                
            }
            else
                return 0;
        }
        public Vector2 getTexCoords(int tile, Corner c)
        {
            int tilesAcross = widthPx / tileWidth;
            int tilesHigh = heightPx / tileHeight;

            float y = tile/tilesAcross;
            float x = tile % tilesAcross;
            float spacingX = 1/(float)tilesAcross;
            float spacingY = 1/(float)tilesHigh;

            Vector2 vec = Vector2.Zero;
            switch (c)
            {
                case Corner.TL: vec = new Vector2(x / tilesAcross, y / tilesHigh); break;
                case Corner.TR: vec = new Vector2(x / tilesAcross + spacingX, y / tilesHigh); break;
                case Corner.BL: vec = new Vector2(x / tilesAcross, y / tilesHigh + spacingY); break;
                case Corner.BR: vec = new Vector2(x / tilesAcross + spacingX, y / tilesHigh + spacingY); break;
            }
            return vec;
        }
        
    }

}
