using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using BradGame3D.Entities.Flora;

namespace BradGame3D
{
    public class Chunk
    {
        public enum DATA { ID, LIGHT };
        public BoundingBox bounds;
        private GameScreen game;
        public World2 world;
        public const int xSize = 16;
        public const int ySize = 128;
        public const int zSize = 16;
        public const int sectorCount = ySize/BlockSector.ySize;

        public BlockSector[] sectors = new BlockSector[sectorCount];
        [XmlIgnoreAttribute]
        public int chunkX;
        [XmlIgnoreAttribute]
        public int chunkZ;

        public int blockCount;
        List<Plant> floraList = new List<Plant>();
        public VertexPositionColorTexture[] floraVertices;
        public bool floraReady = false;
        public bool isVisible = true;

        //public List<Plant> flora = new List<Plant>();
  
        
        public List<Block> currentLighting = new List<Block>();

        public Chunk()
        {
        }
        public Chunk(GameScreen g, World2 w, int tx, int tz)
        {
            
            chunkX = tx;
            chunkZ = tz;
            blockCount = xSize*ySize*zSize;
            game = g;
            world = w;
            bounds = new BoundingBox(new Vector3(chunkX * xSize, 0, chunkZ * zSize), new Vector3((chunkX + 1) * xSize, ySize, (chunkZ + 1) * zSize));
            Random r = new Random();

            for (int i = 0; i < sectorCount; i++)
            {
                sectors[i] = new BlockSector(i * 16, this);
            }

            

        }
        public void genTrees(ref Random r)
        {

            Chunk temp = this;
            int treeCount = r.Next(1,5); // 1,5

            for (int i = 0; i < treeCount; i++)
            {
                int x = r.Next(chunkX * 16, (chunkX + 1) * 16);
                int z = r.Next(chunkZ * 16, (chunkZ + 1) * 16);
                int baseY = 0;
                bool test = true;
                int y = Chunk.ySize - 1;
                while (test)
                {
                    if (y <= -1 || Block.getSolid(getBlockData((int)DATA.ID, x, y, z)))
                    {
                        baseY = 1 + y;
                        test = false;
                    }
                    y -= 1;
                }
                addFlora(new Tree(ref temp, r.Next(), new Vector3(x, baseY, z)));
            }
            int shrubCount = r.Next(4, 14);
            for (int i = 0; i < shrubCount; i++)
            {
                int x = r.Next(chunkX * 16, (chunkX + 1) * 16);
                int z = r.Next(chunkZ * 16, (chunkZ + 1) * 16);
                int baseY = 0;
                bool test = true;
                int y = Chunk.ySize - 1;
                while (test)
                {
                    if (y <= -1 || Block.getSolid(getBlockData((int)DATA.ID, x, y, z)))
                    {
                        baseY = 1 + y;
                        test = false;
                    }
                    y -= 1;
                }
                addFlora(new Shrub(ref temp, r.Next(), new Vector3(x, baseY, z)));
            }
        }
        public void buildFromByteArray(byte[] a)
        {
            int overallCount = 0;
            for (int i = 4; i < a.Length; i+=2)
            {
                int type = a[i];
                int count = a[i + 1]+1;
                for (int j = overallCount; j < overallCount + count; j++)
                {
                    setBlockData((int) Chunk.DATA.ID, (byte) type, j % (xSize * zSize) % xSize + xSize * chunkX, j / (xSize * zSize), j % (xSize * zSize) / zSize + zSize * chunkZ);
                }
                overallCount += count;
               
                //blocks[i] = new Block2((int) a[i], i % (xSize * zSize) % xSize + xSize*chunkX, i / (xSize * zSize), i % (xSize * zSize) / zSize + zSize*chunkZ);
            }

            if (overallCount < xSize * zSize * ySize)
            {
                for (int j = overallCount; j < xSize * zSize * ySize; j++)
                {
                    setBlockData((int) Chunk.DATA.ID, 0, j % (xSize * zSize) % xSize + xSize * chunkX, j / (xSize * zSize), j % (xSize * zSize) / zSize + zSize * chunkZ);

                }
            }
        }


