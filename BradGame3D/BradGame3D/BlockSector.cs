using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BradGame3D
{   
    public class BlockSector
    {
        public const int ySize = 16;
        public byte[] blockIds = new byte[Chunk.xSize * Chunk.zSize * BlockSector.ySize];
        public int yBase;
        public Chunk c;
        public BoundingBox bounds;
        

        int blockCount = Chunk.zSize * Chunk.xSize * BlockSector.ySize;

        private List<VertexPositionColorTexture> vertices;
        public VertexPositionColorTexture[] verticesArray;

        public int faceCount;
        public bool isReady = false;
        public bool canDraw = false;

        public BlockSector(int ybase, Chunk temp)
        {
            yBase = ybase;
            c = temp;
            bounds = new BoundingBox(new Vector3(c.chunkX * Chunk.xSize, yBase, c.chunkZ * Chunk.zSize), new Vector3((c.chunkX + 1) * Chunk.xSize, yBase + ySize, (c.chunkZ + 1) * Chunk.zSize));
        }
        public void buildList()
        {


            vertices = new List<VertexPositionColorTexture>();
            int newfaceCount = 0;
            for (int i = 0; i < blockCount; i++)
            {

                if(Block.getRender(blockIds[i]))
                {
                    int x = i%Chunk.xSize + c.chunkX*Chunk.xSize;
                    int y = (i/(Chunk.xSize*Chunk.zSize)) + yBase;
                    int z = (i%(Chunk.xSize*Chunk.zSize))/Chunk.zSize + c.chunkZ*Chunk.zSize;
                    byte a = c.world.getBlockData((int) Chunk.DATA.ID,x,y,z - 1);
                    if (!Block.getSolid(a))
                    {
                        Block.addFront(ref c.world, ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                    a = c.world.getBlockData((int)Chunk.DATA.ID, x, y, z + 1);
                    if (!Block.getSolid(a))
                    {
                        Block.addBack(ref c.world, ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                   
                    a = c.world.getBlockData((int)Chunk.DATA.ID, x+1, y, z);
                    if (!Block.getSolid(a))
                    {
                        Block.addLeft(ref c.world, ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                   
                    a = c.world.getBlockData((int)Chunk.DATA.ID, x-1, y, z);
                    if (!Block.getSolid(a))
                    {
                        Block.addRight(ref c.world, ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                    
                    a = c.world.getBlockData((int)Chunk.DATA.ID, x, y+1, z);
                    if (!Block.getSolid(a))
                    {
                        Block.addTop(ref c.world, ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                    
                    a = c.world.getBlockData((int)Chunk.DATA.ID, x, y-1, z);
                    if (!Block.getSolid(a))
                    {
                        Block.addBot(ref vertices, new Vector3(x, y, z), blockIds[i], 15);
                        newfaceCount++;
                    }
                }
                
            }
            
            canDraw = false;
            faceCount = newfaceCount;
            verticesArray = vertices.ToArray();
            isReady = true;
            canDraw = true;

            vertices = null;
        }
    }
  
}
