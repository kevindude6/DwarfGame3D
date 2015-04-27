using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using BradGame3D.Blocks;

namespace BradGame3D
{
    public class Block
    {
        //public byte id;
        //public Boolean solid = false;
        public bool usedForLighting = false;
        public enum sides { FRONT, BACK, RIGHT, LEFT, TOP, BOTTOM };
        public static Color blockSelectColor = new Color(1f, 0, 0);
        private bool[] faces = new bool[6];
        public byte lightLevel = 0;
        public byte[] faceLight = new byte[6];
        //public bool isLightSource;
       // public int x, y, z;
        public int id;
        //public VertexPositionColorTexture[][] vertexes = new VertexPositionColorTexture[6][];

        private static Color[] lightLookup = {new Color(0f,0f,0f),
                                              new Color(0.0625f,0.0625f,0.0625f),
                                              new Color(0.125f,0.125f,0.125f),
                                              new Color(0.1875f,0.1875f,0.1875f),
                                              new Color(0.25f,0.25f,0.25f),
                                              new Color(0.3125f,0.3125f,0.3125f),
                                              new Color(0.375f,0.375f,0.375f),
                                              new Color(0.4375f,0.4375f,0.4375f),
                                              new Color(0.5f,0.5f,0.5f),
                                              new Color(0.5625f,0.5625f,0.5625f),
                                              new Color(0.625f,0.625f,0.625f),
                                              new Color(0.6875f,0.6875f,0.6875f),
                                              new Color(0.75f,0.75f,0.75f),
                                              new Color(0.8125f,0.8125f,0.8125f),
                                              new Color(0.875f, 0.875f, 0.875f),
                                              new Color(0.9375f,0.9375f,0.9375f),
                                              new Color(1f,1f,1f)};


        public enum vertexTypes { FTL, FTR, FBL, FBR, BTL, BTR, BBL, BBR };

        private static Vector3 frontTopLeft = new Vector3(-0.5f, 0.5f, -0.5f);
        private static Vector3 frontTopRight = new Vector3(0.5f, 0.5f, -0.5f);
        private static Vector3 frontBottomLeft = new Vector3(-0.5f, -0.5f, -0.5f);
        private static Vector3 frontBottomRight = new Vector3(0.5f, -0.5f, -0.5f);

        private static Vector3 backTopLeft = new Vector3(-0.5f, 0.5f, 0.5f);
        private static Vector3 backTopRight = new Vector3(0.5f, 0.5f, 0.5f);
        private static Vector3 backBottomLeft = new Vector3(-0.5f, -0.5f, 0.5f);
        private static Vector3 backBottomRight = new Vector3(0.5f, -0.5f, 0.5f);

        private const byte ambientOccludeMod = 2;

        /*
        private Vector2 texTopLeft;
        private Vector2 texTopRight;
        private Vector2 texBottomLeft;
        private Vector2 texBottomRight;
        */
        //[XmlIgnoreAttribute]
        public Vector3 pos;