        public byte getBlockData(int datatype, int x, int y, int z)
        {
            if (x < chunkX * Chunk.xSize || x > (chunkX + 1) * Chunk.xSize)
                return 0;
            if (y >= Chunk.ySize || y < 0)
                return 0;
            if (z < chunkZ * Chunk.zSize || z > (chunkZ + 1) * Chunk.zSize)
                return 0;

            int sector = y / 16;
            int remainY = y - sector * 16;
            int remainX = x - chunkX * 16;
            int remainZ = z - chunkZ * 16;

            switch (datatype)
            {
                case (int) DATA.ID: return sectors[sector].blockIds[remainY * (Chunk.zSize * Chunk.xSize) + remainZ * (Chunk.zSize) + remainX];
            }
            return 0;

        }
        public void setBlockData(int datatype,byte val, int x, int y, int z)
        {
            if (x < chunkX * Chunk.xSize || x > (chunkX + 1) * Chunk.xSize)
                return;
            if (y >= Chunk.ySize || y < 0)
                return;
            if (z < chunkZ * Chunk.zSize || z > (chunkZ + 1) * Chunk.zSize)
                return;

            int sector = y / 16;
            int remainY = y - sector * 16;
            int remainX = x - chunkX * 16;
            int remainZ = z - chunkZ * 16;

            switch (datatype)
            {
                case (int)DATA.ID: sectors[sector].blockIds[remainY * (Chunk.zSize * Chunk.xSize) + remainZ * (Chunk.zSize) + remainX] = val; break;
            }
            //buildAllSectors();
            return;

        }
        public void buildAllSectors()
        {
            for (int i = 0; i < sectorCount; i++)
            {
                sectors[i].buildList();
            }
        }

        
        /*
        public void doLighting()
        {
            isReady = false;   
            for (int i = 0; i < blockCount; i++)
            {
                if (blocks[i] != null) blocks[i].lightLevel = 2;
                if (blocks[i] != null && GameScreen.blockDataManager.blocks[blocks[i].id].getLight())
                {
                    lightRecurse(blocks[i], 15);
                    foreach (Block b in currentLighting)
                        b.usedForLighting = false;
                    currentLighting.Clear();
                }
            }
            
            doVisible();
        }
         
        public void lightRecurse(Block a, byte val)
        {
            if(a!=null && !a.usedForLighting && val>2)
            {
                a.usedForLighting = true;
                currentLighting.Add(a);
                if (!GameScreen.blockDataManager.blocks[a.id].getSolid())
                {
                    if (a.lightLevel < val)
                        a.lightLevel = val;
                }
                if (val > 3)
                {
                    lightRecurse(world.getBlockAt((int)a.pos.X + 1, (int)a.pos.Y, (int)a.pos.Z), (byte)(val - 1));
                    lightRecurse(world.getBlockAt((int)a.pos.X - 1, (int)a.pos.Y, (int)a.pos.Z), (byte)(val - 1));
                    lightRecurse(world.getBlockAt((int)a.pos.X, (int)a.pos.Y + 1, (int)a.pos.Z), (byte)(val - 1));
                    lightRecurse(world.getBlockAt((int)a.pos.X, (int)a.pos.Y - 1, (int)a.pos.Z), (byte)(val - 1));
                    lightRecurse(world.getBlockAt((int)a.pos.X, (int)a.pos.Y, (int)a.pos.Z + 1), (byte)(val - 1));
                    lightRecurse(world.getBlockAt((int)a.pos.X, (int)a.pos.Y, (int)a.pos.Z - 1), (byte)(val - 1));
                }
               
            }

            
        }
         */
        /*
        public void doVisible()
        {
            isReady = false;
            Boolean[] temp = new Boolean[6];
            for (int i = 0; i < blockCount; i++)
            {
                
                Block b = blocks[i];


                if (b.getRender())
                {
                    for (int x = 0; x < 6; x++)
                        temp[x] = false;

                    Block a = world.getBlockAt((int)b.pos.X, (int)b.pos.Y, (int)b.pos.Z - 1);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.FRONT] = true;
                        b.faceLight[(int)Block.sides.FRONT] = a.lightLevel;
                    }
                    a = world.getBlockAt((int)b.pos.X, (int)b.pos.Y, (int)b.pos.Z + 1);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.BACK] = true;
                        b.faceLight[(int)Block.sides.BACK] = a.lightLevel;
                    }
                    a = world.getBlockAt((int)b.pos.X+1, (int)b.pos.Y, (int)b.pos.Z);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.LEFT] = true;
                        b.faceLight[(int)Block.sides.LEFT] = a.lightLevel;
                    }
                    a = world.getBlockAt((int)b.pos.X - 1, (int)b.pos.Y, (int)b.pos.Z);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.RIGHT] = true;
                        b.faceLight[(int)Block.sides.RIGHT] = a.lightLevel;
                    }
                    a = world.getBlockAt((int)b.pos.X, (int)b.pos.Y+1, (int)b.pos.Z);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.TOP] = true;
                        b.faceLight[(int)Block.sides.TOP] = a.lightLevel;
                    }
                    a = world.getBlockAt((int)b.pos.X, (int)b.pos.Y-1, (int)b.pos.Z);
                    if (a != null && !a.getRender())
                    {
                        temp[(int)Block.sides.BOTTOM] = true;
                        b.faceLight[(int)Block.sides.BOTTOM] = a.lightLevel;
                    }
                }
                else
                {
                    for (int x = 0; x < 6; x++)
                        temp[x] = false;
                }
                b.setFacesVisible(temp[(int) Block.sides.FRONT],temp[(int) Block.sides.BACK],temp[(int) Block.sides.RIGHT],temp[(int) Block.sides.LEFT],temp[(int) Block.sides.TOP],temp[(int) Block.sides.BOTTOM]);
                
            }
            buildList();
        }
         
        public void destroy()
        {
            //blocks = null;
        }
         */
        public void buildFloraList()
        {
            List<VertexPositionColorTexture> temp = new List<VertexPositionColorTexture>();
            //int faces;
            foreach (Plant i in floraList)
            {
                i.addVertices(ref temp);
            }
            floraReady = false;
            floraVertices = temp.ToArray();
            floraReady = true;
            temp = null;
        }
        public void addFlora(Plant p)
        {
            floraList.Add(p);
            buildFloraList();
        }

    }
}
