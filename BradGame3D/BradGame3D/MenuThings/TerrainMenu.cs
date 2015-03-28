/*
psst guess what i love you lots and one day we're gonna live together and be super happy because we're gonna cuddle everyday and go grocery shopping together and take walks and shower together
hehehe so let's just elope now yep sounds like a plan -- to the ocean we go! and in the ocean there'll be tigers and french people and man eating islands because that's your dream home right? 
i think you said that once at some point yep okay 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using BradGame3D.TerrainGeneration;
using System.Xml.Serialization;
using System.IO;
using System.Threading;



namespace BradGame3D.MenuThings
{
    class TerrainMenu : MenuScreen
    {
        //const int MINIMUMHEIGHT = 30;
        const int mapsize = 512;
        float[][] tileheights;
        Texture2D pixel;
        Color[] colors;
        Texture2D pic;
        int octaveCount = 10;
        SpriteFont font;
        string savingText = "Not Saving";
        public TerrainMenu(Game1 tg) : base(tg)
        {

            pixel = new Texture2D(g.graphics.GraphicsDevice, 1, 1);
            colors = new Color[1];
            colors[0] = Color.White;
            pixel.SetData(colors);
            updateTerrain();
            font = g.Content.Load<SpriteFont>("sego");
        }
        public override void init()
        {
            Action a = delegate() { g.startGame(); };
            Button b = new Button(this, null, 100, 100, 400, 100, g.Content.Load<Texture2D>("grass"), a);  
            b.text = "Exit Game";
            components.Add(b);

            Button update = new Button(this, null, 100, 300, 400, 100, g.Content.Load<Texture2D>("grass"), new Action(updateTerrain));
            update.text = "Update Terrain";
            components.Add(update);

            Button addOctave = new Button(this, null, 100, 500, 400, 100, g.Content.Load<Texture2D>("grass"), (Action)delegate() { octaveCount++; updateTerrain(); });
            addOctave.text = "Add Octave";
            components.Add(addOctave);

            Button lowerOctave = new Button(this, null, 100, 700, 400, 100, g.Content.Load<Texture2D>("grass"), (Action)delegate() { octaveCount--; updateTerrain();});
            lowerOctave.text = "Lower Octave";
            components.Add(lowerOctave);

            Action serialize = delegate()
            {
                Thread t = new Thread(new ThreadStart(saveWorld));
                t.IsBackground = true;
                t.Start();
            };
            Button serialzeTest = new Button(this, null, 100, 900, 400,100, g.Content.Load<Texture2D>("grass"), serialize);
            serialzeTest.text = "Serialize";
            components.Add(serialzeTest);



        }
        public void saveWorld()
        {
            
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.Delete(mydocpath + "\\DwarfGameTest", true);
            Directory.CreateDirectory(mydocpath + "\\DwarfGameTest");
            
            

            int[] heights = new int[256];

            
            long[] chunkPositions = new long[1024];
            int[] chunkLengths = new int[1024];
            int chunkCounter = 0;
            long posCursor = 0;
            int worldz = 0;
            int worldx = 0;
            for (int z = worldz*32; z < (worldz+1)*32; z++)
            {
                for (int x = worldx*32; x < (worldx+1)*32; x++)
                {
                    savingText = "Region " + (worldz*8 + worldx) + " out of 1";
                            
                    for(int tempz = 0; tempz<16;tempz++)
                    {
                        for(int tempx = 0; tempx<16; tempx++)
                        {
                            heights[tempz * 16 + tempx] = (int)Math.Round(getTileHeight(tempz+z*16,tempx+x*16)*Chunk.ySize);
                        }
                    }

                    int length = 0;
                    byte[] bytedata = makeByteArray(heights);
                    length = bytedata.Length;
                    using (BinaryWriter writer = new BinaryWriter(File.Open(mydocpath + "\\DwarfGameTest\\Region" + (worldz * 8 + worldx), FileMode.Append)))
                    {
                        writer.Write(Flags.CHUNKBEGIN);
                        writer.Write((byte)z);
                        writer.Write((byte)x);
                        writer.Write(bytedata);
                        writer.Flush();
                        writer.Close();

                    }

                    chunkPositions[chunkCounter] = posCursor;
                    posCursor = posCursor + length + 4;
                    chunkLengths[chunkCounter] = length;
                    chunkCounter++;
               
                }
            }
                             
                        
                
                //SAVE CHUNK MAP 
                using (BinaryWriter writer = new BinaryWriter(File.Open(mydocpath + "\\DwarfGameTest\\Region" + (worldz * 8 + worldx), FileMode.Append)))
                {
                    writer.Write(Flags.CHUNKBEGIN);
                    for (int i = 0; i < 1024; i++)
                    {
                        writer.Write(chunkPositions[i]);
                        writer.Write(chunkLengths[i]);
                    }
                        
                    writer.Flush();
                    writer.Close();

                }
            
        
            FileStream f = new FileStream(mydocpath + "\\BradGameTest\\PIC.png", FileMode.Create);
            pic.SaveAsPng(f, 512,512);
            f.Flush();
            savingText = "Done";
        }

        public float getTileHeight(int x, int y)
        {
            if (x > 0 && x < mapsize && y > 0 && y < mapsize)
            {
                return tileheights[y][x];
            }
            else
                return 0;

        }

        public byte[] makeByteArray(int[] h)
        {
            byte[] a = new byte[Chunk.ySize*Chunk.xSize*Chunk.zSize];
            for (int y = 0; y < Chunk.ySize; y++)
            {
                for (int z = 0; z < Chunk.zSize; z++)
                {
                    for (int x = 0; x < Chunk.xSize; x++)
                    {
                        if (y < h[z * Chunk.zSize + x])
                        {
                            int distBelowGround = h[z*Chunk.zSize+x] - y;
                            if (distBelowGround == 1)
                                a[y * (Chunk.xSize * Chunk.zSize) + z * Chunk.zSize + x] = 1;
                            else if (distBelowGround < 4)
                                a[y * (Chunk.xSize * Chunk.zSize) + z * Chunk.zSize + x] = 3;
                            else
                                a[y * (Chunk.xSize * Chunk.zSize) + z * Chunk.zSize + x] = 2;
                        }
                        else
                            a[y * (Chunk.xSize * Chunk.zSize) + z * Chunk.zSize + x] = 0;
                    }
                }
            }
            List<byte> temp = new List<byte>();
            byte curId = a[0];
            byte count = 0;
            for (int i = 1; i < Chunk.xSize * Chunk.ySize * Chunk.zSize; i++ )
            {
                if(count == 255)
                {
                    temp.Add(curId);
                    temp.Add(count);
                    curId = a[i];
                    count = 0;
                }
                else if (curId == a[i])
                    count++;
                else
                {
                    temp.Add(curId);
                    temp.Add(count);
                    curId = a[i];
                    count = 0;
                }
                
            }
            return temp.ToArray();
        }
        public float calcAvg(int z, int x)
        {
            float sum = 0;
            for (int a = 0; a < 4; a++)
                for (int b = 0; b < 4; b++)
                    sum += tileheights[z+a][x+b];

            sum /= 16f;
            return sum;
        }
        public void normalizeHeights()
        {
            float chunkmax = Chunk.ySize;
            int terrainmax = 100;
            int terrainfloor = 20;

            float terrainmaxf = terrainmax / chunkmax;
            float terrainfloorf = terrainfloor / chunkmax;

            float maxheight = 0;
            for (int z = 0; z < mapsize; z++)
            {
                for (int x = 0; x < mapsize; x++)
                {
                    tileheights[z][x] += terrainfloorf;
                    if (tileheights[z][x] > maxheight)
                        maxheight = tileheights[z][x];
                }
            }
            for (int z = 0; z < mapsize; z++)
            {
                for (int x = 0; x < mapsize; x++)
                {
                    tileheights[z][x] /= maxheight;
                    tileheights[z][x] *= terrainmaxf;
                }
            }


        }
        public void updateTerrain()
        {
            tileheights = Perlin.GenerateIslandPerlinNoise(Perlin.GenerateWhiteNoise(mapsize, mapsize), octaveCount);
            normalizeHeights();
            //



            pic = makeTexture();


        }
        public Texture2D makeTexture()
        {
            Color[] data = new Color[mapsize * mapsize];
            for (int y = 0; y < mapsize; y++)
            {
                for (int x = 0; x < mapsize; x++)
                {
                    data[y * mapsize + x] = new Color(tileheights[y][x], tileheights[y][x], tileheights[y][x]);
                }
            }

            Texture2D myTex = new Texture2D(g.graphics.GraphicsDevice,mapsize,mapsize,true,SurfaceFormat.Color);
            myTex.SetData<Color>(data);
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(mydocpath + "\\BradGameTest");
            FileStream f = new FileStream(mydocpath + "\\BradGameTest\\PIC.png", FileMode.Create);
            myTex.SaveAsPng(f, 512,512);
            f.Flush();
            f.Close();
            return myTex;
        }
        public override void drawMenu(GameTime gt)
        {
            base.drawMenu(gt);
            g.spriteBatch.Begin();
        
            g.spriteBatch.Draw(pic, new Rectangle(800,100,512,512), null, Color.White);
            g.spriteBatch.DrawString(font, "Octaves: " + octaveCount, new Vector2(100, 980), Color.White);
            g.spriteBatch.DrawString(font, savingText, new Vector2(500, 980), Color.White);
        
            g.spriteBatch.End();
        }
    }
}
