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
        public float DEBUGnuments = 0;

        Thread chunkLoadingThread;
        Thread pathingThread;


        public Matrix View;
        public Matrix Projection;

        public AlphaTestEffect effect;
        private Effect billboardEffect;
        private GraphicsDeviceManager graphics;

        private KeyboardState oldKeyState;
       

        public BoundingFrustum frustum;
        public Texture2D tex;
        public Texture2D treeTex;

        public Camera mCam;
        public LivingEntity test;
        
        public AI.Pathing.Node start = null;
        public AI.Pathing.Node end = null;
        public MouseIndicator mMouseIndicator;

        
        public MouseController mouseControl;
        

        public PlayerInteraction.GuiLibrary.Gui gui;
        public static GameScreen thisScreen;
        public SelectionManager selectionManager;
        public ParticleManager particleMan;
        public List<Citizen> citizenList = new List<Citizen>();
       // private List<MouseIndicator> selectionList = new List<MouseIndicator>();
        public Random r;
        public bool loadedChunkThisFrame = false;

        private List<double> updateTimes = new List<double>();

        static public Blocks.BlockDataManager blockDataManager = new Blocks.BlockDataManager();
        public Art.SpriteSheetManager sheetManager;

        public static GameScreen getScreen()
        {
            return thisScreen;
        }
        public void initBlockData()
        {
            int[] tempSideArray = { 0, 0, 0, 0, 0, 0 };
            blockDataManager.addBlock("Air", false, false, false, tempSideArray, 32, 320); //0

            tempSideArray = new int[] { 0, 0, 0, 0, 1, 4 };
            //tempSideArray = new int[] { 2, 2, 2, 2, 2, 2 };
            blockDataManager.addBlock("Grass", true, true, false, tempSideArray, 32, 320); //1

            tempSideArray = new int[] { 2, 2, 2, 2, 2, 2 };
            blockDataManager.addBlock("Stone", true, true, false, tempSideArray, 32, 320); //2

            tempSideArray = new int[] { 4, 4, 4, 4, 4, 4 };
            blockDataManager.addBlock("Dirt", true, true, false, tempSideArray, 32, 320); //3

            tempSideArray = new int[] { 3, 3, 3, 3, 3, 3 };
            blockDataManager.addBlock("Cobblestone", true, true, false, tempSideArray, 32, 320); //4


            tempSideArray = new int[] { 5, 5, 5, 5, 5, 5 };
            blockDataManager.addBlock("Light", true, true, true, tempSideArray, 32, 320); //5

            tempSideArray = new int[] {0,0,0,0,0,0};
            blockDataManager.addBlock("Flora Placeholder", false, true, false, tempSideArray, 32, 320); //6

            tempSideArray = new int[] {6,6,6,6,6,6};
            blockDataManager.addBlock("Plank", true, true, false, tempSideArray, 32, 320); //7

            tempSideArray = new int[] { 7, 7, 7, 7, 7, 7 };
            blockDataManager.addBlock("Sand", true, true, false, tempSideArray, 32, 320); //8

            tempSideArray = new int[] { 8, 8, 8, 8, 8, 8 };
            blockDataManager.addBlock("Sandstone", true, true, false, tempSideArray, 32, 320); //9

        }

        public GameScreen(Game1 g)
        {
            initBlockData();

            thisScreen = this;
            game = g;
            graphics = game.graphics;
            
            
            w = new World2(this);

            r = new Random();

            
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

            

            particleMan = new ParticleManager(this);
            gui = new PlayerInteraction.GuiLibrary.Gui(this);

            loadThings();
            
            mCam = new Camera(this,w,graphics, new Vector3(256,120,256));
            mouseControl = new MouseController(this);

        }
        public void loadThings()
        {
            tex = game.Content.Load<Texture2D>("bradgameblocks");
            treeTex = game.Content.Load<Texture2D>("floraspritesheet");
            
            //gui.loadGuiTex();


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
                particleMan.updateParticles(gameTime);
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
            if (k.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M) && mouseControl.mouseEnabled)
            {
                mouseControl.mouseEnabled = false;
            }
            else if (k.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M) && !mouseControl.mouseEnabled)
            {
                mouseControl.mouseEnabled = true;
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

            mouseControl.doMouseInput(gameTime);
            //Console.WriteLine("Mouse: (" + currentMouseState.X + "," + currentMouseState.Y + ")");

          

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
            billboardEffect.Parameters["alphaValue"].SetValue(1.0f);

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
        public void drawParticles(GameTime g)
        {
            foreach (EffectPass pass in billboardEffect.Techniques["BillboardingCameraAligned"].Passes)
            {
                billboardEffect.Parameters["colorMap"].SetValue(particleMan.particleSheet.getImage());
                pass.Apply();
                particleMan.draw(graphics, billboardEffect, pass);

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

            }
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {

                effect.Texture = treeTex;
                pass.Apply();
                w.drawTrees(graphics);
                
            }

            drawEntities(g);
            drawParticles(g);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Texture = mMouseIndicator.myTex;
                pass.Apply();
                mMouseIndicator.Draw(graphics);
                lock (selectionManager)
                {
                    foreach (Selection s in selectionManager.selections)
                    {
                        foreach (Selection.Job j in s.jobsInSelection)
                        {
                            j.m.Draw(graphics);
                        }
                    }
                }
                SpriteSheetEnhanced tsheet;
                sheetManager.dict.TryGetValue(Entities.Creatures.Citizen.SheetName, out tsheet);
                foreach (Citizen h in tsheet.ents)
                {
                    if (h.doingTask)
                    {
                        foreach (Selection.Job j in h.tasks)
                        {
                            j.m.Draw(graphics);
                        }
                    }
                }
            }

            //bounding boxes stuff

          

            updateTimes.Add(g.ElapsedGameTime.TotalSeconds);
            if (updateTimes.Count > 20)
                updateTimes.RemoveAt(0);
            game.spriteBatch.Begin();
            game.spriteBatch.DrawString(game.Content.Load<SpriteFont>("sego"), "x " + mCam.camPos.X + "\nz: " + mCam.camPos.Z + "\nFPS: " + 1/updateTimes.Average() + "\nNumber of ents: " + DEBUGnuments, new Vector2(0, 0), Color.White);
            gui.drawGui(game.spriteBatch);
            game.spriteBatch.End();
        }
    }

}
