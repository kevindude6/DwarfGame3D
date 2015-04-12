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

namespace BradGame3D
{
    public class GameScreen
    {
        public Game1 game;
        public static World2 w;

        Thread chunkLoadingThread;

        const float rotationSpeed = 0.0075f;
        public Vector3 camPos = new Vector3(256,120,256);
        public Matrix View;
        public Matrix Projection;
        public Vector3 cameraRotatedTarget;
        public Vector2 camDir = new Vector2(0,0);
        private float yaw, pitch;

        private AlphaTestEffect effect;
        private Effect billboardEffect;
        private GraphicsDeviceManager graphics;

        private KeyboardState oldKeyState;
        private MouseState originalMouseState;
        private int mouseWheelPrevious;
        private bool mouseReady;
        private bool mouseEnabled;
        private Vector3 lookFace;
        public BoundingFrustum frustum;
        public Texture2D tex;
        public Texture2D treeTex;
        public Texture2D dogeTex;

        public LivingEntity test;
        
        public AI.Pathing.Node start = null;
        public AI.Pathing.Node end = null;
        public MouseIndicator mMouseIndicator;

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

            loadThings();

        }
        public void loadThings()
        {
            tex = game.Content.Load<Texture2D>("bradgameblocks");
            treeTex = game.Content.Load<Texture2D>("treeGray");
            dogeTex = game.Content.Load<Texture2D>("dogecoin-300");

            sheetManager = new Art.SpriteSheetManager(this);
            sheetManager.loadSheet("Sprite_creature_squirrel",w);
            sheetManager.loadSheet("pathindicator",w);
        }
        public void updateFrustum()
        {
            frustum.Matrix = effect.View * effect.Projection;
        }
        public void updateGame(GameTime gameTime)
        {

            loadedChunkThisFrame = false;
            doInputGame(gameTime);
            doCameraGame();
            updateFrustum();
            updateEnts(gameTime);
          
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
            if (k.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M) && !mouseEnabled)
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

