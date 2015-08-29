using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using BradGame3D.Art;
using BradGame3D.Entities.Creatures;

namespace BradGame3D.PlayerInteraction
{
    public class MouseController
    {
        public GameScreen gscreen;
        public Camera mCam;
        private MouseState currentMouseState;
        private MouseState originalMouseState;
        private int mouseWheelPrevious;
        private bool mouseReady;
        public bool mouseEnabled;
        const float rotationSpeed = 0.005f;
        private bool currentlySelecting;
        private Vector3 mouseSelectStart;
        private Vector3 mouseSelectEnd;
        private Vector3 blockCastTarget = new Vector3(0, 0, 0);
        private Vector3 lookFace = new Vector3(0, 0, 0);

        public int currentBuild = 0;

        public enum MODE { SELECT, BUILD };
        public MODE mode = MODE.BUILD;
        public MouseController(GameScreen g)
        {
            gscreen = g;
            mCam = g.mCam;
            mouseWheelPrevious = originalMouseState.ScrollWheelValue;
            Mouse.SetPosition(gscreen.game.Window.ClientBounds.Width / 2, gscreen.game.Window.ClientBounds.Height / 2);
            originalMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();
        }

        public void doMouseInput(GameTime gameTime)
        {
            originalMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState != originalMouseState)
            {
                if (mCam.raycastBlock(ref blockCastTarget, ref lookFace))
                {
                    if (mode == MODE.SELECT)
                    {
                        gscreen.mMouseIndicator.setPosition(blockCastTarget);
                    }
                    else if (mode == MODE.BUILD)
                    {
                        gscreen.mMouseIndicator.setPosition(blockCastTarget + lookFace);
                    }
                }
                    

                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    float xDifference = currentMouseState.X - originalMouseState.X;
                    float yDifference = currentMouseState.Y - originalMouseState.Y;
                    mCam.yaw -= rotationSpeed * xDifference;
                    mCam.pitch -= rotationSpeed * yDifference;
                    //Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);

                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && mouseReady && gscreen.gui.checkClick(currentMouseState.X, currentMouseState.Y))
                {

                    mouseReady = false;
                }
                else
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed && mouseReady)
                    {
                        Vector3 a = blockCastTarget;

                        if (mode == MODE.SELECT)
                        {
                            if (!Vector3.Equals(a, new Vector3(-1, -1, -1)))
                            {

                                Citizen test;
                                SpriteSheetEnhanced tsheet;
                                gscreen.sheetManager.dict.TryGetValue(Entities.Creatures.Citizen.SheetName, out tsheet);
                                test = new Entities.Creatures.Citizen(a + lookFace + new Vector3((float)(gscreen.r.NextDouble() - 0.5f), 2, (float)(gscreen.r.NextDouble() - 0.5f)), 100);




                                tsheet.addEnt(test);
                                gscreen.citizenList.Add((Citizen)test);

                                gscreen.DEBUGnuments++;
                                mouseReady = false;


                            }
                        }
                        else
                        {
                            GameScreen.w.setBlockData((byte)currentBuild, (int)Chunk.DATA.ID, blockCastTarget + lookFace);
                            mouseReady = false;
                        }
                    }
                }

                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    mouseReady = true;
                    if (mode == MODE.SELECT)
                    {
                        gscreen.selectionManager.addSelection(mouseSelectStart, mouseSelectEnd, SelectionManager.JOBTYPE.MINING);
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
        }
        
    }
    
}
