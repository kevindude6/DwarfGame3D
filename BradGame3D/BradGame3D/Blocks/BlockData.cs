using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BradGame3D.Blocks
{
    public class BlockData
    {
        int id;
        bool render;
        bool solid;
        bool lightSource;
        string name;
        /*
        private Vector2 texTopLeft;
        private Vector2 texTopRight;
        private Vector2 texBottomLeft;
        private Vector2 texBottomRight;

        private Vector2 topTexTopLeft;
        private Vector2 topTexTopRight;
        private Vector2 topTexBottomLeft;
        private Vector2 topTexBottomRight;
        */

        public enum SIDE { FRONT, BACK, LEFT, RIGHT, TOP, BOTTOM };
        public enum CORNER { TL, TR, BL, BR };

        public Vector2[][] texCoords;


        public BlockData(int tid, string tname, bool trender, bool tsolid, bool light, Vector2[][] coords)
        {
            id = tid;
            name = tname;
            render = trender;
            solid = tsolid;
            texCoords = coords;
            lightSource = light;
        }








        public bool getRender() { return render; }
        public bool getSolid() { return solid; }
        public bool getLight() { return lightSource; }
        /*
        public BlockData(int tid, string tname, bool trender, bool tsolid, Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Vector2 ttl, Vector2 ttr, Vector2 tbl, Vector2 tbr)
        {
            id = tid;
            name = tname;
            render = trender;
            solid = tsolid;
            texTopLeft = tl;
            texTopRight = tr;
            texBottomLeft = bl;
            texBottomRight = br;

            topTexBottomLeft = tbl;
            topTexBottomRight = tbr;
            topTexTopLeft = ttl;
            topTexTopRight = ttr;
        }

        public bool getRender() { return render; }
        public bool getSolid() { return solid; }
        
        public Vector2 getTexTopLeft() { return texTopLeft; }
        public Vector2 getTexTopRight() { return texTopRight; }
        public Vector2 getTexBottomLeft() { return texBottomLeft; }
        public Vector2 getTexBottomRight() { return texBottomRight; }

        public Vector2 getTopTexTopLeft() { return topTexTopLeft; }
        public Vector2 getTopTexTopRight() { return topTexTopRight; }
        public Vector2 getTopTexBottomLeft() { return topTexBottomLeft; }
        public Vector2 getTopTexBottomRight() { return topTexBottomRight; }
         */

    }
}