        public Block()
        {
        }
        public Block(int tid, int tx, int ty, int tz)
        {
            //x = tx;
            //y = ty;
            //z = tz;
            id = tid;

            pos = new Vector3(tx,ty,tz);
   
        

           
        }
        public static Color getColorFace(int a)
        {
            if (a < 0)
                a = 0;
            if (a <= 15)
            {
                //if (a < 15) a = 15;
                return lightLookup[(byte) a];
            }
            else
                return blockSelectColor;
        }
        public static bool getRender(byte id) { if (GameScreen.blockDataManager.blocks[id] != null) return GameScreen.blockDataManager.blocks[id].getRender(); else return false; }
        public static bool getSolid(byte id) { if (GameScreen.blockDataManager.blocks[id] != null) return GameScreen.blockDataManager.blocks[id].getSolid(); else return false; }
        /*
        public Boolean isSolid()
        {
            return solid;
        }
        public void setSolid(Boolean a)
        {
            solid = a;
        }
         */ 
        /*public void addVertices(ref List<VertexPositionColorTexture> vertices, ref int faceCount)
        {

            foreach (int a in Enum.GetValues(typeof(sides)))
            {
                if (faces[a])
                {
                    for (int i = 0; i < 6; i++)
                        vertices.Add(vertexes[a][i]);
                    faceCount += 1;
                }
            }
        }*/
        public static byte ambientOccludeTop(ref World2 w, int vertexType, Vector3 pos)
        {
            byte count = 0;
            if (vertexType == (int)vertexTypes.FTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 1))))
                    count++;
            }
            return count;
            
        }

        public static byte ambientOccludeFront(ref World2 w, int vertexType, Vector3 pos)
        {
            byte count = 0;
            if (vertexType == (int)vertexTypes.FTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FBL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, -1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FBR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, -1, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, 1))))
                    count++;
            }
            return count;

        }
        public static byte ambientOccludeBack(ref World2 w, int vertexType, Vector3 pos)
        {
            byte count = 0;
            if (vertexType == (int)vertexTypes.BTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, 1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BBL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, -1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BBR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(0, -1, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, -1))))
                    count++;
            }
            return count;

        }

        public static byte ambientOccludeRight(ref World2 w, int vertexType, Vector3 pos)
        {
            byte count = 0;
            if (vertexType == (int)vertexTypes.FTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BTL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FBL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BBL)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(-1, -1, 1))))
                    count++;
            }
            return count;

        }
        public static byte ambientOccludeLeft(ref World2 w, int vertexType, Vector3 pos)
        {
            byte count = 0;
            if (vertexType == (int)vertexTypes.FTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BTR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 1, 1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.FBR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, -1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, -1))))
                    count++;
            }
            if (vertexType == (int)vertexTypes.BBR)
            {
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, 0, 1))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, 0))))
                    count++;
                if (getRender(w.getBlockData((int)Chunk.DATA.ID, pos + new Vector3(1, -1, 1))))
                    count++;
            }
            return count;

        }
      
        //=====================================================================================
        public static void addBot(ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int) BlockData.SIDE.BOTTOM][(int) BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(backBottomLeft + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BOTTOM][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(backBottomRight + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BOTTOM][(int)BlockData.CORNER.BL]));

            vertices.Add(new VertexPositionColorTexture(backBottomRight + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BOTTOM][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(frontBottomRight + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BOTTOM][(int)BlockData.CORNER.TL]));
            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace(light), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BOTTOM][(int)BlockData.CORNER.TR]));
        }
        //======================================================================================
 

        public static void addTop(ref World2 w, ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            byte ftl=(byte)(ambientOccludeTop(ref w, (int)vertexTypes.FTL,pos)*ambientOccludeMod);
            byte ftr=(byte)(ambientOccludeTop(ref w, (int)vertexTypes.FTR,pos)*ambientOccludeMod);
            byte btl=(byte)(ambientOccludeTop(ref w, (int)vertexTypes.BTL,pos)*ambientOccludeMod);
            byte btr=(byte)(ambientOccludeTop(ref w, (int)vertexTypes.BTR,pos)*ambientOccludeMod);

            /*
            if (ftl > light) ftl = light;
            if (ftr > light) ftr = light;
            if (btl > light) btl = light;
            if (btr > light) btr = light;
             */

            if (ftr + btl >= ftl + btr)
            {
                vertices.Add(new VertexPositionColorTexture(frontTopLeft + pos, getColorFace((light - ftl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TL]));
                vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TR]));
                vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BR]));

                vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BR]));
                vertices.Add(new VertexPositionColorTexture(backTopLeft + pos, getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BL]));
                vertices.Add(new VertexPositionColorTexture(frontTopLeft + pos, getColorFace((light - ftl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TL]));
            }
            else
            {
                vertices.Add(new VertexPositionColorTexture(backTopLeft + pos, getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BL]));
                vertices.Add(new VertexPositionColorTexture(frontTopLeft + pos, getColorFace((light - ftl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TL]));
                vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TR]));

                vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.TR]));
                vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BR]));
                vertices.Add(new VertexPositionColorTexture(backTopLeft + pos, getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.TOP][(int)BlockData.CORNER.BL]));
            }
        }
    


        public static void addFront(ref World2 w, ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            byte ftl = (byte)(ambientOccludeFront(ref w, (int)vertexTypes.FTL, pos) * ambientOccludeMod);
            byte ftr = (byte)(ambientOccludeFront(ref w, (int)vertexTypes.FTR, pos) * ambientOccludeMod);
            byte fbl = (byte)(ambientOccludeFront(ref w, (int)vertexTypes.FBL, pos) * ambientOccludeMod);
            byte fbr = (byte)(ambientOccludeFront(ref w, (int)vertexTypes.FBR, pos) * ambientOccludeMod);

            vertices.Add(new VertexPositionColorTexture(frontTopLeft + pos, getColorFace((light-ftl)), GameScreen.blockDataManager.blocks[id].texCoords[(int) BlockData.SIDE.FRONT][(int) BlockData.CORNER.TL]));
            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace((light - fbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.FRONT][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.FRONT][(int)BlockData.CORNER.TR]));

            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace((light - fbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.FRONT][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(frontBottomRight + pos, getColorFace((light - fbr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.FRONT][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.FRONT][(int)BlockData.CORNER.TR]));
        }



        public static void addBack(ref World2 w, ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            byte btl = (byte)(ambientOccludeBack(ref w, (int)vertexTypes.BTL, pos) * ambientOccludeMod);
            byte btr = (byte)(ambientOccludeBack(ref w, (int)vertexTypes.BTR, pos) * ambientOccludeMod);
            byte bbl = (byte)(ambientOccludeBack(ref w, (int)vertexTypes.BBL, pos) * ambientOccludeMod);
            byte bbr = (byte)(ambientOccludeBack(ref w, (int)vertexTypes.BBR, pos) * ambientOccludeMod);

            vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(backBottomRight + pos, getColorFace((light - bbr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(backBottomLeft + pos, getColorFace((light - bbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.BL]));

            vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(backBottomLeft + pos, getColorFace((light - bbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(backTopLeft + pos, getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.BACK][(int)BlockData.CORNER.TL]));
        }

        public static void addRight(ref World2 w, ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            byte ftl = (byte)(ambientOccludeRight(ref w, (int)vertexTypes.FTL, pos) * ambientOccludeMod);
            byte btl = (byte)(ambientOccludeRight(ref w, (int)vertexTypes.BTL, pos) * ambientOccludeMod);
            byte fbl = (byte)(ambientOccludeRight(ref w, (int)vertexTypes.FBL, pos) * ambientOccludeMod);
            byte bbl = (byte)(ambientOccludeRight(ref w, (int)vertexTypes.BBL, pos) * ambientOccludeMod);

            vertices.Add(new VertexPositionColorTexture(backTopLeft + pos,  getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int) BlockData.SIDE.RIGHT][(int) BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(backBottomLeft + pos, getColorFace((light - bbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.RIGHT][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace((light - fbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.RIGHT][(int)BlockData.CORNER.BL]));

            vertices.Add(new VertexPositionColorTexture(backTopLeft + pos, getColorFace((light - btl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.RIGHT][(int)BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(frontBottomLeft + pos, getColorFace((light - fbl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.RIGHT][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(frontTopLeft + pos, getColorFace((light - ftl)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.RIGHT][(int)BlockData.CORNER.TL]));
        }

        public static void addLeft(ref World2 w, ref List<VertexPositionColorTexture> vertices, Vector3 pos, byte id, byte light)
        {
            byte ftr = (byte)(ambientOccludeLeft(ref w, (int)vertexTypes.FTR, pos) * ambientOccludeMod);
            byte btr = (byte)(ambientOccludeLeft(ref w, (int)vertexTypes.BTR, pos) * ambientOccludeMod);
            byte fbr = (byte)(ambientOccludeLeft(ref w, (int)vertexTypes.FBR, pos) * ambientOccludeMod);
            byte bbr = (byte)(ambientOccludeLeft(ref w, (int)vertexTypes.BBR, pos) * ambientOccludeMod);

            vertices.Add(new VertexPositionColorTexture(frontTopRight + pos, getColorFace((light - ftr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.TR]));
            vertices.Add(new VertexPositionColorTexture(frontBottomRight + pos, getColorFace((light - fbr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.TL]));

            vertices.Add(new VertexPositionColorTexture(frontBottomRight + pos, getColorFace((light - fbr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.BR]));
            vertices.Add(new VertexPositionColorTexture(backBottomRight + pos, getColorFace((light - bbr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.BL]));
            vertices.Add(new VertexPositionColorTexture(backTopRight + pos, getColorFace((light - btr)), GameScreen.blockDataManager.blocks[id].texCoords[(int)BlockData.SIDE.LEFT][(int)BlockData.CORNER.TL]));
        }

    }
}
