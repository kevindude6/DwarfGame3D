using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BradGame3D.PlayerInteraction;
using System.Diagnostics;

namespace BradGame3D.Entities.Creatures
{
    public class Citizen : LivingEntity
    {
        new public static string SheetName = "Human";
        
        private static int jobsNum = 2;
        public byte[] jobProficiency = new byte[jobsNum];
        public bool[] jobEnabled = new bool[jobsNum];
        public float[] jobEXP = new float[jobsNum];
        public List<Selection.Job> tasks = new List<Selection.Job>();
        public Selection.Job currentTask;
        public bool doingTask = false;

        public Citizen(Vector3 pos, float thealth): base(pos, thealth)
        {
            collideable = true;
            jobProficiency[(int)SelectionManager.JOBTYPE.MINING] = 255;
            jobEnabled[(int)SelectionManager.JOBTYPE.MINING] = true;
        }
        public Citizen(Vector3 pos) : this(pos,150)
        {
            
        }
        public override void doOnArrive()
        {
            switch (currentTask.jobType)
            {
                case SelectionManager.JOBTYPE.MINING: eventQueue.Add(new Event(100, doMine)); break;
                //case SelectionManager.JOBTYPE.MINING: eventQueue.Add(new Event(100,delegate(){this.doMine();})); break;
            }
        }
        public void doMine()
        {
            parentChunk.world.setBlockData((byte) 0, (int) Chunk.DATA.ID, currentTask.x, currentTask.y, currentTask.z);
            setFinalTarget(-1, -1, -1);
            if(tasks.Contains(currentTask))
                tasks.Remove(currentTask);
            doingTask = false;
            bleed();
        }
        public override void update(float gameTime, World2 w)
        {
            base.update(gameTime, w);

            if (tasks.Count > 0 && doingTask == false)
            {
                switch (tasks[0].jobType)
                {
                    case SelectionManager.JOBTYPE.MINING: goToBlock(tasks[0]); break;
                }
            }
        }
        public void goToBlock(Selection.Job mJob)
        {
            //Debug.WriteLine("Mining");
            if (parentChunk.world.isSolid(mJob.x, mJob.y, mJob.z))
            {
                currentTask = mJob;
                currentTask.m.updateColor(Color.Red);
                doingTask = true;
                if (distTo(mJob) > 2)
                {
                    /*
                    if (!parentChunk.world.isSolid(mJob.x - 1, mJob.y, mJob.z))
                    {
                        setFinalTarget(mJob.x - 1, mJob.y, mJob.z);
                    }
                    else if (!parentChunk.world.isSolid(mJob.x + 1, mJob.y, mJob.z))
                    {
                        setFinalTarget(mJob.x + 1, mJob.y, mJob.z);
                    }
                    else if (!parentChunk.world.isSolid(mJob.x, mJob.y, mJob.z - 1))
                    {
                        setFinalTarget(mJob.x, mJob.y, mJob.z - 1);
                    }
                    else if (!parentChunk.world.isSolid(mJob.x, mJob.y, mJob.z + 1))
                    {
                        setFinalTarget(mJob.x, mJob.y, mJob.z + 1);
                    }
                    else if (!parentChunk.world.isSolid(mJob.x, mJob.y + 1, mJob.z))
                    {
                        setFinalTarget(mJob.x, mJob.y + 1, mJob.z);
                    }
                    else if (!parentChunk.world.isSolid(mJob.x, mJob.y - 1, mJob.z))
                    {
                        setFinalTarget(mJob.x, mJob.y - 1, mJob.z);
                    }
                     */
                    setFinalTarget(mJob.x, mJob.y, mJob.z);
                }
                else
                    doOnArrive();

                
            }
            else
            {
                tasks.Remove(mJob);
                //currentTask.jobType = -1; 
                setFinalTarget(-1,-1,-1);
            }
        }
        public override void initSize()
        {
            renderWidth = 0.75f;
            collideRadius = 0.4f;
            height = 0.95f;
            mass = 150;
        }
    }
}
