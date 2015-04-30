using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using BradGame3D.Entities;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;
using System.Xml;
using BradGame3D.Art;
using BradGame3D.PlayerInteraction;
using BradGame3D.Entities.Creatures;

namespace BradGame3D
{
    public class GameScreen
    {
        public Game1 game;
        public int sliceLevel = 65;
        public static World2 w;

        float camSpeedMod = 20;
        float DEBUGnuments = 0;

        Thread chunkLoadingThread;
        Thread pathingThread;

        const float rotationSpeed = 0.005f;

        public Matrix View;
        public Matrix Projection;

        public AlphaTestEffect effect;
        private Effect billboardEffect;
        private GraphicsDeviceManager graphics;

        private KeyboardState oldKeyState;
        private MouseState originalMouseState;
        private int mouseWheelPrevious;
        private bool mouseReady;
        private bool mouseEnabled;

        private Vector3 blockCastTarget = new Vector3(0,0,0);
        private Vector3 lookFace = new Vector3(0,0,0);

        public BoundingFrustum frustum;
        public Texture2D tex;
        public Texture2D treeTex;
        public Texture2D dogeTex;

        public Camera mCam;
        public LivingEntity test;
        
        public AI.Pathing.Node start = null;
        public AI.Pathing.Node end = null;
        public MouseIndicator mMouseIndicator;

        private Vector3 mouseSelectStart;
        private Vector3 mouseSelectEnd;
        private bool currentlySelecting;

        public SelectionManager selectionManager;
        public List<Human> citizenList = new List<Human>();
       // private List<MouseIndicator> selectionList = new List<MouseIndicator>();
        Random r;
        public bool loadedChunkThisFrame = false;

        private List<double> updateTimes = new List<double>();

        static public Blocks.BlockDataManager blockDataManager = new Blocks.BlockDataManager();
        Art.SpriteSheetManager sheetManager;

        int currentBlock;
        public void initBlockData()
        {
            int[] tempSideArray = { 0, 0, 0, 0, 0, 0 };
            blockDataManager.addBlock("Air", false, false, false, tempSideArray, 32, 320);

            tempSideArray = new int[] { 0, 0, 0, 0, 1, 4 };
            //tempSideArray = new int[] { 2, 2, 2, 2, 2, 2 };
            blockDataManager.addBlock("Grass", true, true, false, tempSideArray, 32, 320);

            tempSideArray = new int[] { 2, 2, 2, 2, 2, 2 };
            blockDataManager.addBlock("Stone", true, true, false, tempSideArray, 32, 320);

            tempSideArray = new int[] { 4, 4, 4, 4, 4, 4 };
            blockDataManager.addBlock("Dirt", true, true, false, tempSideArray, 32, 320);

            tempSideArray = new int[] { 3, 3, 3, 3, 3, 3 };
            blockDataManager.addBlock("Cobblestone", true, true, false, tempSideArray, 32, 320);


            tempSideArray = new int[] { 5, 5, 5, 5, 5, 5 };
            blockDataManager.addBlock("Light", true, true, true, tempSideArray, 32, 320);

            tempSideArray = new int[] {0,0,0,0,0,0};
            blockDataManager.addBlock("Flora Placeholder", false, true, false, tempSideArray, 32, 320);
        }

        public GameScreen(Game1 g)
        {
            initBlockData();
            

            game = g;
            graphics = game.graphics;
            
            
            w = new World2(this);

            r = new Random();

            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            originalMouseState = Mouse.GetState();
            mouseWheelPrevious = originalMouseState.ScrollWheelValue;
            oldKeyState = Keyboard.GetState();
            mMouseIndicator = new MouseIndicator(game);

            effect = new AlphaTestEffect(graphics.GraphicsDevice);
            effect.AlphaFunction = CompareFunction.Greater;
            effect.ReferenceAlpha = 0;
            //effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = true;
            
            effect.FogEnabled = true;
            effect.FogEnd=300f;
            effect.FogStart=150f;
            effect.FogColor = new Vector3(0.5f, 0.5f, 0.5f);


            billboardEffect = game.Content.Load<Effect>("Billboard");

            Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, (float)(game.Window.ClientBounds.Width) / game.Window.ClientBounds.Height, 0.1f, 300f);
            effect.Projection = Projection;

            frustum = new BoundingFrustum(effect.Projection * effect.View);

            chunkLoadingThread = new Thread(new ThreadStart(w.fixChunks));
            chunkLoadingThread.IsBackground = true;
            chunkLoadingThread.Start();

            AI.Pathing.PathingManager.w = w;
            pathingThread = new Thread(new ThreadStart(AI.Pathing.PathingManager.findPaths));
            pathingThread.IsBackground = true;
            pathingThread.Start();

            selectionManager = new SelectionManager(this, w);
            Thread selectThread = new Thread(new ThreadStart(selectionManager.workJobs));
            selectThread.IsBackground = true;
            selectThread.Start();