            if (k.IsKeyDown(Keys.Up))
                pitch += 0.005f*gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Down))
                pitch -= 0.005f*gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Right))
                yaw -= 0.005f * gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Left))
                yaw += 0.005f*gameTime.ElapsedGameTime.Milliseconds;

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
           

            MouseState currentMouseState = Mouse.GetState();


            if (currentMouseState != originalMouseState)
            {
                mMouseIndicator.setPosition(raycast());
                if (mouseEnabled)
                {
                    float xDifference = currentMouseState.X - originalMouseState.X;
                    float yDifference = currentMouseState.Y - originalMouseState.Y;
                    yaw -= rotationSpeed * xDifference;
                    pitch -= rotationSpeed * yDifference;
                    Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
                 
                }
         
                if (currentMouseState.LeftButton == ButtonState.Pressed && mouseReady)
                {
                    Vector3 a = raycast();
                    if (!Vector3.Equals(a, new Vector3(-1, -1, -1)))
                    {
                        //w.makeTree(a + lookFace);
                        SpriteSheetEnhanced tsheet;
                        sheetManager.dict.TryGetValue("Squirrel", out tsheet);
                        test = new LivingEntity(a + lookFace + new Vector3(0, 2, 0), 100);
                        tsheet.addEnt(test);
                        Vector3 temp = a+lookFace;
                        //start = new AI.Pathing.Node((int)temp.X, (int)temp.Y,(int) temp.Z);
                        mouseReady = false;

                        
                        //w.setBlockData((byte)currentBlock,(int) Chunk.DATA.ID, temp);

                    }
                }
                if (currentMouseState.RightButton == ButtonState.Pressed && mouseReady)
                {
                    Vector3 a = raycast();
                    if (!Vector3.Equals(a, new Vector3(-1, -1, -1)))
                    {
                        //w.makeTree(a + lookFace);
                        //SpriteSheetEnhanced tsheet;
                        //sheetManager.dict.TryGetValue("Squirrel", out tsheet);
                        //tsheet.addEnt(new BasicEntity(a + lookFace));
                        Vector3 temp = a + lookFace;
                        //end = new AI.Pathing.Node((int)temp.X, (int)temp.Y, (int)temp.Z);

                        //w.setBlockData((byte)0, (int)Chunk.DATA.ID, a);
                        
                        test.followPath(AI.Pathing.Pathing.findPath(test.center,temp,w));
                        
                        mouseReady = false;
                        /*
                        if (start != null && end != null)
                        {
                            AI.Pathing.Path p = AI.Pathing.Pathing.findPath(start, end, w);
                            foreach (AI.Pathing.Node n in p.nodeList)
                            {
                                tsheet.addEnt(new BasicEntity(new Vector3(n.x, n.y, n.z)));
                            }
                        }
                        */
                    }

                }
                
                if (currentMouseState.LeftButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Released)
                {
                    mouseReady = true;
                }

                if (mouseWheelPrevious < currentMouseState.ScrollWheelValue)
                {
                    camPos += (cameraRotatedTarget) * (float)gameTime.ElapsedGameTime.TotalSeconds * 300f;
                    mouseWheelPrevious = currentMouseState.ScrollWheelValue;
                }
                else if (mouseWheelPrevious > currentMouseState.ScrollWheelValue)
                {
                    camPos += (cameraRotatedTarget) * (float)gameTime.ElapsedGameTime.TotalSeconds * -300f;
                    mouseWheelPrevious = currentMouseState.ScrollWheelValue;
                }
            }

            Vector3 flatCamTarget = new Vector3(cameraRotatedTarget.X, 0, cameraRotatedTarget.Z);
            flatCamTarget.Normalize();
            Vector3 flatCamTargetRot = new Vector3(flatCamTarget.Z,0,-flatCamTarget.X);
           
            if (k.IsKeyDown(Keys.W))
                camPos += (flatCamTarget)*(float) gameTime.ElapsedGameTime.TotalSeconds*40f;
            if (k.IsKeyDown(Keys.S))
                camPos += flatCamTarget* (float)gameTime.ElapsedGameTime.TotalSeconds*-40f;
            if (k.IsKeyDown(Keys.A))
            {
                camPos += (flatCamTargetRot) * (float)gameTime.ElapsedGameTime.TotalSeconds * 40f;
            }
            if (k.IsKeyDown(Keys.D))
            {
                camPos += (flatCamTargetRot) * (float)gameTime.ElapsedGameTime.TotalSeconds * -40f;
            }

            oldKeyState = k;

        }
        //private Vector3 raycast()

        
        
        
        private Vector3 raycast()
        {
            MouseState s = Mouse.GetState();
            double mouseX = s.X;
            double mouseY = s.Y;
            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix world = effect.World;

            Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject(nearsource,
                effect.Projection, effect.View, world);

            Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject(farsource,
                effect.Projection, effect.View, world);



            Vector3 dir = Vector3.Normalize(farPoint - nearPoint);
            if (dir == Vector3.Zero)
                return new Vector3(-1,-1,-1);
            int x = (int)Math.Round(camPos.X);
            int y = (int)Math.Round(camPos.Y);
            int z = (int)Math.Round(camPos.Z);

            int stepX = Math.Sign(dir.X);
            int stepY = Math.Sign(dir.Y);
            int stepZ = Math.Sign(dir.Z);

            float tMaxX = intBound(camPos.X, dir.X);
            float tMaxY = intBound(camPos.Y, dir.Y);
            float tMaxZ = intBound(camPos.Z, dir.Z);

            float tDeltaX = stepX / dir.X;
            float tDeltaY = stepY / dir.Y;
            float tDeltaZ = stepZ / dir.Z;

           



            Vector3 tempLookFace;
            while (
                (stepX > 0 ? x < World2.worldSize * Chunk.xSize : x >= 0) &&
                (stepY > 0 ? y < Chunk.ySize : y >= 0) &&
                (stepZ > 0 ? z < World2.worldSize * Chunk.zSize : z >= 0))
            {
                if (tMaxX < tMaxY)
                {
                    if (tMaxX < tMaxZ)
                    {
                        x += stepX;
                        tMaxX += tDeltaX;
                        tempLookFace = new Vector3(-stepX, 0, 0);

                    }
                    else
                    {
                        z += stepZ; 
                        tMaxZ += tDeltaZ;
                        tempLookFace = new Vector3(0, 0, -stepZ);
                    }
                }
                else
                {
                    if (tMaxY < tMaxZ)
                    {
                        y += stepY;
                        tMaxY += tDeltaY;
                        tempLookFace = new Vector3(0, -stepY, 0);
                    }
                    else
                    {
                        z += stepZ;
                        tMaxZ += tDeltaZ;
                        tempLookFace = new Vector3(0, 0, -stepZ);
                    }
                }

                if (Block.getRender(w.getBlockData((int) Chunk.DATA.ID, x, y, z)))
                {
                    lookFace = tempLookFace;
                    return new Vector3(x,y,z);
                }
                //else if (w.getBlockAt(x, y, z) != null)
                //w.getBlockAt(x, y, z).setSolid(true);
            }
            return new Vector3(-1, -1, -1);
        }
         
        private float mod(float value, float modulus)
        {
            return (value % modulus + modulus) % modulus;
        }
        private float intBound(float s, float ds)
        {
            if (ds < 0)
            {
                return intBound(-s, -ds);
            }
            else
            {
                s += 0.5f;
                s = mod(s, 1);
                return ((1f - (s)) / ds) - 0.5f;
            }
        }
        private void doCameraGame()
        {
            Matrix a = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, a);
            Vector3 cameraFinalTarget = camPos + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, a);

            camDir.X = cameraRotatedTarget.X;
            camDir.Y = cameraRotatedTarget.Z;
            camDir.Normalize();

            View = Matrix.CreateLookAt(camPos, cameraFinalTarget, cameraRotatedUpVector);

            effect.View = View;
        }
        public void drawEntities(GameTime g)
        {
            billboardEffect.Parameters["world"].SetValue(Matrix.Identity);
            billboardEffect.Parameters["view"].SetValue(View);
            billboardEffect.Parameters["projection"].SetValue(Projection);
            //billboardEffect.Parameters["colorMap"].SetValue(dogeTex);
            billboardEffect.Parameters["alphaTestDirection"].SetValue(1.0f);

            //BasicEntity b = new BasicEntity(new Vector3(256, 120,256));
            
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
            }

            

            updateTimes.Add(g.ElapsedGameTime.TotalSeconds);
            if (updateTimes.Count > 20)
                updateTimes.RemoveAt(0);
            game.spriteBatch.Begin();
            game.spriteBatch.DrawString(game.Content.Load<SpriteFont>("sego"), "x " + camPos.X + "\nz: " + camPos.Z + "\nFPS: " + 1/updateTimes.Average(), new Vector2(0, 0), Color.White);
            game.spriteBatch.End();
        }
    }

}
