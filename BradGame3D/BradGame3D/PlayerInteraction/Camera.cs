using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.PlayerInteraction
{
    public class Camera
    {
        public Vector3 camPos;
        public Vector3 camDir;
        public Vector3 cameraRotatedTarget;
        //public Vector3 currentRaycastBlockTarget;
        private GameScreen game;
        private World2 world;
        private GraphicsDeviceManager graphics;
        //private AlphaTestEffect effect;
        public float pitch;
        public float yaw;

        public Camera(GameScreen g, World2 w, GraphicsDeviceManager graph, Vector3 pos)
        {
            game = g;
            world = w;
            graphics = graph;
            camPos = pos;
        }

        public void doCameraInput(GameTime gameTime,KeyboardState k)
        {

            if (k.IsKeyDown(Keys.Up))
                pitch += 0.005f * gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Down))
                pitch -= 0.005f * gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Right))
                yaw -= 0.005f * gameTime.ElapsedGameTime.Milliseconds;
            if (k.IsKeyDown(Keys.Left))
                yaw += 0.005f * gameTime.ElapsedGameTime.Milliseconds;

        }
        public void doCameraGame()
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

            game.View = Matrix.CreateLookAt(camPos, cameraFinalTarget, cameraRotatedUpVector);

            game.effect.View = game.View;
        }

        public bool raycastBlock(ref Vector3 outVector, ref Vector3 lookFace)
        {
            MouseState s = Mouse.GetState();
            double mouseX = s.X;
            double mouseY = s.Y;
            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix worldMat = game.effect.World;

            Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject(nearsource,
                game.effect.Projection, game.effect.View, worldMat);

            Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject(farsource,
                game.effect.Projection, game.effect.View, worldMat);



            Vector3 dir = Vector3.Normalize(farPoint - nearPoint);
            if (dir == Vector3.Zero)
                return false;
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

                if (Block.getRender(world.getBlockData((int)Chunk.DATA.ID, x, y, z)))
                {
                    lookFace = tempLookFace;
                    outVector = new Vector3(x, y, z);
                    return true;
                }
            }
            return false;
        }

        private static float mod(float value, float modulus)
        {
            return (value % modulus + modulus) % modulus;
        }
        private static float intBound(float s, float ds)
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
    }
}