            loadThings();
            mCam = new Camera(this,w,graphics, new Vector3(256,120,256));

        }
        public void loadThings()
        {
            tex = game.Content.Load<Texture2D>("bradgameblocks");
            treeTex = game.Content.Load<Texture2D>("floraspritesheet");
            dogeTex = game.Content.Load<Texture2D>("dogecoin-300");

            sheetManager = new Art.SpriteSheetManager(this);
            sheetManager.loadSheet("Sprite_creature_squirrel",w);
            sheetManager.loadSheet("Sprite_creature_human",w);

            
        }
        public void updateFrustum()
        {
            frustum.Matrix = effect.View * effect.Projection;
        }
        public void updateGame(GameTime gameTime)
        {
            if (game.IsActive)
            {
                loadedChunkThisFrame = false;
                doInputGame(gameTime);
                mCam.doCameraGame();
                updateFrustum();
                updateEnts(gameTime);
            }
          
        }
        private void updateEnts(GameTime g)
        {
            foreach (Art.SpriteSheetEnhanced s in sheetManager.dict.Values)
            {
                s.updateEnts(g);
            }
        }
        private void checkFunctionalKeys(KeyboardState k)
        {
            if (k.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M) && mouseEnabled)
            {
                mouseEnabled = false;
            }
            else if (k.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M) && !mouseEnabled)
            {
                mouseEnabled = true;
            }
            if (k.IsKeyDown(Keys.Escape))
            {
                
            }
            if (k.IsKeyDown(Keys.P))
            {
                game.enterMainMenu();
            }
            if (k.IsKeyDown(Keys.F) && oldKeyState.IsKeyUp(Keys.F))
            {
                updateFrustum();
            }
        }
        public void unload()
        {

        }
        public void doInputGame(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            checkFunctionalKeys(k);

            mCam.doCameraInput(gameTime, k);
             
            bool pauseplaceholder = false;

            if (k.IsKeyDown(Keys.R))
                pauseplaceholder = true;
            if (pauseplaceholder)
                pauseplaceholder = false;

            if (k.IsKeyDown(Keys.D0)) currentBlock = 0;
            if (k.IsKeyDown(Keys.D1)) currentBlock = 1;
            if (k.IsKeyDown(Keys.D2)) currentBlock = 2;
            if (k.IsKeyDown(Keys.D3)) currentBlock = 3;
            if (k.IsKeyDown(Keys.D4)) currentBlock = 4;
            if (k.IsKeyDown(Keys.D5)) currentBlock = 5;
            if (k.IsKeyDown(Keys.D6)) currentBlock = 6;

            if (k.IsKeyDown(Keys.PageUp) && oldKeyState.IsKeyUp(Keys.PageUp))
            {
                //w.updateSlice(sliceLevel + 1);
                Thread thread = new Thread(() => w.updateSlice(sliceLevel + 1));
                thread.Start();
                
            }
            if (k.IsKeyDown(Keys.PageDown) && oldKeyState.IsKeyUp(Keys.PageDown))
            {
                //w.updateSlice(sliceLevel - 1);
                Thread thread = new Thread(() => w.updateSlice(sliceLevel-1));
                thread.Start();
            }
            if (k.IsKeyDown(Keys.C))
            {
                Vector3 a = blockCastTarget;
                if (currentBlock == 0)
                {
                    w.setBlockData((byte)currentBlock, (int)Chunk.DATA.ID, a);
                }
                else
                {
                    w.setBlockData((byte)currentBlock, (int)Chunk.DATA.ID, a+lookFace);
                }
            }
           

            MouseState currentMouseState = Mouse.GetState();


            if (currentMouseState != originalMouseState)
            {
                if(mCam.raycastBlock(ref blockCastTarget,ref lookFace))
                    mMouseIndicator.setPosition(blockCastTarget);
                if (mouseEnabled)
                {
                    float xDifference = currentMouseState.X - originalMouseState.X;
                    float yDifference = currentMouseState.Y - originalMouseState.Y;
                    mCam.yaw-= rotationSpeed * xDifference;
                    mCam.pitch -= rotationSpeed * yDifference;
                    Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);

                }
                
         
                if (currentMouseState.LeftButton == ButtonState.Pressed && mouseReady)
                {
                    Vector3 a = blockCastTarget;
                    if (!Vector3.Equals(a, new Vector3(-1, -1, -1)))
                    {
                        
                        
                        SpriteSheetEnhanced tsheet;
                        sheetManager.dict.TryGetValue(Entities.Creatures.Human.SheetName, out tsheet);
                        test = new Entities.Creatures.Human(a + lookFace + new Vector3((float) (r.NextDouble() - 0.5f), 2, (float) (r.NextDouble()-0.5f)), 100);
                        
                        //test.velocity.X = (float) (r.NextDouble() * 10 - 5);
                        //test.velocity.Y = (float)(r.NextDouble() * 10 + 2);
                        //test.velocity.Z = (float)(r.NextDouble() * 10 - 5);
         
                        tsheet.addEnt(test);
                        citizenList.Add((Human)test);
                        //mouseReady = false;
                        DEBUGnuments++;
                        
                        
                        //w.setBlockData(100,(int) Chunk.DATA.LIGHT, a);
                        //mouseSelectStart = a;
                        mouseReady = false;
                        

                    }
                }
                if (currentMouseState.RightButton == ButtonState.Pressed && mouseReady)
                {
                    Vector3 a = blockCastTarget;
                    if (!Vector3.Equals(a, new Vector3(-1, -1, -1)))
                    {
                       
                        Vector3 temp = a + lookFace;
                      
                        //w.setBlockData((byte)0, (int)Chunk.DATA.ID, a);
                    
                        /*
                        SpriteSheetEnhanced tsheet;
                        sheetManager.dict.TryGetValue(Entities.Creatures.Human.SheetName, out tsheet);
                        
                        foreach (LivingEntity e in tsheet.ents)
                        {
                            e.finalTarget = temp;
                        }
                       
                        mouseReady = false;
                       */
                        if (currentlySelecting == false)
                        {
                            mouseSelectStart = a;
                            currentlySelecting = true;
                        }
                        else
                        {
                            mouseSelectEnd = a;
                        }
                    }

                }
                
                if (currentMouseState.LeftButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Released)
                {
                    mouseReady = true;
                    if (currentlySelecting)
                    {
                        selectionManager.addSelection(mouseSelectStart, mouseSelectEnd, SelectionManager.JOBTYPE.MINING);
                        currentlySelecting = false;
                    }
                }

                if (mouseWheelPrevious < currentMouseState.ScrollWheelValue)
                {
                    mCam.camPos += (mCam.cameraRotatedTarget) * (float)gameTime.ElapsedGameTime.TotalSeconds * 300f;
                    mouseWheelPrevious = currentMouseState.ScrollWheelValue;
                }
                else if (mouseWheelPrevious > currentMouseState.ScrollWheelValue)
                {
                    mCam.camPos += (mCam.cameraRotatedTarget) * (float)gameTime.ElapsedGameTime.TotalSeconds * -300f;
                    mouseWheelPrevious = currentMouseState.ScrollWheelValue;
                }
            }

            Vector3 flatCamTarget = new Vector3(mCam.cameraRotatedTarget.X, 0, mCam.cameraRotatedTarget.Z);
            flatCamTarget.Normalize();
            Vector3 flatCamTargetRot = new Vector3(flatCamTarget.Z,0,-flatCamTarget.X);
           
            if (k.IsKeyDown(Keys.W))
                mCam.camPos += (flatCamTarget)*(float) gameTime.ElapsedGameTime.TotalSeconds*camSpeedMod;
            if (k.IsKeyDown(Keys.S))
                mCam.camPos += flatCamTarget* (float)gameTime.ElapsedGameTime.TotalSeconds*-camSpeedMod;
            if (k.IsKeyDown(Keys.A))
            {
                mCam.camPos += (flatCamTargetRot) * (float)gameTime.ElapsedGameTime.TotalSeconds * camSpeedMod;
            }
            if (k.IsKeyDown(Keys.D))
            {
                mCam.camPos += (flatCamTargetRot) * (float)gameTime.ElapsedGameTime.TotalSeconds * -camSpeedMod;
            }

            oldKeyState = k;

        }
        public void drawEntities(GameTime g)
        {
            billboardEffect.Parameters["world"].SetValue(Matrix.Identity);
            billboardEffect.Parameters["view"].SetValue(View);
            billboardEffect.Parameters["projection"].SetValue(Projection);
            billboardEffect.Parameters["alphaTestDirection"].SetValue(1.0f);

            foreach (EffectPass pass in billboardEffect.Techniques["BillboardingCameraAligned"].Passes)
            {
                foreach(Art.SpriteSheetEnhanced s in sheetManager.dict.Values)
                {
                    billboardEffect.Parameters["colorMap"].SetValue(s.s.getImage());
                    pass.Apply();
                    s.draw(graphics);
                }
                
            }
        }
        public void draw(GameTime g)
        {
            //tex = game.Content.Load<Texture2D>("Grass (1)");
            //effect.TextureEnabled = true;
            effect.Texture = tex;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Texture = tex;
                pass.Apply();
                w.draw(graphics);
/*
                effect.Texture = treeTex;
                pass.Apply();
                w.drawTrees(graphics); */
            }
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {

                effect.Texture = treeTex;
                pass.Apply();
                w.drawTrees(graphics);
                
            }
            drawEntities(g);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Texture = mMouseIndicator.myTex;
                pass.Apply();
                mMouseIndicator.Draw(graphics);
                foreach (Selection s in selectionManager.selections)
                {
                    foreach(Selection.Job j in s.jobsInSelection)
                    {
                        j.m.Draw(graphics);
                    }
                }
            }

            //bounding boxes stuff

          

            updateTimes.Add(g.ElapsedGameTime.TotalSeconds);
            if (updateTimes.Count > 20)
                updateTimes.RemoveAt(0);
            game.spriteBatch.Begin();
            game.spriteBatch.DrawString(game.Content.Load<SpriteFont>("sego"), "x " + mCam.camPos.X + "\nz: " + mCam.camPos.Z + "\nFPS: " + 1/updateTimes.Average() + "\nNumber of ents: " + DEBUGnuments, new Vector2(0, 0), Color.White);
            game.spriteBatch.End();
        }
    }

}
