using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using BradGame3D.Entities.Flora;
namespace BradGame3D
{
    public class World2
    {
        public const int seed = 567898765;
        Random r = new Random(seed);
        public const int chunkCount = 25;
        public const int worldSize = 32;
        public bool initialized = false;
        public List<BlockSector> brokedSectors = new List<BlockSector>();
        public int chunkX;
        public int chunkZ;
        public GameScreen game;
        public Chunk[][] chunks = new Chunk[worldSize][];

        //public List<Plant> flora = new List<Plant>();
        
        //Chunk c;
        public World2(GameScreen g)
        {
            game = g;

            chunkX = 16; chunkZ = 16;

            for(int i = 0; i < worldSize; i++)
                chunks[i] = new Chunk[worldSize];


            Thread t = new Thread(new ThreadStart(init));
            t.IsBackground = true;
            t.Start();
            //init();
            
        }
        public void init()
        {

            for (int z = 0; z <= worldSize; z++)
            {
                for (int x = 0; x <= worldSize; x++)
                {
                    if (z >= 0 &&z < worldSize &&x >= 0 &&  x < worldSize)
                    {
                        chunks[z ][x] = loadChunk(game, this, x, z);
                    }
                }
            }
            for (int z = 0; z<=worldSize ; z++)
            {
                for (int x = 0; x<=worldSize ; x++)
                {
                    if (z >= 0 && z < worldSize && x >= 0 && x < worldSize)
                    {
                        if (chunks[z][x] != null)
                            chunks[z][x].buildAllSectors();

                    }
                }
            }
            initialized = true;
        }
        public Chunk loadChunk(GameScreen game, World2 w, int x, int z)
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Chunk c = new Chunk(game, this, x, z);
            byte[] a = new byte[1];
            bool unlocked = false;
            //FileStream f = File.Open(mydocpath + "\\BradGameTest\\Region" + ((z / 32) * 8 + x / 32), FileMode.Open);
            while (unlocked == false)
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(mydocpath + "\\DwarfGameTest\\Region" + ((z / 32) * 8 + x / 32), FileMode.Open)))
                    {
                        unlocked = true;
                        int tx = x - (x / 32) * 32;
                        int tz = z - (z / 32) * 32;
                        int chunkNum = tz * 32 + tx;
                        reader.BaseStream.Position = reader.BaseStream.Length - (1024 - chunkNum) * 12;

                        long chunkPos = reader.ReadInt64();
                        int chunkLength = reader.ReadInt32();

                        //for (int i = 0; i < 8192; i++)
                        // a[i] = reader.ReadByte();
                        reader.BaseStream.Position = chunkPos;
                        a = reader.ReadBytes(chunkLength);

                        reader.Close();
                    }
                }
                catch
                {
                    Thread.Sleep(30);
                }
            }
            
             c.buildFromByteArray(a);
             c.genTrees(ref r);
             return c;
        }
        public void fixChunks()
        {
            while (1 == 1)
            {

                if (game.loadedChunkThisFrame == false && initialized)
                {
                    if (brokedSectors.Count > 0)
                    {
                        BlockSector sec = getClosest();
                        if (sec != null && sec.isReady == false)
                        {
                            sec.buildList();
                        }
                        brokedSectors.Remove(sec);
                        //Thread.Sleep(20);
                    }
                    //game.loadedChunkThisFrame = true;
                }
                
                
            }
        }
        private BlockSector getClosest()
        {
            BlockSector sec = null;
            float minDist = 999999;
            int id = 0;
            for(int i = 0; i < brokedSectors.Count; i++)
            {
                if (brokedSectors[i] != null)
                {
                    float dist = (float)(Math.Pow((brokedSectors[i].c.chunkX*Chunk.xSize - game.mCam.camPos.X), 2) + Math.Pow((brokedSectors[i].c.chunkZ*Chunk.zSize - game.mCam.camPos.Z), 2) + Math.Pow((brokedSectors[i].yBase - game.mCam.camPos.Y), 2));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        id = i;
                    }
                }
            }
            sec = brokedSectors[id];
            return sec;
        }
        
       
        public void addToList(Chunk c)
        {
            for (int i = 0; i < Chunk.sectorCount; i++)
            {

                if (!brokedSectors.Contains(c.sectors[i]))
                    brokedSectors.Add(c.sectors[i]);
                c.sectors[i].isReady = false;
            }
        }
        public void addToList(BlockSector sec)
        {
            if (!brokedSectors.Contains(sec))
                brokedSectors.Add(sec);
            sec.isReady = false;
        }
        public void pause()
        {
            Thread.Sleep(200);
        }


        public void draw(GraphicsDeviceManager g)
        {
            
            for (int z = -(chunkCount-1)/2; z <= (chunkCount-1)/2; z++)
            {
                for (int x = -(chunkCount-1)/2; x <= (chunkCount-1)/2; x++)
                {
                    if (z + chunkZ >= 0 && z + chunkZ < worldSize && x + chunkX >= 0 && x + chunkX < worldSize)
                    {
                        Chunk c = chunks[z + chunkZ][x + chunkX];
                        if (c != null)
                        {
                            if (game.frustum.Intersects(c.bounds))
                            {
                                c.isVisible = true;
                                for (int i = 0; i < Chunk.sectorCount; i++)
                                {
                                    if (c.sectors[i] != null && c.sectors[i].canDraw && c.sectors[i].faceCount != 0)
                                    {
                                        //e.Texture = game.tex;
                                        g.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, c.sectors[i].verticesArray, 0, c.sectors[i].verticesArray.Length / 3);
                                    }
                                }
                            }
                            else
                                c.isVisible = false;
                            
                        }
                    }
                }
            }
        }
        public void drawTrees(GraphicsDeviceManager g)
        {
            for (int z = -(chunkCount - 1) / 2; z <= (chunkCount - 1) / 2; z++)
            {
                for (int x = -(chunkCount - 1) / 2; x <= (chunkCount - 1) / 2; x++)
                {
                    if (z + chunkZ >= 0 && z + chunkZ < worldSize && x + chunkX >= 0 && x + chunkX < worldSize)
                    {
                        Chunk c = chunks[z + chunkZ][x + chunkX];
                        if (c != null && c.isVisible && c.floraReady)
                        {
                            if (c.floraVertices.Length/3 != 0)
                            {
                                g.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, c.floraVertices, 0, c.floraVertices.Length / 3);
                            }
                        }
                    }
                }
            }
        }
        public byte getBlockData(int datatype, int x, int y, int z)
        {
            int cx = x / Chunk.xSize;
            int cz = z / Chunk.zSize;
            if (cz >= 0 && cz < worldSize && cx >= 0 && cx < worldSize)
            {
                Chunk c = chunks[cz][cx];
                if (c != null && z >= 0 && x >= 0 && y >= 0 && y < Chunk.ySize && (x - cx * Chunk.xSize) < Chunk.xSize && (z - cz * Chunk.zSize) < Chunk.zSize)
                    return c.getBlockData(datatype, x, y, z);
                else
                    return 1 ;
            }
            else
                return 1;
        }
        public byte getBlockData(int datatype, Vector3 a)
        {
            return getBlockData(datatype, (int) a.X, (int) a.Y, (int) a.Z);
        }
        public void setBlockData(byte val, int datatype, int x, int y, int z)
        {
            int cx = x / Chunk.xSize;
            int cz = z / Chunk.zSize;
            Chunk c = chunks[cz][cx];
            if (cz >= 0 && cz < worldSize && cx >= 0 && cx < worldSize && c != null && z >= 0 && x >= 0 && y >= 0 && y < Chunk.ySize && (x - cx * Chunk.xSize) < Chunk.xSize && (z - cz * Chunk.zSize) < Chunk.zSize)
            {
                c.setBlockData(datatype, val, x, y, z);
                
            }
            updateBlock(x, y, z);
            
        }
        public void setBlockData(byte val, int datatype, Vector3 a)
        {
            setBlockData(val, datatype, (int)a.X, (int)a.Y, (int)a.Z);
        }
        public void updateBlock(int x, int y, int z)
        {
            //chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize)].isReady = false;
            int cx = x / Chunk.xSize;
            int cz = z / Chunk.zSize;
            Chunk c = chunks[cz][cx];
            if (cz >= 0 && cz < worldSize && cx >= 0 && cx < worldSize && c != null && z >= 0 && x >= 0 && y >= 0 && y < Chunk.ySize && (x - cx * Chunk.xSize) < Chunk.xSize && (z - cz * Chunk.zSize) < Chunk.zSize)
            {
                if (c.sectors != null)
                {
                    addToList(chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize)].sectors[y / BlockSector.ySize]);

                    if (y % BlockSector.ySize == 0)
                    {
                        if (y / BlockSector.ySize - 1 >= 0)
                        {
                            addToList(chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize)].sectors[y / BlockSector.ySize - 1]);
                        }
                    }
                    else if ((y - BlockSector.ySize + 1) % BlockSector.ySize == 0)
                    {
                        if (y / BlockSector.ySize + 1 < Chunk.ySize / BlockSector.ySize)
                        {
                            addToList(chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize)].sectors[y / BlockSector.ySize + 1]);
                        }
                    }
                }
                //-x
                if (x % Chunk.xSize == 0 && isViableChunk(cx - 1, cz))
                {
                    addToList(chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize) - 1].sectors[y / BlockSector.ySize]);
                }
                if ((x - Chunk.xSize + 1) % Chunk.xSize == 0 && isViableChunk(cx + 1, cz))
                {
                    addToList(chunks[(int)(z / Chunk.zSize)][(int)(x / Chunk.xSize) + 1].sectors[y / BlockSector.ySize]);
                }
                if (z % Chunk.zSize == 0 && isViableChunk(cx, cz - 1))
                {
                    addToList(chunks[(int)(z / Chunk.zSize) - 1][(int)(x / Chunk.xSize)].sectors[y / BlockSector.ySize]);
                }
                if ((z - Chunk.zSize + 1) % Chunk.zSize == 0 && isViableChunk(cx, cz + 1))
                {
                    addToList(chunks[(int)(z / Chunk.zSize) + 1][(int)(x / Chunk.xSize)].sectors[y / BlockSector.ySize]);
                }
            }

           
            
        }
        public bool isViableChunk(int cx, int cz)
        {
            if (cz >= 0 && cz < worldSize && cx >= 0 && cx < worldSize)
            {
                if (chunks[cz][cx] != null)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public bool isSolid(int x, int y, int z)
        {
            return GameScreen.blockDataManager.blocks[getBlockData((int) Chunk.DATA.ID, x, y, z)].getSolid();
        }

        public void makeTree(Vector3 a)
        {
            Chunk temp = getChunk(a);
            Tree t = new Tree(ref temp, new Random().Next(),a);
            t.setTexture(game.treeTex);
            temp.addFlora(t);
            //flora.Add(t);
        }
        public Chunk getChunk(Vector3 a)
        {
            return chunks[(int)a.Z / Chunk.zSize][(int)(a.X / Chunk.xSize)];
        }
    }
}
