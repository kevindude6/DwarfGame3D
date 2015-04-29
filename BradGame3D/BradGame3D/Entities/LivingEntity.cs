﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BradGame3D.AI.Pathing;
using BradGame3D.Art;
using System.Diagnostics;

namespace BradGame3D.Entities
{
    public class LivingEntity : BasicEntity
    {
        public float health = 100;
      
        public Vector2 lookDir = new Vector2(1,0);
        public Vector3 finalTarget;
        public float runSpeed=5f;
        public float walkSpeed=2f;
        public float jumpForce = 600;
        public float pathTolerance = 0.95f;
        public float currentSpeed = 0;
        private bool waitingForPath = false;
        static string[] anims = new string[] { "Idle", "Walk" };
        public Path currentPath = null;
        public bool followingPath = false;
        public enum ANIMSTATES {IDLE,WALK};
        new public static string SheetName = "Squirrel";
        //public bool collideable = true;
        
        public LivingEntity(Vector3 pos, float tHealth) : this(pos) 
        {
            health = tHealth;
        }
        public LivingEntity(Vector3 pos): base(pos)
        {
            collideable = true;
            finalTarget.X = -1;
            finalTarget.Y = -1;
            finalTarget.Z = -1;
        }

        public void followPath(Path p)
        {
            if (p != null)
            {
                currentPath = p;
                followingPath = true;
            }
            else
            {
                finalTarget.X = -1;
                finalTarget.Y = -1;
                finalTarget.Z = -1;
            }
        }
        public static float findAng(Vector2 a, Vector2 b)
        {
            //Debug.WriteLine(Vector2.Dot(a, b) / (a.Length() * b.Length()));
            return (float)Math.Acos(Vector2.Dot(a, b) / (a.Length() * b.Length())) * 57.2957795f;
        }
        public float distTo(Vector3 a)
        {
            double xdist = Math.Pow((float)center.X - a.X, 2);
            double ydist = Math.Pow((float)center.Y - a.Y, 2);
            double zdist = Math.Pow((float)center.Z - a.Z, 2);
            return (float)Math.Sqrt(xdist + ydist + zdist);
        }
        public override void update(float gameTime, World2 w)
        {
            base.update(gameTime,w);

            currentLook = SpriteSheet.Direction.TOWARD;
            if (followingPath)
            {
                waitingForPath = false;
                currentAnim = anims[(int)ANIMSTATES.WALK];

                if (currentPath.nodeList.ElementAt(0).distTo(center) < pathTolerance)
                {
                    currentPath.nodeList.RemoveAt(0);
                }
                if (currentPath.nodeList.Count() == 0)
                {
                    followingPath = false;
                    currentPath = null;

                    if (distTo(finalTarget) < pathTolerance)
                    {
                        finalTarget.X = -1;
                        finalTarget.Y = -1;
                        finalTarget.Z = -1;
                    }
                    
                    //velocity = Vector3.Zero;
                }
                else
                {
                    Node n = currentPath.nodeList.ElementAt(0);
                    //int x = (int)Math.Round(center.X);
                    int y = (int)Math.Round(center.Y);
                    //int z = (int)Math.Round(center.Z);
                    Vector3 dir = new Vector3(n.x - center.X, n.y - center.Y, n.z - center.Z);

                    dir.Normalize();
                    dir = dir * runSpeed;
                       
                    Vector3 steeringForce = dir - velocity;
                    //steeringForce /= mass;
                    //velocity = steeringForce + velocity;
                    if (isOnGround)
                    {
                        addForce(steeringForce * mass * 10);
                        if (velocity.Length() > runSpeed)
                        {
                            velocity.Normalize();
                            velocity = velocity * runSpeed;
                        }
                        if (n.y>y)
                        {
                            //addForce(new Vector3(0, 1000, 0));
                            velocity.Y += 5.2f;
                            isOnGround = false;
                        }
                    }
                    else
                    {
                        steeringForce.Y = 0;
                        addForce(steeringForce * mass * 5);
                    }

                       
                }
            }
            else
            {
                if (waitingForPath == false)
                {
                    if (finalTarget.X != -1 && finalTarget.Y != -1 && finalTarget.Z != -1)
                    {
                        int x = (int)Math.Round(center.X);
                        int y = (int)Math.Round(center.Y);
                        int z = (int)Math.Round(center.Z);
                        if ((int)finalTarget.X != x || (int)finalTarget.Y != y || (int)finalTarget.Z != z)
                        {
                            PathData a;
                            a.start = center;
                            a.end = finalTarget;
                            a.b = this;
                            PathingManager.data.Add(a);
                            waitingForPath = true;
                        }
                    }
                    
                }

                if (isOnGround)
                {
                    currentAnim = anims[(int)ANIMSTATES.IDLE];
                    addForce(velocity * -mass * 7);
                }

            }

            if (!isOnGround)
            {
                addForce(new Vector3(0, -10 * mass, 0));
            }

               

            if (velocity.LengthSquared() != 0)
            {
                lookDir.X = velocity.X;
                lookDir.Y = velocity.Z;
                lookDir.Normalize();
            }
            Vector2 temp = new Vector2(w.game.mCam.camPos.X-center.X,w.game.mCam.camPos.Z-center.Z);
            temp.Normalize();
            temp = Vector2.Negate(temp);


            float ang = findAng(lookDir, temp);

            
            if (ang <= 45)
            {
                currentLook = SpriteSheet.Direction.AWAY;
            }
            else if (ang >= 135)
            {
                currentLook = SpriteSheet.Direction.TOWARD;
            }
            else
            {
                Vector2 rightLook = new Vector2(lookDir.Y, -lookDir.X);
                float newAng = findAng(rightLook, temp);

                if (newAng < 45)
                {
                    currentLook = SpriteSheet.Direction.RIGHT;
                }
                else if (newAng > 135)
                {
                    currentLook = SpriteSheet.Direction.LEFT;
                }
            }



            //center += velocity * (gameTime / 1000f);

        }
    }
}
